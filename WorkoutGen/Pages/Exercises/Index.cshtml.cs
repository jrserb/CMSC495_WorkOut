using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkoutGen.Models;

namespace WorkoutGen.Pages.Exercises
{
    public class IndexModel : PageModel
    {
        private readonly WorkoutGenContext _context;

        public IndexModel(WorkoutGenContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<Exercise> Exercises { get; set; }

        public void OnGet()
        {
            Exercises = _context.Exercise.ToList();
        }

        // This is the code to get the correct exercises that match the users choices
        public void OnPost(int[] muscle_groups, int[] equipment)
        {
           
            // This list will hold the valid exercise ids based on if the user had the required muscle groups selected
            List<int> validMuscleGroupExerciseIds = new List<int>();

            // This list will hold the final list of valid exercise ids based on if the user had the required muscle groups and equipment selected
            List<int> validExerciseIds = new List<int>();


            // First thing to do is select all the exercise ids related to the muscle groups the user selected
            List<int> muscleGroupExerciseIds = _context.ExerciseMuscleGroup
                                                .Where( e => muscle_groups.Contains(e.MuscleGroupId) )
                                                .Select(r => r.ExerciseId)
                                                .ToList();

            int[] requiredIds = { };

            // Now that we have the potential exercise ids, we need to make sure the user selected all the required muscle groups for them
            // Loop each exercise id and figure out the requirements
            for (int index = 0; index < muscleGroupExerciseIds.Count(); index++ ) {

                // Get the muscle group ids required for the exercise
                // This is essentially the requirement. The user has to have selected all of these to get the exercise
                requiredIds = _context.ExerciseMuscleGroup
                                .Where(c => c.ExerciseId == muscleGroupExerciseIds[index])
                                .OrderBy( e => e.MuscleGroupId )
                                .Select(e => e.MuscleGroupId)
                                .ToArray();

                // If both arrays contain the same ids then the user meets the requirements
                // This exercise should be added to the valid list
                if (requiredIds.SequenceEqual(muscle_groups)) {
                    validMuscleGroupExerciseIds.Add(muscleGroupExerciseIds[index]);
                }
            }

            // If there are valid exercise ids then we continue on to the equipment
            if (validMuscleGroupExerciseIds.Count > 0)
            {
                for (int index = 0; index < validMuscleGroupExerciseIds.Count(); index++)
                {

                    // Get the equipment ids required for the exercise
                    // This is essentially the requirement. The user has to have selected all of these to get the exercise
                    requiredIds = _context.ExerciseEquipment
                                    .Where(c => c.ExerciseId == validMuscleGroupExerciseIds[index])
                                    .OrderBy(e => e.EquipmentId)
                                    .Select(e => e.EquipmentId)
                                    .ToArray();

                    // If both arrays contain the same ids then the user meets the requirements
                    // This exercise should be added to the valid list
                    if (requiredIds.SequenceEqual(equipment))
                    {
                        validExerciseIds.Add(validMuscleGroupExerciseIds[index]);
                    }
                }
            }

            // We now have the valid set of exercise ids
            // Get the exercise records and return them
            Exercises = _context.Exercise
                        .Where(m => validExerciseIds.Contains(m.Id))
                        .ToList();
        }
    }
}
