using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutGen.Data.Services.Equipment;

namespace WorkoutGen.Data.Services.Exercise
{
    public class ExerciseService : IExerciseService
    {
        private readonly WorkoutGenContext _context;
        private readonly IEquipmentService _equipmentDb;

        public ExerciseService(WorkoutGenContext context, IEquipmentService equipmentDb)
        {
            _context = context;
            _equipmentDb = equipmentDb;

        }    

        public async Task<Models.Exercise> GetExercise(int id)
        {
            return await _context.Exercise
                        .Where(x => x.Id == id)
                        .SingleAsync();
        }

        public async Task<IEnumerable<Models.Exercise>> GetExercises()
        {
            return await _context.Exercise
                        .OrderBy(x => x.Name)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Models.Exercise>> GetExercises(int[] ids)
        {
            return await _context.Exercise
                        .Where(x => ids.Contains(x.Id))
                        .ToListAsync();
        }

        public async Task<int[]> GetExerciseIdsFromMuscleGroups(int[] ids)
        {
            return await _context.ExerciseMuscleGroup
                        .Where(x => ids.Contains(x.MuscleGroupId))
                        .Select(x => x.ExerciseId)
                        .Distinct()
                        .ToArrayAsync();
        }

        public async Task<int[]> GetExerciseIdsFromEquipment(int[] ids)
        {
            return await _context.ExerciseEquipment
                        .Where(x => ids.Contains(x.EquipmentId))
                        .Select(x => x.ExerciseId)
                        .Distinct()
                        .ToArrayAsync();
        }

        public async Task<IEnumerable<Models.Exercise>> GetExercisesFromRequiredEquipment(int[] ids)
        {
            int[] alternateEquipmentIds = { };
            bool hasRequirement = true;

            // This list will hold the final list of valid exercise ids based on if the user had the required muscle groups and equipment selected
            List<int> validExerciseIds = new List<int>();

            // First thing to do is select all the distinct exercise ids related to the muscle groups the user selected
            int[] exerciseIds = await GetExerciseIdsFromEquipment(ids);

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
                    if (!ids.Contains(objEquipment.Id))
                    {
                        // Get the alternate equipment ids where the exercise equipment id matches
                        alternateEquipmentIds = await _equipmentDb.GetAlternateEquipmentIdsFromEquipment(objEquipment.Id);

                        hasRequirement = false;

                        // Loop the alternate equipment ids
                        foreach (int alternateEquipmentId in alternateEquipmentIds)
                        {
                            // If an alternate equipment id matches with what the user has then they get the exercise
                            if (ids.Contains(alternateEquipmentId))
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
            return await GetExercises(validExerciseIds.ToArray());
        }
    }
}
