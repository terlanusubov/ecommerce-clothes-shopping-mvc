namespace Comercio.Models
{
    public class Entity<T>
    {
        public T Id { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
