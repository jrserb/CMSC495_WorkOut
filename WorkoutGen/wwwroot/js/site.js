/*
    Name: Brett Snyder
    Date: 12/12/2019
    Course: CMSC 495 - Current Trends And Projects in Computer Science
    Desc: Global script page for master page _Layout.cshtml
*/

// Global function that is called from other pages
// Handles Ajax requests to the server and returns data result
function CallController(objRequest, callback) {

    //$.ajax({

    //    type: objRequest.type,
    //    url: objRequest.url,
    //    data: objRequest.data,
    //    contentType: objRequest.contentType,
    //    dataType: objRequest.dataType,
    //    headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() }

    //}).done(function (responseData) {

    //    callback(responseData);

    //});

    $.ajax({

        type: objRequest.type,
        url: objRequest.url,
        data: objRequest.data,
        contentType: objRequest.contentType,
        dataType: objRequest.dataType

    }).done(function (responseData) {

        callback(responseData);

    });
}