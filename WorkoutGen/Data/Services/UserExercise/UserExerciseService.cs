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

        public UserExerciseService(ApplicationDbContext context, 
            IEquipmentService equipmentDb)
        {
            _context = context;
            _equipmentDb = equipmentDb;
        }

        // Returns all active UserExercise objects
        public async Task<IEnumerable<Models.UserExercise>> GetUserExercises()
        {
            return await _context.UserExercise
                        .Where(x => x.DateDeleted == null)
                        .ToListAsync();
        }

        // Returns single active UserExercise object that matches the id
        public async Task<Models.UserExercise> GetUserExercise(int id)
        {
            return await _context.UserExercise
                        .Where(x => x.Id == id && x.DateDeleted == null)
                        .SingleOrDefaultAsync();
        }

        // Returns active UserExercise objects that match the ids
        public async Task<IEnumerable<Models.UserExercise>> GetUserExercises(int[] ids)
        {
            return await _context.UserExercise
                        .Where(x => ids.Contains(x.Id) && x.DateDeleted == null)
                        .ToListAsync();
        }

        // Returns active UserExercise objects that are related to the user id
        public async Task<IEnumerable<Models.UserExercise>> GetUserExercisesFromUserId(string userId) {

            return await _context.UserExercise
                        .Where(x => x.UserId == userId && x.DateDeleted == null)
                        .ToListAsync();
        }

        // Returns array of ids of active UserExercise objects that are related to muscle groups
        public async Task<int[]> GetUserExerciseIdsFromMuscleGroups(string userId, int[] ids)
        {
            return await _context.UserExerciseMuscleGroup
                        .Where(x => x.UserId == userId && ids.Contains(x.MuscleGroupId) && x.DateDeleted == null)
                        .Select(x => x.UserExerciseId)
                        .Distinct()
                        .ToArrayAsync();
        }

        // Returns active UserExerciseMuscleGroup objects that are related to an exercise
        public async Task<IEnumerable<Models.UserExerciseMuscleGroup>> GetUserExerciseMuscleGroupsFromExercise(int exerciseId)
        {
            return await _context.UserExerciseMuscleGroup
                        .Where(x => x.UserExerciseId == exerciseId && x.DateDeleted == null)
                        .Distinct()
                        .ToListAsync();
        }

        public async Task<int[]> GetUserExerciseIdsFromEquipment(string userId, int[] ids)
        {
            return await _context.UserExerciseEquipment
                        .Where(x => x.UserId == userId && ids.Contains(x.EquipmentId) && x.DateDeleted == null)
                        .Select(x => x.UserExerciseId)
                        .Distinct()
                        .ToArrayAsync();
        }

        public async Task<IEnumerable<Models.UserExerciseEquipment>> GetUserExerciseEquipmentFromExercise(int exerciseId)
        {
            return await _context.UserExerciseEquipment
                        .Where(x => x.UserExerciseId == exerciseId && x.DateDeleted == null)
                        .Distinct()
                        .ToListAsync();
        }

        public async Task<IEnumerable<Models.Equipment>> GetEquipmentFromUserExercise(string userId, int id)
        {
            int[] equipmentIds = await GetEquipmentIdsFromUserExercise(userId, id);
            return await _equipmentDb.GetEquipment(equipmentIds);
        }

        public async Task<int[]> GetEquipmentIdsFromUserExercise(string userId, int id)
        {
            return await _context.UserExerciseEquipment
                        .Where(x => x.UserId == userId && x.UserExerciseId == id && x.DateDeleted == null)
                        .Select(x => x.EquipmentId)
                        .Distinct()
                        .ToArrayAsync();
        }

        public async Task<IEnumerable<Models.UserSet>> GetUserSetsFromExercise(int exerciseId)
        {
            return await _context.UserSet
                        .Where(x => x.UserExerciseId == exerciseId && x.DateDeleted == null)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Models.UserExercise>> GetUserExercisesFromRequiredEquipment(string userId, int[] muscleGroupIds, int[] equipmentIds)
        {
            int[] alternateEquipmentIds;
            bool hasRequirement;

            // This list will hold the final list of valid exercise ids based on if the user had the required muscle groups and equipment selected
            List<int> validExerciseIds = new List<int>();

            // Get all the user exercise ids related to the muscle group ids selected
            int[] muscleGroupExerciseIds = await GetUserExerciseIdsFromMuscleGroups(userId, muscleGroupIds);

            // Loop each exercise id
            for (int i = 0; i < muscleGroupExerciseIds.Count(); i++)
            {
                // Get the equipment objects related to the exercise
                // This is essentially the requirement. The user has to have selected all of these to get the exercise
                var requiredEquipment = await GetEquipmentFromUserExercise(userId, muscleGroupExerciseIds[i]);

                // Loop the equipment objects and make sure user has selected them all
                hasRequirement = true;
                foreach (var objEquipment in requiredEquipment)
                {
                    // If they don't have a required equipment id then we check the alternate table for a matching equipment id
                    if (!equipmentIds.Contains(objEquipment.Id))
                    {
                        hasRequirement = false;

                        // Get the alternate equipment ids where the exercise equipment id matches
                        alternateEquipmentIds = await _equipmentDb.GetAlternateEquipmentIdsFromEquipment(objEquipment.Id);
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
            return await GetUserExercises(validExerciseIds.ToArray());
        }

        public async Task<int> AddUserExercise(Models.UserExercise userExercise)
        {
            await _context.UserExercise.AddAsync(userExercise);
            await _context.SaveChangesAsync();

            return userExercise.Id;
        }

        public async Task UpdateUserExercise(Models.UserExercise userExercise)
        {

            //_context.UserExercise.Attach(userExercise).State = EntityState.Modified;
         
            var exerciseMuscleGroups = await GetUserExerciseMuscleGroupsFromExercise(userExercise.Id);
            var exerciseEquipment = await GetUserExerciseEquipmentFromExercise(userExercise.Id);

            foreach (Models.UserExerciseMuscleGroup muscleGroup in exerciseMuscleGroups)
            {
                muscleGroup.DateDeleted = DateTime.Now;
            }
            foreach (Models.UserExerciseEquipment equipment in exerciseEquipment)
            {
                equipment.DateDeleted = DateTime.Now;
            }

            _context.UserExercise.Update(userExercise);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserExercise(Models.UserExercise userExercise)
        {
            var exerciseMuscleGroups = await GetUserExerciseMuscleGroupsFromExercise(userExercise.Id);
            var exerciseEquipment = await GetUserExerciseEquipmentFromExercise(userExercise.Id);
            var exerciseSets = await GetUserSetsFromExercise(userExercise.Id);

            foreach (Models.UserExerciseMuscleGroup muscleGroup in exerciseMuscleGroups)
            {
                muscleGroup.DateDeleted = DateTime.Now;
            }
            foreach (Models.UserExerciseEquipment equipment in exerciseEquipment)
            {
                equipment.DateDeleted = DateTime.Now;
            }
            foreach (Models.UserSet set in exerciseSets)
            {
                set.DateDeleted = DateTime.Now;
            }

            userExercise.DateDeleted = DateTime.Now;

            _context.UserExercise.Update(userExercise);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UserExerciseExists(int id) {
            return await _context.UserExercise.AnyAsync(e => e.Id == id);
        }
    }
}
