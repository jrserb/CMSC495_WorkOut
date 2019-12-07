using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkoutGen.Data.Services.Equipment;
using WorkoutGen.Data.Services.Exercise;
using WorkoutGen.Data.Services.MuscleGroup;
using WorkoutGen.Data.Services.UserExercise;
using WorkoutGen.Data.Session;
using WorkoutGen.Models;

namespace WorkoutGen.Pages.Equipment
{
    public class IndexModel : PageModel
    {
        private readonly IExerciseService _exerciseDb;
        private readonly IEquipmentService _equipmentDb;
        private readonly IMuscleGroupService _muscleGroupDb;
        private readonly IUserExerciseService _userExerciseDb;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(IExerciseService exerciseDb,
            IEquipmentService equipmentDb,
            IMuscleGroupService muscleGroupDb,
            IUserExerciseService userExerciseDb,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _exerciseDb = exerciseDb;
            _equipmentDb = equipmentDb;
            _muscleGroupDb = muscleGroupDb;
            _userExerciseDb = userExerciseDb;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // Binding properties allows them to be passed to the razor page model
        // So we can access it during requests and responses
        [BindProperty]
        public IEnumerable<Models.MuscleGroup> MuscleGroups { get; set; }
        public int ExerciseCount { get; set; }
        public int[] EquipmentIds { get; set; }
        public int[] MuscleGroupIds { get; set; }
        public SelectList Options_Equipment { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Grab currently selected ids from session
            EquipmentIds = HttpContext.Session.Get<int[]>("EquipmentIds");
            MuscleGroupIds = HttpContext.Session.Get<int[]>("MuscleGroupIds");
            MuscleGroups = await _muscleGroupDb.GetMuscleGroups(MuscleGroupIds);

            ClearExerciseSession();

            // If either session is not present then we know the user is trying to access this page directly
            // So we will redirect them
            if (MuscleGroupIds == null || EquipmentIds == null)
            {
                return RedirectToPage("/MuscleGroup/Index");
            }

            var exercises = await _exerciseDb.GetExercisesFromRequiredEquipment(MuscleGroupIds, EquipmentIds);
            var userExercises = Enumerable.Empty<UserExercise>();

            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                userExercises = await _userExerciseDb.GetUserExercisesFromRequiredEquipment(user.Id, MuscleGroupIds, EquipmentIds);
            }
            ExerciseCount = exercises.Count() + userExercises.Count();

            var equipment = await GetEquipmentFromMuscleGroupIds(MuscleGroupIds);
            SetEquipmentDropDown(equipment);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int[] muscleGroupIds)
        {
            MuscleGroupIds = muscleGroupIds;

            // Get muscle groups to display to user while they select equipment
            MuscleGroups = await _muscleGroupDb.GetMuscleGroups(MuscleGroupIds);

            IEnumerable<Models.Equipment> equipment;
            ExerciseCount = 0;

            GetSessionProperties();

            // Here we get the exercise count
            var exercises = await _exerciseDb.GetExercisesFromRequiredEquipment(MuscleGroupIds, EquipmentIds);
            var userExercises = Enumerable.Empty<UserExercise>();

            // If user is signed in we also grab the user exercises
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                userExercises = await _userExerciseDb.GetUserExercisesFromRequiredEquipment(user.Id, MuscleGroupIds, EquipmentIds);
            }
            ExerciseCount = exercises.Count() + userExercises.Count();

            // This was the full body muscle group option
            // I think we have removed this will be doing something different
            if (MuscleGroupIds[0] == 6)
            {
                equipment = await _equipmentDb.GetEquipment();
            }
            else
            {
                equipment = await GetEquipmentFromMuscleGroupIds(MuscleGroupIds);
            }

            SetEquipmentDropDown(equipment);
            return Page();
        }

        // Returns subset list of equipment that is related to the list of exercises tied to the selected muscle groups
        public async Task<IEnumerable<Models.Equipment>> GetEquipmentFromMuscleGroupIds(int[] muscleGroupIds)
        {
            // Get our standard sets of ids
            int[] exerciseIds = await _exerciseDb.GetExerciseIdsFromMuscleGroups(muscleGroupIds);
            int[] equipmentIds = await _equipmentDb.GetEquipmentIdsFromExercises(exerciseIds);
            // Used to get list of alternate equipment
            int[] exerciseEquipmentIds = await _equipmentDb.GetExerciseEquipmentIdsFromExercises(exerciseIds);

            // Get our user sets of ids
            int[] userExerciseEquipmentIds = { };
            int[] userEquipmentIds = { };
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                int[] userExerciseIds = await _userExerciseDb.GetUserExerciseIdsFromUserExerciseMuscleGroups(user.Id, muscleGroupIds);
                userEquipmentIds = await _equipmentDb.GetEquipmentIdsFromUserExercises(user.Id, userExerciseIds);
                userExerciseEquipmentIds = await _equipmentDb.GetUserExerciseEquipmentIdsFromExercises(exerciseIds);
            }
            // Join the array of exercise equipment ids with the user array of exercise equipment ids
            exerciseEquipmentIds = exerciseEquipmentIds.Concat(userExerciseEquipmentIds).Distinct().ToArray();

            // Get ids of alternate equipment from the exercise equipment ids
            int[] alternateEquipmentIds = await _equipmentDb.GetAlternateEquipmentIdsFromExerciseEquipment(exerciseEquipmentIds);

            // Join the arrays of equipment with user equipment and get distinct set
            int[] fullEquipmentIds = equipmentIds.Concat(userEquipmentIds).Distinct().ToArray();

            // Finally join the alternate equipment ids it the full set
            fullEquipmentIds = fullEquipmentIds.Concat(alternateEquipmentIds).Distinct().ToArray();

            // Get all the equipment based on our final array of equipment ids
            return await _equipmentDb.GetEquipment(fullEquipmentIds);
        }

        public void GetSessionProperties()
        {
            // Attempt to get session variable
            EquipmentIds = HttpContext.Session.Get<int[]>("EquipmentIds");

            // If session does not yet exist, create it and set default
            if (EquipmentIds == null)
            {
                EquipmentIds = new int[0];
                HttpContext.Session.Set("EquipmentIds", EquipmentIds);
            }
        }

        public async Task<JsonResult> OnPostUpdateExerciseCount(int[] muscleGroupIds, int[] equipmentIds)
        {
            var exercises = await _exerciseDb.GetExercisesFromRequiredEquipment(muscleGroupIds, equipmentIds);
            var userExercises = Enumerable.Empty<UserExercise>();

            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                userExercises = await _userExerciseDb.GetUserExercisesFromRequiredEquipment(user.Id, muscleGroupIds, equipmentIds);
            }

            return new JsonResult(exercises.Count() + userExercises.Count());
        }

        // Gets all the equipment records and binds it to the select list object
        // The select list object gets binded to the select 2 drop down
        public void SetEquipmentDropDown(IEnumerable<Models.Equipment> equipment)
        {
            Options_Equipment = new SelectList(equipment, "Id", "Name");
        }

        // Updates the session object for selected equipment
        public void OnPostUpdateEquipmentIdsSession(int[] equipmentIds)
        {
            HttpContext.Session.Set("EquipmentIds", equipmentIds);
        }


        public void ClearExerciseSession()
        {
            HttpContext.Session.Remove("sets");
            HttpContext.Session.Remove("exercise");
            HttpContext.Session.Remove("workout");
        }
    }
}