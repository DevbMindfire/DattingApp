using DattingApp.API.Controllers.Model;
using Microsoft.EntityFrameworkCore;

namespace DattingApp.API.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> option):base(option){}

        public DbSet<Value> Values { get; set; }
        
    }
}