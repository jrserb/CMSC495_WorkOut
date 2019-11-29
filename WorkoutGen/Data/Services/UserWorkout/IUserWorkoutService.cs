using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkoutGen.Data.Services.UserWorkout
{
    public interface IUserWorkoutService
    {
        public Task<Models.UserWorkout> GetUserWorkout(int id);

        public Task<IEnumerable<Models.UserWorkout>> GetUserWorkouts();

        public Task<IEnumerable<Models.UserWorkout>> GetUserWorkouts(int[] ids);

        public Task<IEnumerable<Models.UserWorkout>> GetUserWorkoutsByUserId(string userId);

        public Task<int> AddUserWorkout(string userId);
    }
}
