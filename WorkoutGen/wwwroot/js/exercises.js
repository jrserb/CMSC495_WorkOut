$(document).ready(function () {

    $('#btnRight, #btnLeft').on('click', function () {

        let item = exercises[Math.floor(Math.random() * exercises.length)];
        const exerciseId = parseInt($('#exerciseName').data("exercise"));

        while (exerciseId === item.id) {
            item = exercises[Math.floor(Math.random() * exercises.length)];
        }

        $('#exerciseName').data("exercise", item.id);
        $('#exerciseName').html(item.name);

    });
});