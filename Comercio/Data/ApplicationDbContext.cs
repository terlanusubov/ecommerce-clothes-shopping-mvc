using Comercio.Models;
using Microsoft.EntityFrameworkCore;

namespace Comercio.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<GenderType> GenderTypes { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<OptionGroup> OptionGroups { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOption> ProductsOption { get; set; }
        public DbSet<ProductPhoto> ProductPhotos { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<TrendyProduct> TrendyProducts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }

        public ApplicationDbContext(DbContextOptions options):base(options)
        {

        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region User
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(p=>p.Name).IsRequired().HasMaxLength(100);
              
                entity.Property(p=>p.Surname).IsRequired().HasMaxLength(100);
                
                entity.Property(p=>p.Email).IsRequired().HasMaxLength(100);
                
                entity.Property(p=>p.ProfilePicture).HasMaxLength(1000);
                
                entity.Property(p=>p.Phone).IsRequired().HasMaxLength(15);
                
                entity.Property(p=>p.PasswordHash).IsRequired().HasMaxLength(32);
                
                entity.Property(p=>p.Salt).IsRequired();

                entity.Property(p=>p.EmailConfirmed).IsRequired().HasDefaultValue(false);

                entity.Property(p => p.UserRoleId).IsRequired();
            });
            #endregion

            #region User address
            modelBuilder.Entity<UserAddress>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
              
                entity.Property(p => p.Address).IsRequired().HasMaxLength(500);
                
                entity.Property(p => p.Phone).IsRequired().HasMaxLength(15);
                
                entity.Property(p => p.CityId).IsRequired();
                
                entity.Property(p => p.UserId).IsRequired();
            });
            #endregion

            #region User role
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            });
            #endregion

            #region Trendy products
            modelBuilder.Entity<TrendyProduct>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(200);

                entity.Property(p => p.Slug).IsRequired().HasMaxLength(500);

                entity.Property(p => p.ImageURL).IsRequired().HasMaxLength(1000);

                entity.Property(p => p.Price).IsRequired();

                entity.Property(p => p.ProductId).IsRequired();
            });
            #endregion

            #region Subscriber
            modelBuilder.Entity<Subscriber>(entity =>
            {
                entity.Property(p => p.Email).IsRequired().HasMaxLength(200);
            });
            #endregion

            #region Slider
            modelBuilder.Entity<Slider>(entity =>
            {
                entity.Property(p => p.Title).IsRequired().HasMaxLength(200);
                
                entity.Property(p => p.Slogan).IsRequired().HasMaxLength(500);
                
                entity.Property(p => p.Link).IsRequired();
              
                entity.Property(p => p.BackgrounImageURL).IsRequired().HasMaxLength(1000);
            });
            #endregion

            #region Setting
            modelBuilder.Entity<Setting>(entity =>
            {
                entity.Property(p => p.Facebook).HasMaxLength(1000);

                entity.Property(p => p.Instagram).HasMaxLength(1000);

                entity.Property(p => p.Twitter).HasMaxLength(1000);

                entity.Property(p => p.Pinterest).HasMaxLength(1000);
               
                entity.Property(p => p.PhoneNumber).HasMaxLength(15);
                
                entity.Property(p => p.Logo).IsRequired().HasMaxLength(1000);
            });
            #endregion

            #region ProductVariant
            modelBuilder.Entity<ProductVariant>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(500);
            });
            #endregion

            #region Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.ProductVariantId).IsRequired();
               
                entity.Property(p => p.Name).IsRequired().HasMaxLength(1000);
                
                entity.Property(p => p.Slug).IsRequired().HasMaxLength(1000);
                
                entity.Property(p => p.Barcode).IsRequired().HasMaxLength(50);
                
                entity.Property(p => p.CategoryId).IsRequired();
                
                entity.Property(p => p.GenderTypeId).IsRequired();
                
                entity.Property(p => p.Description).IsRequired().HasMaxLength(1000);
                
                entity.Property(p => p.SellAmount).IsRequired();
                
                entity.Property(p => p.BuyAmount).IsRequired();
                
                entity.Property(p => p.BuyLimit).IsRequired().HasDefaultValue(10);
                
                entity.Property(p => p.Quantity).IsRequired();
                
                entity.Property(p => p.InStock).IsRequired();
                
                entity.Property(p => p.HasShipping).IsRequired();
            });
            #endregion

            #region Product photo 
            modelBuilder.Entity<ProductPhoto>(entity =>
            {
                entity.Property(p => p.ImageURL).IsRequired().HasMaxLength(1000);
                
                entity.Property(p => p.ProductId).IsRequired();
            });
            #endregion

            #region Product review
            modelBuilder.Entity<ProductReview>(entity =>
            {
                entity.Property(p => p.ProductId).IsRequired();
                
                entity.Property(p => p.UserId).IsRequired();

                entity.Property(p => p.Text).IsRequired().HasMaxLength(500);
               
                entity.Property(p => p.ProductReviewStatusId).IsRequired();
            });
            #endregion

            #region Product option
            modelBuilder.Entity<ProductOption>(entity =>
            {
                entity.Property(p => p.ProductId).IsRequired();
                entity.Property(p => p.OptionId).IsRequired();
            });
            #endregion

            #region Option
            modelBuilder.Entity<Option>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
              
                entity.Property(p => p.OptionGroupId).IsRequired();
            });
            #endregion

            #region Option group
            modelBuilder.Entity<OptionGroup>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            });
            #endregion

            #region Gender type
            modelBuilder.Entity<GenderType>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(50);
            });
            #endregion

            #region City
            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
              
                entity.Property(p => p.CountryId).IsRequired();
            });
            #endregion

            #region Country
            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            });
            #endregion

            #region Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
               
                entity.Property(p => p.BackgroundImageURL).HasMaxLength(1000);
              
                entity.Property(p => p.Slogan).HasMaxLength(1000);

                entity.Property(p => p.IsMainPage).IsRequired().HasDefaultValue(false);
            });
            #endregion
        }
    }
}
