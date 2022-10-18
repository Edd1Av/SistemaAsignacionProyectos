using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WEB.Models;

namespace WEB.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
            this.ChangeTracker.LazyLoadingEnabled = true;
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();

            modelBuilder.Entity<Colaborador>().HasIndex(x => x.CURP).IsUnique();
            modelBuilder.Entity<Colaborador>().HasIndex(x => x.Id_Odoo).IsUnique();

            modelBuilder.Entity<Proyecto>().HasIndex(x => x.Clave).IsUnique();

            modelBuilder.Entity<Asignacion>().HasIndex(x=>x.IdColaborador).IsUnique();

        }

        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Asignacion> Asignacion { get; set; }
        public DbSet<AsignacionReal> AsignacionReal { get; set; }
        public DbSet<DistribucionReal> DistribucionReal { get; set; }
        public DbSet<Distribucion> Distribucion { get; set; }
    }
}