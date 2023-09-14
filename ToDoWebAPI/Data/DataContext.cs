using Microsoft.EntityFrameworkCore;

namespace ToDoWebAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"
                                          Server=.\SQLEXPRESS;
                                          Database=ToDoDB;
                                          Trusted_Connection=True;
                                          TrustServerCertificate=True;");
        }
    }
}
