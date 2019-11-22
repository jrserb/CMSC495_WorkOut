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

            SetModelPropertiesFromSession();

            // Create a workout record for existing user when they hit this page
            if (_signInManager.IsSignedIn(User))
            {
                CreateUserWorkout();
            }

            return Page();
        }

        // Responsible for creating sets and saving them in session
        public async Task<JsonResult> OnPostSaveSet(int workoutId, int exerciseId, string weight, string reps)
        {
            int setId = 0;

            // If existing user then save the set to the database
            // Else store the set in session
            if (_signInManager.IsSignedIn(User))
            {
                UserSet set = new UserSet { ExerciseId = exerciseId, WorkoutId = workoutId, Repetitions = reps, Weight = weight };
                setId = _userSetDb.AddUserSet(set);
            }
            else
            {
                Sets = GetSession<List<Set>>("sets");

                // If sets session is being created for the first time
                if (Sets == null)
                {
                    Sets = new List<Set>();
                }

                // Add set information to the dictionary then save it in session
                Sets.Add( new Set { exerciseId = exerciseId, set = $"{weight}lbs x {reps} reps\n" } );
                SetSession("sets", Sets);
            }

            return new JsonResult(setId);
        }  

        public void CreateUserWorkout()
        {

            // Only create a workout if one is not already saved in session
            if (WorkoutId == null)
            {
                string userId = _userManager.GetUserAsync(User).Result.Id;
                WorkoutId = _userWorkoutDb.AddUserWorkout(userId);

                // Set the created workout to session
                SetSession("workout", WorkoutId);
            }
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
            if (WorkoutId == null)
            {
                // Set the model property to 0 initially if no workout exists yet in session
                WorkoutId = 0;
            }

            // See if there are any sets saved in session
            Sets = GetSession<List<Set>>("sets");
        }

        public void OnPostUpdateExerciseSession(int exerciseIndex)
        {
            SetSession("exercise", exerciseIndex);
        }

        public JsonResult OnPostGetSetFromSession(int exerciseId)
        {
            var sets = GetSession<List<Set>>("sets").Where( x => x.exerciseId == exerciseId );
            return new JsonResult(sets);
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

    public class Set {
        public int exerciseId { get; set; }
        public string set { get; set; }
    }
}
