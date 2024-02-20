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
        public byte[] Salt { get; set;}

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
        public DateTime? LastLogged{ get; set; }

        /// <summary>
        /// User IP address which used when he/she register
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// User gender
        /// </summary>
        public bool? Gender { get; set; }



        public UserRole UserRole { get; set; }
        public City City { get; set; }
    }
}
