using IdentitySample.Models;
using PSIMS.Models.InventoryModel;
using PSIMS.Models.PurchaseModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using PSIMS.Models;
using PSIMS.Models.Locations;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace IdentitySample.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public string FullName { get; set; }
        public string Address_2 { get; set; }
        public string City { get; set; }

        public string State { get; set; }
        public bool Active { get; set; }

        public int? LocationID { get; set; }
        public virtual Location Location { get; set; }




        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }


   }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("PSIMSDB", throwIfV1Schema: false)
        {
            //Database.SetInitializer<DbContext>(null);
        }

        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
           // Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<MenuModels> MenuModels { get; set; }
        public DbSet<MenuUserRoleModel> MenuUserRoleModels { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseItem> PurchaseItems { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<SalesReturn> SalesReturns { get; set; }
        public DbSet<SalesReturnDetail> SalesReturnDetails { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<SalesItem> SalesItems { get; set; }

        public DbSet<LocationStock> LocationStocks { get; set; }
        public System.Data.Entity.DbSet<PSIMS.Models.SalesModel.Customer> Customers { get; set; }


        public System.Data.Entity.DbSet<PSIMS.Models.SalesModel.InvoiceOptionEntry> InvoiceOptionEntries { get; set; }

        public System.Data.Entity.DbSet<PSIMS.Models.PurchaseModel.Country> Countries { get; set; }

        public System.Data.Entity.DbSet<PSIMS.Models.InventoryModel.ProductCategory> ProductCategories { get; set; }

        public System.Data.Entity.DbSet<PSIMS.Models.Locations.Location> Locations { get; set; }

        public System.Data.Entity.DbSet<PSIMS.Models.InventoryModel.StockMovement> StockDistributions { get; set; }

        public System.Data.Entity.DbSet<PSIMS.Models.Finance.Payment> Payments { get; set; }

        public System.Data.Entity.DbSet<PSIMS.Models.Finance.Bank> Banks { get; set; }

        public System.Data.Entity.DbSet<PSIMS.Models.QuotationModel.Quotation> Quotations { get; set; }

        public System.Data.Entity.DbSet<PSIMS.Models.QuotationModel.QuotationItem> QuotationItems { get; set; }
        public DbSet<AccessType> AccessTypes { get; set; }

        public DbSet<PSIMS.Models.Account.RoleAccessType> RoleAccessTypes { get; set; }

        public DbSet<DaliyStock> DaliyStocks { get; set; }
        public DbSet<DiscardStock> DiscardStocks { get; set; }

        public System.Data.Entity.DbSet<PSIMS.Models.Finance.Audit_tray_recipt_master> Audit_tray_recipt_masters { get; set; }
        public System.Data.Entity.DbSet<PSIMS.Models.Finance.Audit_tray_recipt_details> Audit_tray_recipt_details { get; set; }




        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<StockMovement>()
                        .HasRequired(m => m.FromLocation)
                        .WithMany()
                        .HasForeignKey(m => m.FromLocationID)
                        .WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StockMovement>()
                        .HasRequired(m => m.ToLocation)
                        .WithMany()
                        .HasForeignKey(m => m.ToLocationID)
                        .WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<PurchaseItem>()
            //    .HasRequired(d => d.Purchase)
            //    .WithMany()
            //    .HasForeignKey(d => d.PurchaseID)
            //    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<PurchaseItem>()
            //            .HasRequired(m => m.Purchase)
            //            .WithMany()
            //            .HasForeignKey(m => m.PurchaseID)
            //            .WillCascadeOnDelete(true);
            //base.OnModelCreating(modelBuilder);



        }

        public System.Data.Entity.DbSet<PSIMS.Models.QuotationModel.QuotationCategory> QuotationCategories { get; set; }

        public System.Data.Entity.DbSet<PSIMS.Models.PurchaseModel.Clearance> Clearances { get; set; }

        public System.Data.Entity.DbSet<PSIMS.Models.InventoryModel.StockMovementMaster> StockMovementMasters { get; set; }

        public System.Data.Entity.DbSet<PSIMS.Models.InventoryModel.StockMovementDetals> StockMovementDetals { get; set; }

        public System.Data.Entity.DbSet<PSIMS.Models.PurchaseModel.PurchasePackSizeCode> PurchasePackSizeCodes { get; set; }

        public System.Data.Entity.DbSet<PSIMS.Models.Finance.PaymentSettement> PaymentSettements { get; set; }

        public System.Data.Entity.DbSet<PSIMS.Models.Finance.PaymentSettelmentMaster> PaymentSettelmentMasters { get; set; }

        public System.Data.Entity.DbSet<PSIMS.Models.Finance.PaymentSettelmentDetails> PaymentSettelmentDetails { get; set; }

        // public System.Data.Entity.DbSet<PSIMS.ViewModel.StockDistributionVM> StockDistributionVMs { get; set; }









    }
}