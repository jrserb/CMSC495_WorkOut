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

        public async Task<IEnumerable<Models.Exercise>> GetExercisesFromRequiredEquipment(int[] muscleGroupIds, int[] equipmentIds)
        {
            int[] alternateEquipmentIds = { };
            bool hasRequirement = true;

            // This list will hold the final list of valid exercise ids based on if the user had the required muscle groups and equipment selected
            List<int> validExerciseIds = new List<int>();

            int[] mgExerciseIds = await GetExerciseIdsFromMuscleGroups(muscleGroupIds);
            int[] exerciseIds = await GetExerciseIdsFromEquipment(equipmentIds);

            int[] ids = mgExerciseIds.Intersect(exerciseIds).ToArray();

            // Loop each exercise id
            for (int i = 0; i < ids.Count(); i++)
            {

                // Get the equipment objects related to the exercise
                // This is essentially the requirement. The user has to have selected all of these to get the exercise
                var requiredEquipment = await _equipmentDb.GetEquipmentFromExercise(ids[i]);


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
                    validExerciseIds.Add(ids[i]);
                }
            }

            // We now have the valid set of exercise ids
            // Get the exercise records and return them
            return await GetExercises(validExerciseIds.ToArray());
        }
    }
}
