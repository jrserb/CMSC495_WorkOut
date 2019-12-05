using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkoutGen.Models;
using WorkoutGen.Data.Services.Exercise;
using System.Threading.Tasks;
using WorkoutGen.Data.Session;
using WorkoutGen.Data.Services.UserWorkout;
using WorkoutGen.Data.Services.Equipment;
using WorkoutGen.Data.Services.MuscleGroup;
using Microsoft.AspNetCore.Identity;
using WorkoutGen.Data.Services.UserSet;
using System.Linq;
using WorkoutGen.Data.Services.UserExercise;
using Newtonsoft.Json;

namespace WorkoutGen.Pages.Exercises
{
    public class IndexModel : PageModel
    {
        private readonly IExerciseService _exerciseDb;
        private readonly IUserWorkoutService _userWorkoutDb;
        private readonly IUserSetService _userSetDb;
        private readonly IEquipmentService _equipmentDb;
        private readonly IMuscleGroupService _muscleGroupDb;
        private readonly IUserExerciseService _userExerciseDb;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(IExerciseService exerciseDb,
            IUserWorkoutService userWorkoutDb,
            IUserSetService userSetDb,
            IEquipmentService equipmentDb,
            IMuscleGroupService muscleGroupDb,
            IUserExerciseService userExerciseDb,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _exerciseDb = exerciseDb;
            _userWorkoutDb = userWorkoutDb;
            _userSetDb = userSetDb;
            _equipmentDb = equipmentDb;
            _muscleGroupDb = muscleGroupDb;
            _userExerciseDb = userExerciseDb;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IEnumerable<Exercise> Exercises { get; set; }
        public IEnumerable<UserExercise> UserExercises { get; set; }
        public IEnumerable<Models.MuscleGroup> MuscleGroups { get; set; }
        public IEnumerable<Models.Equipment> Equipment { get; set; }
        public int WorkoutId { get; set; }
        public int? ExerciseIndex { get; set; }
        public bool IsUserExercise { get; set; }
        public List<SessionSet> Sets { get; set; }

        // This method can be triggered if a user tries to go to this page URL directly OR whenever they refresh the exercise page
        public async Task<IActionResult> OnGetAsync()
        {
            // Get our selected items from session
            int[] equipmentIds = HttpContext.Session.Get<int[]>("EquipmentIds");
            int[] muscleGroupIds = HttpContext.Session.Get<int[]>("MuscleGroupIds");

            // If there are no muscle group ids in session that means that someone is trying to access this page directly and didnt come from equipment selection page
            // Redirect them to the muscle group selection page
            if (muscleGroupIds == null)
            {
                return RedirectToPage("/MuscleGroup/Index");
            }

            // If there are no equipment ids in session that means that someone is trying to access this page directly and didnt come from equipment selection page
            // Redirect them to the equipment selection page
            if (equipmentIds == null)
            {
                return RedirectToPage("/Equipment/Index");
            }

            MuscleGroups = await _muscleGroupDb.GetMuscleGroups(muscleGroupIds);
            Equipment = await _equipmentDb.GetEquipment(equipmentIds);

            Exercises = await _exerciseDb.GetExercisesFromRequiredEquipment(muscleGroupIds, equipmentIds);
            UserExercises = Enumerable.Empty<UserExercise>();
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                UserExercises = await _userExerciseDb.GetUserExercisesFromRequiredEquipment(user.Id, muscleGroupIds, equipmentIds);
            }

            SetModelPropertiesFromSession();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int[] muscleGroupIds, int[] equipmentIds)
        {
            HttpContext.Session.Set("EquipmentIds", equipmentIds);
            HttpContext.Session.Set("MuscleGroupIds", muscleGroupIds);

            await SetModelProperties(muscleGroupIds, equipmentIds);
            SetSessionVariables();
            return Page();
        }



        // Sets initial model properties
        public async Task SetModelProperties(int[] muscleGroupIds, int[] equipmentIds)
        {

            MuscleGroups = await _muscleGroupDb.GetMuscleGroups(muscleGroupIds);
            Equipment = await _equipmentDb.GetEquipment(equipmentIds);
            Exercises = await _exerciseDb.GetExercisesFromRequiredEquipment(muscleGroupIds, equipmentIds);
            UserExercises = Enumerable.Empty<UserExercise>();
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                UserExercises = await _userExerciseDb.GetUserExercisesFromRequiredEquipment(user.Id, muscleGroupIds, equipmentIds);
            }
            ExerciseIndex = 0;
            IsUserExercise = false;
            WorkoutId = 0;
            Sets = new List<SessionSet>();

