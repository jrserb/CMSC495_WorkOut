using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkoutGen.Data;
using WorkoutGen.Data.Services.Equipment;
using WorkoutGen.Data.Services.MuscleGroup;
using WorkoutGen.Data.Services.UserExercise;
using WorkoutGen.Models;

namespace WorkoutGen.Pages.Exercises
{
    public class EditModel : PageModel
    {
        private readonly IUserExerciseService _userExerciseDb;
        private readonly IEquipmentService _equipmentDb;
        private readonly IMuscleGroupService _muscleGroupDb;

        public EditModel(IUserExerciseService userExerciseDb, 
            IEquipmentService equipmentDb,
            IMuscleGroupService muscleGroupDb)
        {
            _userExerciseDb = userExerciseDb;
            _muscleGroupDb = muscleGroupDb;
            _equipmentDb = equipmentDb;
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


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserExercise = await _userExerciseDb.GetUserExercise((int)id);

            if (UserExercise == null)
            {
                return NotFound();
            }

            var muscleGroups = await _muscleGroupDb.GetMuscleGroups();
            Options_MuscleGroups = new SelectList(muscleGroups, "Id", "Name");

            var exerciseMuscleGroups = await _userExerciseDb.GetUserExerciseMuscleGroupsFromExercise((int)id);
            MuscleGroupIds = exerciseMuscleGroups.Select(x => x.MuscleGroupId).ToArray();

            var equipment = await _equipmentDb.GetEquipment();
            Options_Equipment = new SelectList(equipment, "Id", "Name");

            var exerciseEquipment = await _userExerciseDb.GetUserExerciseEquipmentFromExercise((int)id);
            EquipmentIds = exerciseEquipment.Select(x => x.EquipmentId).ToArray();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                foreach (int id in MuscleGroupIds)
                {

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

                await _userExerciseDb.UpdateUserExercise(UserExercise);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _userExerciseDb.UserExerciseExists(UserExercise.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Account/Manage/MyAccount", new { area = "Identity" });
        }

        public async Task<IActionResult> OnPostDeleteExercise()
        {
            try
            {
                await _userExerciseDb.DeleteUserExercise(UserExercise);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _userExerciseDb.UserExerciseExists(UserExercise.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Account/Manage/MyAccount", new { area = "Identity" });
        }
    }
}
