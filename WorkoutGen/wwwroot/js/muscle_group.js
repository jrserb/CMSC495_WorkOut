$(document).ready(function () {

    // Initialize the select 2 drop downs
    $('#select2_muscle_groups').select2();

    $('#select2_muscle_groups').on('select2:select', function (e) {

        const s2 = $("#select2_muscle_groups");
        const arrayMuscleGroupIds = $(this).val().join();

        if ($.inArray("6", arrayMuscleGroupIds) !== -1) {

            $("option", s2).not(':selected').each(function () {
                $(this).prop('disabled', true);
            });
        }
        else {

            const isDisabled = $("option[value='6']", s2).prop('disabled');

            if (!isDisabled) {
                $("option[value='6']", s2).prop('disabled', true);
            }
        }

    });

    $('#select2_muscle_groups').on('select2:unselect', function (e) {

        const arrayMuscleGroupIds = $(this).val().join();

        // Default to full body if selection is cleared
        if (!arrayMuscleGroupIds) {
            $("#select2_muscle_groups option").each(function () {
                $(this).prop('disabled', false);
            });
        }
    });

    $('#btnClearSelection').on('click', function (e) {
        $('#select2_muscle_groups').val(null).trigger('change');
        $("#select2_muscle_groups option").prop('disabled', false);
    });

});