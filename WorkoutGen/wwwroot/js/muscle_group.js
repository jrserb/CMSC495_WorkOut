﻿/*
    Name: Brett Snyder
    Date: 12/12/2019
    Course: CMSC 495 - Current Trends And Projects in Computer Science
    Desc: Script page for MuscleGroup/index.cshtml
*/

$(document).ready(function () {

    // Initialize the select 2 drop down for muscle group selection
    $('#select2_muscle_groups').select2();

    $('#select2_muscle_groups').on({

        // Event trigger when option is selected from the drop down
        'select2:select': function () {

            const s2 = $("#select2_muscle_groups");
            const arrayMuscleGroupIds = $(this).val(); // Join the selected ids into an array

            // If full body was selected ( 6 is the id )
            // Else something else was selected
            if ($.inArray("6", arrayMuscleGroupIds) !== -1) {

                // Disable all other options in the drop down
                $("option", s2).not(':selected').each(function () {
                    $(this).prop('disabled', true);
                });
            }
            else {

                const isDisabled = $("option[value='6']", s2).prop('disabled');

                // If not yet disabled, disable full body option
                if (!isDisabled) {
                    $("option[value='6']", s2).prop('disabled', true);
                }
            }

            HideShowContinueButton();
            UpdateMuscleGroupSession(arrayMuscleGroupIds);
        },

        // Event trigger when option is unselected from the drop down
        'select2:unselect': function () {

            const arrayMuscleGroupIds = $(this).val(); // Join the selected ids into an array

            // If there are no items selected
            if (!arrayMuscleGroupIds) {

                // Enable all of the select options
                $("#select2_muscle_groups option").prop('disabled', false);
            }

            HideShowContinueButton();
            UpdateMuscleGroupSession(arrayMuscleGroupIds);
        }
    });

    // Event trigger when clear selection button is clicked
    $('#btnClearSelection').on('click', function (e) {

        // Clear out and enable all the selected options
        $('#select2_muscle_groups').val(null).trigger('change');
        $("#select2_muscle_groups option").prop('disabled', false);
        $("#btnContinue").addClass("d-none");

        UpdateMuscleGroupSession($('#select2_muscle_groups').val());

    });

    // Event trigger when select all button is clicked
    $('#btnSelectAll').on('click', function (e) {

        $("#select2_muscle_groups > option").prop("selected", "selected");
        $("#select2_muscle_groups").trigger("change");

        HideShowContinueButton();
        UpdateMuscleGroupSession($('#select2_muscle_groups').val());
    });

});

// Hide/Show continue button
function HideShowContinueButton() {

    const selectedCount = $('#select2_muscle_groups option:selected').length;

    // If no options are selected then hide the button
    // Else show
    if (selectedCount === 0) {
        $('#btnContinue').addClass('d-none');
    } else {
        $('#btnContinue').removeClass('d-none');
    }
}

// Updates exercise count based on equipment selected
function UpdateMuscleGroupSession(arrayMuscleGroupIds) {

    const objRequest = {
        type: "POST",
        url: "MuscleGroup/UpdateMuscleGroupSession",
        data: {
            muscleGroupIds: arrayMuscleGroupIds
        }
    };

    CallController(objRequest, function () {});
}