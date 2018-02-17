$(document).ready(main);

function resize() {
    if ($(window).width() < 480) {
        $("#wrapper").removeClass("active");
    }   
}

function alerta() {
    if ($('.alert').css("display") == "none") {
        $('.alert').show();
        window.setTimeout(function () {
            $(".alert").fadeTo(500).slideUp(500, function () {
                $(this).hide();
            });
        }, 1000);
    }
    else {
        $('.alert').hide();
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

