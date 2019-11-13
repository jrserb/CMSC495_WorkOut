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
        public void OnPost(int[] equipment)
        {

            int[] alternateEquipmentIds = { };
            bool hasRequirement = true;

            // This list will hold the final list of valid exercise ids based on if the user had the required muscle groups and equipment selected
            List<int> validExerciseIds = new List<int>();

            // First thing to do is select all the distinct exercise ids related to the muscle groups the user selected
            //List<int> exerciseIds = _context.ExerciseMuscleGroup
            //                                    .Where( e => muscle_groups.Contains(e.MuscleGroupId) )
            //                                    .Select(r => r.ExerciseId)
            //                                    .Distinct()
            //                                    .ToList();     

            // First thing to do is select all the distinct exercise ids related to the muscle groups the user selected
            List<int> exerciseIds = _context.ExerciseEquipment
                                    .Where(e => equipment.Contains(e.EquipmentId))
                                    .Select(r => r.ExerciseId)
                                    .Distinct()
                                    .ToList();

            // We don't need this logic if the user does not need to have ALL the muscle groups selected for an exercise
            #region Muscle Group Validation code - not sure if needed

            // This list will hold the valid exercise ids based on if the user had the required muscle groups selected
            //List<int> validMuscleGroupExerciseIds = new List<int>();

            // Now that we have the potential exercise ids, we need to make sure the user selected all the required muscle groups for them
            // Loop each exercise id and figure out the requirements
            //for (int index = 0; index < muscleGroupExerciseIds.Count(); index++ ) {

            //    // Get the muscle group ids required for the exercise
            //    // This is essentially the requirement. The user has to have selected all of these to get the exercise
            //    requiredIds = _context.ExerciseMuscleGroup
            //                    .Where(c => c.ExerciseId == muscleGroupExerciseIds[index])
            //                    .OrderBy( e => e.MuscleGroupId )
            //                    .Select(e => e.MuscleGroupId)
            //                    .ToArray();


            //    // Loop the required ids and make sure user has selected them all
            //    hasRequirement = true;
            //    foreach ( int id in requiredIds)
            //    {
            //        // If they don't have a required id then they do not get the exercise
            //        if (!muscle_groups.Contains(id)) {
            //            hasRequirement = false;
            //            break;
            //        }
            //    }

            //    // If the user meets all the requirements
            //    // This exercise should be added to the valid list
            //    if (hasRequirement) {
            //        validMuscleGroupExerciseIds.Add(muscleGroupExerciseIds[index]);
            //    }
            //}
            #endregion

            // If there are exercises related to the muscle groups the user selected then continue on
            if (exerciseIds.Count > 0)
            {
                // Loop each exercise id
                for (int i = 0; i < exerciseIds.Count(); i++)
                {

                    // Get the equipment objects related to the exercise
                    // This is essentially the requirement. The user has to have selected all of these to get the exercise
                    var requiredEquipment = _context.ExerciseEquipment
                                            .Where(c => c.ExerciseId == exerciseIds[i])
                                            .OrderBy(e => e.EquipmentId)
                                            .Select(e => new { exerciseEquipmentId = e.Id, equipmentId = e.EquipmentId });


                    // Loop the equipment objects and make sure user has selected them all
                    hasRequirement = true;
                    foreach (var objEquipment in requiredEquipment)
                    {
                        // If they don't have a required equipment id then we check the alternate table for a matching equipment id
                        if (!equipment.Contains(objEquipment.equipmentId))
                        {
                            // Get the alternate equipment ids where the exercise equipment id matches
                            alternateEquipmentIds = _context.ExerciseAlternateEquipment
                                                    .Where(c => c.ExerciseEquipmentId == objEquipment.exerciseEquipmentId)
                                                    .Select(e => e.AlternateEquipmentId)
                                                    .ToArray();

                            hasRequirement = false;

                            // Loop the alternate equipment ids
                            foreach (int alternateEquipmentId in alternateEquipmentIds)
                            {
                                // If an alternate equipment id matches with what the user has then they get the exercise
                                if (equipment.Contains(alternateEquipmentId))
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
            }

            // We now have the valid set of exercise ids
            // Get the exercise records and return them
            Exercises = _context.Exercise
                        .Where(m => validExerciseIds.Contains(m.Id))
                        .ToList();
        }
    }
}
