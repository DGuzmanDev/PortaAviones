const DESPEGUE = "1";
const ATERRIZAJE = "2";

var despegue = {};
var aterrizaje = {};

function limpiar_despegue() {
    $('#resultados_despegue').hide();
    $('#accion_despegue').hide();
    $('#formulario_despegue')[0].reset();
    $('#formulario_busqueda_aeronave')[0].reset();
    $('#formulario_piloto')[0].reset();
    $('#tabla_despegue_body').html('');
}

function limpiar_aterrizaje() {
    $('#resultados_aterrizaje').hide();
    $('#formulario_busqueda_aterraizaje')[0].reset();
    $('#formulario_datos_aterrizaje')[0].reset();
    $('#tabla_aterrizaje_body').html('');
}

function registrar_evento_operacion() {
    $('#tipo_opercion').on('change', function (event) {
        let operacion = this.value;
        despegue = {};
        aterrizaje = {};

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

$(document).ready(function () {
    console.log('misiones.js JavaScript - Daniel Guzman Chaves - Programación Avanzanda en Web -  UNED IIIQ 2023');
    registrar_evento_operacion();
});