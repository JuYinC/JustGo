//clock
// $('#checkbox').prop('checked', true);
if ($('#navi-toggle').prop('checked', true)) {
    $("html").addClass("noscroll");
}else{
    $("html").addClass("fixWindow"); 
}

//unclock

if ($('#navi-toggle').prop('checked', false)) {
     $("html").removeClass("noscroll");
 }else{
    $("html").removeClass("fixWindow");
}



var navbar = document.getElementById("navbarC");
var menu = document.getElementById("navbarC__menu");

window.onscroll = function () {
    if (window.pageYOffset >= menu.offsetTop) {
        console.log("YYYYY")
        navbar.classList.add("sticky");
    }
    else {
        navbar.classList.remove("sticky");
    }
}

 