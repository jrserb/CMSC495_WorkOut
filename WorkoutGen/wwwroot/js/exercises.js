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

    // First thing we do is update the page if needed form session values
    UpdatePageFromSession();

    $('#btnRight').on('click', function () {

        const currentExerciseId = parseInt($('#exerciseName').data("exercise"));
        const currentExerciseIndex = exercises.findIndex(x => x.id === currentExerciseId);
        const nextIndex = (currentExerciseIndex + 1);
        const nextExercise = exercises[nextIndex];

        // If exercises is exceeded then we end workout
        if (nextIndex > exercises.length) {

            $('#modalEndOfExercise').modal({ backdrop: 'static', keyboard: false }, 'show');
            return;
        }

        UpdateExerciseProgressBar(nextIndex, exercises.length);
        UpdateExerciseFields(nextExercise.id, nextExercise.name, nextIndex);
        UpdateExerciseSession(nextIndex);
        GetSetFromSession(nextExercise.id);
    });

    $('#btnLeft').on('click', function () {

        const currentExerciseId = parseInt($('#exerciseName').data("exercise"));
        const currentExerciseIndex = exercises.findIndex(x => x.id === currentExerciseId);
        const prevIndex = (currentExerciseIndex - 1);
        const prevExercise = exercises[prevIndex];

        // If user is at the beginning then do nothing
        if (currentExerciseIndex === 0)
            return;

        UpdateExerciseProgressBar(prevIndex, exercises.length);
        UpdateExerciseFields(prevExercise.id, prevExercise.name, prevIndex);
        UpdateExerciseSession(prevIndex);
        GetSetFromSession(prevExercise.id);

    });

    $('#btnAddSet').on('click', function (e) {

        e.preventDefault();

        const workoutId = parseInt($('#workout').val());
        const exerciseId = parseInt($('#exerciseName').data("exercise"));
        const weight = $('#weight').val();
        const reps = $('#reps').val();

        // Error out if user did not specify weight or reps
        if (!weight || !reps)
            return;

        const objRequest = {
            type: "POST",
            url: "/Exercises/SaveSet",
            data: {
                workoutId: workoutId,
                exerciseId: exerciseId,
                weight: weight,
                reps: reps
            }
        };

        CallController(objRequest, function (responseData) {
            // Append new set to the text area
            const text = `${weight}lbs x ${reps} reps\n`;
            const box = $("#txtSets");
            box.val(box.val() + text);
        });
    });

    $('#btnStartOver').on('click', function () {

        $('#modalEndOfExercise').modal('hide');
        location.reload();

    });

});

function UpdatePageFromSession() {

    // Get the index of the exercise from session
    const exercise = exercises[sessionExerciseIndex];

    UpdateExerciseProgressBar(sessionExerciseIndex, exercises.length);
    UpdateExerciseFields(exercise.id, exercise.name, sessionExerciseIndex);

    // Loop the sets object and populate the text area with the sets for the current exercise the user is on
    $("#txtSets").val("");
    for (var obj in sets) {
        if (parseInt(sets[obj].exerciseId) === parseInt(exercise.id)) {
            const box = $("#txtSets");
            box.val(box.val() + sets[obj].set);
        }
    }
}

// Updates the exercise progress bar
function UpdateExerciseProgressBar(exerciseIndex, exercisesLength) {

    const progress = Math.round((exerciseIndex / exercisesLength) * 100);

    $('.progress-bar').css('width', `${progress}%`);
    $('.progress-bar').text(`${progress}%`);

}

// Updates exercise related info on the page
function UpdateExerciseFields(exerciseId, exerciseName, exerciseIndex) {

    $('#weight').val('');
    $('#reps').val('');
    $('#exerciseName').data("exercise", exerciseId);
    $('#exerciseName').html(exerciseName);
    $('#currentExerciseCount').text(exerciseIndex);
}

function UpdateExerciseSession(exerciseIndex) {

    const objRequest = {
        type: "POST",
        url: "/Exercises/UpdateExerciseSession",
        data: {
            exerciseIndex: exerciseIndex
        }
    };

    CallController(objRequest, function () {

    });
}

function GetSetFromSession(exerciseId) {

    const objRequest = {
        type: "POST",
        url: "/Exercises/GetSetFromSession",
        data: {
            exerciseId: exerciseId
        }
    };

    CallController(objRequest, function (responseData) {

        // Loop the sets object and populate the text area with the sets for the current exercise the user is on

        $("#txtSets").val("");

        for (var obj in responseData) {
            const box = $("#txtSets");
            box.val(box.val() + responseData[obj].set);
        }

    });
}