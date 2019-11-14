using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WorkoutGen.Data.Services.MuscleGroup;

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
        [MustHaveItems(ErrorMessage = "You must select an option")]
        public int[] muscleGroupIds { get; set; }

        public SelectList Options_MuscleGroups { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await SetMuscleGroupsDropDown();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int[] muscleGroupIds)
        {        
            if (!ModelState.IsValid) {

                await SetMuscleGroupsDropDown();
                return Page();
            }

            TempData["muscleGroupIds"] = muscleGroupIds;
            return RedirectToPage("/Equipment/Index");
        }

        // Gets all the muscle group records and binds it to the select list object
        // The select list object gets binded to the select 2 drop down
        public async Task SetMuscleGroupsDropDown()
        {
            var muscleGroups = await _muscleGroupDb.GetMuscleGroups();
            Options_MuscleGroups = new SelectList(muscleGroups, "Id", "Name");
        }


        // Custom validation class
        // int array must have a value
        public class MustHaveItems : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                var list = value as int[];
                if (list.Length > 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}