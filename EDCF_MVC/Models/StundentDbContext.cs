using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace EDCF_MVC.Models
{
    public class StundentDbContext : DbContext
    {
        public StundentDbContext(DbContextOptions option) : base(option)
        {

        }

        public DbSet<Student> Students { get; set; } 
    }
}
