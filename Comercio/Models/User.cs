using Comercio.Enums;
using System.Security.Cryptography;
using System.Text;

namespace Comercio.Models
{
    /// <summary>
    /// User entity in order to store users
    /// </summary>
    public class User : Entity<Guid>
    {
        /// <summary>
        /// User name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User surname
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// User email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User phone number
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// User date of birth
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// User country which lives in
        /// </summary>
        public short? CityId { get; set; }

        /// <summary>
        /// User profile picture name
        /// </summary>
        public string ProfilePicture { get; set; }

        /// <summary>
        /// User password hash with SHA256 
        /// </summary>
        public byte[] PasswordHash { get; set; }

        /// <summary>
        /// Salt is a key for getting hash of password
        /// </summary>
        public byte[] Salt { get; set; }

        /// <summary>
        /// User role in website
        /// </summary>
        public byte UserRoleId { get; set; }

        /// <summary>
        /// User status (active,deactive, blocked ...)
        /// </summary>
        public byte UserStatusId { get; set; }

        /// <summary>
        /// User last login date
        /// </summary>
        public DateTime? LastLogged { get; set; }

        /// <summary>
        /// User IP address which used when he/she register
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// User gender
        /// </summary>
        public bool? Gender { get; set; }

        public bool EmailConfirmed { get; set; }


        public UserRole UserRole { get; set; }
        public ICollection<UserWishlist> UserWishlists { get; set; }
        public City City { get; set; }


        public User(string name,
                    string surname,
                    string email)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Phone = "";
            ProfilePicture = "";
            Created = DateTime.Now;
            Updated = DateTime.Now;
            UserStatusId = (byte)UserStatusEnum.Active;
        }

        public void UpdatePassword(string newPassword)
        {
            var salt = Guid.NewGuid();

            newPassword += salt.ToString();
            using (SHA256 sha256 = SHA256.Create())
            {
                var buffer = Encoding.UTF8.GetBytes(newPassword);

                var hash = sha256.ComputeHash(buffer);

                Salt = Encoding.UTF8.GetBytes(salt.ToString());

                PasswordHash = hash;
            }
        }
        public void AddPassword(string password)
        {
            var salt = Guid.NewGuid();

            password += salt.ToString();
            using (SHA256 sha256 = SHA256.Create())
            {
                var buffer = Encoding.UTF8.GetBytes(password);

                var hash = sha256.ComputeHash(buffer);

                Salt = Encoding.UTF8.GetBytes(salt.ToString());

                PasswordHash = hash;
            }
        }

        public bool CheckPassword(string password)
        {
            var salt = Encoding.UTF8.GetString(Salt);
            password += salt;
            
            using (SHA256 sha256 = SHA256.Create())
            {
                var buffer = Encoding.UTF8.GetBytes(password);

                var hash = sha256.ComputeHash(buffer);

                if (hash.SequenceEqual(PasswordHash)) 
                    return true;
                else
                    return false;
            }
        }

        public void AddUserRole()
        {
            UserRoleId = (byte)UserRoleEnum.User;
        }
    }
}
