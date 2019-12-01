using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkoutGen.Data.Services.Equipment;
using WorkoutGen.Data.Services.MuscleGroup;
using WorkoutGen.Data.Services.UserExercise;
using WorkoutGen.Models;

namespace WorkoutGen.Pages.Exercises
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IMuscleGroupService _muscleGroupDb;
        private readonly IEquipmentService _equipmentDb;
        private readonly IUserExerciseService _userExerciseDb;

        public CreateModel(IMuscleGroupService muscleGroupDb,
            IEquipmentService equipmentDb,
            IUserExerciseService userExerciseDb)
        {
            _muscleGroupDb = muscleGroupDb;
            _equipmentDb = equipmentDb;
            _userExerciseDb = userExerciseDb;
        }

        [BindProperty]
        public UserExercise UserExercise { get; set; }

        public SelectList Options_MuscleGroups { get; set; }

        public SelectList Options_Equipment { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "At least 1 muscle Group is required")]
        public int[] MuscleGroupIds { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "At least 1 equipment is required")]
        public int[] EquipmentIds { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var muscleGroups = await _muscleGroupDb.GetMuscleGroups();
            Options_MuscleGroups = new SelectList(muscleGroups, "Id", "Name");

            var equipment = await _equipmentDb.GetEquipment();
            Options_Equipment = new SelectList(equipment, "Id", "Name");

            return Page();
        }   

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            foreach (int id in MuscleGroupIds) {

                UserExercise.UserExerciseMuscleGroup.Add(new UserExerciseMuscleGroup
                {
                    UserId = UserExercise.UserId,
                    MuscleGroupId = id,
                    UserExerciseId = UserExercise.Id
                });
            }
            foreach (int id in EquipmentIds)
            {
                UserExercise.UserExerciseEquipment.Add(new UserExerciseEquipment
                {
                    UserId = UserExercise.UserId,
                    EquipmentId = id,
                    UserExerciseId = UserExercise.Id
                });
            }

            UserExercise.Description = UserExercise.Description.Replace(Environment.NewLine, "<br/>");

            await _userExerciseDb.AddUserExercise(UserExercise);

            return RedirectToPage("/Account/Manage/MyAccount", new { area = "Identity" });
        }
    }
}
