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

        public Task<int[]> GetUserExerciseIdsFromMuscleGroups(int[] ids);

        public Task<int[]> GetUserExerciseIdsFromEquipment(int[] ids);

        public Task<IEnumerable<Models.UserExercise>> GetUserExercisesFromRequiredEquipment(int[] muscleGroupIds, int[] equipmentIds);

        public void UpdateUserExercise(Models.UserExercise userExercise);

        public Task<bool> UserExerciseExists(int id);
    }
}
