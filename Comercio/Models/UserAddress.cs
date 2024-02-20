namespace Comercio.Models
{
    /// <summary>
    /// Addresses of users
    /// </summary>
    public class UserAddress:Entity<int>
    {
        /// <summary>
        /// Address alias
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Address text
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Phone in order to contact
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Address city
        /// </summary>
        public short CityId { get; set; }

        /// <summary>
        /// Address belongs to which user
        /// </summary>
        public Guid UserId { get; set; }

        public City City { get; set; }
    }
}
