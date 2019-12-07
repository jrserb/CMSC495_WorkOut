using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorkoutGen.Data.Services.MuscleGroup
{
    public interface IMuscleGroupService
    {      

        public Task<Models.MuscleGroup> GetMuscleGroup(int id);

        public Task<IEnumerable<Models.MuscleGroup>> GetMuscleGroups();

        public Task<IEnumerable<Models.MuscleGroup>> GetMuscleGroups(int[] ids);

        public Task<IEnumerable<Models.MuscleGroup>> GetMuscleGroupsFromExercise(int id);

        public Task<int[]> GetMuscleGroupIdsFromExercise(int id);



        //USER METHODS

        public Task<IEnumerable<Models.MuscleGroup>> GetMuscleGroupsFromUserExercise(string userId, int id);

        public Task<int[]> GetMuscleGroupIdsFromUserExercise(string userId, int id);
    }
}
