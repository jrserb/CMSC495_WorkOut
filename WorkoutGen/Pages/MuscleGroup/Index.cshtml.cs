using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkoutGen.Data.Services;
using WorkoutGen.Models;

namespace WorkoutGen.Pages.MuscleGroup
{
    public class IndexModel : PageModel
    {
        private readonly WorkoutGenContext _context;

        public IndexModel(WorkoutGenContext context)
        {
            _context = context;
        }

        // Binding properties allows them to be passed to the razor page model
        // So we can access it during requests and responses
        [BindProperty]
        public List<SelectListItem> Options_MuscleGroups { get; set; }

        public async void OnGet()
        {
            // Easy way to render the drop down when page first loads
            // Populate binded property
            Options_MuscleGroups = await _context.MuscleGroup
                                        .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name + " (" + a.Id + ")" })
                                        .ToListAsync();
        }
    }
}