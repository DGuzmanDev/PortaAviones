var DESC = "DESC";
var ASC = "ASC";
var ORDEN_SINTENSIS = DESC;
var ORDEN_INVENTARIO = DESC;

function cargar_inventario_sintetizado(lista) {
    // TODO: Mandar HTTP request al backend para obtener la lista
    if (lista.length > 0) {
        $('#sintesis_body').html("");

        for (let index = 0; index < lista.length; index++) {
            let registro = lista[index];

            var tr = '<tr>'
                + '<td>' + registro.modelo + '</td>'
                + '<td>' + registro.marca + '</td>'
                + '<td>' + registro.cuenta + '</td>'
                + '</tr>';

            $('#sintesis_body').append(tr);
        }
    }
}

function ordenar_lista_sintesis(orden) {
    ORDEN_SINTENSIS = orden;
    var lista = [];

    if (ORDEN_SINTENSIS === DESC) {
        lista = inventario_sinstesis.sort(function (a, b) {
            return a.modelo.localeCompare(b.modelo);
        });
    } else {
        lista = inventario_sinstesis.sort(function (a, b) {
            return b.modelo.localeCompare(a.modelo);
        });
    }

    cargar_inventario_sintetizado(lista);
}

function cargar_inventario(lista) {
    // TODO: Mandar HTTP request al backend para obtener la lista
    if (lista.length > 0) {
        $('#inventario_body').html("");
        for (let index = 0; index < lista.length; index++) {
            let registro = lista[index];

            var tr = '<tr>'
                + '<td>' + registro.modelo + '</td>'
                + '<td>' + registro.marca + '</td>'
                + '<td>' + registro.serie + '</td>'
                + '<td>' + registro.nombre + '</td>'
                + '<td>' + registro.fecha_ingreso + '</td>'
                + '<td>' + registro.tecnico + '</td>'
                + '</tr>';

            $('#inventario_body').append(tr);
        }
    }
}

function ordenar_lista_inventario(orden) {
    ORDEN_INVENTARIO = orden;
    var lista = [];

    if (orden === DESC) {
        lista = inventario.sort(function (a, b) {
            return a.modelo.localeCompare(b.modelo);
        });
    } else {
        lista = inventario.sort(function (a, b) {
            return b.modelo.localeCompare(a.modelo);
        });
    }

    cargar_inventario(lista);
}

function registrar_evento_orden_sinstesis() {
    $('#ordenar_inventario_sintesis').on('click', function (event) {
        let orden = ORDEN_SINTENSIS === DESC ? ASC : DESC;
        ordenar_lista_sintesis(orden);
    });
}

function registrar_evento_orden_inventario() {
    $('#ordenar_inventario').on('click', function (event) {
        let orden = ORDEN_INVENTARIO === DESC ? ASC : DESC;
        ordenar_lista_inventario(orden);
    });
}

$(document).ready(function () {
    console.log('inventario.js JavaScript - Daniel Guzman Chaves - Programación Avanzanda en Web -  UNED IIIQ 2023');
    ordenar_lista_sintesis(DESC);
    ordenar_lista_inventario(DESC);
    registrar_evento_orden_sinstesis();
    registrar_evento_orden_inventario();
});