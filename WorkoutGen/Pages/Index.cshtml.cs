using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using WorkoutGen.Models;

namespace WorkoutGen.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel()
        {
        }

        // Triggered on initial page load
        public void OnGet()
        {
        }
    }
}
