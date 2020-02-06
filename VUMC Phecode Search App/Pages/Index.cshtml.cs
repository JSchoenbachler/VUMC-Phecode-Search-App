using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VUMC_Phecode_Search_App.Models;

namespace VUMC_Phecode_Search_App.Pages
{
    public class IndexModel : PageModel
    {
        public String CurrentFilter { get; set; }
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }
        public IList<phecode_to_icd9_mapVals> mapVals { get; set; }
        public async Task OnGetAsync(string searchString)
        {
            mapVals = new List<phecode_to_icd9_mapVals>();
            IList<phecode_info> phecode_infos = await _context.phecode_info.ToListAsync();
            CurrentFilter = searchString;
            var mapValsQ = from s in _context.phecode_to_icd9_map
                                                                        join phecode_info in phecode_infos on s.phecode equals phecode_info.phecode
                                                                        select new { s, phecode_info };
            if (!String.IsNullOrEmpty(searchString))
            {
                mapValsQ = from s in _context.phecode_to_icd9_map
                                                               where s.icd9.Contains(searchString)
                                                               join phecode_info in phecode_infos on s.phecode equals phecode_info.phecode
                                                               select new { s, phecode_info };
            }
            var mapValsAnon = await mapValsQ.ToListAsync();
            foreach(var mapValAnon in mapValsAnon)
            {
                mapVals.Add(new phecode_to_icd9_mapVals(mapValAnon.s,mapValAnon.phecode_info));
            }
        }
        public class phecode_to_icd9_mapVals
        {
            public phecode_to_icd9_map codeMap { get; set; }
            public phecode_info pc { get; set; }

            public phecode_to_icd9_mapVals(phecode_to_icd9_map cm, phecode_info p)
            {
                codeMap = cm;
                pc = p;
            }
        }
    }
}
