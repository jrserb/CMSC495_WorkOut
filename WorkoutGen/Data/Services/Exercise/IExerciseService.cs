using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorkoutGen.Data.Services.Exercise
{
    public interface IExerciseService
    {
        public Task<Models.Exercise> GetExercise(int id);

        public Task<IEnumerable<Models.Exercise>> GetExercises();

        public Task<IEnumerable<Models.Exercise>> GetExercises(int[] ids);

        public Task<int[]> GetExerciseIdsFromMuscleGroups(int[] ids);

        public Task<int[]> GetExerciseIdsFromEquipment(int[] ids);

        public Task<IEnumerable<Models.Exercise>> GetExercisesFromRequiredEquipment(int[] muscleGroupIds, int[] equipmentIds);
    }
}
