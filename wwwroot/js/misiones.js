const DESPEGUE = "1";
const ATERRIZAJE = "2";
const EVENTO_LIMPIAR_DESPEGUE = 'total_despegue';
const EVENTO_LIMPIAR_ATERRIZAJE = 'total_aterrizaje';

var registro_despegue = {
    aeronaves: []
};
var registro_aterrizaje = {};

function ocultar_resultados_busqueda_aeronave(hide_duration) {
    $('#resultados_despegue').hide(hide_duration);
    $('#accion_despegue').hide(hide_duration);
}

function mostrar_resultados_busqueda_aeronave(show_duration) {
    $('#resultados_despegue').show(show_duration);
    $('#accion_despegue').show(show_duration);
}

function ocultar_resultados_busqueda_despegue(hide_duration) {
    $('#resultados_aterrizaje').hide(hide_duration);
    $('#tabla_aterrizaje').hide(hide_duration);
}

function mostrar_resultados_busqueda_despegue(show_duration) {
    $('#resultados_aterrizaje').show(show_duration);
    $('#tabla_aterrizaje').show(show_duration);
}

function limpiar_despegue() {
    registro_despegue = { aeronaves: [] };
    ocultar_resultados_busqueda_aeronave(0);
    $('#formulario_despegue')[0].reset();
    $($('#formulario_despegue')[0]).removeClass('was-validated');
    $('#formulario_busqueda_aeronave')[0].reset();
    $($('#formulario_busqueda_aeronave')[0]).removeClass('was-validated');
    $('#formulario_piloto')[0].reset();
    $($('#formulario_piloto')[0]).removeClass('was-validated');
    $('#tabla_despegue_body').html('');
    configurar_limite_fechas();
}

function limpiar_aterrizaje() {
    registro_aterrizaje = {};
    ocultar_resultados_busqueda_despegue(0);
    $('#Codigo').val('');
    $('#formulario_datos_aterrizaje')[0].reset();
    $($('#formulario_datos_aterrizaje')[0]).removeClass('was-validated');
    $('#tabla_aterrizaje_body').html('');
    configurar_limite_fechas();
}

function registrar_evento_operacion() {
    $('#tipo_operacion').on('change', function (event) {
        let operacion = this.value;

        switch (operacion) {
            case DESPEGUE:
                $('#aterrizaje').hide(500);
                limpiar_aterrizaje();
                $('#despegue').show(500);
                break;

            case ATERRIZAJE:
                $('#despegue').hide(500);
                limpiar_despegue();
                cargar_registro_despegues();
                $('#aterrizaje').show(500);
                break;

            default:
                $('#aterrizaje').hide(500);
                $('#despegue').hide(500);
                limpiar_aterrizaje();
                limpiar_despegue();
        }
    });
}

function rellenar_ceros_fecha(valor) {
    return valor < 10 ? "0" + valor : valor;
}

function configurar_limite_fechas() {
    var min = new Date();
    var minHour = rellenar_ceros_fecha(min.getHours());
    var minMinutes = rellenar_ceros_fecha(min.getMinutes());
    var minDay = rellenar_ceros_fecha(min.getDate());
    var minMonth = rellenar_ceros_fecha(min.getMonth() + 1);

    $("#FechaDespegue").val(min.getFullYear() + "-" + minMonth + "-" + minDay + "T" + minHour + ":" + minMinutes);
    $("#FechaDespegue").attr("min", min.getFullYear() + "-" + minMonth + "-" + minDay + "T" + minHour + ":" + minMinutes);
    $("#FechaAterrizaje").val(min.getFullYear() + "-" + minMonth + "-" + minDay + "T" + minHour + ":" + minMinutes);
    $("#FechaAterrizaje").attr("min", min.getFullYear() + "-" + minMonth + "-" + minDay + "T" + minHour + ":" + minMinutes);
}

function registrar_evento_modal_limpiar() {
    $('#modal_confirm').on('show.bs.modal', function (event) {
        let button = event.relatedTarget;
        let tipo_evento = button.getAttribute('data-bs-target-value');
        registrar_evento_limpiar_formulario(tipo_evento);
    });
}

