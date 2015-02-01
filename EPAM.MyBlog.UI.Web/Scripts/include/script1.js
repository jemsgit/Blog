
/// <reference path="../jquery-1.11.2.js" />
/// <reference path="../jquery.unobtrusive-ajax.min.js" />
/// <reference path="../jquery-migrate-1.2.1.js" />



$("#ajaxform").submit(function (e) {
    var formObj = $(this);
    var formURL = formObj.attr("action");
    var formData = new FormData(this);
    $.ajax({
        url: formURL,
        type: 'POST',
        data: formData,
        mimeType: "multipart/form-data",
        contentType: false,
        cache: false,
        processData: false,
        success: function (data) {
            $("#content").html(data);
        }
    });
    e.preventDefault();
    return false;
});

$("#ajaxformfav").submit(function (e) {
    var formObj = $(this);
    var formURL = formObj.attr("action");
    var formData = new FormData(this);
    $.ajax({
        url: formURL,
        type: 'POST',
        data: formData,
        contentType: false,
        cache: false,
        processData: false,
        success: function (data) {
            $("#content").html(data);
        }
    });
    e.preventDefault();
    return false;
});

$("#login_form").submit(function (e) {
    var formObj = $(this);
    var formURL = formObj.attr("action");
    var formData = new FormData(this);
    $.ajax({
        url: formURL,
        type: 'POST',
        data: formData,
        contentType: false,
        cache: false,
        processData: false,
        success: function (data) {
            $("#content").html(data);
            setTimeout(menu_update(), 1000);
        }
    });
    e.preventDefault();
    return false;
});

$("#reg_form").submit(function (e) {
    var formObj = $(this);
    var formURL = formObj.attr("action");
    var formData = new FormData(this);
    $.ajax({
        url: formURL,
        type: 'POST',
        data: formData,
        contentType: false,
        cache: false,
        processData: false,
        success: function (data) {
            $("#content").html(data);
            setTimeout(menu_update(), 1000);
        }
    });
    e.preventDefault();
    return false;
});

$("#logout").submit(function (e) {
    var formObj = $(this);
    var formURL = formObj.attr("action");
    var formData = new FormData(this);
    $.ajax({
        url: formURL,
        type: 'POST',
        data: formData,
        contentType: false,
        cache: false,
        processData: false,
        success: function (data) {
            $("#content").html(data);
            setTimeout(menu_update(), 1000);
        }
    });
    e.preventDefault();
    return false;
});

$("#delete_form").submit(function (e) {
    var formObj = $(this);
    var formURL = formObj.attr("action");
    var formData = new FormData(this);
    $.ajax({
        url: formURL,
        type: 'POST',
        data: formData,
        contentType: false,
        cache: false,
        processData: false,
        success: function (data) {
            $("#content").html(data);
            setTimeout(menu_update(), 1000);
        }
    });
    e.preventDefault();
    return false;
});

function menu_update() {
    $("#menu").html("");
    $.ajax({
        url: '/Account/MenuState',
        type: 'GET',
        success: function (data) {
            $("#menu").html(data);
            setTimeout(title_update(), 10);
        }
    });
}

function title_update() {
    $("#name").html("");
    $.ajax({
    url: '/Account/TitleState',
    type: 'GET',
    success: function (data) {
        $("#name").html(data);
    }
});
}