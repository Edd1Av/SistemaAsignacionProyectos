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

            modelBuilder.Entity<Colaborador>().HasAlternateKey(x => x.CURP);
            modelBuilder.Entity<Colaborador>().HasAlternateKey(x => x.Id_Odoo);

            modelBuilder.Entity<Proyecto>().HasIndex(x => x.Clave).IsUnique();



        }

        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Asignacion> Asignacion { get; set; }

        public DbSet<Distribucion> Distribucion { get; set; }
    }
}