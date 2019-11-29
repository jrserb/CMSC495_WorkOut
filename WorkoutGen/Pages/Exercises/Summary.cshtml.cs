﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WorkoutGen.Data.Session;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkoutGen.Data.Services.UserWorkout;
using WorkoutGen.Data.Services.Exercise;
using WorkoutGen.Data.Services.UserSet;
using Microsoft.AspNetCore.Identity;
using WorkoutGen.Models;
using System.Collections;

namespace WorkoutGen.Pages.Exercises
{
    public class HistoryModel : PageModel
    {
        private readonly IExerciseService _exerciseDb;
        private readonly IUserWorkoutService _userWorkoutDb;
        private readonly IUserSetService _userSetDb;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HistoryModel(IExerciseService exerciseDb,
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
        public List<SessionSet> Sets { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost(int workoutId, int[] exerciseIds)
        {
            Exercises = await _exerciseDb.GetExercises(exerciseIds);
            Sets = GetSession<List<SessionSet>>("Sets");  
            
            HttpContext.Session.Clear();

            return Page();
        }

        public T GetSession<T>(string sessionName)
        {
            return HttpContext.Session.Get<T>(sessionName);
        }
    }
}