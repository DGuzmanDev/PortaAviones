var retiro = {
    tecnico: undefined,
    razon: undefined,
    aeronaves: []
}
var retiro_actual = {};

function registrar_evento_modal() {
    $('#modal_confirm').on('show.bs.modal', function (event) {
        let button = event.relatedTarget;
        let tipo_evento = button.getAttribute('data-bs-target-value');
        registrar_evento_limpiar_formulario(tipo_evento);
    });
}

function limpiar_formulario() {
    retiro = {
        tecnico: undefined,
        razon: undefined,
        aeronaves: []
    }
    retiro_actual = {};
    $('#retiros-body').html('');
    $('#resultados').hide(500);

    let forms = $(".needs-validation");
    for (let index = 0; index < forms.length; index++) {
        var form = forms[index];
        $(form).removeClass("was-validated");
        form.reset();
    }
}

function registrar_evento_limpiar_formulario(tipo) {
    $('#limpiar_confirm').off('click');
    $('#limpiar_confirm').on('click', function (event) {
        limpiar_formulario();
        $("#close_confirm").trigger("click");
    });
}

function mostrar_resultado_busqueda(aeronave) {
    retiro_actual = aeronave;
    $('#Serie_Info').val(aeronave.serie);
    $('#Nombre_Info').val(aeronave.nombre);
    $('#Marca_Info').val(aeronave.marca.nombre);
    $('#Modelo_Info').val(aeronave.modelo.nombre);
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
            $.ajax({
                type: "GET",
                url: "/api/Aeronaves/buscar/serie/" + encodeURIComponent(serie),
                success: function (data, status) {
                    if (data.id !== undefined && data.serie != undefined) {
                        mostrar_resultado_busqueda(data);
                    } else {
                        animate_feedback('error_no_records', 3000, 500, 500);
                    }

                    $(form).removeClass("was-validated");
                },
                error: function (data, status) {
                    window.location.replace("/Home/Error?errorMessage=" +
                        encodeURIComponent(data.responseText) + "&httpError=" +
                        encodeURIComponent(data.status + " " + data.statusText));
                },
                dataType: "json",
                contentType: "application/json; charset=utf-8",
            });
        }
    });
}

function agregar_avion() {
    var agregado = retiro.aeronaves.find(item => {
        return item.serie == retiro_actual.serie
    }) !== undefined;

    if (!agregado) {
        if (retiro.aeronaves.length === 0) {
            $('#retiros-body').html("");
        }

        retiro.aeronaves.push(retiro_actual);
        var indice = retiro.aeronaves.length - 1;

        var tr = '<tr id="retiro-avion-' + indice + '">'
            + '<td>' + retiro_actual.serie + '</td>'
            + '<td>' + retiro_actual.nombre + '</td>'
            + '<td>' + retiro_actual.marca.nombre + '</td>'
            + '<td>' + retiro_actual.modelo.nombre + '</td>'
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
            retiro.aeronaves.splice(posAvion, 1);
            $('#' + this.id).remove();
        });
    } else {
        $('#error_retiro_msg').html("Ya se ha agregado esta arenove para retiro");
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
        var tecnico = $('#Tecnico').val();
        var razon = $('#Detalle').val();

        if (form.checkValidity() && retiro.aeronaves.length > 0 && tecnico !== undefined && tecnico.trim() !== ''
            && razon !== undefined && razon.trim() !== '') {
            retiro.tecnico = tecnico;
            retiro.razon = razon;

            $.ajax({
                type: "PUT",
                url: "/api/Aeronaves/retirar",
                data: JSON.stringify(retiro),
                success: function (data, status) {
                    limpiar_formulario('total');
                    animate_feedback("exito_registro", 3000, 500, 500);
                },
                error: function (data, status) {
                    window.location.replace("/Home/Error?errorMessage=" +
                        encodeURIComponent(data.responseText) + "&httpError=" +
                        encodeURIComponent(data.status + " " + data.statusText));
                },
                dataType: "json",
                contentType: "application/json; charset=utf-8",
            });
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