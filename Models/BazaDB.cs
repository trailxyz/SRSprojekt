using System;
using System.Collections.Generic;
using MySql.Data.EntityFramework;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ComponentModel;
using SRSprojekt.Models;

namespace SRSprojekt.Models
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class BazaDB : DbContext
    {
        public DbSet<Klub> KlubBaza { get; set; }
        public DbSet<Rezervator> RezervatorBaza { get; set; }
        public DbSet<oKlub> ovlastenaosobakluba { get; set; }
        public DbSet<Stolovi> StoloviBaza { get; set; }
        public DbSet<ovlast> OvlastBaza { get; set; }
        public DbSet<Korisnik> KorisnikBaza { get; set; }
        public DbSet<rezervacija> rezervacijaBaza { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
          
            modelBuilder.Entity<Stolovi>()
                .HasOptional(s => s.aktivniR)
                .WithMany(r => r.Stolovi)
                .HasForeignKey(s => s.sifraR);

            modelBuilder.Entity<rezervacija>()
           .HasRequired(r => r.brStola)
           .WithMany()
           .HasForeignKey(r => r.ID_stola)
           .WillCascadeOnDelete(false);

            // Ensure proper configuration of entity mappings if necessary
            modelBuilder.Entity<Stolovi>()
                .Property(s => s.Sifra)
                .HasColumnName("sifra");

            modelBuilder.Entity<rezervacija>()
                .Property(r => r.ID_rezervacije)
                .HasColumnName("id_rezervacije");

            modelBuilder.Entity<rezervacija>()
                .Property(r => r.ID_stola)
                .HasColumnName("idStola");

            modelBuilder.Entity<rezervacija>()
                .Property(r => r.Zauzetost)
                .HasColumnName("zauz");

            modelBuilder.Entity<rezervacija>()
                .Property(r => r.DatVri)
                .HasColumnName("datVri");

            base.OnModelCreating(modelBuilder);

        }

    }
}