namespace Comercio.Models
{
    public class ProductReview : Entity<int>
    {
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; }
        public DateTime? Accepted { get; set; }
        public int ProductReviewStatusId { get; set; }

        public Product Product { get; set; }
        public User User { get; set; }
    }
}
