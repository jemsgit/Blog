/// <reference path="../jquery-1.11.2.js" />
/// <reference path="../jquery.unobtrusive-ajax.min.js" />
/// <reference path="../jquery-migrate-1.2.1.js" />



$("a:not('.nojs')").click(function (e) {
    $.ajax({
        type: "GET",
        url: $(this).attr('href'),
        success: function (response) {
            $("#content").html(response);
        }
    });
    event.returnValue = false;
});