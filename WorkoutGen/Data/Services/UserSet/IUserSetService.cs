using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkoutGen.Data.Services.UserSet
{
    public interface IUserSetService
    {
        public Task<Models.UserSet> GetUserSet(int id);

        public Task<Models.UserSet> GetLastUserSetForExercise(int exerciseId, int[] workoutIds);

        public Task<IEnumerable<Models.UserSet>> GetUserSets();

        public Task<IEnumerable<Models.UserSet>> GetUserSets(int[] ids);

        public Task<IEnumerable<Models.UserSet>> GetUserSetsFromWorkout(int workoutId);

        public int AddUserSet(Models.UserSet userSet);
    }
}
