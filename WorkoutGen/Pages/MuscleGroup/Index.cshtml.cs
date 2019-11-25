using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using WorkoutGen.Data.Services.MuscleGroup;
using WorkoutGen.Data.Session;

namespace WorkoutGen.Pages.MuscleGroup
{
    public class IndexModel : PageModel
    {
        private readonly IMuscleGroupService _muscleGroupDb;

        // Inject muscle group db context
        public IndexModel(IMuscleGroupService muscleGroupDb)
        {
            _muscleGroupDb = muscleGroupDb;
        }

        [BindProperty]
        public int[] MuscleGroupIds { get; set; }
        public SelectList Options_MuscleGroups { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            GetSessionProperties();

            // Get muscle group list from DB and bind it to the drop down property
            await SetMuscleGroupsDropDown();
            return Page();
        }

        public void OnPost()
        {
        }

        public void GetSessionProperties() {

            // Attempt to get session variable
            MuscleGroupIds = HttpContext.Session.Get<int[]>("MuscleGroupIds");

            // If session does not yet exist, create it and set default
            if (MuscleGroupIds == null) {
                MuscleGroupIds = new int[0];
                HttpContext.Session.Set<int[]>("MuscleGroupIds", MuscleGroupIds);
            }
        }

        // Gets all the muscle group records and binds it to the select list object
        // The select list object gets binded to the select 2 drop down
        public async Task SetMuscleGroupsDropDown()
        {
            var muscleGroups = await _muscleGroupDb.GetMuscleGroups();
            Options_MuscleGroups = new SelectList(muscleGroups, "Id", "Name");
        }

        // Updates the session object for selected muscle groups
        public void OnPostUpdateMuscleGroupSession(int[] muscleGroupIds)
        {
            HttpContext.Session.Set("MuscleGroupIds", muscleGroupIds);
        }
    }
}