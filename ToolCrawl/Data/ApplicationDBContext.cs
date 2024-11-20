using ToolCrawl.Models.Grab.Feedback;
using ToolCrawl.Models.Grab.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ToolCrawl.Data
{
    public class ApplicationDBContext : DbContext
    {
        //public ApplicationDBContext()
        //{

        //}

        public DbSet<Preparing> Preparings { get; set; }
        public DbSet<Ready> Readys { get; set; }
        public DbSet<Upcoming> Upcomings { get; set; }
        public DbSet<History> Historys { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }


        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        // Cấu hình cho DbContext
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("workstation id=DigiFnb.mssql.somee.com;packet size=4096;user id=ducluong710_SQLLogin_1;pwd=dspaz2kvaa;data source=DigiFnb.mssql.somee.com;persist security info=False;initial catalog=DigiFnb;TrustServerCertificate=True");
        //    }
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", true, true);
        //    IConfigurationRoot configuration = builder.Build();
        //    optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //=> optionsBuilder.UseSqlServer(GetConnectionString());

        //private string GetConnectionString()
        //{
        //    IConfiguration config = new ConfigurationBuilder()
        //     .SetBasePath(Directory.GetCurrentDirectory())
        //    .AddJsonFile("appsettings.json", true, true)
        //    .Build();
        //    var strConn = config["ConnectionStrings:DefaultConnection"];

        //    return strConn;
        //}
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}
    } 
}
