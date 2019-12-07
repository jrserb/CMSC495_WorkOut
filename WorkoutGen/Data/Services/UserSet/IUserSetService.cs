using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkoutGen.Data.Services.UserSet
{
    public interface IUserSetService
    {
        public Task<Models.UserSet> GetUserSet(int id);        

        public Task<IEnumerable<Models.UserSet>> GetUserSets();

        public Task<IEnumerable<Models.UserSet>> GetUserSets(int[] ids);

        public Task<IEnumerable<Models.UserSet>> GetUserSetsFromExercise(int id);

        public Task<IEnumerable<Models.UserSet>> GetUserSetsFromUserExercise(int id);

        public Task<IEnumerable<Models.UserSet>> GetUserSetsFromWorkout(int workoutId);

        public Task<IEnumerable<Models.UserSet>> GetUserSetsFromWorkouts(int[] workoutIds);

        public Task<Models.UserSet> GetLastUserSetForExercise(bool isUserExercise, int exerciseId, int[] workoutIds);

        public int AddUserSet(Models.UserSet userSet);

        public Task DeleteUserSetsFromExercise(int workoutId, int exerciseId);

        public Task DeleteUserSetsFromUserExercise(int workoutId, int exerciseId);
    }
}
