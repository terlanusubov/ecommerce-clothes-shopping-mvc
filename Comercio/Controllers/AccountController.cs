using Comercio.Data;
using Comercio.Exceptions;
using Comercio.Models;
using Comercio.ServiceModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Comercio.Enums;
using System.Net.Mail;
using System.Net;
using System.Net.Http;
using Comercio.Notifications.Slack;

namespace Comercio.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SlackService _slackService;

        public AccountController(ApplicationDbContext context,
                                 SlackService slackService)
        {
            _context = context;
            _slackService = slackService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var user = await _context.Users
                                        .Include(c=>c.UserRole)
                                        .Where(c => c.Email == request.Email &&
                                                       c.UserStatusId == (int)UserStatusEnum.Active).FirstOrDefaultAsync();

            if (user is null)
            {
                ModelState.AddModelError("", "Email or password is not correct");
                return View(request);
            }

            var result = user.CheckPassword(request.Password);

            if (!result)
            {
                ModelState.AddModelError("", "Email or password is not correct");
                return View(request);
            }

            var claims = new List<Claim>
              {
                  new Claim("Name", user.Name),
                  new Claim("Surname", user.Surname),
                  new Claim("Email", user.Email),
                  new Claim("Id", user.Id.ToString()),
                  new Claim("RoleId", user.UserRoleId.ToString()),
                  new Claim(ClaimTypes.Role, user.UserRole.Name)
              };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);


            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel request)
        {
            try
            {
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var user = await _context.Users.Where(c => c.Email == request.Email).FirstOrDefaultAsync();

                if (user is not null)
                {
                    ModelState.AddModelError("", "There is already user like this");
                    return View(request);
                }

                user = new User(request.Name,
                                    request.Surname,
                                    request.Email);
                user.IP = ip ?? "::00";

                user.AddPassword(request.Password);

                user.AddUserRole();

                await _context.Users.AddAsync(user);

                await _context.SaveChangesAsync();

                _slackService.SendRegisterUserInfo(user.Name + " " + user.Surname + " " + user.Email + " " + user.Created?.ToString("dd:MM:yyyy hh:mm:ss") + " tarixində qeydiyyatdan keçmişdir.");

                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                //Slack notification
                //Telegram notification
                //Database record
                return RedirectToAction("Internal", "Error");
            }

        }

        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Email is not correct");
                return View("ForgotPassword", email);
            }

            var user = await _context.Users.Where(c => c.Email == email &&
                                                    c.UserStatusId == (int)UserStatusEnum.Active)
                                                    .FirstOrDefaultAsync();

            if (user is null)
            {
                ModelState.AddModelError("", "There is not any user like that");
                return View("ForgotPassword", email);
            }


            //1. Random bir OTP generate et
            //2. Hemin kodu sessiyada saxla
            //3. Adama emailne hemin kodu gonder (async sekilde)

            var otp = GenerateOTP();
            HttpContext.Session.SetString("otp", otp);
            HttpContext.Session.SetString("otp_userId", user.Id.ToString());

            // Sender's email credentials
            string senderEmail = "taceddin_guven@hotmail.com";
            string senderPassword = "Melek2012";

            // Receiver's email
            string receiverEmail = user.Email;

            // SMTP server details
            string smtpServer = "smtp.office365.com";
            int port = 587; // SMTP port (587 for TLS/STARTTLS)

            // Create a new SMTP client
            SmtpClient client = new SmtpClient(smtpServer, port);
            client.EnableSsl = true; // Enable SSL/TLS

            // Set sender credentials
            client.Credentials = new NetworkCredential(senderEmail, senderPassword);

            MailMessage mailMessage = new MailMessage(senderEmail, receiverEmail);
            mailMessage.Subject = "Şifrə yeniləmək üçün OTP";
            mailMessage.Body = otp;

            try
            {

                client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("ApproveOTP", "Account");
        }


        [HttpGet]
        public async Task<IActionResult> ApproveOTP()
        {
            var oneTimePassword = HttpContext.Session.GetString("otp");
            if (oneTimePassword is null)
            {
                return RedirectToAction("Index", "Home");
            }


            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ApproveOTP(int otpFromEmail)
        {
            var oneTimePassword = HttpContext.Session.GetString("otp");
            if (oneTimePassword is null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (oneTimePassword != otpFromEmail.ToString())
            {
                ModelState.AddModelError("", "OTP is not correct!");
                return View("ApproveOTP", otpFromEmail);
            }

            HttpContext.Session.SetString("otp_approved", "true");

            return RedirectToAction("NewPassword", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> NewPassword()
        {
            var otpApproved = HttpContext.Session.GetString("otp_approved");
            if (otpApproved is null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> NewPassword(UpdatePasswordModel request)
        {
            var otpApproved = HttpContext.Session.GetString("otp_approved");
            if (otpApproved is null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var userId = HttpContext.Session.GetString("otp_userId");

            var user = await _context.Users.Where(c => c.Id.ToString() == userId).FirstOrDefaultAsync();
            
            if(user is null)
            {
                ModelState.AddModelError("", "There is not any user like that");
                return View(request);
            }

            user.UpdatePassword(request.NewPassword);

            await _context.SaveChangesAsync();

            TempData["success"] = "Password changed successfully";

            HttpContext.Session.Remove("otp");
            HttpContext.Session.Remove("otp_userId");
            HttpContext.Session.Remove("otp_approved");

            return RedirectToAction("Index","Home");
        }

        private string GenerateOTP()
        {
            Random rand = new Random();
            return rand.Next(1000, 10000).ToString();
        }
    }
}
