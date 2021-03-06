﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VUMC_Phecode_Search_App.Models;

namespace VUMC_Phecode_Search_App.Pages.phecode_to_icd9_maps
{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;

        public DetailsModel(AppDbContext context)
        {
            _context = context;
        }

        public phecode_to_icd9_map phecode_to_icd9_map { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            phecode_to_icd9_map = await _context.phecode_to_icd9_map.FirstOrDefaultAsync(m => m.Id == id);

            if (phecode_to_icd9_map == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
