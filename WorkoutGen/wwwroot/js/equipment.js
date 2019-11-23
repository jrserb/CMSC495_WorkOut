$(document).ready(function () {

    for (let i = 0; i < muscleGroupIds.length; i++) {
        $('#formEquipment').append(`<input type='hidden' name=muscleGroupIds[] value=${muscleGroupIds[i]} />`);
    }

    // Initialize the select 2 drop down for equipment selection
    $('#select2_equipment').select2();

    $('#select2_equipment').on({

        // Event trigger when option is selected from the drop down
        'select2:select': function () {
            UpdateExerciseCount($(this));
        },

        // Event trigger when option is unselected from the drop down
        'select2:unselect': function () {
            UpdateExerciseCount($(this));
        }

    });

});

// Hide/Show continue button
function HideShowExerciseButton(exerciseCount) {

    if (exerciseCount === 0) {
        $('#btnGenerateExercise').addClass('d-none');
    } else {
        $('#btnGenerateExercise').removeClass('d-none');
    }
}

// Updates exercise count based on equipment selected
function UpdateExerciseCount(equipmentDropDown) {

    const objRequest = {
        type: "POST",
        url: "Equipment/UpdateExerciseCount",
        data: {
            muscleGroupIds: muscleGroupIds,
            equipmentIds: equipmentDropDown.val()
        }
    };

    CallController(objRequest, function (responseData) {
        HideShowExerciseButton(parseInt(responseData));
        $('#eCount').html(responseData);
    });
}