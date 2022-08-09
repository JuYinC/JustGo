// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

window.onscroll = function() {
    if (window.pageYOffset >= navbarC__menu.offsetTop) {

        navbarC.classList.add("sticky");
    }
    else {
        navbarC.classList.remove("sticky");
    }
}
