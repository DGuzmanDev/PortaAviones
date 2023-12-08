const DESPEGUE = "1";
const ATERRIZAJE = "2";
const EVENTO_LIMPIAR_DESPEGUE = 'total_despegue';
const EVENTO_LIMPIAR_ATERRIZAJE = 'total_aterrizaje';

var despegue = {
    aeronaves: []
};
var aterrizaje = {};

function ocultar_resultados_busqueda_aeronave(hide_duration) {
    $('#resultados_despegue').hide(hide_duration);
    $('#accion_despegue').hide(hide_duration);
}

function mostrar_resultados_busqueda_aeronave(show_duration) {
    $('#resultados_despegue').show(show_duration);
    $('#accion_despegue').show(show_duration);
}

function limpiar_despegue() {
    despegue = { aeronaves: [] };
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
    aterrizaje = {};
    $('#resultados_aterrizaje').hide();
    $('#formulario_busqueda_aterrizaje')[0].reset();
    $($('#formulario_busqueda_aterrizaje')[0]).removeClass('was-validated');
    $('#formulario_datos_aterrizaje')[0].reset();
    $($('#formulario_datos_aterrizaje')[0]).removeClass('was-validated');
    $('#tabla_aterrizaje_body').html('');
    configurar_limite_fechas();
}

function registrar_evento_operacion() {
    $('#tipo_opercion').on('change', function (event) {
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

function configurar_limite_fechas() {
    var min = new Date();
    var minHour = min.getHours() < 10 ? "0" + min.getHours() : min.getHours();
    var minMinutes = min.getMinutes() < 10 ? "0" + min.getMinutes() : min.getMinutes();
    var minDay = min.getDate() < 10 ? "0" + min.getDate() : min.getDate();
    var minMonth = (min.getMonth() + 1) < 10 ? "0" + (min.getMonth() + 1) : (min.getMonth() + 1);

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
        }
        $("#close_confirm").trigger("click");
    });
}

function validar_aeronave_agregada_despegue(aeronave) {
    var agregado = despegue.aeronaves.find(item => {
        return item.serie === aeronave.serie;
    }) !== undefined;

    if (agregado) {
        animate_feedback('error_serie_duplicado', 5000, 500, 500);
    }

    return !agregado;
}

function agregar_aeronave_despegue(aeronave_despegue) {
    despegue.aeronaves.push(aeronave_despegue);
    var indice = despegue.aeronaves.length - 1;

    var tr = '<tr id="aeronave-despegue-' + indice + '">'
        + '<td>' + aeronave_despegue.marca + '</td>'
        + '<td>' + aeronave_despegue.modelo + '</td>'
        + '<td>' + aeronave_despegue.serie + '</td>'
        + '<td>' + aeronave_despegue.piloto + '</td>'
        + '<td><div class="d-flex justify-content-center">'
        + '<button id="aeronave-' + indice + '" type="button" class="btn btn-danger ms-2">'
        + '<i class="far fa-trash-alt"></i></button></div></td>'
        + '</tr>';

    $('#tabla_despegue_body').append(tr);
    $('#aeronave-' + indice).on('click', function (event) {
        var posAvion = this.id.replace("aeronave-", "");
        despegue.aeronaves.splice(posAvion, 1);
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
                aeronave.piloto = piloto.trim();
                agregar_aeronave_despegue(aeronave);
                ocultar_resultados_busqueda_aeronave(500);
                $(formPiloto).removeClass('was-validated');
                formPiloto.reset();
            } else {
                animate_feedback('error_piloto', 3000, 500, 500);
            }
        }
    });
}

function cargar_resultados_aeronave() {
    // TODO:Enviar el request al BE para encontrar toda la informacion de la aeronave
    var aeronave = inventario[0];
    $('#Serie_Info').val(aeronave.serie);
    $('#Nombre_Info').val(aeronave.nombre);
    $('#Marca_Info').val(aeronave.marca);
    $('#Modelo_Info').val(aeronave.modelo);
    configurar_boton_agregar_aeronave_despegue(aeronave);
    mostrar_resultados_busqueda_aeronave(500);
}

function registrar_evento_buscar_aeronave() {
    $('#buscar_serie').on('click', function (event) {
        ocultar_resultados_busqueda_aeronave(500);

        var serie = $('#Serie').val();
        var formAeronave = $('#formulario_busqueda_aeronave')[0];
        $(formAeronave).addClass('was-validated');

        if (formAeronave.checkValidity() && serie !== undefined && serie.trim() !== '') {
            cargar_resultados_aeronave();
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

function registrar_evento_guardar_despegue() {
    $('#enviar_despegue').on('click', function (event) {
        var formDespegue = $('#formulario_despegue')[0];
        $(formDespegue).addClass('was-validated');

        if (formDespegue.checkValidity() && validar_datos_despegue()) {
            if (despegue.aeronaves.length > 0) {
                // TODO: Enviar el request al back-end con el modelo despegue
                limpiar_despegue();
                animate_feedback('exito_registro', 5000, 500, 500);
            } else {
                animate_feedback('error_aeronaves_despegue', 5000, 500, 500);
            }
        } else {
            animate_feedback('error_formulario_despegue', 5000, 500, 500);
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
});

// var action_buttons = '<td>'
// + '<div class="d-flex justify-content-center">'
// + '<button type="button" class="btn btn-primary ms-2" data-bs-toggle="modal"'
// + 'data-bs-target="#modalRevision" data-bs-target-value="data-val" data-bs-target-type="view"><i class="far fa-eye"></i></button>'
// + '<button type="button" class="btn btn-success ms-2" data-bs-toggle="modal"'
// + 'data-bs-target="#modalRevision" data-bs-target-value="data-val" data-bs-target-type="update" disabled><i class="fas fa-edit"></i></button>'
// + '<button type="button" class="btn btn-danger ms-2" data-bs-toggle="modal"'
// + 'data-bs-target="#modalCancelar" data-bs-target-value="data-val" data-bs-target-type="cancel" disabled><i class="far fa-trash-alt"></i>'
// + '</button>'
// + '</div>'
// + '</td>'