$(document).ready(function () {

    // Initialize the select 2 drop down for equipment selection
    $('#select2_muscle_groups').select2();
    $('#select2_equipment').select2();

    $('#btnDeleteExercise').on('click', function (e) {

        const exerciseId = $('#UserExercise_Id').attr('id');
        $('#formEditExercise')
            .attr('action', `/Exercises/Edit/${exerciseId}?handler=DeleteExercise`)
            .submit();

    });
});