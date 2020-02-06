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
        public String SearchFilter { get; set; }
        public Boolean ExactFilter { get; set; }
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }
        public IList<phecode_to_icd9_mapVals> mapVals { get; set; }
        public async Task OnGetAsync(string searchBy, string searchString, String searchExact)
        {
            HashSet<String> stringHashList = new HashSet<String>();
            mapVals = new List<phecode_to_icd9_mapVals>();
            IList<phecode_info> phecode_infos = await _context.phecode_info.ToListAsync();
            IList<icd9Info> icd9Infos = await _context.icd9Info.ToListAsync();
            CurrentFilter = searchString;
            SearchFilter = searchBy;
            ExactFilter = (searchExact != null && searchExact == "on");
            var mapValsQ = from s in _context.phecode_to_icd9_map orderby s.icd9 ascending
                           join phecode_info in phecode_infos on s.phecode equals phecode_info.phecode
                           join icd9Info in icd9Infos on s.icd9 equals icd9Info.icd9_code
                           select new { s, icd9Info, phecode_info };
            if (!String.IsNullOrEmpty(searchString))
            {

                if (!String.IsNullOrEmpty(searchBy) && searchBy.Equals("phecode"))
                {
                    if(ExactFilter)
                    {
                        mapValsQ = from s in _context.phecode_to_icd9_map
                                   where s.phecode.Equals(searchString)
                                   join phecode_info in phecode_infos on s.phecode equals phecode_info.phecode
                                   join icd9Info in icd9Infos on s.icd9 equals icd9Info.icd9_code
                                   select new { s, icd9Info, phecode_info };
                    }
                    else
                    {
                        mapValsQ = from s in _context.phecode_to_icd9_map
                                   where s.phecode.Contains(searchString)
                                   join phecode_info in phecode_infos on s.phecode equals phecode_info.phecode
                                   join icd9Info in icd9Infos on s.icd9 equals icd9Info.icd9_code
                                   select new { s, icd9Info, phecode_info };
                    }
                    
                }
                else
                {
                    if (ExactFilter != null && ExactFilter)
                    {
                        mapValsQ = from s in _context.phecode_to_icd9_map
                                   where s.icd9.Equals(searchString)
                                   join phecode_info in phecode_infos on s.phecode equals phecode_info.phecode
                                   join icd9Info in icd9Infos on s.icd9 equals icd9Info.icd9_code
                                   select new { s, icd9Info, phecode_info };
                    }
                    else
                    {
                        mapValsQ = from s in _context.phecode_to_icd9_map
                                   where s.icd9.Contains(searchString)
                                   join phecode_info in phecode_infos on s.phecode equals phecode_info.phecode
                                   join icd9Info in icd9Infos on s.icd9 equals icd9Info.icd9_code
                                   select new { s, icd9Info, phecode_info };
                    }
                }
            }
            mapValsQ = mapValsQ.Take(500);
            var mapValsAnon = await mapValsQ.ToListAsync();
            foreach(var mapValAnon in mapValsAnon)
            {
                if (!stringHashList.Contains(mapValAnon.icd9Info.icd9_string + "---" + mapValAnon.phecode_info.phecode_string))
                {
                    mapVals.Add(new phecode_to_icd9_mapVals(mapValAnon.s, mapValAnon.icd9Info, mapValAnon.phecode_info));
                    stringHashList.Add(mapValAnon.icd9Info.icd9_string + "---" + mapValAnon.phecode_info.phecode_string);
                }
            }
        }
        public class phecode_to_icd9_mapVals
        {
            public phecode_to_icd9_map codeMap { get; set; }
            public icd9Info icd9 { get; set; }
            public phecode_info pc { get; set; }

            public phecode_to_icd9_mapVals(phecode_to_icd9_map cm, icd9Info i, phecode_info p)
            {
                codeMap = cm;
                icd9 = i;
                pc = p;
            }
        }
    }
}
