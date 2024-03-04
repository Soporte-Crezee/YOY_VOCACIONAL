//Consultar situación de aprendizaje red social.
situacionAprendizaje = null;

function initPageSituacion() {

    var confSituacion = {
        situaciones_tabs: $('#situaciones-tabs'),
        situaciones: $('#situaciones'),
        content: $('#content'),
        resultados: $('#resultados_situaciones'),
        more: $('#more_situaciones'),
        template: $('#situacionaprendizajeTmpl'),
        item_template: $('#situacionItemTmpl'),
        hdntab: $('<%=hdnEstatusTab.ClientID %>'),
        filternombre: '',
        filterarea: '',
        pagesize: 10
    };
    var situacionCore = new SituacionCore();
    situacionAprendizaje = situacionCore.initSituacion("consultarsituaciones", confSituacion);
    $('#NombreSituacionIn').focus();
   
}

/**
* Implementar dialogo sin redireccion
*/
MessageApi.prototype.PrepareDialog
    = function () {
        $("#dialog:ui-dialog").dialog("destroy");
        $("#dialog-question").dialog({ autoOpen: false });
        $("#dialog").dialog({
            modal: true,
            autoOpen: false,
            width: 400,

            buttons: {
                Cerrar: function () {

                    $(this).dialog("close");
                }
            }
        });
    };

    /*Implementar consultar Situaciones*/
    function buscarSituaciones() {
        var txtnombre = $('#NombreSituacionIn').val();
        situacionAprendizaje.conf.nombre = txtnombre;
            var datos = { dto: {
                nombresituacion: txtnombre
            }
        };
       
       $(controlsViewState.txtContenido).val(txtnombre);
       $(controlsViewState.hdnPushButton).val("true");

        situacionAprendizaje.conf.pageindex = 1;
        situacionAprendizaje.searchSituaciones(datos);

    }

    function buscarMasSituaciones() {
        situacionAprendizaje.conf.pageindex = situacionAprendizaje.conf.pageindex + 1;

        var datos = { dto: {
            currentpage: situacionAprendizaje.conf.pageindex,
            nombrejuego: situacionAprendizaje.conf.nombre
        }
        };
    situacionAprendizaje.searchSituaciones(datos);
    }