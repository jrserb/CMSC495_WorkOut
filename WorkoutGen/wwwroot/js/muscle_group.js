$(document).ready(function () {

    // Initialize the select 2 drop downs
    $('#select2_muscle_groups').select2();

    // When we select a muscle group option, populate the equipment drop down accordingly
    $('#select2_muscle_groups').on('select2:select', function (e) {

        //const arrayMuscleGroupIds = $(this).val().join();
        //GetEquipmentFromMuscleGroups(arrayMuscleGroupIds);

    });

    // When we deselect a muscle group option, populate the equipment drop down accordingly
    $('#select2_muscle_groups').on('select2:unselect', function (e) {

        //$('#select2_equipment').html('');

        //const arrayMuscleGroupIds = $(this).val().join();

        //if (!arrayMuscleGroupIds) {
        //$('#select2_equipment').parent().addClass('d-none');
        //$('#btnGenerateExercise').parent().addClass('d-none');
        //return;
        //}

        //GetEquipmentFromMuscleGroups(arrayMuscleGroupIds);

    });
});