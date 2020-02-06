using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using VUMC_Phecode_Search_App.Models;

namespace VUMC_Phecode_Search_App.Pages.phecode_to_icd9_maps
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public phecode_to_icd9_map phecode_to_icd9_map { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.phecode_to_icd9_map.Add(phecode_to_icd9_map);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}