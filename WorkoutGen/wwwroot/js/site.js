//Kent's test comment for github pushing

$(document).ready(function () {

    // Initialize the select 2 drop downs
    $('#select2_muscle_groups').select2();
    $('#select2_equipment').select2();

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

    // When we select an equipment option, hide/show the exercise gen button accordingly
    $('#select2_equipment').on({
        'select2:select': function () {
            HideShowExerciseButton($(this));
        },
        'select2:unselect': function () {
            HideShowExerciseButton($(this));
        }
    });

    // Gets the list of equipment records that match the selected muscle group ids
    // Populates equipment drop down
    function GetEquipmentFromMuscleGroups(arrayMuscleGroupIds) {

        const objRequest = {
            type: "POST",
            url: "/UpdateEquipment",
            data: {
                muscleGroupIds: arrayMuscleGroupIds
            }
        };

        CallController(objRequest, function (responseData) {

            if (responseData) {

                // create the option and append to Select2
                for (var key in responseData) {

                    if (responseData.hasOwnProperty(key)) {

                        // here you have access to
                        let id = responseData[key].id;
                        let text = responseData[key].name;

                        let option = new Option(text + ' (' + id + ')', id, true, true);

                        // Only add new items if they do not exist already
                        if ( $('#select2_equipment').find("option[value='" + id + "']").length === 0 )
                            $('#select2_equipment').append(option);
                    }
                }

                $('#select2_equipment').parent().removeClass('d-none');
                $('#select2_equipment').val(null);
                //$('#select2_equipment').select2('open');
            }

        });
    }

    // Based on if any equipment is selected
    function HideShowExerciseButton(equipmentDropDown) {

        if (equipmentDropDown.val().join()) {
            $('#btnGenerateExercise').parent().removeClass('d-none');
        } else {
            $('#btnGenerateExercise').parent().addClass('d-none');
        }
    }

    // Ajax server request handler
    // Returns response data
    function CallController(objRequest, callback) {

        $.ajax({

            type: objRequest.type,
            url: objRequest.url,
            data: objRequest.data,
            contentType: objRequest.contentType,
            dataType: objRequest.dataType,
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() }

        }).done(function (responseData) {

            callback(responseData);

        });

    }

});