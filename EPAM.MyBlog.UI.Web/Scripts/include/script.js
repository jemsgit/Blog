/// <reference path="../jquery-1.11.2.js" />
/// <reference path="../jquery.unobtrusive-ajax.min.js" />
/// <reference path="../jquery-migrate-1.2.1.js" />



$("a#my_posts").click(function () {
    $.ajax({
        type: "GET",
        url: $(this).data('request-url'),
        datatype: 'json',
        success: function s(data1) {
            alert(data1);
        }
    });
    event.preventDefault();
});

