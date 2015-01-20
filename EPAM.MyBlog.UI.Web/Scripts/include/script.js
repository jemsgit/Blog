/// <reference path="../jquery-2.1.1.min.js" />

function get_page() {
    $.ajax({
        type: "GET",
        url: Links.GetPosts,
        success: function (data) {
            if (data != "") {
                $("content").html(data);
            }
        }
    });
};

var $Post = $("#my_posts")

$Post.click(get_page());