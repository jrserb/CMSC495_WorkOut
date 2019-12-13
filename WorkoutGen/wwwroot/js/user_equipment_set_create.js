/*
    Name: Brett Snyder
    Date: 12/12/2019
    Course: CMSC 495 - Current Trends And Projects in Computer Science
    Desc: Script page for equipmentsets/create.cshtml
*/

$(document).ready(function () {

    // Initialize the select 2 drop down for equipment selection
    $('#select2_equipment').select2();

    // Event trigger when clear selection button is clicked
    $('.btnClearSelection').on('click', function (e) {

        const objSelect2 = $(this).parents('.form-group').children('select');

        objSelect2.val(null).trigger('change');
        $("option", objSelect2).prop('disabled', false);

    });

    // Event trigger when select all button is clicked
    $('.btnSelectAll').on('click', function (e) {

        const objSelect2 = $(this).parents('.form-group').children('select');

        $("option", objSelect2).prop("selected", "selected");
        objSelect2.trigger("change");

    });
});