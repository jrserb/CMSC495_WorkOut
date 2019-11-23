using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkoutGen.Models;
using WorkoutGen.Data.Services.Exercise;
using System.Threading.Tasks;
using WorkoutGen.Data.Session;
using WorkoutGen.Data.Services.UserWorkout;
using Microsoft.AspNetCore.Identity;
using WorkoutGen.Data.Services.UserSet;
using System.Linq;
using System;

namespace WorkoutGen.Pages.Exercises
{
    public class IndexModel : PageModel
    {
        private readonly IExerciseService _exerciseDb;
        private readonly IUserWorkoutService _userWorkoutDb;
        private readonly IUserSetService _userSetDb;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(IExerciseService exerciseDb,
            IUserWorkoutService userWorkoutDb,
            IUserSetService userSetDb,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _exerciseDb = exerciseDb;
            _userWorkoutDb = userWorkoutDb;
            _userSetDb = userSetDb;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public IEnumerable<Exercise> Exercises { get; set; }
        public int? WorkoutId { get; set; }
        public int? ExerciseIndex { get; set; }
        public List<Set> Sets { get; set; }

        // This method can be triggered if a user tries to go to this page URL directly OR whenever they refresh the exercise page
        public async Task<IActionResult> OnGetAsync()
        {

            int[] equipmentIds = GetSession<int[]>("equipment");

            // If there are no equipment ids in session that means that someone is trying to access this page directly and didnt come from equipment selection page
            // Redirect them to the equipment selection page
            if (equipmentIds == null)
            {
                return RedirectToPage("/Equipment/Index");
            }

            // Get our list of exercises
            Exercises = await _exerciseDb.GetExercisesFromRequiredEquipment(equipmentIds);

            SetModelPropertiesFromSession();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int[] equipmentIds)
        {
            // Store the equipment ids in session
            SetSession("equipment", equipmentIds);

            // Get our list of exercises
            Exercises = await _exerciseDb.GetExercisesFromRequiredEquipment(equipmentIds);

            // Create a workout record for existing user when they hit this page

            CreateWorkout();

            SetModelPropertiesFromSession();

            return Page();
        }

        // Responsible for creating sets and saving them in session
        public async Task<JsonResult> OnPostSaveSet(int workoutId, int exerciseId, string weight, string reps)
        {
            int setId = 0;
            Sets = GetSession<List<Set>>("sets");

            // If existing user then save the set to the database
            // Else store the set in session
            if (_signInManager.IsSignedIn(User))
            {
                UserSet set = new UserSet { ExerciseId = exerciseId, UserWorkoutId = workoutId, Repetitions = reps, Weight = weight };

                setId = _userSetDb.AddUserSet(set);

                Set s = new Set { exerciseId = (int)set.ExerciseId, set = $"{set.Weight}lbs x {set.Repetitions} reps\n" };
                Sets.Add(s);

                SetSession("sets", Sets);
            }
            else
            {
                // If sets session is being created for the first time
                if (Sets == null)
                {
                    Sets = new List<Set>();
                }

                // Add set information to the dictionary then save it in session
                Sets.Add(new Set { exerciseId = exerciseId, set = $"{weight}lbs x {reps} reps\n" });
                SetSession("sets", Sets);
            }

            return new JsonResult(setId);
        }

        public void CreateWorkout()
        {
            WorkoutId = 0;

            if (_signInManager.IsSignedIn(User))
            {
                string userId = _userManager.GetUserAsync(User).Result.Id;
                WorkoutId = _userWorkoutDb.AddUserWorkout(userId);
            }

            SetSession("workout", WorkoutId);
        }


        // Responsible for setting some of the important exercise session properties
        public void SetModelPropertiesFromSession()
        {
            ExerciseIndex = GetSession<int>("exercise");
            if (ExerciseIndex == null)
            {
                // Set the model property to 0 initially if no workout exists yet in session
                ExerciseIndex = 0;
            }

            WorkoutId = GetSession<int>("workout");

            // See if there are any sets saved in session
            Sets = GetSession<List<Set>>("sets");
            if (Sets == null)
            {
                // Set the model property to 0 initially if no workout exists yet in session
                Sets = new List<Set>();
                SetSession("sets", Sets);
            }
        }

        public void OnPostUpdateExerciseSession(int exerciseIndex)
        {
            SetSession("exercise", exerciseIndex);
        }

        public async Task<JsonResult> OnPostGetSet(int exerciseId)
        {
            if (_signInManager.IsSignedIn(User))
            {
                Sets = GetSession<List<Set>>("sets").Where(x => x.exerciseId == exerciseId).ToList();

                // If there are not any sets in session
                if (Sets.Count() == 0)
                {
                    string userId = _userManager.GetUserAsync(User).Result.Id;
                    int[] workoutIds = await _userWorkoutDb.GetUserWorkoutsByUserId(userId);
                    var set = await _userSetDb.GetLastUserSetForExercise(exerciseId, workoutIds);

                    if (set != null)
                    {
                        Sets.Add(new Set { exerciseId = (int)set.ExerciseId, set = $"{set.Weight}lbs x {set.Repetitions} reps\n" });
                        SetSession("sets", Sets);
                    }
                }

                return new JsonResult(Sets);
            }
            else
            {
                var sets = GetSession<List<Set>>("sets").Where(x => x.exerciseId == exerciseId);
                return new JsonResult(sets);
            }
        }

        public void OnPostClearSet(int exerciseId)
        {
            var sets = GetSession<List<Set>>("sets").Where(x => x.exerciseId != exerciseId);
            SetSession("sets", sets);

        }

        public void SetSession<T>(string sessionName, T value)
        {
            HttpContext.Session.Set(sessionName, value);
        }

        public T GetSession<T>(string sessionName)
        {
            return HttpContext.Session.Get<T>(sessionName);
        }

    }

    public class Set
    {
        public int exerciseId { get; set; }
        public string set { get; set; }
    }
}
