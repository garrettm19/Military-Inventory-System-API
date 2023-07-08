using Microsoft.EntityFrameworkCore;
using Military_Inventory_System_API.Models;

namespace Military_Inventory_System_API.Context
{
    public class MilitaryInventorySystemContext : DbContext
    {
        public MilitaryInventorySystemContext(DbContextOptions<MilitaryInventorySystemContext> options) 
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<AdminCredential> AdminCredentials { get; set; }
        public DbSet<WeaponManagementUser> WeaponManagementUsers { get; set; }
    }
}
