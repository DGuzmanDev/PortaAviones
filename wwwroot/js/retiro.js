var retiros = [];
var retiro_actual = {};

function registrar_evento_modal() {
    $('#modal_confirm').on('show.bs.modal', function (event) {
        let button = event.relatedTarget;
        let tipo_evento = button.getAttribute('data-bs-target-value');
        registrar_evento_limpiar_formulario(tipo_evento);
    });
}

function registrar_evento_limpiar_formulario(tipo) {
    $('#limpiar_confirm').off('click');
    $('#limpiar_confirm').on('click', function (event) {
        retiros = [];
        retiro_actual = {};
        $('#retiros-body').html('<tr><td>No se han ingresado datos</td></tr>');
        var form = $(".needs-validation")[0];
        $(form).removeClass("was-validated");
        form.reset();
        $("#close_confirm").trigger("click");
    });
}

function mostrar_resultado_busqueda() {
    $('#Serie_Info').val(retiro_actual.serie);
    $('#Nombre_Info').val(retiro_actual.nombre);
    $('#Marca_Info').val(retiro_actual.marca);
    $('#Modelo_Info').val(retiro_actual.modelo);
    $('#resultados').show(1000);
}

function registrar_evento_buscar() {
    $('#buscar').on('click', function (event) {
        event.preventDefault();
        $('#resultados').hide(500);
        let serie = $('#Serie').val();

        var form = $("#formulario_busqueda")[0];
        var validForms = form.checkValidity();
        $(form).addClass("was-validated");

        if (!validForms || serie === undefined && serie.trim() === '') {
            animate_feedback("error_formulario_busqueda", 3000, 500, 500);
        } else {
            // TODO: AJAX request al BE
            let avion = {
                serie: "ABCDE-12345",
                nombre: "Velo Zi Raptor",
                marca: "Lockheed Martin",
                modelo: "F-22 Raptor",
                ancho: 15,
                alto: 4,
                largo: 30
            };
            retiro_actual = avion;
            mostrar_resultado_busqueda();
            $(form).removeClass("was-validated");
        }
    });
}

function agregar_avion() {
    var agregado = retiros.find(item => {
        return item.serie == retiro_actual.serie
    }) !== undefined;

    if (!agregado) {
        if (retiros.length === 0) {
            $('#retiros-body').html("");
        }

        retiros.push(retiro_actual);
        var indice = retiros.length - 1;

        var tr = '<tr id="retiro-avion-' + indice + '">'
            + '<td>' + retiro_actual.serie + '</td>'
            + '<td>' + retiro_actual.nombre + '</td>'
            + '<td>' + retiro_actual.marca + '</td>'
            + '<td>' + retiro_actual.modelo + '</td>'
            + '<td>' + retiro_actual.ancho + 'm</td>'
            + '<td>' + retiro_actual.alto + 'm</td>'
            + '<td>' + retiro_actual.largo + 'm</td>'
            + '<td><div class="d-flex justify-content-center">'
            + '<button id="retiro-avion-' + indice + '" type="button" class="btn btn-danger ms-2">'
            + '<i class="far fa-trash-alt"></i></button></div></td>'
            + '</tr>';

        $('#retiros-body').append(tr);

        $("#retiro-avion-" + indice).on("click", function (event) {
            var posAvion = this.id.replace("retiro-avion-", "");
            retiros.splice(posAvion, 1);
            $('#' + this.id).remove();
        });
    } else {
        $('#error_retiro_msg').html("Ya se ha agregado un avión con el mismo número de serie anteriormente");
        animate_feedback("error_retiro", 3000, 500, 500);
    }
}

function registrar_evento_agregar_avion() {
    $("#agregar").on('click', function (event) {
        event.preventDefault();
        let form = $("#formulario_busqueda")[0];
        agregar_avion();
        $(form).removeClass("was-validated");
        form.reset();
        $('#resultados').hide(500);
        retiro_actual = {};
    });
}

function registrar_evento_enviar_formulario() {
    $("#enviar").on('click', function (event) {
        event.preventDefault();

        let form = $("#formulario_detalles")[0];
        $(form).addClass("was-validated");

        if (form.checkValidity() && retiros.length > 0) {
            // TODO enviar el formulario al backend con los datos de todos los aviones ingresados
            $('#retiros-body').html("");
            retiros = [];
            retiro_actual = {};
            animate_feedback("exito_registro", 3000, 500, 500);
            $(form).removeClass("was-validated");
            form.reset();
        } else {
            animate_feedback("error_registro", 3000, 500, 500);
        }
    });
}

$(document).ready(function () {
    console.log('retiro.js JavaScript - Daniel Guzman Chaves - Programación Avanzanda en Web -  UNED IIIQ 2023');
    $('#resultados').hide(0);
    registrar_evento_modal();
    registrar_evento_buscar();
    registrar_evento_agregar_avion();
    registrar_evento_enviar_formulario();
});