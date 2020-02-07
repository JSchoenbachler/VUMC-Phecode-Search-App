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
            //Sets exact match either if the checkbox is checked or it is comma delimited
            ExactFilter = ((searchExact != null && searchExact == "on") || (!String.IsNullOrEmpty(CurrentFilter) && CurrentFilter.Contains(",")));
            //Instantiates anonymous list as variable
            var mapValsQ = from s in _context.phecode_to_icd9_map orderby s.icd9 ascending
                           join phecode_info in phecode_infos on s.phecode equals phecode_info.phecode
                           join icd9Info in icd9Infos on s.icd9 equals icd9Info.icd9_code
                           select new { s, icd9Info, phecode_info };
            //If string is not empty, begins searching parameters
            if (!String.IsNullOrEmpty(searchString))
            {
                //If the string contains a "," create a Hash Set for use in a contains statement as part of the query.
                HashSet<String> searchStrings = new HashSet<String>();
                if (searchString.Contains(","))
                {
                    searchStrings = new HashSet<String>(searchString.Split(","));
                }
                //Searches by Phecode
                if (!String.IsNullOrEmpty(searchBy) && searchBy.Equals("phecode"))
                {
                    if(ExactFilter)
                    {
                        if (searchStrings.Count() > 0)
                        {
                            mapValsQ = from s in _context.phecode_to_icd9_map
                                       where searchStrings.Contains(s.phecode)
                                       join phecode_info in phecode_infos on s.phecode equals phecode_info.phecode
                                       join icd9Info in icd9Infos on s.icd9 equals icd9Info.icd9_code
                                       select new { s, icd9Info, phecode_info };
                        }
                        else
                        {
                            mapValsQ = from s in _context.phecode_to_icd9_map
                                       where s.phecode.Equals(searchString)
                                       join phecode_info in phecode_infos on s.phecode equals phecode_info.phecode
                                       join icd9Info in icd9Infos on s.icd9 equals icd9Info.icd9_code
                                       select new { s, icd9Info, phecode_info };
                        }
                        
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
                //Searches by icd9
                else
                {
                    if (ExactFilter)
                    {
                        if (searchStrings.Count() > 0)
                        {
                            mapValsQ = from s in _context.phecode_to_icd9_map
                                       where searchStrings.Contains(s.icd9)
                                       join phecode_info in phecode_infos on s.phecode equals phecode_info.phecode
                                       join icd9Info in icd9Infos on s.icd9 equals icd9Info.icd9_code
                                       select new { s, icd9Info, phecode_info };
                        }
                        else
                        {
                            mapValsQ = from s in _context.phecode_to_icd9_map
                                       where s.icd9.Equals(searchString)
                                       join phecode_info in phecode_infos on s.phecode equals phecode_info.phecode
                                       join icd9Info in icd9Infos on s.icd9 equals icd9Info.icd9_code
                                       select new { s, icd9Info, phecode_info };
                        }
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
            //Limits results to not overload page on too many results returned.
            mapValsQ = mapValsQ.Take(500);
            var mapValsAnon = await mapValsQ.ToListAsync();
            //Iterates over anonymized list to create list of wrapper class containing all 3 object information
            foreach(var mapValAnon in mapValsAnon)
            {
                //Due to there being duplicate instances of both ICd9 codes and Phecodes, the joins produced multiple duplicates. The hash set contains method checks to ensure no dupes are added.
                if (!stringHashList.Contains(mapValAnon.icd9Info.icd9_string + "---" + mapValAnon.phecode_info.phecode_string))
                {
                    mapVals.Add(new phecode_to_icd9_mapVals(mapValAnon.s, mapValAnon.icd9Info, mapValAnon.phecode_info));
                    stringHashList.Add(mapValAnon.icd9Info.icd9_string + "---" + mapValAnon.phecode_info.phecode_string);
                }
            }
        }
        //Wrapper class to contain all 3 objects
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
