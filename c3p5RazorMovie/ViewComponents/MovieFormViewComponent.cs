using DataModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace c3p5RazorMovie.ViewComponents
{
    public class MovieFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(MovieForm frm)
        {
            return View(frm);
        }

    }
}
