namespace Comercio.Models
{
    /// <summary>
    /// Cities
    /// </summary>
    public class City:Entity<short>
    {
        /// <summary>
        /// Name of city
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// This city belongs which country
        /// </summary>
        public short CountryId { get; set; }
    }
}
