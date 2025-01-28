$(document).ready(function () {

    $(".PlatformMainContent").find("label.col-form-label").each(function (index, node) {
        var htm = $(node).html();
        $(node).html(`<span class="font-weight-boldest">${htm}<span>`);
    });


    $(".PlatformMainContent").find("button.btn").addClass("font-weight-boldest");

});