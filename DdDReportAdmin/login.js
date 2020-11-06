$(function() {
    $("#LoginControl_LoginButton").hide();
    var loginTd = $("#LoginControl_RememberMe").parent();
    loginTd.attr("align", "right")
    var oldText = loginTd.html();
    loginTd.html(oldText + "<img class=\"button\" src=\"images/login.png\">")

    $("input").css("max-width", 141);

    $("img.button").bind("click", function(e) {
        $("#LoginControl_LoginButton").click();
    });

    $("img.button").hover(function(e) {
        $("img.button").attr("className", "hoverbutton");
    }, function(e) {
        $("img.button").attr("className", "button");
    });

    // IE hax
    $("#LoginControl_UserName").keyup(function (e) {
        if (e.keyCode == 13) { // enter
            $("#LoginControl_LoginButton").click();
        }
    });

    $("#LoginControl_Password").keyup(function (e) {
        if (e.keyCode == 13) { // enter
            $("#LoginControl_LoginButton").click();
        }
    });
});
