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

//$(document).on("submit", '#login_form', function (e) {
//    var formObj = $(this);
//    var formURL = formObj.attr("action");
//    var formData = new FormData(this);
//    alert("login");
//    console.log("login");
//    load_img_show();
//    $.ajax({
//        url: formURL,
//        type: 'POST',
//        data: formData,
//        mimeType: "multipart/form-data",
//        success: function (data) {
//            $("#content").html(data);
//        }
//    });
//    setTimeout(menu_update(), 1000);
    
//    e.preventDefault();
//    return false;
//});


//function load_img_show() {
//    alert("load_IMG");
//    console.log("IMG");
//    $("#loading").show();
//    setTimeout(function () {
//        $("#loading").hide();
//    }, 1000);
//}



//var histAPI = !!(window.history && history.pushState);
//function popstate(url) {
//    if (url.indexOf("Page") == -1) {
//        ajaxInProfile(url);
//    } else {
//        ajaxInWrapperContent(url);
//    }
//}

//window.onload = function () {
//    if (histAPI) {
//        window.setTimeout(function () {
//            window.addEventListener("popstate",
//                function () {
//                    popstate(location.pathname);
//                }, false);
//        }, 1);
//    }
//}



//function menu_update() {
//    alert("Update");
//    console.log("Update");
//    $("#menu").html("");
//    $.ajax({
//        url: '/Account/MenuState',
//        type: 'GET',
//        success: function (data) {
//            $("#menu").html(data);
//        }
//    });
//}

