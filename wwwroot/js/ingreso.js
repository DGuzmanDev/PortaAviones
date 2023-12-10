var ingreso = {
    aeronaves: []
}

function cargar_marcas() {
    $.ajax({
        type: "GET",
        url: "/api/Marcas/buscar",
        success: function (data, status) {

            for (let index = 0; index < data.length; index++) {
                var marca = data[index];

                $('#Marca').append($('<option>', {
                    value: marca.id,
                    text: marca.nombre
                }));
            }

            $("#Marca").on('change', function (event) {
                cargar_modelos(this.value);
            });

            $("#Marca").val(data[0].id);
            cargar_modelos(data[0].id);
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

function cargar_modelos(marcaId) {
    $("#Modelo").empty();

    $.ajax({
        type: "GET",
        url: "/api/Modelos/buscar/marca/" + encodeURIComponent(marcaId),
        success: function (data, status) {

            $.each(data, function (i, modelo) {
                $('#Modelo').append($('<option>', {
                    value: modelo.id,
                    text: modelo.nombre
                }));
            });
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

function reiniciar_formulario(formId) {
    var form = $("#" + formId)[0];
    $(form).removeClass("was-validated");
    form.reset();
}

function registrar_evento_modal() {
    $('#modal_confirm').on('show.bs.modal', function (event) {
        let button = event.relatedTarget;
        let tipo_evento = button.getAttribute('data-bs-target-value');
        registrar_evento_limpiar_formulario(tipo_evento);
    });
}

function limpiar_formulario(tipo) {
    if (tipo === 'total') {
        ingreso = { aeronaves: [] };
        $('#ingreso-body').html('<tr><td>No se han ingresado datos</td></tr>');
    }

    reiniciar_formulario('formulario_general');
    reiniciar_formulario('formulario_registro');
}

function registrar_evento_limpiar_formulario(tipo) {
    $('#limpiar_confirm').off('click');
    $('#limpiar_confirm').on('click', function (event) {
        limpiar_formulario(tipo);
        $("#close_confirm").trigger("click");
    });
}

function validar_datos_entrada() {
    var serie = $('#Serie').val();
    var nombre = $('#Nombre').val();
    var ancho = $('#Ancho').val();
    var alto = $('#Alto').val();
    var largo = $('#Largo').val();

    if ((serie === undefined || serie.trim() === '')
        || (nombre === undefined || nombre.trim() === '')
        || (ancho === undefined || ancho.trim() === '' || ancho <= 0)
        || (alto === undefined || alto.trim() === '' || alto <= 0)
        || (largo === undefined || largo.trim() === '' || largo <= 0)) {
        return false;
    }

    return true;
}

function actualizar_tabla_aviones(indice, nueva_aeronave) {
    var tr = '<tr id="nuevo-avion-tr' + indice + '">'
        + '<td>' + nueva_aeronave.marca.nombre + '</td>'
        + '<td>' + nueva_aeronave.modelo.nombre + '</td>'
        + '<td>' + nueva_aeronave.serie + '</td>'
        + '<td>' + nueva_aeronave.nombre + '</td>'
        + '<td>' + nueva_aeronave.ancho + 'm</td>'
        + '<td>' + nueva_aeronave.alto + 'm</td>'
        + '<td>' + nueva_aeronave.largo + 'm</td>'
        + '<td><div class="d-flex justify-content-center">'
        + '<button id="nuevo-avion-' + indice + '" type="button" class="btn btn-danger ms-2">'
        + '<i class="far fa-trash-alt"></i></button></div></td>'
        + '</tr>';

    $('#ingreso-body').append(tr);
}

function agregar_avion() {
    var serie = $('#Serie').val();

    var agregado = ingreso.aeronaves.find(item => {
        return item.serie == serie
    }) !== undefined;

    if (!agregado) {
        if (ingreso.aeronaves.length === 0) {
            $('#ingreso-body').html("");
        }

        var nueva_aeronave = {
            marca: {
                id: $('#Marca').val(),
                nombre: $('#Marca option:selected').text()
            },
            modelo: {
                id: $('#Modelo').val(),
                nombre: $('#Modelo option:selected').text()
            },
            serie: serie,
            nombre: $('#Nombre').val(),
            ancho: $('#Ancho').val(),
            alto: $('#Alto').val(),
            largo: $('#Largo').val()
        };

        ingreso.aeronaves.push(nueva_aeronave);
        var indice = ingreso.aeronaves.length - 1;
        actualizar_tabla_aviones(indice, nueva_aeronave);

        $("#nuevo-avion-" + indice).on("click", function (event) {
            var posAvion = this.id.replace("nuevo-avion-", "");
            ingreso.aeronaves.splice(posAvion, 1);
            $('#nuevo-avion-tr' + indice).remove();
        });
    } else {
        $('#error_ingreso_msg').html("Ya se ha agregado una aeronave con el mismo número de serie anteriormente");
        animate_feedback("error_ingreso_msg", 3000, 500, 500);
    }
}

function registrar_evento_agregar_avion() {
    $("#agregar").on('click', function (event) {
        event.preventDefault();

        var form = $("#formulario_registro")[0];
        $(form).addClass("was-validated");

        if (!form.checkValidity() || !validar_datos_entrada()) {
            $('#error_formulario').html("Por favor revise el formulario y complete la información requerida antes de continuar");
            animate_feedback("error_formulario", 3000, 500, 500);
        } else {
            agregar_avion();
            $(form).removeClass("was-validated");
            form.reset();
        }
    });
}

function post_ingreso() {
    $.ajax({
        type: "POST",
        url: "/api/aeronaves/ingresar",
        data: JSON.stringify(ingreso),
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
}

function registrar_evento_enviar_formulario() {
    $("#enviar").on('click', function (event) {
        event.preventDefault();

        if (ingreso.aeronaves.length > 0) {
            var tecnico = $('#Tecnico').val();
            var formDatosGenerales = $('#formulario_general')[0];
            $(formDatosGenerales).addClass("was-validated");

            if (formDatosGenerales.checkValidity() && tecnico !== undefined && tecnico.trim() != '') {
                ingreso.tecnico = tecnico;
                post_ingreso();
                $(formDatosGenerales).removeClass("was-validated");
                formDatosGenerales.reset();
            } else {
                animate_feedback("error_registro", 3000, 500, 500);
            }
        } else {
            animate_feedback("error_registro", 3000, 500, 500);
        }
    });
}

$(document).ready(function () {
    console.log('ingreso.js JavaScript - Daniel Guzman Chaves - Programación Avanzanda en Web -  UNED IIIQ 2023');

    cargar_marcas();
    registrar_evento_modal();
    registrar_evento_agregar_avion();
    registrar_evento_enviar_formulario();
});