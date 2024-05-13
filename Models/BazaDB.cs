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
    

    }
}