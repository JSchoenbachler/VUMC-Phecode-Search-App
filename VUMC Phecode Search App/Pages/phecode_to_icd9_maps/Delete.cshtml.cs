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
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;

        public DeleteModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            phecode_to_icd9_map = await _context.phecode_to_icd9_map.FindAsync(id);

            if (phecode_to_icd9_map != null)
            {
                _context.phecode_to_icd9_map.Remove(phecode_to_icd9_map);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
