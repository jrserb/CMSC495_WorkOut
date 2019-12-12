$(document).ready(function () {

    // Initialize the select 2 drop down for user exercises
    $('#selectUserExercises').select2();
    $('#selectUserEquipmentSets').select2();

    // Initial set of the edit button link. Append the exercise id to the end
    SetEditHrefLink("Exercises", $('#selectUserExercises'));
    SetEditHrefLink("EquipmentSets", $('#selectUserEquipmentSets'));

    $('#selectUserExercises').on({

        // Event trigger when option is selected from the drop down
        'select2:select': function () {

            SetEditHrefLink("Exercises", $(this));
        }
    });

    $('#selectUserEquipmentSets').on({

        // Event trigger when option is selected from the drop down
        'select2:select': function () {

            SetEditHrefLink("EquipmentSets", $(this));
        }
    });
});

//Append the exercise id to the end of the edit button href link
function SetEditHrefLink(page, objSelect2) {

    const editButton = objSelect2.parents('div.selectContainer').find('.btn-warning');
    editButton.attr('href', `/${page}/Edit/${objSelect2.val()}`);
}