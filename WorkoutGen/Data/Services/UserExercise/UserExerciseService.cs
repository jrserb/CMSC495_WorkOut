using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutGen.Data.Services.Equipment;

namespace WorkoutGen.Data.Services.UserExercise
{
    public class UserExerciseService : IUserExerciseService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEquipmentService _equipmentDb;

        public UserExerciseService(ApplicationDbContext context, IEquipmentService equipmentDb)
        {
            _context = context;
            _equipmentDb = equipmentDb;
        }

        public async Task<Models.UserExercise> GetUserExercise(int id)
        {
            return await _context.UserExercise
                        .Where(x => x.Id == id)
                        .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Models.UserExercise>> GetUserExercises()
        {
            return await _context.UserExercise
                        .OrderBy(x => x.Name)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Models.UserExercise>> GetUserExercises(int[] ids)
        {
            return await _context.UserExercise
                        .Where(x => ids.Contains(x.Id))
                        .ToListAsync();
        }

        public async Task<IEnumerable<Models.UserExercise>> GetUserExercisesFromUserId(string userId) {

            return await _context.UserExercise
                        .Where(x => x.UserId == userId)
                        .ToListAsync();

        }

        public async Task<int[]> GetUserExerciseIdsFromMuscleGroups(int[] ids)
        {
            return await _context.UserExerciseMuscleGroup
                        .Where(x => ids.Contains(x.MuscleGroupId))
                        .Select(x => x.UserExerciseId)
                        .Distinct()
                        .ToArrayAsync();
        }

        public async Task<int[]> GetUserExerciseIdsFromEquipment(int[] ids)
        {
            return await _context.UserExerciseEquipment
                        .Where(x => ids.Contains(x.EquipmentId))
                        .Select(x => x.UserExerciseId)
                        .Distinct()
                        .ToArrayAsync();
        }

        public async Task<IEnumerable<Models.UserExercise>> GetUserExercisesFromRequiredEquipment(int[] muscleGroupIds, int[] equipmentIds)
        {
            int[] alternateEquipmentIds = { };
            bool hasRequirement = true;

            // This list will hold the final list of valid exercise ids based on if the user had the required muscle groups and equipment selected
            List<int> validExerciseIds = new List<int>();

            int[] mgExerciseIds = await GetUserExerciseIdsFromMuscleGroups(muscleGroupIds);
            int[] exerciseIds = await GetUserExerciseIdsFromEquipment(equipmentIds);

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
            return await GetUserExercises(validExerciseIds.ToArray());
        }

        public void UpdateUserExercise(Models.UserExercise userExercise)
        {

            //_context.UserExercise.Attach(userExercise).State = EntityState.Modified;
            _context.UserExercise.Update(userExercise);
            _context.SaveChanges();
        }

        public void DeleteUserExercise(Models.UserExercise userExercise)
        {
            _context.UserExercise.Remove(userExercise);
            _context.SaveChanges();
        }

        public async Task<bool> UserExerciseExists(int id) {
            return await _context.UserExercise.AnyAsync(e => e.Id == id);
        }
    }
}
