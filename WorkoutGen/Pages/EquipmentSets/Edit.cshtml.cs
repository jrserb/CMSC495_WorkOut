using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkoutGen.Data.Services.Equipment;
using WorkoutGen.Models;

namespace WorkoutGen.Pages.EquipmentSets
{
    [Authorize]
    public class EditModel : PageModel
    {

        private readonly IEquipmentService _equipmentDb;

        public EditModel(IEquipmentService equipmentDb)
        {
            _equipmentDb = equipmentDb;
        }

        [BindProperty]
        public UserEquipmentSet UserEquipmentSet { get; set; }
        public SelectList Options_Equipment { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "At least 1 piece of equipment is required")]
        public int[] EquipmentIds { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserEquipmentSet = await _equipmentDb.GetUserEquipmentSet((int)id);

            if (UserEquipmentSet == null)
            {
                return NotFound();
            }

            var equipment = await _equipmentDb.GetEquipment();
            Options_Equipment = new SelectList(equipment, "Id", "Name");

            var userSetEquipment = await _equipmentDb.GetEquipmentFromUserEquipmentSet((int)id);
            EquipmentIds = userSetEquipment.Select(x => x.Id).ToArray();

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
                await _equipmentDb.UpdateUserEquipmentSet(UserEquipmentSet, EquipmentIds);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _equipmentDb.UserEquipmentSetExists(UserEquipmentSet.Id))
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

        public async Task<IActionResult> OnPostDeleteEquipmentSet()
        {
            try
            {
                await _equipmentDb.DeleteUserEquipmentSet(UserEquipmentSet, EquipmentIds);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _equipmentDb.UserEquipmentSetExists(UserEquipmentSet.Id))
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