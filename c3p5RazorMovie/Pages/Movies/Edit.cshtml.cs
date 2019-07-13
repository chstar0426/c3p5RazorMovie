using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataModels;
using Microsoft.AspNetCore.Http;

namespace c3p5RazorMovie.Pages.Movies
{
    public class EditModel : PageModel
    {
        private readonly DataModels.dbContext _context;

        public EditModel(DataModels.dbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Movie Movie { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Movie = await _context.Movies.FirstOrDefaultAsync(m => m.ID == id);

            if (Movie == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            var formSecurity = Request.Form["SecurityText"].ToString();
            var sessionSecurity = HttpContext.Session.GetString("SecurityText");


            if (formSecurity != sessionSecurity)
            {

                ErrorMessage = "(시크릿트 문자를 확인하세요)";
                return Page();
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var MovieToUpdate = await _context.Movies.FindAsync(id);


            if (await TryUpdateModelAsync<Movie>(
                MovieToUpdate, "movie",
                m => m.Title, m => m.ReleaseDate, m => m.Gener, m => m.Price))
            {
                await _context.SaveChangesAsync();
                return Redirect("./Index" + Request.Query["Path"].ToString());

            }

            return Page();

        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.ID == id);
        }
    }
}
