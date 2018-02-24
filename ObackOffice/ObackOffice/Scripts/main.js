$(document).ready(main);

function resize() {
    if ($(window).width() < 480) {
        $("#wrapper").removeClass("active");
    }   
}

function alerta(Mensaje,color) {
    if ($('.alert').css("display") == "none") {
        $('.alert').show();
        if (Mensaje != undefined)
            $('.alert span').html(Mensaje);

        var stilo = "";
        if (color != undefined) {
            switch (color) {
                case "negro": {
                    stilo = "alert-dark";
                    break;
                }
                case "blanco": {
                    stilo = "alert-light";
                    break;
                }
                case "rojo": {
                    stilo = "alert-danger";
                    break;
                }
                case "amarillo": {
                    stilo = "alert-warning";
                    break;
                }
                case "azul": {
                    stilo = "alert-info";
                    break;
                }
                case "verde": {
                    stilo = "alert-success";
                    break;
                }
            }
            $('.alert').addClass(stilo);
        }

        window.setTimeout(function () {
            $(".alert").fadeTo(500).slideUp(500, function () {
                if (stilo != "")
                    $('.alert').removeClass(stilo);
                $(this).hide();
            });
        }, 1000);
    }
    else {
        $('.alert').hide();
    }
}

function formatDate(date) {
    var monthNames = [
        "Enero", "Febrero", "Marzo",
        "Abril", "Mayo", "Junio", "Julio",
        "Agosto", "Setiembre", "Octubre",
        "Noviembre", "Diciembre"
    ];

    var day = date.getDate();
    var monthIndex = date.getMonth();
    var year = date.getFullYear();

    return day + ' ' + monthNames[monthIndex] + ' ' + year;
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

function validateNumber(evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    var regex = /[0-9]|\./;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}

