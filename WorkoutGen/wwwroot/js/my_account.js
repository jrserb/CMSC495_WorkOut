$(document).ready(function () {

    // Initialize the select 2 drop down for user exercises
    $('#selectUserExercises').select2();

    // Initial set of the edit button link. Append the exercise id to the end
    SetEditHrefLink($('#selectUserExercises').val());

    $('#selectUserExercises').on({

        // Event trigger when option is selected from the drop down
        'select2:select': function () {

            SetEditHrefLink($(this).val());
        }
    });
});

//Append the exercise id to the end of the edit button href link
function SetEditHrefLink(exerciseId) {
    $('#btnEditExercise').attr('href', `/Exercises/Edit/${exerciseId}`);
}