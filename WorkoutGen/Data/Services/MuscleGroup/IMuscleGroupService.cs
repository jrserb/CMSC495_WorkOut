using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorkoutGen.Data.Services.MuscleGroup
{
    public interface IMuscleGroupService
    {      
        public Task<Models.MuscleGroup> GetMuscleGroup(int id);

        public Task<IEnumerable<Models.MuscleGroup>> GetMuscleGroups();

        public Task<IEnumerable<Models.MuscleGroup>> GetMuscleGroups(int[] ids);
    }
}
