using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APP_TEST_ALTIORACORP.Models;
using Microsoft.EntityFrameworkCore;

namespace APP_TEST_ALTIORACORP.Data
{
    public class AltioraContext : DbContext
    {
        public AltioraContext(DbContextOptions<AltioraContext> options) : base(options)
        {
        }

        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<Pedidos> Pedidos { get; set; }
        public DbSet<Productos> Productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pedidos>().ToTable("Pedidos").HasKey(sc => new { sc.ID });
            modelBuilder.Entity<Pedidos>().ToTable("Pedidos")
                .HasOne(p => p.Clientes)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.CLIENTE);

            modelBuilder.Entity<Pedidos>().ToTable("Pedidos")
                .HasOne(p => p.Productos)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.IDPRODUCTO);

            modelBuilder.Entity<Clientes>().ToTable("Clientes");
            
            modelBuilder.Entity<Productos>().ToTable("Productos");
        }

    }
}
