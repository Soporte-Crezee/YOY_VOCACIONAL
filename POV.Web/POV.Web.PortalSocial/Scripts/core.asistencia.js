// Visualizar asistencia red social.

/*utilidades*/
function extend(subClass, superClass) {
    var F = function () { };
    F.prototype = superClass.prototype;
    subClass.prototype = new F();
    subClass.prototype.constructor = subClass;
}

var Asistencia = function () { };
Asistencia.prototype = {
    /*carga inicial del objeto asistencia*/
    initAsistencia: function (tipo, conf) {
        var asistencia = this.createAsistencia(tipo, conf);
        asistencia.bindControls(conf);
        return asistencia;
    },
    createAsistencia: function (tipo) {
        throw new Error('Clase abstracta');
    },
    bindControls: function () {
        throw new Error('Clase abstracta');
    }
};

// /** Asistencia **/
var AsistenciaCore = function () { };
extend(AsistenciaCore, Asistencia);

AsistenciaCore.prototype.createAsistencia = function (tipo, conf) {
    var asistencia;
    switch (tipo) {
        case 'consultarasistencia':
            asistencia = (function() {
                var loadAsistencia = function(data) {
                    var self = conf;
                    //**Elementos de configuracion requeridos*/
                    if (!self) {
                        if (self.content) {
                            $(self.content).unblock();
                        }
                        return;
                    }
                    if (!self.resultados || !self.more || !self.template) {
                        if (self.content) {
                            myApiBlockUI.unblockContainer();
                        }
                        return;
                    }

                    var item;
                    if (self.pageindex == 1) {
                        self.resultados.empty();
                    }
                    //**la consulta no retorna elementos***
                    var len = data.d.length;
                    if (len <= 0) {
                        if (self.pageindex == 1) {
                            item = '<span>No se encontró ninguna asistencia</span>';
                        } else {
                            item = '<span>No se encontraron más asistencias</span>';
                        }
                        self.more.css('visibility', 'visible');
                        self.more.empty();
                        self.more.html(item);
                    }
                        //**desplegar elementos***
                    else {
                        self.more.css('visibility', 'visible');
                        var index = data.d.length;
                        for (var i = 0; i < index; i++) {
                            var asistenciamov = data.d[i];
                            if (asistenciamov.success) {
                                $(self.template).tmpl(asistenciamov).appendTo(self.resultados).fadeIn(100);
                            } else {
                                if (asistenciamov.errors && asistenciamov.errors != null) {
                                    showError(asistenciamov);
                                } else {
                                    showError();
                                }
                                item = '<span>No se encontró ninguna asistencia</span>';
                                self.more.empty();
                                self.more.html(item);
                            }

                        }
                        ;

                        //agregar opción ver más
                        if (index < self.pagesize) {
                            item = '<span>No se encontraron más asistencias</span>';
                            self.more.empty();
                            self.more.html(item);
                        } else {
                            item = '<a class="link_blue" onclick="javascript:buscarMasAsistencia();" >Mostrar más</a>';
                            self.more.empty();
                            self.more.html(item);
                        }
                    }

                    if (self.content) {
                        myApiBlockUI.unblockContainer();
                    }
                };

                var showError = function(data) {
                    var self = conf;
                    myApiBlockUI.unblockContainer();
                    var mensajeApi = new MessageApi();
                    if (data && data.d) {
                        var index = data.d.length;
                        if (index > 0) {
                            var asistenciamov = data.d[0];
                            mensajeApi.CreateMessage(asistenciamov.errors, "ERROR");
                            mensajeApi.Show();
                        }
                    }
                    if (data) {
                        //elemento único
                        if (!data.success) {
                            mensajeApi.CreateMessage(data.errors, "ERROR");
                            mensajeApi.Show();
                        }
                    } else {
                        //mostrar error genérico
                        mensajeApi.CreateMessage('Ocurrió un error a procesar su solicitud', "ERROR");
                        mensajeApi.Show();
                    }
                };
                return {
                    searchAsistencia: function(data) {
                        var self = this;
                        conf.pageindex = self.conf.pageindex;
                        var asistenciasApi = new AsistenciaApi();
                        var dataString = $.toJSON(data);
                        //boqueo de pantalla
                        if (conf.content) {
                            myApiBlockUI.search();
                        }
                        asistenciasApi.SearchAsistencia({
                            success: function(result) { loadAsistencia(result); },
                            error: function(result) { showError(); }
                        }, dataString);
                    },
                    searchDetalleAsistencia: function (data) {
                        var self = this;
                        var asistenciasApi = new AsistenciaApi();
                        var dataString = $.toJSON(data);
                        //boqueo de pantalla
                        if (conf.content) {
                            myApiBlockUI.search();
                        }
                        asistenciasApi.SearchDetalleAsistencia({
                            success: function (result) { loadAsistencia(result); },
                            error: function (result) { showError(); }
                        }, dataString);
                    },
                    bindControls: function(conf) {
                        var self = this;
                        self.conf = { nombre: conf.nombre, temaid: conf.temaid, tipodocumentoid: conf.tipodocumentoid, pageindex: conf.pageindex };
                    }
                };
            })(conf);

            return asistencia;
            break;

        default:
            asistencia = new AsistenciaCore(conf);
            return asistencia;
            break;
    }
};
AsistenciaCore.prototype.bindControls = function (conf) {
};
AsistenciaCore.prototype.bindEvents = function (conf) {
};
/** Asistencia **/