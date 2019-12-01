using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkoutGen.Data.Services.Exercise;
using WorkoutGen.Data.Services.UserExercise;
using WorkoutGen.Data.Services.UserSet;
using WorkoutGen.Data.Services.UserWorkout;
using WorkoutGen.Models;

namespace WorkoutGen.Areas.Identity.Pages.Account.Manage
{
    public class MyAccountModel : PageModel
    {
        private readonly IUserExerciseService _userExerciseDb;
        private readonly IUserWorkoutService _userWorkoutDb;
        private readonly IUserSetService _userSetDb;
        private readonly UserManager<ApplicationUser> _userManager;

        public MyAccountModel(IUserExerciseService userExerciseDb,
            IUserWorkoutService userWorkoutDb,
            IUserSetService userSetDb,
            UserManager<ApplicationUser> userManager)
        {
            _userExerciseDb = userExerciseDb;
            _userWorkoutDb = userWorkoutDb;
            _userSetDb = userSetDb;
            _userManager = userManager;
        }


        [BindProperty]
        public IEnumerable<UserWorkout> UserWorkouts { get; set; }
        public IEnumerable<UserSet> UserSets { get; set; }
        public IEnumerable<UserExercise> UserExercises { get; set; }
        public SelectList Options_UserExercises { get; set; }


        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);

            UserWorkouts = await _userWorkoutDb.GetUserWorkoutsByUserId(user.Id);
            UserWorkouts = UserWorkouts.OrderByDescending(x => x.Id);

            int[] workoutIds = UserWorkouts.Select(x => x.Id)
                            .Distinct()
                            .ToArray();

            UserSets = await _userSetDb.GetUserSetsFromWorkouts(workoutIds);

            UserExercises = await _userExerciseDb.GetUserExercisesFromUserId(user.Id);

            Options_UserExercises = new SelectList(UserExercises, "Id", "Name");

            return Page();
        }

    }
}
