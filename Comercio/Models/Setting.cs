namespace Comercio.Models
{
    public class Setting:Entity<int>
    {
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Twitter { get; set; }
        public string Pinterest { get; set; }
        public string Logo { get; set; }
        public string PhoneNumber { get; set; }
    }
}
