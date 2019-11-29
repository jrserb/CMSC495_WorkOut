$(document).ready(function () {

    $('#btnDeleteExercise').on('click', function (e) {

        const exerciseId = $('#UserExercise_Id').attr('id');
        $('#formEditExercise')
            .attr('action', `/Exercises/Edit/${exerciseId}?handler=DeleteExercise`)
            .submit();

    });
});