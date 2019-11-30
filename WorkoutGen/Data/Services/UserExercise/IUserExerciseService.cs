using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorkoutGen.Data.Services.UserExercise
{
    public interface IUserExerciseService
    {
        public Task<Models.UserExercise> GetUserExercise(int id);        

        public Task<IEnumerable<Models.UserExercise>> GetUserExercises();

        public Task<IEnumerable<Models.UserExercise>> GetUserExercises(int[] ids);

        public Task<IEnumerable<Models.UserExercise>> GetUserExercisesFromUserId(string userId);

        public Task<int[]> GetUserExerciseIdsFromMuscleGroups(string userId, int[] ids);

        public Task<IEnumerable<Models.UserExerciseMuscleGroup>> GetUserExerciseMuscleGroupsFromExercise(int exerciseId);

        public Task<int[]> GetUserExerciseIdsFromEquipment(string userId, int[] ids);

        public Task<IEnumerable<Models.UserExerciseEquipment>> GetUserExerciseEquipmentFromExercise(int exerciseId);

        public Task<IEnumerable<Models.UserExercise>> GetUserExercisesFromRequiredEquipment(string userId, int[] muscleGroupIds, int[] equipmentIds);

        public Task<int> AddUserExercise(Models.UserExercise userExercise);

        public Task UpdateUserExercise(Models.UserExercise userExercise);

        public Task DeleteUserExercise(Models.UserExercise userExercise);

        public Task<bool> UserExerciseExists(int id);
    }
}
