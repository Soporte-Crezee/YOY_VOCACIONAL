var CompartirApi = function (options) {
	this._myConfigs = $.extend({
		hdnPageSize: 10,
		hdnCurrentPage: "#hdn-current-page-compartir",
	    folderWcf: '',
		evento: 'pub',
	}, options || {});

	this.initApi = function () {
		$("#pnl-contenido-seleccionado-pub").hide();
		$(this._myConfigs.hdnCurrentPage).val("1");
		this.consultarAreasAplicacion();
	};

	this.showDialogCompartir = function (evento, source) {
		this._myConfigs.evento = evento;
		if (source == "reactivo") {
			this.consultarReactivosCompartir();
			$("#dialog-seleccionar-reactivo").dialog({
				closeOnEscape: true,
				resizable: false,
				width: 900,
				height: 500,
				modal: true
			});
			
		} else if (source == "juego") {
		    this.consultarJuegosCompartir();
			$("#dialog-seleccionar-juego").dialog({
				closeOnEscape: true,
				resizable: false,
				width: 900,
				height: 500,
				modal: true
			});
		} else if (source == "contenido") {
		    this.consultarContenidosDigitales();
			$("#dialog-seleccionar-contenido").dialog({
				closeOnEscape: true,
				resizable: false,
				width: 900,
				height: 500,
				modal: true
			});
		}
	};

	this.seleccionarContenidoCompartir = function (source, name, id) {
		
		if (this._myConfigs.evento == 'pub') {
				$("#pnl-contenido-seleccionado-pub").show();
				$("#hdn-tipo-contenido-pub").val(source);
				$("#hdn-identificador-contenido-pub").val(id);
				$("#txt-nombre-contenido-pub").html("<strong>" + name + "</strong>");
		} else if (this._myConfigs.evento == 'com') {
		    $("#pnl-contenido-seleccionado-com").show();
		    $("#hdn-tipo-contenido-com").val(source);
		    $("#hdn-identificador-contenido-com").val(id);
		    $("#txt-nombre-contenido-com").html("<strong>" + name + "</strong>");
		}

		$(".dialog-compartir-contenido").dialog('close');
	};

	this.quitarContenido = function (evento) {

		if (evento == 'pub') {
			$("#pnl-contenido-seleccionado-pub").hide();
			$("#hdn-tipo-contenido-pub").val("");
			$("#hdn-identificador-contenido-pub").val("");
			$("#txt-nombre-contenido-pub").html("");
		} else {
		    $("#pnl-contenido-seleccionado-com").hide();
		    $("#hdn-tipo-contenido-com").val("");
		    $("#hdn-identificador-contenido-com").val("");
		    $("#txt-nombre-contenido-com").html("");
		}
	};

    //reactivos
	this.consultarReactivosCompartir = function () {
	    var page = $(this._myConfigs.hdnCurrentPage)
	    page.val("1");
		var data = {
			dto: {
				"pagesize": this._myConfigs.hdnPageSize,
				"currentpage": page.val(),
				"nombrereactivo": $("#txt-nombre-reactivo").val(),
				"tipocomplejidadid": 0,
				"areaaplicacionid": $("#ddl-area-reactivo").val()
			}
		};
		var apiReactivo = new ReactivoApi();
		var self = this;
		var dataString = $.toJSON(data);
		apiReactivo.Search({
			success: function (result) {
				self.initResultsReactivosCompartir(result, false);
			}
		}, dataString);
	};

	this.loadReactivosCompartir = function () {
		var data = {
			dto: {
				"pagesize": this._myConfigs.hdnPageSize,
				"currentpage": $(this._myConfigs.hdnCurrentPage).val(),
				"nombrereactivo": $("#txt-nombre-reactivo").val(),
				"tipocomplejidadid": 0,
				"areaaplicacionid": $("#ddl-area-reactivo").val()
			}
		};
		var self = this;
		var dataString = $.toJSON(data);
		var apiReactivo = new ReactivoApi();
		apiReactivo.Search({ success: function (result) { self.initResultsReactivosCompartir(result, true); } }, dataString);
	};

	this.initResultsReactivosCompartir = function (data, append) {

		var dataString = $.toJSON(data);

		if (!append) {
			$("#pnl-resultados-compartir-reactivos").empty();
		}
		var page = $(this._myConfigs.hdnCurrentPage);
		if (data.d.reactivos.length <= 0) {
			if (parseInt(page.val()) == 1) {
				var htm = '<span>No se encontraron resultados</span>';
				$("#pnl-more-resultados-compartir-reactivos").empty();
				$("#pnl-more-resultados-compartir-reactivos").html(htm);
			} else {
				var htm = '<span>No existen m&aacute;s resultados</span>';
				$("#pnl-more-resultados-compartir-reactivos").empty();
				$("#pnl-more-resultados-compartir-reactivos").html(htm);
			}
		} else {
			for (var itemIndex = 0; itemIndex < data.d.reactivos.length; itemIndex++) {
			    var htmlData = '<li class="item_pub" style="min-height:45px;line-height:25px;">';
				htmlData += '<strong>' + data.d.reactivos[itemIndex].nombrereactivo + '</strong> - ' + data.d.reactivos[itemIndex].areatitulo + ' ';
				htmlData += '<button type="button" class="button_clip_39215E" style="float:right;margin-top:5px;" '
					+ 'onclick="myApiCompartir.seleccionarContenidoCompartir(\'reactivo\',\''
					+ data.d.reactivos[itemIndex].nombrereactivo + '\',\'' 
					+ data.d.reactivos[itemIndex].reactivoid + '\');" >Seleccionar</button></li>';
				$("#pnl-resultados-compartir-reactivos").append(htmlData);
			}


			if (data.d.reactivos.length < (10 - 1)) {
				var htm = '<span>No existen m&aacute;s resultados</span>';
				$("#pnl-more-resultados-compartir-reactivos").empty();
				$("#pnl-more-resultados-compartir-reactivos").html(htm);
			} else {
				$("#pnl-more-resultados-compartir-reactivos").html('<a class="link_blue" onclick="javascript:myApiCompartir.loadReactivosCompartir();" >Mostrar m&aacute;s resultados</a>');
			}
		}

		page.val(parseInt(page.val()) + 1);
	};

	this.consultarAreasAplicacion = function () {
	    var data = {
	        dto: {
	        }
	    };
	     
	    var self = this;
	    var dataString = $.toJSON(data);
	    this.callGetAreaAplicacion({
	        success: function (result) {
	            var ddl = $("#ddl-area-reactivo");
	            ddl.empty();
	            ddl.append('<option value="0">CUALQUIERA</option>');
	            for (var index = 0; index < result.d.length; index++) {
	                ddl.append('<option value="' + result.d[index] .areaid + '">' + result.d[index].descripcion + '</option>');
	            }

	        }
	    }, dataString);
	};

	this.callGetAreaAplicacion = function (options, areaReactivo) {
	    var config = $.extend({
	        success: function () { },
	        error: function () { }
	    }, options);

	    $.apiCall({
	        url: encodeURI(this._myConfigs.folderWcf + "/ReactivosService.svc/GetAreaAplicacion"),
	        type: 'POST',
	        data: areaReactivo,
	        contentType: "application/json; charset=utf-8",
	        dataType: "json",
	        processData: false,
	        success: function (result) { config.success(result); },
	        error: function (result) { config.error(result); }
	    });
	};

    //Juegos
	this.consultarJuegosCompartir = function () {
	    var page = $(this._myConfigs.hdnCurrentPage)
	    page.val("1");
	    var data = {
	        dto: {
	            "pagesize": this._myConfigs.hdnPageSize,
	            "currentpage": page.val(),
	            "nombrejuego": $("#txt-compartir-nombre-juego").val(),
	            "areaconocimientoid": $("#ddl-compartir-area-juego").val()
	        }
	    }
	    var self = this;
	    var apiJuegosDocente = new JuegosDocenteApi();
	    var dataString = $.toJSON(data);
	    apiJuegosDocente.Search({ success: function (result) { self.initResultsJuego(result, false); } }, dataString);
	};

	this.loadJuegoDocente = function () {
	    var data = {
	        dto: {
	            "pagesize": this._myConfigs.hdnPageSize,
	            "currentpage": $(this._myConfigs.hdnCurrentPage).val(),
	            "nombrejuego": $("#txt-compartir-nombre-juego").val(),
	            "areaconocimientoid": $("#ddl-compartir-area-juego").val()

	        }
	    }
	    var self = this;
	    var apiJuegosDocente = new JuegosDocenteApi();

	    var dataString = $.toJSON(data);
	    apiJuegosDocente.Search({ success: function (result) { self.initResultsJuego(result, true); myApiBlockUI.unblockContainer(); } }, dataString);
	};

	this.initResultsJuego = function (data, append) {
	    var dataString = $.toJSON(data);
	    var pnlMore = $("#pnl-more-resultados-compartir-juegos");
	    var page = $(this._myConfigs.hdnCurrentPage);

	    if (!append) {
	        $("#pnl-resultados-compartir-juegos").empty();
	    }

	    if (data.d.length <= 0) {
	        if (parseInt(page.val()) == 1) {
	            var htm = '<span>No se encontraron resultados</span>';
	            pnlMore.empty();
	            pnlMore.html(htm);
	        } else {
	            var htm = '<span>No existen m&aacute;s resultados</span>';
	            pnlMore.empty();
	            pnlMore.html(htm);
	        }
	    } else {
	        for (var itemIndex = 0; itemIndex < data.d.length; itemIndex++) {
	            var htmlData = '<li class="item_pub" style="min-height:45px;line-height:25px;">';
	            htmlData += '<strong>' + data.d[itemIndex].juego.nombrejuego + '</strong> - ' + data.d[itemIndex].juego.areasconocimiento[0].nombre + ' ';
	            htmlData += '<button type="button" class="button_clip_39215E" style="float:right;margin-top:5px;" '
					+ 'onclick="myApiCompartir.seleccionarContenidoCompartir(\'juego\',\''
					+ data.d[itemIndex].juego.nombrejuego + '\',\''
					+ data.d[itemIndex].juego.juegoid + '\');" >Seleccionar</button></li>';
	            $("#pnl-resultados-compartir-juegos").append(htmlData);
	        }

	        if (data.d.length < (10 - 1)) {
	            var htm = '<span>No existen m&aacute;s resultados</span>';
	            pnlMore.empty();
	            pnlMore.html(htm);
	        } else {
	            pnlMore.html('<a class="link_blue" onclick="javascript:myApiCompartir.loadJuegoDocente();" >Mostrar m&aacute;s resultados</a>');
	        }
	    }

	    page.val(parseInt(page.val()) + 1);
	};

	this.consultarAreasAplicacionJuego = function () {
	    var data = {
	        dto: {
	        }
	    };

	    var self = this;
	    var dataString = $.toJSON(data);
	    this.callGetAreaAplicacionJuego({
	        success: function (result) {
	            var ddl = $("#ddl-compartir-area-juego");
	            ddl.empty();
	            ddl.append('<option value="0">CUALQUIERA</option>');
	            for (var index = 0; index < result.d.length; index++) {
	                ddl.append('<option value="' + result.d[index].areaconocimientoid + '">' + result.d[index].nombre + '</option>');
	            }

	        }
	    }, dataString);
	};

	this.callGetAreaAplicacionJuego = function (options, areaJuego) {
	    var config = $.extend({
	        success: function () { },
	        error: function () { }
	    }, options);

	    $.apiCall({
	        url: encodeURI(this._myConfigs.folderWcf + "/JuegosService.svc/GetAreasConocimientoJuego"),
	        type: 'POST',
	        data: areaJuego,
	        contentType: "application/json; charset=utf-8",
	        dataType: "json",
	        processData: false,
	        success: function (result) { config.success(result); },
	        error: function (result) { config.error(result); }
	    });
	};

    //contenidos
	this.consultarContenidosDigitales = function () {
	    var page = $(this._myConfigs.hdnCurrentPage)
	    page.val("1");
	    var data = {
	        dto: {
	            "pagesize": this._myConfigs.hdnPageSize,
	            "currentpage": page.val(),
	            "nombrecontenido": $("#txt-compartir-nombre-contenido").val()
	        }
	    };
	    var self = this;
	    var apiContenidos = new ContenidosApi();

	    var dataString = $.toJSON(data);
	    apiContenidos.Search({ success: function (result) { self.initResultsContenidos(result, false); } }, dataString);
	};

	this.loadResultContenidos = function () {
	    var page = $(this._myConfigs.hdnCurrentPage)
	    var data = {
	        dto: {
	            "pagesize": this._myConfigs.hdnPageSize,
	            "currentpage": page.val(),
	            "nombrecontenido": $("#txt-compartir-nombre-contenido").val()
	        }
	    };

	    var self = this;
	    var apiContenidos = new ContenidosApi();

	    var dataString = $.toJSON(data);
	    apiContenidos.Search({ success: function (result) { self.initResultsContenidos(result, true); } }, dataString);
	};

	this.initResultsContenidos = function (data, append) {
	    var dataString = $.toJSON(data);

	    if (!append) {
	        $("#pnl-resultados-compartir-contenidos").empty();
	    }
	    var page = $(this._myConfigs.hdnCurrentPage);
	    if (data.d.length <= 0) {
	        if (parseInt(page.val()) == 1) {
	            var htm = '<span>No se encontraron resultados</span>';
	            $("#pnl-more-resultados-compartir-contenidos").empty();
	            $("#pnl-more-resultados-compartir-contenidos").html(htm);
	        } else {
	            var htm = '<span>No existen m&aacute;s resultados</span>';
	            $("#pnl-more-resultados-compartir-contenidos").empty();
	            $("#pnl-more-resultados-compartir-contenidos").html(htm);
	        }
	    } else {
	        for (var itemIndex = 0; itemIndex < data.d.length; itemIndex++) {
	            var htmlData = '<li class="item_pub" style="min-height:45px;line-height:25px;">';
	            htmlData += '<span class="icon_prof ' + data.d[itemIndex].imagendocumento + '" style="height:35px; width:35px;display:block;float:left;margin-right:10px;"></span>  ';
	            htmlData += '<strong>' + data.d[itemIndex].nombrecontenido + '</strong> - ' + data.d[itemIndex].tipodocumento + ' ';
	            htmlData += '<button type="button" class="button_clip_39215E" style="float:right;margin-top:5px;" '
					+ 'onclick="myApiCompartir.seleccionarContenidoCompartir(\'contenido\',\''
					+ data.d[itemIndex].nombrecontenido + '\',\''
					+ data.d[itemIndex].contenidoid + '\');" >Seleccionar</button></li>';
	            $("#pnl-resultados-compartir-contenidos").append(htmlData);
	        }


	        if (data.d.length < (10 - 1)) {
	            var htm = '<span>No existen m&aacute;s resultados</span>';
	            $("#pnl-more-resultados-compartir-contenidos").empty();
	            $("#pnl-more-resultados-compartir-contenidos").html(htm);
	        } else {
	            $("#pnl-more-resultados-compartir-contenidos").html('<a class="link_blue" onclick="javascript:myApiCompartir.loadResultContenidos();" >Mostrar m&aacute;s resultados</a>');
	        }
	    }

	    page.val(parseInt(page.val()) + 1);
	};
};