function registrar_evento_limpiar_formulario(tipo) {
    $('#limpiar_confirm').off('click');
    $('#limpiar_confirm').on('click', function (event) {
        if (tipo === EVENTO_LIMPIAR_DESPEGUE) {
            limpiar_despegue();
        } else {
            limpiar_aterrizaje();
        }
        $("#close_confirm").trigger("click");
    });
}

// Funcionalidades Despegue
function validar_aeronave_agregada_despegue(aeronave) {
    var agregado = registro_despegue.aeronaves.find(item => {
        return item.serie === aeronave.serie;
    }) !== undefined;

    if (agregado) {
        animate_feedback('error_serie_duplicado', 5000, 500, 500);
    }

    return !agregado;
}

function agregar_aeronave_despegue(aeronave_despegue) {
    registro_despegue.aeronaves.push(aeronave_despegue);
    var indice = registro_despegue.aeronaves.length - 1;

    var tr = '<tr id="aeronave-despegue-' + indice + '">'
        + '<td>' + aeronave_despegue.aeronave.marca.nombre + '</td>'
        + '<td>' + aeronave_despegue.aeronave.modelo.nombre + '</td>'
        + '<td>' + aeronave_despegue.aeronave.serie + '</td>'
        + '<td>' + aeronave_despegue.piloto + '</td>'
        + '<td><div class="d-flex justify-content-center">'
        + '<button id="aeronave-' + indice + '" type="button" class="btn btn-danger ms-2">'
        + '<i class="far fa-trash-alt"></i></button></div></td>'
        + '</tr>';

    $('#tabla_despegue_body').append(tr);
    $('#aeronave-' + indice).on('click', function (event) {
        var posAvion = this.id.replace("aeronave-", "");
        registro_despegue.aeronaves.splice(posAvion, 1);
        $('#aeronave-despegue-' + indice).remove();
    });
}

function configurar_boton_agregar_aeronave_despegue(aeronave) {
    $('#agregar').off('click');
    $('#agregar').on('click', function (event) {
        if (validar_aeronave_agregada_despegue(aeronave)) {
            var piloto = $('#Piloto').val();
            var formPiloto = $('#formulario_piloto')[0];
            $(formPiloto).addClass('was-validated');

            if (formPiloto.checkValidity() && piloto !== undefined && piloto.trim() !== '') {
                var despegue_aeronave = {
                    aeronave: aeronave,
                    piloto: piloto,
                    aeronaveFk: aeronave.id
                }
                aeronave.piloto = piloto.trim();
                agregar_aeronave_despegue(despegue_aeronave);
                ocultar_resultados_busqueda_aeronave(500);
                $(formPiloto).removeClass('was-validated');
                formPiloto.reset();
            } else {
                animate_feedback('error_piloto', 3000, 500, 500);
            }
        }
    });
}

