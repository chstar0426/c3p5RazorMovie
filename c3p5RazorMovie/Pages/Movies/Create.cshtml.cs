using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace c3p5RazorMovie.Pages.Movies
{
    public class CreateModel : PageModel
    {
        private readonly DataModels.dbContext _context;
        private readonly IWebHostEnvironment _hostingEnvir;

        public CreateModel(DataModels.dbContext context, IWebHostEnvironment hostingEnvir)
        {
            _context = context;
            _hostingEnvir = hostingEnvir;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Movie Movie { get; set; }
        public string ErrorMessage { get; set; }


        public async Task<IActionResult> OnPostAsync()
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

            var CreateMovie = new Movie();

            if (await TryUpdateModelAsync<Movie>(
                CreateMovie, "movie",
                m => m.Title, m => m.ReleaseDate, m => m.Gener, m => m.Price))

            {
                _context.Movies.Add(CreateMovie);
                await _context.SaveChangesAsync();


                await OnPostUploadAsync(CreateMovie.ID);
                return RedirectToPage("./Index");

            }
            
            return Page();
        }


        public async Task<IActionResult> OnPostAjaxArticleAsync()
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

            var CreateMovie = new Movie();

            if (await TryUpdateModelAsync<Movie>(
                CreateMovie, "movie",
                m => m.Title, m => m.ReleaseDate, m => m.Gener, m => m.Price))

            {
                _context.Movies.Add(CreateMovie);
                await _context.SaveChangesAsync();

                return new OkObjectResult(CreateMovie.ID);
                
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUploadAsync(int Id)
        {
            string[] pathSplit;
            var ajax_file = Request.Form.Files;  //(매개변수로 받음)

            string path_for_Uploaded_Files = _hostingEnvir.WebRootPath + "\\UserFiles\\";

            foreach (var uploaded_file in ajax_file)
            {

                pathSplit = uploaded_file.FileName.Split('\\');
                string uploaded_Filename = pathSplit[pathSplit.Length - 1];
                string new_Filename_on_Server = path_for_Uploaded_Files + "\\" + uploaded_Filename;



                FileInfo finfo = new FileInfo(new_Filename_on_Server);

                if (finfo.Exists)
                {
                    int fIndex = 0;
                    string fExtension = finfo.Extension;
                    string fRealName = uploaded_Filename.Replace(fExtension, "");
                    do
                    {
                        fIndex++;
                        uploaded_Filename = fRealName + "_" + fIndex.ToString() + fExtension;
                        finfo = new FileInfo(Path.Combine(path_for_Uploaded_Files, uploaded_Filename));
                    } while (finfo.Exists);

                    new_Filename_on_Server = System.IO.Path.Combine(
                        path_for_Uploaded_Files, uploaded_Filename);

                }


                using (FileStream stream = new FileStream(new_Filename_on_Server, FileMode.Create))
                {
                    await uploaded_file.CopyToAsync(stream);
                }


                if (Id > 0)
                {
                    _context.AttachFiles.Add(new AttachFile
                    {
                        MovieID = Id,
                        FileName = uploaded_Filename
                    });

                    await _context.SaveChangesAsync();

                }
                
            }

            return Page();
            
        }

       
    }
}