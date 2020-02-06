using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VUMC_Phecode_Search_App.Models;

namespace VUMC_Phecode_Search_App.Pages.phecode_to_icd9_maps
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<phecode_to_icd9_map> phecode_to_icd9_map { get;set; }

        public async Task OnGetAsync()
        {
            phecode_to_icd9_map = await _context.phecode_to_icd9_map.ToListAsync();
        }
    }
}
