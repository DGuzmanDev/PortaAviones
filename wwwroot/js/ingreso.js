var nuevo_ingreso = {};
var nuevos_ingresos = [];

function cargar_marcas() {
    // aqui tiene que ser eventualmente un http request al BE
    for (let index = 0; index < aviones.length; index++) {
        var avion = aviones[index];

        $('#Marca').append($('<option>', {
            value: avion.marca,
            text: avion.marca
        }));
    }

    $("#Marca").on('change', function (event) {
        cargar_modelos(this.value);
    });

    $("#Marca").val(aviones[0].marca);
    cargar_modelos(aviones[0].marca);
}

function cargar_modelos(marca) {
    // aqui tengo que encontrar la marca y jalar los modelos que tenga
    var marcaSeleccionada = aviones.find(item => {
        return item.marca == marca
    });

    var modelos = marcaSeleccionada.modelos;

    $("#Modelo").empty();
    $.each(modelos, function (i, modelo) {
        $('#Modelo').append($('<option>', {
            value: modelo,
            text: modelo
        }));
    });
}

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

        if (tipo === 'total') {
            nuevos_ingresos = [];
            $('#ingreso-body').html('<tr><td>No se han ingresado datos</td></tr>');
        }

        nuevo_ingreso = {};
        var form = $(".needs-validation")[0];
        $(form).removeClass("was-validated");
        form.reset();
        $("#close_confirm").trigger("click");
    });
}

function validar_datos_entrada() {
    var tecnico = $('#Tecnico').val();
    var serie = $('#Serie').val();
    var nombre = $('#Nombre').val();
    var ancho = $('#Ancho').val();
    var alto = $('#Alto').val();
    var largo = $('#Largo').val();

    if ((tecnico === undefined || tecnico.trim() === '')
        || (serie === undefined || serie.trim() === '')
        || (nombre === undefined || nombre.trim() === '')
        || (ancho === undefined || ancho.trim() === '' || ancho <= 0)
        || (alto === undefined || alto.trim() === '' || alto <= 0)
        || (largo === undefined || largo.trim() === '' || largo <= 0)) {
        return false;
    }

    return true;
}

function agregar_avion() {
    // aqui es obtener todos los datos del formulario y agregar el elemento a la lista
    var tecnico = $('#Tecnico').val();
    var marca = $('#Marca').val();
    var modelo = $('#Modelo').val();
    var serie = $('#Serie').val();
    var nombre = $('#Nombre').val();
    var ancho = $('#Ancho').val();
    var alto = $('#Alto').val();
    var largo = $('#Largo').val();

    // aqui eventualmente la marca tiene y el modelo tiene que ser un object
    // con el ID de la DB y el nombre para poder desplegarlo en la lista
    var agregado = nuevos_ingresos.find(item => {
        return item.serie == serie
    }) !== undefined;

    if (!agregado) {
        if (nuevos_ingresos.length === 0) {
            $('#ingreso-body').html("");
        }

        nuevo_ingreso = {
            tecnico: tecnico,
            marca: marca,
            modelo: modelo,
            serie: serie,
            nombre: nombre,
            ancho: ancho,
            alto: alto,
            largo: largo
        };

        nuevos_ingresos.push(nuevo_ingreso);

        var tr = '<tr>'
            + '<td>' + nuevo_ingreso.marca + '</td>'
            + '<td>' + nuevo_ingreso.modelo + '</td>'
            + '<td>' + nuevo_ingreso.serie + '</td>'
            + '<td>' + nuevo_ingreso.nombre + '</td>'
            + '<td>' + nuevo_ingreso.ancho + 'm</td>'
            + '<td>' + nuevo_ingreso.alto + 'm</td>'
            + '<td>' + nuevo_ingreso.largo + 'm</td>'
            + '</tr>';

        $('#ingreso-body').append(tr);
    } else {
        $('#error_formulario').html("Ya se ha agregado un avión con el mismo número de serie anteriormente");
    }
}

function registrar_evento_agregar_avion() {
    $("#agregar").on('click', function (event) {
        event.preventDefault();

        var form = $(".needs-validation")[0];
        var validForms = form.checkValidity();
        $(form).addClass("was-validated");

        if (!validForms || !validar_datos_entrada()) {
            $('#error_formulario').html("Por favor revise el formulario y complete la información requerida antes de continuar");
            animate_feedback("error_formulario", 3000, 500, 500);
        } else {
            agregar_avion();
            $(form).removeClass("was-validated");
            form.reset();
        }
    });
}

function registrar_evento_enviar_formulario() {
    $("#enviar").on('click', function (event) {
        event.preventDefault();

        if (nuevos_ingresos.length > 0) {
            // enviar el formulario al backend con los datos de todos los aviones ingresados
            // tengo que limpiar la tabla y el formulario
            // nice to have: validar si hay algo en el formulario y no lo ha ingresado antes del save
            animate_feedback("exito_registro", 3000, 500, 500);
            $(form).removeClass("was-validated");
            form.reset();
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