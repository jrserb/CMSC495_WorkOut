$(document).ready(function () {

    // First loop the muscle group ids and append hidden fields to the form
    // These will need to get passed when the form is posted to the exercises page
    for (let i = 0; i < muscleGroupIds.length; i++) {
        $('#formEquipment').append(`<input type='hidden' name=muscleGroupIds[] value=${muscleGroupIds[i]} />`);
    }

    // Initialize the select 2 drop down for equipment selection
    $('#select2_equipment').select2();

    $('#select2_equipment').on({

        // Event trigger when option is selected from the drop down
        'select2:select': function () {
            UpdateExerciseCount($(this).val());
            UpdateEquipmentIdsSession($(this).val());
        },

        // Event trigger when option is unselected from the drop down
        'select2:unselect': function () {
            UpdateExerciseCount($(this).val());
            UpdateEquipmentIdsSession($(this).val());
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
function UpdateExerciseCount(arrayEquipmentIds) {

    const objRequest = {
        type: "POST",
        url: "Equipment/UpdateExerciseCount",
        data: {
            muscleGroupIds: muscleGroupIds,
            equipmentIds: arrayEquipmentIds
        }
    };

    CallController(objRequest, function (responseData) {
        HideShowExerciseButton(parseInt(responseData));
        $('#eCount').html(responseData);
    });
}

// Updates session with equipment ids
function UpdateEquipmentIdsSession(arrayEquipmentIds) {

    const objRequest = {
        type: "POST",
        url: "Equipment/UpdateEquipmentIdsSession",
        data: {
            equipmentIds: arrayEquipmentIds
        }
    };

    CallController(objRequest, function () {});
}