

var coreReporteAbuso = {};

coreReporteAbuso = {
    init: function (elements) {
        var self = this;
        self.elements = elements;

        self.loadReportesAbusoDocente();
        self.bindEvents();
    },
    bindEvents: function () {
        $('.boton_1').button();

        $('input:button').on('click', function(e, sender) {
            e.preventDefault();
        });
    },
    loadReportesAbusoDocente: function () {
        var self = coreReporteAbuso;
        var oReporteAbuso = new ReporteAbusoApi();

        var pageSize = 10;
        self.elements.currentPage = 1;

        var current_page = 1;

        var data = { dto: {
            "pagesize": pageSize,
            "currentpage": current_page
        }
        };
        var dataString = $.toJSON(data);
        myApiBlockUI.loading(); 
        oReporteAbuso.GetReportesAbuso({ success: function (result) { self.printReportesAbusoDocente(result); myApiBlockUI.unblockContainer(); }, error: function (result) {self.onError(result);myApiBlockUI.unblockContainer();  } }, dataString);
    },
    onError:function (result) {
        if (result.d && result.d.length > 0 && result.d.error.length > 0) {
            $(hdnmessageinputid).val(result.d.error);
            $(hdnmessagetypeinputid).val('1');
            mostrarError();
            return;
        } else {
            
            $(hdnmessageinputid).val("ocurri&oocute; un error al procesar su solicitud");
            $(hdnmessagetypeinputid).val('1');
            mostrarError();
        }
    },
    printReportesAbusoDocente: function (data) {
        var self = coreReporteAbuso;
        var page_size = 10;

        var current_page = self.elements.currentPage;

        if (data.d.length <= 0) {
            if (current_page == 1) {
                var htm = '<span>No tienes reportes de abuso</span>';
                $(self.elements.more).empty();
                $(self.elements.more).html(htm);
            } else {
                var htm = '<span>No existen m&aacute;s reportes de abuso</span>';
                $(self.elements.more).empty();
                $(self.elements.more).html(htm);
            }
        } else {

            $(self.elements.containerTmpl).tmpl(data.d, { getCss: coreReporteAbuso.getCss }).appendTo(self.elements.containerStream);

            if (data.d.length < page_size) {
                var htm = '<span>No existen m&aacute;s reportes de abuso</span>';
                $(self.elements.more).empty();
                $(self.elements.more).html(htm);
            } else {
                $(self.elements.more).html('<a class="link_blue" onclick="javascript:coreReporteAbuso.loadMoreReportesAbusoDocente();" >Mostrar m&aacute;s reportes de abuso</a>');
            }

        }
        self.bindEvents();
    },
    loadMoreReportesAbusoDocente: function () {

        var self = coreReporteAbuso;
        var oReporteAbuso = new ReporteAbusoApi();
        var pageSize = 10;

        if (self.elements.currentPage || self.elements.currentPage == 1) {
            self.elements.currentPage++;
        } else {
            self.elements.currentPage = 1;
        }

        var current_page = self.elements.currentPage;
        var data = { dto: {
            "pagesize": pageSize,
            "currentpage": current_page
        }
        };
        var dataString = $.toJSON(data);
        myApiBlockUI.loading(); 
        oReporteAbuso.GetReportesAbuso({ success: function (result) { self.printReportesAbusoDocente(result);myApiBlockUI.unblockContainer();  }, error: function (result) { self.onError(result); myApiBlockUI.unblockContainer(); } }, dataString);

    },
    deleteReporteAbuso: function (reporteabusoid) {

        var self = coreReporteAbuso;

        var oReporteAbuso = new ReporteAbusoApi();

        var data = { dto: {
            "reporteabusoid": reporteabusoid
        }
        };
        var dataString = $.toJSON(data);
        myApiBlockUI.process(); 
        oReporteAbuso.DeleteReporteAbuso({ success: function (result) { self.removeElementoReportadaFromUI(reporteabusoid); myApiBlockUI.unblockContainer(); }, error: function (result) { self.onError(result); myApiBlockUI.unblockContainer(); } }, dataString);
    },
    removeElementoReportadaFromUI:function (reporteabusoid) {

        if (reporteabusoid) {
            var obj = document.getElementById(reporteabusoid);
            $(obj).remove();
        }
    },
    confirmReporteAbuso: function (reporteabusoid) {

        var self = coreReporteAbuso;

        var oReporteAbuso = new ReporteAbusoApi();

        var data = { dto: {
            "reporteabusoid": reporteabusoid
        }
        };
        var dataString = $.toJSON(data);
        myApiBlockUI.process(); 
        oReporteAbuso.ConfirmReporteAbuso({ success: function (result) { self.removeElementoReportadaFromUI(reporteabusoid); myApiBlockUI.unblockContainer(); }, error: function (result) { self.onError(result); myApiBlockUI.unblockContainer(); } }, dataString);
    },
    
    getCss: function (estatus) {

    }

};

