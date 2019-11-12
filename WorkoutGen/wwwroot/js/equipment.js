$(document).ready(function () {

    // Initialize the select 2 drop downs
    $('#select2_equipment').select2();

    // When we select an equipment option, hide/show the exercise gen button accordingly
    $('#select2_equipment').on({
        'select2:select': function () {
            HideShowExerciseButton($(this));
        },
        'select2:unselect': function () {
            HideShowExerciseButton($(this));
        }
    });

    // Based on if any equipment is selected
    function HideShowExerciseButton(equipmentDropDown) {

        if (equipmentDropDown.val().join()) {
            $('#btnGenerateExercise').parent().removeClass('d-none');
        } else {
            $('#btnGenerateExercise').parent().addClass('d-none');
        }
    }
});