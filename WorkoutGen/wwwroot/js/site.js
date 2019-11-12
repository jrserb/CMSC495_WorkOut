$(document).ready(function () {

    // Gets the list of equipment records that match the selected muscle group ids
    // Populates equipment drop down
    //function GetEquipmentFromMuscleGroups(arrayMuscleGroupIds) {

    //    const objRequest = {
    //        type: "POST",
    //        url: "/UpdateEquipment",
    //        data: {
    //            muscleGroupIds: arrayMuscleGroupIds
    //        }
    //    };

    //    CallController(objRequest, function (responseData) {

    //        if (responseData) {

    //            // create the option and append to Select2
    //            for (var key in responseData) {

    //                if (responseData.hasOwnProperty(key)) {

    //                    // here you have access to
    //                    let id = responseData[key].id;
    //                    let text = responseData[key].name;

    //                    let option = new Option(text + ' (' + id + ')', id, true, true);

    //                    // Only add new items if they do not exist already
    //                    if ( $('#select2_equipment').find("option[value='" + id + "']").length === 0 )
    //                        $('#select2_equipment').append(option);
    //                }
    //            }

    //            $('#select2_equipment').parent().removeClass('d-none');
    //            $('#select2_equipment').val(null);
    //            //$('#select2_equipment').select2('open');
    //        }

    //    });
    //}

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