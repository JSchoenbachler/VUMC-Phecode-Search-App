using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VUMC_Phecode_Search_App.Models;

    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<VUMC_Phecode_Search_App.Models.icd9Info> icd9Info { get; set; }
        public DbSet<VUMC_Phecode_Search_App.Models.phecode_info> phecode_info { get; set; }
        public DbSet<VUMC_Phecode_Search_App.Models.phecode_to_icd9_map> phecode_to_icd9_map { get; set; }
}
