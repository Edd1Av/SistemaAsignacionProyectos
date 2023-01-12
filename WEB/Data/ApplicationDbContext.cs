using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WEB.Models;

namespace WEB.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();

            modelBuilder.Entity<Colaborador>().HasIndex(x => x.CURP).IsUnique();
            modelBuilder.Entity<Colaborador>().HasIndex(x => x.Id_Odoo).IsUnique();

            modelBuilder.Entity<Proyecto>().HasIndex(x => x.Clave).IsUnique();

            var hasher = new PasswordHasher<IdentityUser>();

            var user = new ApplicationUser()
            {
                Id = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                //IdColaborador = 1,
                Email = "admin@admin.com",
                EmailConfirmed = true,
                NormalizedEmail = "ADMIN@ADMIN.COM",
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                PasswordHash = hasher.HashPassword(null, "Pa$word1")
            };

            var rolAdmin = new IdentityRole()
            {
                Id = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                Name = "Administrador",
                NormalizedName = "ADMINISTRADOR"
            };

            var rolDesarrollador = new IdentityRole()
            {
                Id = "3c6e284e-4b1e-557f-97af-594d67fd8321",
                Name = "Desarrollador",
                NormalizedName = "DESARROLLADOR"
            };

            //modelBuilder.Entity<Colaborador>().HasData(userData);
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole[] { rolAdmin, rolDesarrollador });
            modelBuilder.Entity<ApplicationUser>().HasData(user);
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
            });
        }

        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Asignacion> Asignacion { get; set; }
        public DbSet<AsignacionReal> AsignacionReal { get; set; }
        public DbSet<DistribucionReal> DistribucionReal { get; set; }
        public DbSet<Distribucion> Distribucion { get; set; }
        public DbSet<Log> Logger { get; set; }
    }
}