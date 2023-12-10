var DESC = "DESC";
var ASC = "ASC";
var ORDEN_SINTENSIS = DESC;
var ORDEN_INVENTARIO = DESC;

var inventario = [];
var inventario_sintetizado = [];


function mostrar_resultados_sintetizados(resultados) {
    if (resultados.length > 0) {
        $('#sintesis_body').html("");

        for (let index = 0; index < resultados.length; index++) {
            let registro = resultados[index];

            var tr = '<tr>'
                + '<td>' + registro.modelo.nombre + '</td>'
                + '<td>' + registro.marca.nombre + '</td>'
                + '<td>' + registro.cuenta + '</td>'
                + '</tr>';

            $('#sintesis_body').append(tr);
        }
    } else {
        $('#sintesis_body').html("<tr><td>No hay datos</td></tr>");
    }
}

function mostrar_resultados_inventario(lista) {
    if (lista.length > 0) {
        $('#inventario_body').html("");
        for (let index = 0; index < lista.length; index++) {
            let registro = lista[index];

            var tr = '<tr>'
                + '<td>' + registro.modelo.nombre + '</td>'
                + '<td>' + registro.marca.nombre + '</td>'
                + '<td>' + registro.serie + '</td>'
                + '<td>' + registro.nombre + '</td>'
                + '<td>' + registro.fechaRegistro + '</td>'
                + '<td>' + registro.tecnicoIngreso + '</td>'
                + '</tr>';

            $('#inventario_body').append(tr);
        }
    } else {
        $('#inventario_body').html("<tr><td>No hay datos</td></tr>");
    }
}

function ordenar_lista_sintesis(orden) {
    ORDEN_SINTENSIS = orden;
    var lista = [];

    if (ORDEN_SINTENSIS === DESC) {
        lista = inventario_sintetizado.sort(function (a, b) {
            return a.modelo.nombre.localeCompare(b.modelo.nombre);
        });
    } else {
        lista = inventario_sintetizado.sort(function (a, b) {
            return b.modelo.nombre.localeCompare(a.modelo.nombre);
        });
    }

    mostrar_resultados_sintetizados(lista);
}

function ordenar_lista_inventario(orden) {
    ORDEN_INVENTARIO = orden;
    var lista = [];

    if (orden === DESC) {
        lista = inventario.sort(function (a, b) {
            return a.modelo.nombre.localeCompare(b.modelo.nombre);
        });
    } else {
        lista = inventario.sort(function (a, b) {
            return b.modelo.nombre.localeCompare(a.modelo.nombre);
        });
    }

    mostrar_resultados_inventario(lista);
}

function cargar_inventario_sintetizado() {
    $.ajax({
        type: "GET",
        url: "/api/Aeronaves/contar/agrupado/activos",
        success: function (data, status) {
            inventario_sintetizado = data;
            ordenar_lista_sintesis(DESC);
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

function cargar_inventario() {
    $.ajax({
        type: "GET",
        url: "/api/Aeronaves/buscar/activos",
        success: function (data, status) {
            inventario = data;
            ordenar_lista_inventario(DESC);
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
    cargar_inventario_sintetizado();
    cargar_inventario();
    registrar_evento_orden_sinstesis();
    registrar_evento_orden_inventario();
});