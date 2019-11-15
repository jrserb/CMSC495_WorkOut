$(document).ready(function () {

    // Initialize the select 2 drop down for equipment selection
    $('#select2_equipment').select2();

    $('#select2_equipment').on({

        // Event trigger when option is selected from the drop down
        'select2:select': function () {
            HideShowExerciseButton();
            UpdateExerciseCount($(this));
        },

        // Event trigger when option is unselected from the drop down
        'select2:unselect': function () {
            HideShowExerciseButton();
            UpdateExerciseCount($(this));
        }

    });
});

// Hide/Show continue button
function HideShowExerciseButton() {

    const selectedCount = $('#select2_equipment option:selected').length;

    // If no options are selected then hide the button
    // Else show
    if (selectedCount === 0) {
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
            equipmentIds: equipmentDropDown.val()
        }
    };

    CallController(objRequest, function (responseData) {
        $('#eCount').html(responseData);
    });
}