var coreElementoReporteAbuso = {};

coreElementoReporteAbuso = {
    init: function (elements) {
        var self = coreElementoReporteAbuso;
        self.elements = elements;

        self.loadPublicacionReporteAbuso();
    },
    bindEvents: function () {
        var self = coreElementoReporteAbuso;

        $('.boton_1').button();

    },
     onError:function (result) {
        if (result.d && result.d.length > 0 && result.d.error.length > 0) {
            $(hdnmessageinputid).val(result.d.error);
            $(hdnmessagetypeinputid).val('1');
            mostrarError();
            return;
        } else {
            
            $(hdnmessageinputid).val("ocurri&oocute; un error al procesar su solicitud");
            $(hdnmessagetypeinputid).val('1');
            mostrarError();
        }
    },
    loadPublicacionReporteAbuso: function () {
        var self = coreElementoReporteAbuso;
        var oReporteAbuso = new ReporteAbusoApi();
        var id = self.elements.reporteabuso;

        var data = { dto: {
            "reporteabusoid": id
        }
        };
        var dataString = $.toJSON(data);
        myApiBlockUI.loading(); 
        oReporteAbuso.GetPublicacionReporteAbuso({ success: function (result) { self.printPublicacionReporteAbuso(result); myApiBlockUI.unblockContainer(); }, error: function (result) { self.onError(result);myApiBlockUI.unblockContainer();  } }, dataString);

    },
    printPublicacionReporteAbuso: function (data) {

        var self = coreElementoReporteAbuso;
        $(self.elements.containerTmpl).tmpl(data.d, { getCss: self.getCss }).appendTo(self.elements.containerStream);

        self.bindEvents();

    },
    getCss: function (tipo) {

    },
    confirmElementoReportable: function (reportableid) {
        var self = coreElementoReporteAbuso;
        var oReporteAbuso = new ReporteAbusoApi();
        var tipo = self.elements.tiporeporteabuso;
        var data = { dto: {
            "reportableid": reportableid,
            "tiporeporte": tipo
        }
        };
        var dataString = $.toJSON(data);
        myApiBlockUI.process(); 
        oReporteAbuso.ConfirmReporteAbuso({ success: function (result) { self.removeElementoReportadaFromUI(reportableid); myApiBlockUI.unblockContainer(); }, error: function (result) { self.onError(result);myApiBlockUI.unblockContainer();  } }, dataString);
    },
    deleteElementoReportable: function (reportableid) {
        var self = coreElementoReporteAbuso;

        var oReporteAbuso = new ReporteAbusoApi();
        var tipo = self.elements.tiporeporteabuso;
        var data = {
            dto: {
                "reportableid": reportableid,
                "tiporeporte": tipo
            }
        };
        var dataString = $.toJSON(data);
        oReporteAbuso.DeleteReporteAbuso({ success: function (result) { self.removeElementoReportadaFromUI(reportableid); }, error: function (result) { self.onError(result); } }, dataString);
    },
    removeElementoReportadaFromUI:function (reportableid) {

        if (reportableid) {
            var obj = document.getElementById(reportableid);
            $(obj).remove();
        }
    },
};