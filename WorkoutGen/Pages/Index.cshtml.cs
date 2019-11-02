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
        private readonly WorkoutGenContext _context;

        public IndexModel(WorkoutGenContext context)
        {
            _context = context;
        }

        // Binding properties allows them to be passed to the razor page model
        // So we can access it during requests and responses
        [BindProperty]
        public List<SelectListItem> Options_MuscleGroups { get; set; }
        public List<SelectListItem> Options_Equipment { get; set; }


        // Triggered on initial page load
        public void OnGet()
        {
            // Easy way to render the drop down when page first loads
            // Populate binded property
            Options_MuscleGroups = _context.MuscleGroup
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name + " (" + a.Id + ")" })
                .ToList();

            Options_Equipment = _context.Equipment
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name + " (" + a.Id + ")" })
                .ToList();
        }

        // Returns list of equipment based on muscle group ids
        public JsonResult OnPostUpdateEquipment(string muscleGroupIds) {

            int[] ids = Array.ConvertAll(muscleGroupIds.Split(","), int.Parse);

            // First get distinct list of exercise ids associated with the muscle group ids
            List<int> exerciseIds = _context.ExerciseMuscleGroup
                                    .Where(m => ids.Contains(m.MuscleGroupId))
                                    .Select(r => r.ExerciseId)
                                    .Distinct()
                                    .ToList();


            // Then get the equipment that exists for those exercises
            IQueryable<Equipment> equipment = from eq in _context.Equipment
                                              join ee in _context.ExerciseEquipment on eq.Id equals ee.EquipmentId
                                              where exerciseIds.Contains(ee.ExerciseId)
                                              select eq;

            // Return json list of equipment items
            return new JsonResult(equipment.ToList());
        }
    }
}