            int firstExerciseId;
            if (_signInManager.IsSignedIn(User) && Exercises.Count() == 0)
            {
                firstExerciseId = UserExercises.FirstOrDefault().Id;
                IsUserExercise = true;
            }
            else
            {
                firstExerciseId = Exercises.FirstOrDefault().Id;
            }

            // If logged in user, save a new workout record to their profile and return the workout  id
            // If logged in user make a call to the DB to grab the last set that was used for the exercise
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);

                WorkoutId = await _userWorkoutDb.AddUserWorkout(user.Id);

                var userWorkouts = await _userWorkoutDb.GetUserWorkoutsByUserId(user.Id);
                int[] workoutIds = userWorkouts.Select(x => x.Id).ToArray();

                var userSet = await _userSetDb.GetLastUserSetForExercise(IsUserExercise, firstExerciseId, workoutIds);

                if (userSet != null)
                {                   
                    UserSet set = new UserSet();
                            set.UserWorkoutId = WorkoutId;
                            set.Repetitions = userSet.Repetitions;
                            set.Weight = userSet.Weight;
                            set.UserExerciseId = userSet.UserExerciseId;
                            set.ExerciseId = userSet.ExerciseId;

                    int setId = _userSetDb.AddUserSet(set);


                    var s = new SessionSet();
                        s.userExerciseId = userSet.UserExerciseId;
                        s.exerciseId = userSet.ExerciseId;
                        s.set = $"{userSet.Weight}lbs x {userSet.Repetitions} reps\n";

                    Sets.Add(s);
                }
            }
        }

        public void SetModelPropertiesFromSession()
        {
            ExerciseIndex = HttpContext.Session.Get<int>("ExerciseIndex");
            IsUserExercise = HttpContext.Session.Get<bool>("IsUserExercise");
            WorkoutId = HttpContext.Session.Get<int>("WorkoutId");
            Sets = HttpContext.Session.Get<List<SessionSet>>("Sets");
        }

        public void SetSessionVariables()
        {
            HttpContext.Session.Set("ExerciseIndex", ExerciseIndex);
            HttpContext.Session.Set("IsUserExercise", IsUserExercise);
            HttpContext.Session.Set("WorkoutId", WorkoutId);
            HttpContext.Session.Set("Sets", Sets);
        }

        public async Task<JsonResult> OnPostGetSets(bool isUserExercise, int exerciseId)
        {
            // Grab current sets in the session that are tied to the exercise
            Sets = HttpContext.Session.Get<List<SessionSet>>("Sets");
            WorkoutId = HttpContext.Session.Get<int>("WorkoutId");

            var exerciseSets = Sets.Where(x => x.exerciseId == exerciseId)
                                   .ToList();

            if (isUserExercise)
            {
                exerciseSets = Sets.Where(x => x.userExerciseId == exerciseId)
                                   .ToList();
            }

            if (exerciseSets.Count > 0)
            {
                return new JsonResult(exerciseSets);
            }

            // If user is signed in then we make a call to the DB to grab the last set that was used for this exercise
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                var userWorkouts = await _userWorkoutDb.GetUserWorkoutsByUserId(user.Id);
                int[] workoutIds = userWorkouts.Select(x => x.Id).ToArray();
                var userSet = await _userSetDb.GetLastUserSetForExercise(isUserExercise, exerciseId, workoutIds);

                if (userSet != null)
                {

                    UserSet set = new UserSet();
                            set.UserWorkoutId = WorkoutId;
                            set.Repetitions = userSet.Repetitions;
                            set.Weight = userSet.Weight;
                            set.UserExerciseId = userSet.UserExerciseId;
                            set.ExerciseId = userSet.ExerciseId;

                    int setId = _userSetDb.AddUserSet(set);

                    var s = new SessionSet();
                        s.userExerciseId = userSet.UserExerciseId;
                        s.exerciseId = userSet.ExerciseId;
                        s.set = $"{userSet.Weight}lbs x {userSet.Repetitions} reps\n";

                    exerciseSets.Add(s);
                    Sets.Add(s);
                    HttpContext.Session.Set("Sets", Sets);
                }
            }

            return new JsonResult(exerciseSets);
        }

        // Responsible for creating sets and saving them in session
        public JsonResult OnPostSaveSet(bool isUserExercise, int workoutId, int exerciseId, string weight, string reps)
        {
            Sets = HttpContext.Session.Get<List<SessionSet>>("Sets");                  
            
            if (_signInManager.IsSignedIn(User))
            {
                UserSet set = new UserSet();
                set.UserWorkoutId = workoutId;
                set.Repetitions = reps;
                set.Weight = weight;

                // If existing user then save the set to the database
                if (isUserExercise)
                {
                    set.UserExerciseId = exerciseId;
                }
                else
                {
                    set.ExerciseId = exerciseId;
                }
                int setId = _userSetDb.AddUserSet(set);
            }


            SessionSet s = new SessionSet();

            // If existing user then save the set to the database
            if (isUserExercise)
            {
                s.userExerciseId = exerciseId;
            }
            else
            {
                s.exerciseId = exerciseId;
            }

            s.set = $"{weight}lbs x {reps} reps\n";         

            Sets.Add(s);
            HttpContext.Session.Set("Sets", Sets);

            return new JsonResult("{}");
        }

        public async Task<JsonResult> OnPostClearSet(bool isUserExercise, int exerciseId)
        {
            WorkoutId = HttpContext.Session.Get<int>("WorkoutId");

            // Get a new list of sets that are not tied to the exercise we are clearing sets for
            // This removes the sets we are clearing
            var sets = HttpContext.Session.Get<List<SessionSet>>("Sets")
                                          .Where(x => x.exerciseId != exerciseId);

            if (isUserExercise) {

                sets = HttpContext.Session.Get<List<SessionSet>>("Sets")
                                          .Where(x => x.userExerciseId != exerciseId);
            }
        
            // If user is logged in then delete from db
            if (_signInManager.IsSignedIn(User))
            {
                if (isUserExercise)
                {
                    await _userSetDb.DeleteUserSetsFromUserExercise(WorkoutId, exerciseId);
                }
                else 
                {
                    await _userSetDb.DeleteUserSetsFromExercise(WorkoutId, exerciseId);
                }
            }

            // Set session with new set list
            HttpContext.Session.Set("Sets", sets);

            return new JsonResult("{}");
        }

        public JsonResult OnPostUpdateExerciseSessions(bool isUserExercise, int exerciseIndex)
        {
            HttpContext.Session.Set("ExerciseIndex", exerciseIndex);
            HttpContext.Session.Set("IsUserExercise", isUserExercise);

            return new JsonResult("{}");
        }

        public async Task<ContentResult> OnPostGetMuscleGroupsEquipmentFromExercise(bool isUserExercise, int exerciseId)
        {
            int[] equipmentIds = HttpContext.Session.Get<int[]>("EquipmentIds");
            int[] muscleGroupIds = HttpContext.Session.Get<int[]>("MuscleGroupIds");

            var muscleGroups = Enumerable.Empty<Models.MuscleGroup>();
            var equipment = Enumerable.Empty<Models.Equipment>();

            if (isUserExercise)
            {
                muscleGroups = await _muscleGroupDb.GetMuscleGroupsFromUserExercise(exerciseId);
                equipment = await _equipmentDb.GetEquipmentFromUserExercise(exerciseId);
            }
            else
            {
                muscleGroups = await _muscleGroupDb.GetMuscleGroupsFromExercise(exerciseId);
                equipment = await _equipmentDb.GetEquipmentFromExercise(exerciseId);
            }

            var alternateEquipment = await _equipmentDb.GetAlternateEquipmentFromEquipment(equipment.Select( x => x.Id ).ToArray());

            equipment = equipment.Concat(alternateEquipment);
            equipment = equipment.Where(x => equipmentIds.Contains(x.Id));

            var muscleGrouquipment = new ExerciseMuscleGroupEquipment { MuscleGroups = muscleGroups, Equipment = equipment };

            return Content(JsonConvert.SerializeObject(muscleGrouquipment));
        }
    }

    // Simple class to hold these lists to serialize to JSON and return to client
    public class ExerciseMuscleGroupEquipment {
        public IEnumerable<Models.MuscleGroup> MuscleGroups { get; set; }
        public IEnumerable<Models.Equipment> Equipment { get; set; }
    }
}
