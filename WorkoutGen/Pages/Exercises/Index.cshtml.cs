using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkoutGen.Models;
using WorkoutGen.Data;
using WorkoutGen.Data.Services.Exercise;
using WorkoutGen.Data.Services.Equipment;
using System.Threading.Tasks;

namespace WorkoutGen.Pages.Exercises
{
    public class IndexModel : PageModel
    {
        private readonly IExerciseService _exerciseDb;
        private readonly IEquipmentService _equipmentDb;

        public IndexModel(IExerciseService exerciseDb, 
            IEquipmentService equipmentDb)
        {
            _exerciseDb = exerciseDb;
            _equipmentDb = equipmentDb;
        }

        [BindProperty]
        public IEnumerable<Exercise> Exercises { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Exercises = await _exerciseDb.GetExercises();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int[] equipmentIds)
        {
            int[] alternateEquipmentIds = { };
            bool hasRequirement = true;

            // This list will hold the final list of valid exercise ids based on if the user had the required muscle groups and equipment selected
            List<int> validExerciseIds = new List<int>();

            // First thing to do is select all the distinct exercise ids related to the muscle groups the user selected
            int[] exerciseIds = await _exerciseDb.GetExerciseIdsFromEquipment(equipmentIds);

            // Loop each exercise id
            for (int i = 0; i < exerciseIds.Count(); i++)
            {

                // Get the equipment objects related to the exercise
                // This is essentially the requirement. The user has to have selected all of these to get the exercise
                var requiredEquipment = await _equipmentDb.GetEquipmentFromExercise(exerciseIds[i]);


                // Loop the equipment objects and make sure user has selected them all
                hasRequirement = true;
                foreach (var objEquipment in requiredEquipment)
                {
                    // If they don't have a required equipment id then we check the alternate table for a matching equipment id
                    if (!equipmentIds.Contains(objEquipment.Id))
                    {
                        // Get the alternate equipment ids where the exercise equipment id matches
                        alternateEquipmentIds = await _equipmentDb.GetAlternateEquipmentIdsFromEquipment(objEquipment.Id);

                        hasRequirement = false;

                        // Loop the alternate equipment ids
                        foreach (int alternateEquipmentId in alternateEquipmentIds)
                        {
                            // If an alternate equipment id matches with what the user has then they get the exercise
                            if (equipmentIds.Contains(alternateEquipmentId))
                            {
                                hasRequirement = true;
                            }
                        }

                        if (hasRequirement == false) break;
                    }
                }

                // If both arrays contain the same ids then the user meets the requirements
                // This exercise should be added to the valid list
                if (hasRequirement)
                {
                    validExerciseIds.Add(exerciseIds[i]);
                }
            }

            // We now have the valid set of exercise ids
            // Get the exercise records and return them
            Exercises = await _exerciseDb.GetExercises(validExerciseIds.ToArray());

            return Page();
        }
    }
}
