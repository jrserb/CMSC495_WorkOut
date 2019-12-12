using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkoutGen.Data.Services.Equipment;

namespace WorkoutGen.Pages.EquipmentSets
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IEquipmentService _equipmentDb;

        public CreateModel(IEquipmentService equipmentDb) {
            _equipmentDb = equipmentDb;
        }

        [BindProperty]
        public Models.UserEquipmentSet UserEquipmentSet { get; set; }

        public SelectList Options_Equipment { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "At least 1 piece of equipment is required")]
        public int[] EquipmentIds { get; set; }

        public async Task<IActionResult> OnGet()
        {
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

            int equipmentSetId = await _equipmentDb.AddUserEquipmentSet(UserEquipmentSet, EquipmentIds);

            return RedirectToPage("/Account/Manage/MyAccount", new { area = "Identity" });
        }
    }
}