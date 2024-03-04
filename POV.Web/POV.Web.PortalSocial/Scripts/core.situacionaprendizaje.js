//Consultar situaciones de aprendizaje red social.

/*utilidades*/
function extend(subClass, superClass) {
    var F = function () { };
    F.prototype = superClass.prototype;
    subClass.prototype = new F();
    subClass.prototype.constructor = subClass;
}

var Situaciones = function () { };
Situaciones.prototype = {
    /*carga inicial del objeto Situacion*/
    initSituacion: function (tipo, conf) {
        var situacion = this.createSituacion(tipo, conf);
        situacion.bindControls(conf);
        return situacion;
    },
    createSituacion: function (tipo) {
        throw new Error('Clase abstracta');
    },
    bindControls: function () {
        throw new Error('Clase abstracta');
    }
};

// /** Situaciones **/
var SituacionCore = function () { };
extend(SituacionCore, Situaciones);

SituacionCore.prototype.createSituacion = function (tipo, conf) {
    var situacion;
    switch (tipo) {
        case 'consultarsituaciones':
            situacion = (function () {
                var loadSituacion = function (data) {
                    var self = conf;
                    //**Elementos de configuracion requeridos*/
                    if (!self) { if (self.content) { $(self.content).unblock(); } return; }
                    if (!self.resultados || !self.more || !self.template) {
                        if (self.content) {
                            myApiBlockUI.unblockContainer();
                        } return;
                    }

                    var item;
                    if (self.pageindex == 1) { self.resultados.empty(); }
                    //**la consulta no retorna elementos***
                    var len = data.d.length;
                    if (len <= 0) {
                        if (self.pageindex == 1) { item = '<span>No se encontró ningún tema</span>'; }
                        else { item = '<span>No se encontraron más temas</span>'; }
                        self.more.css('visibility', 'visible');
                        self.more.empty();
                        self.more.html(item);
                    }
                    //**desplegar elementos***
                    else {
                        self.more.css('visibility', 'visible');
                        var index = data.d.length;
                        for (var i = 0; i < index; i++) {
                            var situacionmov = data.d[i];
                            if (situacionmov.success) {
                                $(self.template).tmpl(situacionmov).appendTo(self.resultados).fadeIn(100);
                            } else {
                                if (situacionmov.errors && situacionmov.errors != null) {
                                    showError(situacionmov);
                                } else {
                                    showError();
                                }
                                item = '<span>No se encontró ningún tema</span>';
                                self.more.empty();
                                self.more.html(item);
                            }

                        };

                        //agregar opción ver más
                        if (index < self.pagesize) {
                            item = '<span>No se encontraron más temas</span>';
                            self.more.empty();
                            self.more.html(item);
                        }
                        else {
                            item = '<a  onclick="javascript:buscarMasSituaciones();" >Mostrar más temas</a>';
                            self.more.empty();
                            self.more.html(item);
                        }
                    }

                    if (self.content) {
                        myApiBlockUI.unblockContainer();
                    }
                };

                var showError = function (data) {
                    var self = conf;
                    myApiBlockUI.unblockContainer();
                    var mensajeApi = new MessageApi();
                    if (data && data.d) {
                        var index = data.d.length;
                        if (index > 0) {
                            var juegomov = data.d[0];
                            mensajeApi.CreateMessage(situacionmov.errors, "ERROR"); mensajeApi.Show();
                        }
                    }
                    if (data) {
                        //elemento único
                        if (!data.success) { mensajeApi.CreateMessage(data.errors, "ERROR"); mensajeApi.Show(); }
                    } else {
                        //mostrar error genérico
                        mensajeApi.CreateMessage('Ocurrió un error a procesar su solicitud', "ERROR");
                        mensajeApi.Show();
                    }
                };
                var endSuscription = function (data) {
                    var self = conf;

                    //**Elementos de configuracion requeridos*/
                    if (!self) {
                        if (self.content) {
                            myApiBlockUI.unblockContainer();
                        }
                        return;
                    }
                    if (!self.resultados || !self.item_template) {
                        if (self.content) {
                            myApiBlockUI.unblockContainer();
                        }
                        return;
                    }
                    if (data && data.d) {
                        if (data.d.success && data.d.juegoid) {
                            var situacionsus = $(self.resultados.find('#' + data.d.situacionid)[0]);


                            if (situacionsus) {
                                situacionsus.empty();
                                $(self.item_template).tmpl(data.d).appendTo(situacionsus).fadeIn(100);
                            }


                        } else {
                            showError(data);
                        }

                    } else {
                        showError();
                    }

                    if (self.content) {
                        myApiBlockUI.unblockContainer();
                    }
                };
                return {
                    searchSituaciones: function (data) {
                        var self = this;
                        conf.pageindex = self.conf.pageindex;
                        var situacionesApi = new SituacionApi();
                        var dataString = $.toJSON(data);
                        //boqueo de pantalla
                        if (conf.content) {
                            myApiBlockUI.search();
                        }
                        situacionesApi.SearchSituacionesAprendizaje({
                            success: function (result) { loadSituacion(result); },
                            error: function (result) { showError(); }
                        }, dataString);
                    },
                    bindControls: function (conf) {
                        var self = this;
                        self.conf = { nombre: conf.nombre, pageindex: conf.pageindex };
                    }
                };
            })(conf);

            return situacion;
            break;

        default:
            situacion = new SituacionCore(conf);
            return situacion;
            break;

    }
};
SituacionCore.prototype.bindControls = function (conf) {
};
SituacionCore.prototype.bindEvents = function (conf) {
};
/** Situaciones **/