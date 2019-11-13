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