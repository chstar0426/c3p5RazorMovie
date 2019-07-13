using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataModels;

namespace c3p5RazorMovie.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly DataModels.dbContext _context;

        public IndexModel(DataModels.dbContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get;set; }
        
        #region Searching, Paging 변수
        public SearchingVar searchingVar { get; set; }


        public int TotalCount { get; set; }
        public int PageIndex { get; set; }

        #endregion



        public async Task OnGetAsync()
        {
            ///// Paging 변수 초기화

            int pageIndex = 0;
            int pageSize = 3;


            ////////  Searching 변수 초기화  ///////////////////////////////////////////////////////////////////////////

            string searchField = string.Empty;
            string searchQuery = string.Empty;
            bool searchMode = false;

            if (!String.IsNullOrEmpty(Request.Query["SearchField"]) &&
                 !String.IsNullOrEmpty(Request.Query["SearchQuery"]))
            {
                searchMode = true;
                searchField = Request.Query["SearchField"];
                searchQuery = Request.Query["SearchQuery"];

            }

            searchingVar = new SearchingVar();
            searchingVar.SearchMode = searchMode;
            searchingVar.SearchField = searchField;
            searchingVar.SearchQuery = searchQuery;
            searchingVar.DicField = new Dictionary<string, string>()
            {
                { "Title", "제목" },
                { "Genee", "장르" }
            };

            //////////////////////////////////////////////////////////////////////////////

            var iMovie = (IQueryable<Movie>)_context.Movies.Include(m => m.AttachFiles).AsNoTracking();
            

            if (searchMode)
            {
                switch (searchField)
                {
                    case "Title":
                        iMovie = iMovie.Where(s => s.Title.Contains(searchQuery));
                        break;

                    case "Gener":
                        iMovie = iMovie.Where(s => s.Gener.Contains(searchQuery));
                        break;

                    default:
                        iMovie = iMovie.Where(s => s.Title.Contains(searchQuery));
                        break;
                }

            }


            iMovie = iMovie.OrderByDescending(s => s.ID);



            //[1] 쿼리스트링에 따른 페이지 보여주기
            if (!string.IsNullOrEmpty(Request.Query["Page"]))
            {
                // Page는 보여지는 쪽은 1, 2, 3, ... 코드단에서는 0, 1, 2, ...
                pageIndex = Convert.ToInt32(Request.Query["Page"]) - 1;
                //Response.Cookies.Append("Page", pageIndex.ToString());
            }

            TotalCount = iMovie.Count();
            PageIndex = pageIndex + 1;

            Movie = await iMovie
                .Skip((pageIndex) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
