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

    $('#btnHowTo').on('click', function () {

        const currentExerciseId = parseInt($('#exerciseName').data("exercise"));
        const currentExerciseIndex = exercises.findIndex(x => x.id === currentExerciseId);
        const exercise = exercises[currentExerciseIndex];

        $('#howToExercise').text(exercise.name);
        $('#howToContent').html(exercise.description);
        $('#howToLink').attr('href', exercise.hyperlink);
        $('#modalHowTo').modal('show');
    });

    $('#btnRight').on('click', function () {

        const currentExerciseId = parseInt($('#exerciseName').data("exercise"));
        const currentExerciseIndex = exercises.findIndex(x => x.id === currentExerciseId);
        const nextIndex = (currentExerciseIndex + 1);
        const nextExercise = exercises[nextIndex];

        UpdateExerciseProgressBar(nextIndex, exercises.length);

        $('#currentExerciseCount').text(nextIndex);

        // If exercises is exceeded then we end workout
        if (nextIndex === exercises.length) {
            $('#modalEndOfExercise').modal({ backdrop: 'static', keyboard: false }, 'show');
            return;
        }

        UpdateExerciseSession(nextIndex);
        UpdateExerciseFields(nextExercise.id, nextExercise.name, nextIndex);      
        GetSet(nextExercise.id);

    });

    $('#btnLeft').on('click', function () {

        const currentExerciseId = parseInt($('#exerciseName').data("exercise"));
        const currentExerciseIndex = exercises.findIndex(x => x.id === currentExerciseId);
        const prevIndex = (currentExerciseIndex - 1);
        const prevExercise = exercises[prevIndex];

        // If user is at the beginning then do nothing
        if (currentExerciseIndex === 0)
            return;

        $('#currentExerciseCount').text(prevIndex);

        UpdateExerciseProgressBar(prevIndex, exercises.length);
        UpdateExerciseSession(prevIndex);
        UpdateExerciseFields(prevExercise.id, prevExercise.name);        
        GetSet(prevExercise.id);

    });

    $('#btnAddSet').on('click', function (e) {

        e.preventDefault();

        const workoutId = parseInt($('#workout').val());
        const exerciseId = parseInt($('#exerciseName').data("exercise"));
        const weight = $('#weight').val();
        const reps = $('#reps').val();


        $('#weight').removeClass("border border-danger");
        $('#reps').removeClass("border border-danger");

        // Error out if user did not specify weight or reps
        if (!weight || !reps) {
            $('#weight').addClass("border border-danger");
            $('#reps').addClass("border border-danger");
            return;
        }

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

    $('#btnClearSet').on('click', function (e) {

        e.preventDefault();

        $('#weight').val('');
        $('#reps').val('');
        $('#txtSets').val('');

        const currentExerciseId = parseInt($('#exerciseName').data("exercise"));

        const objRequest = {
            type: "POST",
            url: "/Exercises/ClearSet",
            data: {
                exerciseId: currentExerciseId
            }
        };

        CallController(objRequest, function () { });
    });

    $('#btnStartOver').on('click', function () {

        $('#modalEndOfExercise').modal('hide');

        $('#currentExerciseCount').text(0);

        UpdateExerciseProgressBar(0, exercises.length);
        UpdateExerciseFields(exercises[0].id, exercises[0].name);
        UpdateExerciseSession(0);
        GetSet(exercises[0].id);
    });

    $('#btnEndWorkout').on('click', function () {

        $('#modalEndOfExercise').modal('hide');

        const ids = exercises.map(exercises => exercises.id);
        $('#exerciseIds').val(ids);

        $('#formEndWorkout').submit(); 
    });

});

function UpdatePageFromSession() {

    // Get the index of the exercise from session
    const exercise = exercises[sessionExerciseIndex];

    UpdateExerciseProgressBar(sessionExerciseIndex, exercises.length);
    UpdateExerciseFields(exercise.id, exercise.name, sessionExerciseIndex);

    $('#currentExerciseCount').text(sessionExerciseIndex);

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
function UpdateExerciseFields(exerciseId, exerciseName) {

    $('#weight').val('');
    $('#reps').val('');
    $('#exerciseName').data("exercise", exerciseId);
    $('#exerciseName').html(exerciseName);   
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

function GetSet(exerciseId) {

    const objRequest = {
        type: "POST",
        url: "/Exercises/GetSet",
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