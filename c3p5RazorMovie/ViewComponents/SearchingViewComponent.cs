using DataModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace c3p5RazorMovie.ViewComponents
{
    public class SearchingViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(SearchingVar searchingVar)
        {
            return View(searchingVar);
        }
    }
}
