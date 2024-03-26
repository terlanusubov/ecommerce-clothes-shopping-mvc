namespace Comercio.ServiceModels
{
    public class ProductListQueryModel
    {
        public int? CategoryId { get; set; }
        public int Page { get; set; } = 1;

        public int? Take { get; set; }
    }
}
