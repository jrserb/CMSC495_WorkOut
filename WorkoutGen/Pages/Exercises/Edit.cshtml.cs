using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkoutGen.Data;
using WorkoutGen.Data.Services.UserExercise;
using WorkoutGen.Models;

namespace WorkoutGen.Pages.Exercises
{
    public class EditModel : PageModel
    {
        private readonly IUserExerciseService _userExerciseDb;

        public EditModel(IUserExerciseService userExerciseDb)
        {
            _userExerciseDb = userExerciseDb;
        }

        [BindProperty]
        public UserExercise UserExercise { get; set; }

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
                _userExerciseDb.UpdateUserExercise(UserExercise);
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
