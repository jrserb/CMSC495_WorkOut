﻿$(document).ready(function () {

    // First thing we do is update the page if needed form session values  
    UpdatePageFromSession();

    $('#btnRight').on('click', function () {

        //const currentExerciseId = parseInt($('#exerciseName').data("exercise"));
        //const currentExerciseIndex = exercises.findIndex(x => x.id === currentExerciseId);
        const currentExerciseIndex = sessionExerciseIndex;

        const nextIndex = (currentExerciseIndex + 1);
        sessionExerciseIndex = nextIndex;
        const nextExercise = mergedExercises[nextIndex];

        $('#currentExerciseCount').text(nextIndex);
        UpdateExerciseProgressBar(nextIndex);
        ClearSetFields();

        // If exercises are completed then we end workout
        if (nextIndex === mergedExercises.length) {
            $('#modalEndOfExercise').modal({ backdrop: 'static', keyboard: false }, 'show');
            return;
        }

        //Update page with the details for next exercise       
        SetExerciseFields(nextExercise);     
        SetSetsFromSession(nextExercise.id);
        UpdateHowTo(nextIndex);
        UpdateExerciseSessions(nextIndex);
    });

    $('#btnLeft').on('click', function () {

        //const currentExerciseId = parseInt($('#exerciseName').data("exercise"));
        //const currentExerciseIndex = exercises.findIndex(x => x.id === currentExerciseId);
        const currentExerciseIndex = sessionExerciseIndex;

        const prevIndex = (currentExerciseIndex - 1);
        sessionExerciseIndex = prevIndex;
        const prevExercise = mergedExercises[prevIndex];

        // If user is at the beginning then do nothing
        if (currentExerciseIndex === 0)
            return;

        $('#currentExerciseCount').text(prevIndex);

        UpdateExerciseProgressBar(prevIndex);     
        ClearSetFields();
        SetExerciseFields(prevExercise);         
        SetSetsFromSession(prevExercise.id);
        UpdateHowTo(prevIndex);
        UpdateExerciseSessions(prevIndex);
    });

    $('#btnAddSet').on('click', function (e) {

        const workoutId = parseInt($('#workout').val());
        const exerciseId = parseInt($('#exerciseName').data("exercise"));
        const weight = $('#weight').val();
        const reps = $('#reps').val();

        $('#weight').removeClass("border border-danger");
        $('#reps').removeClass("border border-danger");

        // Error out if user did not specify weight AND reps
        if (!weight || !reps) {
            $('#weight').addClass("border border-danger");
            $('#reps').addClass("border border-danger");
            return;
        }

        const objRequest = {
            type: "POST",
            url: "/Exercises/SaveSet",
            data: {
                isUserExercise: $('#exerciseName').data("user"),
                workoutId: workoutId,
                exerciseId: exerciseId,
                weight: weight,
                reps: reps
            }
        };

        CallController(objRequest, function () {});

        // Append new set to the text area
        const text = `${weight}lbs x ${reps} reps\n`;
        const box = $("#txtSets");
        box.val(box.val() + text);
    });

    $('#btnClearSet').on('click', function (e) {

        $('#weight').val('');
        $('#reps').val('');
        $('#txtSets').val('');

        const currentExerciseId = parseInt($('#exerciseName').data("exercise"));

        const objRequest = {
            type: "POST",
            url: "/Exercises/ClearSet",
            data: {
                isUserExercise: $('#exerciseName').data("user"),
                exerciseId: currentExerciseId
            }
        };

        CallController(objRequest, function () {});
    });

    $('#btnStartOver').on('click', function () {

        $('#modalEndOfExercise').modal('hide');
        $('#currentExerciseCount').text(0);

        UpdateExerciseProgressBar(0);
        ClearSetFields();
        SetExerciseFields(mergedExercises[0]);      
        SetSetsFromSession(mergedExercises[0].id);
        UpdateHowTo(0);
        sessionExerciseIndex = 0;
        UpdateExerciseSessions(0);  
    });

    $('#btnEndWorkout').on('click', function () {
   
        // Get all the ids of all the exercises for the workout and stick them in the form element
        for (let i = 0; i < userExercises.length; i++) {
            $('#formEndWorkout').append(`<input type='hidden' name=userExerciseIds[] value=${userExercises[i].id} />`);
        }
        for (let i = 0; i < exercises.length; i++) {
            $('#formEndWorkout').append(`<input type='hidden' name=exerciseIds[] value=${exercises[i].id} />`);
        }

        $('#modalEndOfExercise').modal('hide');
        $('#formEndWorkout').submit();
    });

    $('#btnHowTo').on('click', function (e) {

        const currentExerciseId = parseInt($('#exerciseName').data("exercise"));

        const objRequest = {
            type: "POST",
            url: "/Exercises/GetMuscleGroupsEquipmentFromExercise",
            data: {
                isUserExercise: $('#exerciseName').data("user"),
                exerciseId: currentExerciseId
            }
        };

        CallController(objRequest, function (responseData) {

            const obj = JSON.parse(responseData);
            $('#exerciseEditMuscleGroups').html('');
            $('#exerciseEditEquipment').html('');

            $.each(obj.MuscleGroups, function (index, value) {
                $('#exerciseEditMuscleGroups').append(`<span class="badge badge-dark p-2 m-1">${value.Name}</span>`);
            });

            if (obj.Equipment.length === 0) {

                $('#exerciseEditEquipment').html('No equipment');

            } else {

                $.each(obj.Equipment, function (index, value) {
                    $('#exerciseEditEquipment').append(`<span class="badge badge-dark p-2 m-1">${value.Name}</span>`);
                });
            }

            $('#modalHowTo').modal('show');
           
        });
    });

});