function cargar_resultados_aeronave(serie) {
    $.ajax({
        type: "GET",
        url: "/api/Aeronaves/buscar/serie/" + encodeURIComponent(serie),
        success: function (aeronave, status) {
            if (aeronave !== undefined && aeronave.id !== undefined && aeronave.id != null) {
                $('#Serie_Info').val(aeronave.serie);
                $('#Nombre_Info').val(aeronave.nombre);
                $('#Marca_Info').val(aeronave.marca.nombre);
                $('#Modelo_Info').val(aeronave.modelo.nombre);
                configurar_boton_agregar_aeronave_despegue(aeronave);
                mostrar_resultados_busqueda_aeronave(500);
            } else {
                animate_feedback('error_no_records', 3000, 500, 500)
            }
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

function registrar_evento_buscar_aeronave() {
    $('#buscar_serie').on('click', function (event) {
        ocultar_resultados_busqueda_aeronave(500);

        var serie = $('#Serie').val();
        var formAeronave = $('#formulario_busqueda_aeronave')[0];
        $(formAeronave).addClass('was-validated');

        if (formAeronave.checkValidity() && serie !== undefined && serie.trim() !== "") {
            cargar_resultados_aeronave(serie);
            $(formAeronave).removeClass('was-validated');
            formAeronave.reset();
        } else {
            animate_feedback('error_serie', 3000, 500, 500);
        }
    });
}

function validar_datos_despegue() {
    var tecnico = $('#Tecnico').val();
    var mision = $('#Mision').val();
    var fecha = $('#FechaDespegue').val();

    return ((tecnico !== undefined && tecnico.trim() !== '')
        && (mision !== undefined && mision.trim() !== '')
        && (fecha !== undefined && fecha.trim() !== ''));
}

function post_despegue(despegue) {
    $.ajax({
        type: "POST",
        url: "/api/Despegue/guardar",
        data: JSON.stringify(despegue),
        success: function (data, status) {
            limpiar_despegue();
            $('#exito_registro_despegue_msg').html('El registro del despegue ' + data.codigo + ' se ha completado con éxito');
            animate_feedback('exito_registro_despegue_msg', 5000, 500, 500);
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

function registrar_evento_guardar_despegue() {
    $('#enviar_despegue').on('click', function (event) {
        var formDespegue = $('#formulario_despegue')[0];
        $(formDespegue).addClass('was-validated');

        if (formDespegue.checkValidity() && validar_datos_despegue()) {
            if (registro_despegue.aeronaves.length > 0) {
                registro_despegue.tecnico = $('#Tecnico').val();
                registro_despegue.mision = $('#Mision').val();
                registro_despegue.fechaDespegue = $('#FechaDespegue').val();
                post_despegue(registro_despegue);
            } else {
                animate_feedback('error_aeronaves_despegue', 5000, 500, 500);
            }
        } else {
            animate_feedback('error_formulario_despegue', 5000, 500, 500);
        }
    });
}

// Funcionalidades Aterrizaje
function cargar_registro_despegues() {
    $.ajax({
        type: "GET",
        url: "/api/Despegue/buscar",
        success: function (despegues, status) {
            if (despegues !== undefined && despegues.length > 0) {
                for (let index = 0; index < despegues.length; index++) {
                    var despegue = despegues[index];

                    $('#lista_codigos_despegue').append($('<option>', {
                        value: despegue.codigo,
                        text: despegue.codigo + " - " + despegue.mision
                    }));
                }
            } else {
                animate_feedback('error_sin_despegues', 10000, 500, 500)
            }
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

function buscar_despegue(codigo) {
    codigo = encodeURIComponent(codigo);

    $.ajax({
        type: "GET",
        url: "/api/Despegue/buscar/codigo/" + codigo,
        success: function (despegue, status) {
            registro_aterrizaje.despegue = despegue;
            registro_aterrizaje.aeronaves = despegue.aeronaves;

            $('#Codigo_Info').val(registro_aterrizaje.despegue.codigo);
            $('#Mision_Info').val(registro_aterrizaje.despegue.mision);
            $('#Tecnico_Info').val(registro_aterrizaje.despegue.tecnico);
            $('#Fecha_Info').val(registro_aterrizaje.despegue.fechaDespegue);

            for (let index = 0; index < registro_aterrizaje.despegue.aeronaves.length; index++) {
                var aeronave = registro_aterrizaje.despegue.aeronaves[index].aeronave;

                var tr = '<tr id="aeronave-aterrizaje' + index + '">'
                    + '<td>' + aeronave.marca.nombre + '</td>'
                    + '<td>' + aeronave.modelo.nombre + '</td>'
                    + '<td>' + aeronave.serie + '</td>'
                    + '<td class="text-center"><button type="button" class="btn btn-success ms-2" data-bs-toggle="modal"'
                    + 'data-bs-target="#modal_aterrizaje" data-bs-target-value="' + index + '" data-bs-target-parent="' + registro_aterrizaje.despegue.codigo + '">'
                    + '<i class="fas fa-edit"></i></button></td>'
                    + '</tr>';

                $('#tabla_aterrizaje_body').append(tr);
            }

            mostrar_resultados_busqueda_despegue(500);
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

function registrar_evento_cargar_despegue() {
    $('#Codigo').on('change', function (event) {
        var codigo = this.value;
        limpiar_aterrizaje();
        ocultar_resultados_busqueda_despegue(500);
        buscar_despegue(codigo);
    });
}

function registrar_evento_modal_aterrizaje() {
    $("#modal_aterrizaje").on('show.bs.modal', function (event) {
        var button = event.relatedTarget;
        var data_index = button.getAttribute('data-bs-target-value');
        var aeronave = registro_aterrizaje.aeronaves[data_index];

        $("#guardar_datos_aterrizaje").attr("data-bs-target-value", data_index);
        $('#Piloto_Info').val(aeronave.piloto);

        if (aeronave.fechaAterrizaje !== undefined) {
            $('#FechaAterrizaje').val(aeronave.fechaAterrizaje);
        } else {
            configurar_limite_fechas();
        }

        if (aeronave.perdidaMaterial !== undefined && aeronave.perdidaMaterial) {
            // $('#PerdidaMaterial').prop('checked', aeronave.perdidaMaterial);
            $("#PerdidaHumana").val(aeronave.perdidaHumana);
            $('#Perdida').prop('checked', true);
            $('#datos_perdidas').show(500);
        } else {
            $('#datos_perdidas').hide(500);
            $('#Perdida').prop('checked', false);
        }
    });
}

function registrar_evento_perdidas_aterrizaje() {
    $('#Perdida').on('change', function (event) {
        if (this.checked) {
            $('#datos_perdidas').show(500);
        } else {
            $('#datos_perdidas').hide(500);
        }
    });
}

function registrar_evento_guardar_datos_aterrizaje() {
    $('#guardar_datos_aterrizaje').on('click', function (event) {
        var form_datos_aterrizaje = $('#formulario_datos_aterrizaje')[0];
        var fecha_aterrizaje = $('#FechaAterrizaje').val();
        $(form_datos_aterrizaje).addClass('was-validated');

        if (form_datos_aterrizaje.checkValidity() && fecha_aterrizaje !== undefined && fecha_aterrizaje.trim() !== '') {
            var data_index = $("#guardar_datos_aterrizaje").attr("data-bs-target-value");
            var aeronave = registro_aterrizaje.aeronaves[data_index];

            var registra_perdidas = $('#Perdida').is(":checked");

            if (registra_perdidas) {
                // aeronave.perdidaMaterial = $('#PerdidaMaterial').is(":checked");
                aeronave.perdidaMaterial = true;
                aeronave.perdidaHumana = $("#PerdidaHumana").val();
            }

            aeronave.fechaAterrizaje = fecha_aterrizaje;
            $(form_datos_aterrizaje).removeClass('was-validated');
            $('#close_datos_aterrizaje').trigger('click');
            form_datos_aterrizaje.reset();
        }
    });
}

function post_aterrizaje() {
    $.ajax({
        type: "POST",
        url: "/api/Aterrizaje/guardar",
        data: JSON.stringify(registro_aterrizaje),
        success: function (data, status) {
            limpiar_aterrizaje();
            animate_feedback('exito_registro_aterrizaje', 3000, 500, 500);
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

function registrar_evento_enviar_aterrizaje() {
    $('#enviar_aterrizaje').on('click', function (event) {
        event.preventDefault();

        var es_valido = registro_aterrizaje.aeronaves.find(item => {
            return item.fechaAterrizaje === undefined || item.fechaAterrizaje.trim() === '';
        }) === undefined;

        if (es_valido) {
            post_aterrizaje();
        } else {
            animate_feedback('error_formulario_aterrizaje', 5000, 500, 500);
        }
    });
}

$(document).ready(function () {
    console.log('misiones.js JavaScript - Daniel Guzman Chaves - Programación Avanzanda en Web -  UNED IIIQ 2023');
    configurar_limite_fechas();
    registrar_evento_modal_limpiar();
    registrar_evento_operacion();
    registrar_evento_buscar_aeronave();
    registrar_evento_guardar_despegue();
    registrar_evento_cargar_despegue();
    registrar_evento_modal_aterrizaje();
    registrar_evento_perdidas_aterrizaje();
    registrar_evento_guardar_datos_aterrizaje();
    registrar_evento_enviar_aterrizaje();
});