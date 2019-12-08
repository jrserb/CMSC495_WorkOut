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
        private readonly ApplicationDbContext _context;
        private readonly IEquipmentService _equipmentDb;

        public ExerciseService(ApplicationDbContext context, IEquipmentService equipmentDb)
        {
            _context = context;
            _equipmentDb = equipmentDb;

        }    

        public async Task<Models.Exercise> GetExercise(int id)
        {
            return await _context.Exercise
                        .Where(x => x.Id == id && x.DateDeleted == null)
                        .SingleAsync();
        }

        public async Task<IEnumerable<Models.Exercise>> GetExercises()
        {
            return await _context.Exercise
                        .Where(x => x.DateDeleted == null)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Models.Exercise>> GetExercises(int[] ids)
        {
            return await _context.Exercise
                        .Where(x => ids.Contains(x.Id) && x.DateDeleted == null)                        
                        .ToListAsync();
        }

        public async Task<int[]> GetExerciseIdsFromMuscleGroups(int[] ids)
        {
            return await _context.ExerciseMuscleGroup
                        .Where(x => ids.Contains(x.MuscleGroupId) && x.DateDeleted == null)
                        .Select(x => x.ExerciseId)
                        .Distinct()
                        .ToArrayAsync();
        }

        public async Task<int[]> GetExerciseIdsFromEquipment(int[] ids)
        {
            return await _context.ExerciseEquipment
                        .Where(x => ids.Contains(x.EquipmentId) && x.DateDeleted == null)
                        .Select(x => x.ExerciseId)
                        .Distinct()
                        .ToArrayAsync();
        }

        public async Task<IEnumerable<Models.Exercise>> GetExercisesFromRequiredEquipment(int[] muscleGroupIds, int[] equipmentIds)
        {
            bool hasRequirement;
            int[] alternateEquipmentIds;
            int[] exerciseEquipmentIds;

            // This list will hold the final list of valid exercise ids based on if the user had the required muscle groups and equipment selected
            List<int> validExerciseIds = new List<int>();

            // Get all the exercise ids related to the muscle group ids selected
            int[] muscleGroupExerciseIds = await GetExerciseIdsFromMuscleGroups(muscleGroupIds);

            // Loop each exercise id
            for (int i = 0; i < muscleGroupExerciseIds.Count(); i++)
            {
                // Get the equipment objects related to the exercise
                // This is essentially the requirement. The user has to have selected all of these to get the exercise
                var requiredEquipment = await _equipmentDb.GetEquipmentFromExercise(muscleGroupExerciseIds[i]);

                // Loop the equipment objects and make sure user has selected them all
                // If no equipment is required then requirement stays true and they get the exercise
                hasRequirement = true;
                foreach (var objEquipment in requiredEquipment)
                {
                    // If they don't have a required equipment id then we check the alternate table for a matching equipment id
                    if (!equipmentIds.Contains(objEquipment.Id))
                    {
                        hasRequirement = false;

                        // Get the exercise equipment ids where the exercise id and the equipment id matches
                        exerciseEquipmentIds = await _equipmentDb.GetExerciseEquipmentIdsFromExerciseAndEquipment(muscleGroupExerciseIds[i], objEquipment.Id);

                        // Get the alternate equipment ids where the exercise equipment id matches
                        alternateEquipmentIds = await _equipmentDb.GetAlternateEquipmentIdsFromExerciseEquipment(exerciseEquipmentIds);

                        int[] alternateMatches = equipmentIds.Where(x => alternateEquipmentIds.Contains(x)).ToArray();

                        // If user selected equipment that matches an alternate equipment then we give them the exercise
                        if (alternateMatches.Count() > 0)
                        {
                            hasRequirement = true;
                        }

                        if (hasRequirement == false) break;
                    }
                }

                // If both arrays contain the same ids then the user meets the requirements
                // This exercise should be added to the valid list
                if (hasRequirement)
                {
                    validExerciseIds.Add(muscleGroupExerciseIds[i]);
                }
            }

            // We now have the valid set of exercise ids
            // Get the exercise records and return them
            return await GetExercises(validExerciseIds.ToArray());
        }
    }
}