function UpdatePageFromSession() {

    // Get the index of the exercise in session
    const exercise = mergedExercises[sessionExerciseIndex];

    // Update progress bar
    UpdateExerciseProgressBar(sessionExerciseIndex);

    // Clear out set fields
    ClearSetFields();

    // Update exrcise fields with current exercise data
    SetExerciseFields(exercise);

    // Update exercise count beneath the progress bar
    $('#currentExerciseCount').text(sessionExerciseIndex);

    // Loop the sets object and populate the text area with the sets for the current exercise the user is on
    $("#txtSets").val("");
    for (var obj in sets) {
        if (parseInt(sets[obj].exerciseId) === parseInt(exercise.id) || parseInt(sets[obj].userExerciseId) === parseInt(exercise.id)) {
            const box = $("#txtSets");
            box.val(box.val() + sets[obj].set);
        }
    }

    UpdateHowTo(sessionExerciseIndex);
}

// Updates the exercise progress bar
function UpdateExerciseProgressBar(exerciseIndex) {

    const progress = Math.round((exerciseIndex / mergedExercises.length) * 100);
    $('.progress-bar').css('width', `${progress}%`);
    $('.progress-bar').text(`${progress}%`);

}

// Clears out set input fields
function ClearSetFields() {

    $('#weight').val('');
    $('#reps').val('');
}

// Updates exercise fields with current exercise data
function SetExerciseFields(exercise) {

    $('#exerciseName').data("exercise", exercise.id);
    $('#exerciseName').html(exercise.name); 

    $('#exerciseName').data("user", false);
    if (exercise.hasOwnProperty('userId')) {
        $('#exerciseName').data("user", true);
    }
}

// Updates how to modal popup with current exercise data
function UpdateHowTo(exeriseIndex) {

    const exercise = mergedExercises[exeriseIndex];

    $('#howToExercise').text(exercise.name);
    $('#howToContent').html(exercise.description);
    $('#howToLink').attr('href', exercise.hyperlink);

    if ($('#howToLink').hasClass('d-none')) {
        $('#howToLink').removeClass('d-none');
    }

    if (!exercise.hyperlink) {
        $('#howToLink').addClass('d-none');
    }
}

// Updates the session variable for keeping track of the current exercise the user is on
function UpdateExerciseSessions(exerciseIndex) {

    const objRequest = {
        type: "POST",
        url: "/Exercises/UpdateExerciseSessions",
        data: {
            isUserExercise: $('#exerciseName').data("user"),
            exerciseIndex: exerciseIndex
        }
    };

    CallController(objRequest, function () {});
}

// Updates set fields from session variable
function SetSetsFromSession(exerciseId) {

    const objRequest = {
        type: "POST",
        url: "/Exercises/GetSets",
        data: {
            isUserExercise: $('#exerciseName').data("user"),
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