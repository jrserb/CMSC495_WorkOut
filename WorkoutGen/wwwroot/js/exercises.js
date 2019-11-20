$(document).ready(function () {

    //$('#btnRight, #btnLeft').on('click', function () {

    //    let item = exercises[Math.floor(Math.random() * exercises.length)];
    //    const exerciseId = parseInt($('#exerciseName').data("exercise"));

    //    while (exerciseId === item.id) {
    //        item = exercises[Math.floor(Math.random() * exercises.length)];
    //    }

    //    $('#exerciseName').data("exercise", item.id);
    //    $('#exerciseName').html(item.name);

    //});

    $('#btnRight').on('click', function () {

        const currentExerciseId = parseInt($('#exerciseName').data("exercise"));
        const currentExerciseIndex = exercises.findIndex(x => x.id === currentExerciseId);

        const progress = Math.round( ((currentExerciseIndex+1) / exercises.length) * 100);
        $('.progress-bar').css('width', `${progress}%`);
        $('.progress-bar').text(`${progress}%`);

        $('#currentExerciseCount').text(currentExerciseIndex+1);

        if (currentExerciseIndex + 1 === exercises.length) {

            $('#modalEndOfExercise').modal({ backdrop: 'static', keyboard: false }, 'show');
            return;
        }
            

        const nextExercise = exercises[currentExerciseIndex + 1];

        $('#exerciseName').data("exercise", nextExercise.id);
        $('#exerciseName').html(nextExercise.name);

    });

    $('#btnLeft').on('click', function () {

        const currentExerciseId = parseInt($('#exerciseName').data("exercise"));
        const currentExerciseIndex = exercises.findIndex(x => x.id === currentExerciseId);    

        if (currentExerciseIndex === 0)
            return;

        const progress = Math.round(((currentExerciseIndex - 1) / exercises.length) * 100);
        $('.progress-bar').css('width', `${progress}%`);
        $('.progress-bar').text(`${progress}%`);

        $('#currentExerciseCount').text(currentExerciseIndex - 1);

        const prevExercise = exercises[currentExerciseIndex - 1];

        $('#exerciseName').data("exercise", prevExercise.id);
        $('#exerciseName').html(prevExercise.name);

    });

    $('#btnStartOver').on('click', function () {

        $('#modalEndOfExercise').modal('hide');
        location.reload();

    });

});