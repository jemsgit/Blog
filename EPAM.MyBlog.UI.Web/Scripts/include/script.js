/// <reference path="../jquery-1.11.2.js" />

(function get_page() {
    $.ajax({
        type: "GET",
        url: Links.GetPosts,
        success: function (data) {
            if (data != "") {
                $("content").html(data);
            }
        }
    });
})();

//var $Post = $("#my_posts")

//$Post.click(get_page());