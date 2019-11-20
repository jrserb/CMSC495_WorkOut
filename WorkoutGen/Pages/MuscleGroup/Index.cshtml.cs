using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WorkoutGen.Data.Services.MuscleGroup;
using WorkoutGen.Data.Session;

namespace WorkoutGen.Pages.MuscleGroup
{
    public class IndexModel : PageModel
    {
        private readonly IMuscleGroupService _muscleGroupDb;

        public IndexModel(IMuscleGroupService muscleGroupDb)
        {
            _muscleGroupDb = muscleGroupDb;
        }

        [BindProperty]
        public int[] muscleGroupIds { get; set; }

        public SelectList Options_MuscleGroups { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

            muscleGroupIds = HttpContext.Session.Get<int[]>("muscle_groups");

            await SetMuscleGroupsDropDown();
            return Page();
        }

        public void OnPost()
        {        
        }
        
        // Gets all the muscle group records and binds it to the select list object
        // The select list object gets binded to the select 2 drop down
        public async Task SetMuscleGroupsDropDown()
        {
            var muscleGroups = await _muscleGroupDb.GetMuscleGroups();
            Options_MuscleGroups = new SelectList(muscleGroups, "Id", "Name");
        }
    }
}