﻿$(document).ready(main);

function resize() {
    if ($(window).width() < 480) {
        $("#wrapper").removeClass("active");
    }   
}

function main() {

    //Mostramos y ocultamos submenus
    $('.submenu').click(function () {
        $(this).children('.children').slideToggle();
    });

    $(".bt-menu").on('click',function (e) {
        e.preventDefault();
        $("#wrapper").toggleClass("active");
    });

    $("#click-registro-prueba").on('click', function (e) {
        e.preventDefault();
        $(".subtable").toggleClass("mostrar-tabla");
    });

    $(window).resize(resize);
    resize();

}

