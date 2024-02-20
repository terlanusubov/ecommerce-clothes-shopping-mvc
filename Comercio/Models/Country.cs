namespace Comercio.Models
{
    /// <summary>
    /// Countries
    /// </summary>
    public class Country : Entity<short>
    {
        /// <summary>
        /// Name of country
        /// </summary>
        public string Name { get; set; }
    }
}
