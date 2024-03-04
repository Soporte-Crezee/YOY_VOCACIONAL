// Utility
if (typeof Object.create !== 'function') {
    Object.create = function (obj) {
        function F() { };
        F.prototype = obj;
        return new F();
    };
}


var Search = {
    init: function (config) {
        var self = this;

        this.config = config;
        self.process = 0;
        self.processAnterior = 0;
        self.contador = 1;
        this.selectedcontact = [];
        this.messagearea = config.messagearea;
        this.msheader = config.msheader;
        this.msmain = config.msmain;
        this.msfooter = config.msfooter;
        this.searchcontactContainer = config.searchcontactContainer;
        this.messageContainer = config.messageContainer;
        this.contact = config.contact;
        this.$imgSearchContact = $(this.config.searchmsContactContainer).find('input:image');
        this.messageContainerNotificacion = config.messageContainerNotificacion;
        this.notificacionId = config.hdnNotificacionID;
        this.mensajeNotificadoId = config.hdnMensajeID;

        self.bindEvents();
        self.setupTemplates();
        self.loadContacts();
        self.loadNotificacion();
        self.loadLastMensajes();
        /*Procesos en Mensajes*/
        // 0:Carga Inicial Ultimos Mensajes.
        // 2:Consultar Mensajes por remitente.
        // 3:Consultar Mensaje Notificacion.
        // 5:Guardar Respuesta
        // 6:Eliminar Mensaje
        // 7:Eliminar Mensaje UI
    },
    bindEvents: function () {
        var self = Search;
        this.config.txtcontactos.on('keypress', self.searchContact);
        this.config.btnconsultar.on('click', self.searchMessages);
        this.$imgSearchContact.on('click', self.contactSelected);
    },
    bindContactControls: function () {
        var self = Search;
        $(self.config.txtcontactos).autocomplete({
            source: self.sourceautocompletado,
            select: function (event, ui) {
                self.process = 2;
                self.addContact(ui);
                $(this).val('');
                return false;
            }
        });
    },
    display: function (obj) {
        $(obj).css({ 'display': 'block', 'visibility': 'visible' });
    },
    hide: function (obj) {
        $(obj).css({ 'display': 'none', 'visibility': 'hidden' });
    },
    setupTemplates: function () {
        var self = Search;
        self.compiletemplateaddcontact = $(self.config.templateaddcontact).template();
    },
    fetchLastMessage: function (data) {
        var self = Search;
        if (data.d.length <= 0) {
            if (self.contador === 1) {
                $(self.messageContainer).empty();
                var htm = '<span>No tiene mensajes</span>';
                $(self.msfooter).find("#more").html(htm);
            }
            if (self.contador > 1) {
                $(self.messageContainer).empty();
                var htm = '<span>No existen m&aacute;s mensajes</span>';
                $(self.msfooter).find("#more").html(htm);
            }
            return;
        }
        else {

            $(self.messageContainer).empty();
            self.display(self.messageContainer);
            var display = $.tmpl(self.config.templatemessages, data.d);
            display.appendTo(self.messageContainer);

            var htm = '<a class="link_blue" onclick="javascript:Search.loadMoreMessage();">Mostrar m&aacute;s mensajes</a>';
            $(self.msfooter).find("#more").html(htm);


            $('button[id|="btn-del"]').each(function (index) {
                $(this).button({ icons: { primary: "ui-icon-close" }, text: false });
            });

            $('button[id|="btn-mas"]').each(function (index) {
                $(this).button({ icons: { primary: "ui-icon-circle-plus" }, text: false });
            });
        }
    },
    refreshMessage: function (data) {
        var self = Search;
        var mensajes = $(self.messageContainer).find('div.mensajeoutput');
        var display = $.tmpl(self.config.templatemessages, data);
        var mensaje = $(mensajes).find("#" + data.mensajeid);
        var mensajeid = $(mensaje).find('input:hidden');

        if (data.mensajeid == mensajeid.val()) {
            mensaje.parent().parent().parent().remove();
            display.appendTo(self.messageContainer);
        }
        else {
            display.appendTo(self.messageContainer);
        }
    },
    refreshMessages: function () {
        var self = Search;
        var cont = self.contador;

        if (self.process === 2) {
            if (self.selectedcontact[0] && self.selectedcontact[0].val != undefined) {
                var remitenteid = self.selectedcontact.val;
                $(self.messageContainer).empty();
                self.loadPagesMessages(1, cont, remitenteid);

            }
        }

        if (self.process === 0) {
            $(self.messageContainer).empty();
            self.loadPagesMessages(1, cont);
        }


    },
    loadPagesMessages: function (indexpage, cont, remitenteid) {
        var self = Search;
        var data = {};
        var api = new MensajeApi();
        var height = self.scrollTop - self.mensajeheight;
        if (remitenteid != null || remitenteid != undefined) {
            var data = { dto: { currentusuarioid: '0', remitenteid: remitenteid, currentpage: indexpage } };
        }
        else {
            var data = { dto: { currentusuarioid: '0', currentpage: indexpage } };
        }

        var dataString = $.toJSON(data);
        myApiBlockUI.loading();
        api.LoadPageMensajes({
            success: function (result) {

                if (result.d.length <= 0) {
                    myApiBlockUI.unblockContainer();
                    return;
                }


                for (var j = 0; j < result.d.length; j++) {

                    var display = $.tmpl(self.config.templatemessages, result.d[j]);
                    display.appendTo(self.messageContainer);

                    $('button[id|="btn-del"]').each(function (index) {
                        $(this).button({ icons: { primary: "ui-icon-close" }, text: false });
                    });

                    $('button[id|="btn-mas"]').each(function (index) {
                        $(this).button({ icons: { primary: "ui-icon-circle-plus" }, text: false });
                    });
                };

                if (cont > indexpage) {
                    self.loadPagesMessages(indexpage + 1, cont);
                } else {
                    $('html, body').animate({ scrollTop: height }, 400);
                }
                myApiBlockUI.unblockContainer();
            }
        }, dataString);
    },
    fetchMessage: function (data) {
        var self = Search;

        if (data.d.length > 0) {

            if (self.process == 3) {
                var display = $.tmpl(self.config.templatemessages, data.d);
                $(self.config.messageContainerNotificacion).empty();
                self.display(self.config.messageContainerNotificacion);
                display.appendTo(self.config.messageContainerNotificacion);

            }

            if (self.process == 0 || self.process == 2) {

                if (self.contador <= 1) {
                    $(self.messageContainer).empty();
                }

                for (var i = 0; i < data.d.length; i++) {
                    rsmensaje = data.d[i];
                    self.refreshMessage(rsmensaje);
                };

            }

            $('button[id|="btn-del"]').each(function (index) {
                $(this).button({ icons: { primary: "ui-icon-close" }, text: false });
            });

            $('button[id|="btn-mas"]').each(function (index) {
                $(this).button({ icons: { primary: "ui-icon-circle-plus" }, text: false });
            });


            var htm = '<a class="link_blue" onclick="javascript:Search.loadMoreMessage();">Mostrar m&aacute;s mensajes</a>';
            $(self.msfooter).find('#more').html(htm);

        }
        else {
            if (self.contador === 1) {
                $(self.messageContainer).empty();

                var htm = '<span>No tiene mensajes</span>';
                $(self.msfooter).find("#more").html(htm);
            }
            if (self.contador > 1) {
                var htm = '<span>No existen más mensajes</span>';
                $(self.msfooter).find("#more").html(htm);

            }
        }
    },
    addContact: function (data) {
        var self = Search;
        var rept = false;
        this.contador = 1;
        //CONSULTAR MENSAJES
        if (self.selectedcontact.length <= 0) {
            self.display(self.config.searchms);
            self.display(self.config.searchmsContactContainer);
            self.selectedcontact.push(data.item);

        } else {
            if (self.selectedcontact.length == 1) {
                self.display(self.config.searchms);
                self.display(self.config.searchmsContactContainer);
                self.selectedcontact.pop();
                self.selectedcontact.push(data.item);
            }
        }
        self.fetchContact();
    },
    removeContact: function (contactcontainer) {
        var self = Search;
        var rept = false;
        this.contador = 1;
        self.selectedcontact = [];
    },
    fetchContact: function () {
        var self = Search;
        //CONSULTAR MENSAJE
        if (self.process === 2) {
            self.config.contact.empty();
            var display = $.tmpl(self.compiletemplateaddcontact, self.selectedcontact);
            display.appendTo(self.config.contact);
            self.process = 2;
            self.loadLastMensajes();

            self.process = 2;
        }
    },
    contactSelected: function (e) {
        var self = Search;
        e.preventDefault();
        //CONSULTAR MENSAJE
        var contact = $(this).parent().parent().parent().parent();
        self.removeContact(contact);
        contact.remove();
        self.endProcessSearch();
        self.process = 0;
    },
    loadLastMensajes: function () {
        var self = Search;
        var data = {};
        var api = new MensajeApi();
        //CONSULTAR NOTIFICACION
        if (self.process === 3) {
            return;
        }
        //CONSULTAR MENSAJE
        if (self.process === 2) {
            self.display(self.config.actions);
            self.display(self.config.msfooter);
            if (self.selectedcontact[0] && self.selectedcontact[0].val != undefined) {
                var remitenteid = self.selectedcontact[0].val;
                data = { dto: { currentusuarioid: '1', remitenteid: remitenteid } };
                var dataString = $.toJSON(data);
                myApiBlockUI.loading();
                api.LoadLastMensajes({ success: function (result) { self.fetchLastMessage(result); myApiBlockUI.unblockContainer(); } }, dataString);
                self.process = 2;
            }
            else {
                //No se tiene seleccionado un remitente
                self.display(self.config.actions);
                self.display(self.config.msfooter);
                self.process = 0;
                data = { dto: { currentusuarioid: '0' } };
                var dataString = $.toJSON(data);
                myApiBlockUI.loading();
                api.LoadLastMensajes({ success: function (result) { self.fetchLastMessage(result); myApiBlockUI.unblockContainer(); } }, dataString);
                return;
            }
        }

        if (self.process === 0) {
            self.display(self.config.actions);
            self.display(self.config.msfooter);
            data = { dto: { currentusuarioid: '0' } };
            var dataString = $.toJSON(data);
            myApiBlockUI.loading();
            api.LoadLastMensajes({ success: function (result) { self.fetchLastMessage(result); myApiBlockUI.unblockContainer(); } }, dataString);
            return;
        }
        self.process = 0;
    },
    loadNotificacion: function () {
        var self = Search;
        var mensajeNotificadoId = self.mensajeNotificadoId.val();
        var notificacionid = self.notificacionId.val();
        var apiNotificacion = new NotificacionesApi();

        self.hide(self.config.actions);
        self.hide(self.config.msfooter);
        var api = new MensajeApi();
        if (mensajeNotificadoId[0]) {
            self.process = 3;
            if (mensajeNotificadoId == '0') {

                //No se encontró el mensaje
                var htm = '<p style="text-align:center">No se encontró el mensaje seleccionado</p>';
                $(self.config.messageContainerNotificacion).html(htm);
                self.display(self.config.messageContainerNotificacion);

                //Eliminar la notificación del mensaje privado no encontrado.
                var data = { dto: { notificacionid: notificacionid } };
                var dataString = $.toJSON(data);
                myApiBlockUI.process();
                apiNotificacion.DeleteNotificacion({ success: function () { myApiBlockUI.unblockContainer(); } }, dataString);
                return;
            }
            var data = { dto: { currentusuarioid: '0', mensajeid: mensajeNotificadoId } };
            var dataString = $.toJSON(data);
            api.LoadLastMensajes({
                success: function (result) {
                    self.fetchMessage(result);
                }
            }, dataString);

            //dar de baja la notificacion enviada.
            var datan = {
                dto: {
                    "notificacionid": notificacionid
                }
            };
            var dataStringn = $.toJSON(datan);
            oNotificacionesApi.ConfirmNotificacion({
                success: function (result) {

                }
            }, dataStringn);

        }
    },
    loadMoreMessage: function () {

        var self = Search;
        var cont = self.contador;
        cont = ++cont;
        self.contador = cont;
        var data = {};
        var api = new MensajeApi();
        if (self.process === 2) {

            if (self.selectedcontact[0] && self.selectedcontact[0].val != undefined) {
                var remitenteid = self.selectedcontact[0].val;
                var data = { dto: { currentusuarioid: '0', remitenteid: remitenteid, currentpage: cont } };
                var dataString = $.toJSON(data);
                api.LoadPageMensajes({ success: function (result) { self.fetchMessage(result); } }, dataString);
                self.process = 2;
            }
        }

        if (self.process === 0) {
            var data = { dto: { currentusuarioid: '0', currentpage: cont } };
            var dataString = $.toJSON(data);
            myApiBlockUI.loading();
            api.LoadPageMensajes({ success: function (result) { self.fetchMessage(result); myApiBlockUI.unblockContainer(); } }, dataString);
        }
    },
    endProcessSearch: function () {
        var self = Search;
        this.selectedcontact = [];
        this.contador = 1;
        self.process = 0;
        $(self.config.contactContainer).empty();
        $(self.config.contact).empty();
        self.hide(self.config.searchms);
        self.loadLastMensajes();
    },
    loadContacts: function () {
        var self = Search;
        var data = {
            dto: {
                "pagesize": 10,
                "currentpage": 1
            }
        };

        var dataString = $.toJSON(data);
        var apicontactos = new ContactosApi();
        apicontactos.GetContactos({
            success: function (results) {
                if (self.sourcecontacts === undefined) {
                    self.sourcecontacts = results;
                    self.sourceautocompletado = [];

                    $.map(self.sourcecontacts.d, function (item, index) {
                        self.sourceautocompletado.push({ "label": item.screenname, "val": item.usuariosocialid });
                    });
                    self.bindContactControls();
                }
            }
        }, dataString);
    },
    initProcessSearchMessages: function () {
        var self = Search;

        //Preparar Vista.
        self.contador = 1;
        self.selectedcontact = [];

        $(self.config.contactContainer).empty();
        $(self.config.contact).empty();

        self.hide(self.config.searchms);
        self.display(self.searchcontactContainer);

        var htm = '<span>Mostrar más mensajes</span>';
        $(self.msfooter).find('a.link_blue').html(htm);

        $(self.config.btnsdelete).each(function (index) {
            $(this).button({
                icons: { primary: "ui-icon-close" }, text: false
            });
        });
        self.process = 2;
    },
    searchMessages: function (e) {
        var self = Search;
        e.preventDefault();
        if (self.process === 2) {
            self.process = 0;
            self.loadLastMensajes();
        }
        self.initProcessSearchMessages();
        self.process = 0;
    },
    askRemoveMessage: function (mensajeid) {
        var self = Search;
        self.mensajeid = mensajeid;
        var mensajetxt = "¿Desea desvincularse de este mensaje?";
        var classHeader = "modal-header color-info";
        $("#modalHeader").attr('class',classHeader);
        $("#emergentes").text('');
        $("#emergentes").text(mensajetxt);
        var btns = '<button type="button" class="btn btn-green" id="btnAceptarMensaje">S&iacute;</button>' +
                   '<button type="button" class="btn btn-cancel" data-dismiss="modal" id="btnRechazarMensaje" >No</button>';
        $("#modalFooter").html(btns);
        $("#btnAceptarMensaje").unbind('click');
        $("#btnAceptarMensaje").click(function () { self.removeMessage(); $("#btnRechazarMensaje").click(); });

        $('#updPanelMaster').on('shown.bs.modal', function () {
            $('#btnAceptarMensaje').focus()
        });
        $('#updPanelMaster').modal('show');
    },
    removeMessage: function () {
        var self = Search;

        self.scrollTop = $(window).scrollTop();
        //6: Eliminar Mensaje (Desvincular al usuario del mensaje);
        if (self.mensajeid != undefined) {
            var mensajeid = self.mensajeid;
            var mensajermv = $(self.messageContainer).find('#' + self.mensajeid);
            self.mensajeheight = mensajermv.parent().parent().parent().parent().height();

            var api = new MensajeApi();
            var data = {
                dto: {
                    mensajeid: mensajeid
                }
            };
            var dataString = $.toJSON(data);
            api.Delete({
                success: function (result) {

                    if (self.process == 3) {
                        self.removeMessageUI(result);                        
                        $("#content").unblock();
                    } else {
                        self.refreshMessages();
                        $("#content").unblock();
                    }

                }
            }, dataString);
        }
        
    },
    removeMessageUI: function (result) {
        var self = Search;
        var mensajermv;
        if (self.process == 3) {
            mensajermv = $(self.config.messageContainerNotificacion).find('div.' + self.mensajeid);
            $(mensajermv.parent()).remove();
            self.process = 0;
            window.location.search = "";

        } else {
            mensajermv = $(self.messageContainer).find('div.' + self.mensajeid);
            $(mensajermv.parent()).remove();
        }       

    },
    commentenable: function (mensajeid) {
        var self = Search;
        var mns = document.getElementById(mensajeid);
        var data = { mensajeid: mensajeid };
        var dataString = $.toJSON(data);

        $("#panelcomment").remove().fadeIn("slow");
        $("#areacomTmpl").tmpl(data).appendTo(mns).fadeIn(1000);

        var txtarea = $(mns).find("#panelcomment").find("textarea");
        $(txtarea).autoResize({
            onResize: function () { $(this).css({ opacity: 0.9 }); }, animateCallback: function () { $(this).css({ opacity: 1 }); }, animateDuration: 200,
            extraSpace: 1
        });
        var options = {
            'maxCharacterSize': 300,
            'originalStyle': 'display_info_textarea',
            'warningStyle': 'display_warning_textarea',
            'warningNumber': 300,
            'displayFormat': '#left caracteres restantes de #max max.'
        };
        $(txtarea).textareaCount(options);
        txtarea.focus();
    },
    cancelComment: function () {
        $("#panelcomment").remove().fadeIn("slow");
    },
    sendcomment: function (mensajeid, commentdata) {
        var self = Search;
        var api = new MensajeApi();
        var contenido = $("#txtComment").val();
        contenido = $.trim(contenido);
        if (contenido != '' && mensajeid != '') {
            var data = {
                dto: {
                    "mensajeid": mensajeid,
                    "contenido": contenido,
                    "asunto": 'Respuesta'
                }
            };

            var stringdata = $.toJSON(data);
            myApiBlockUI.send();
            api.SendComment({ success: function (result) { self.showComment(result); myApiBlockUI.unblockContainer(); } }, stringdata);
        }
    },
    showComment: function (data) {
        var self = Search;
        var obj = document.getElementById(data.d.guidconversacion);
        $(obj).find("#panelcomment").remove();
        var commt = $(obj).children().find('div.comments');

        template = $.trim(self.config.templatecomment);
        fr = '';
        $.each(data, function (index, obj) {
            fr +=
             template.replace(/{{remitenteid}}/ig, obj.remitenteid)
                     .replace(/{{remitentenombre}}/ig, obj.remitentenombre)
                     .replace(/{{contenido}}/ig, obj.contenido)
                     .replace(/{{fechamensaje}}/ig, obj.fechamensaje);
        });

        $(commt).append(fr);
    },
    showContacts: function (guidconversacion) {
        var self = Search;
        var obj = document.getElementById("#lsdestinatarios-" + guidconversacion);
        var paneldestinatarios = $('.destinatariosmnsj').find("#lsdestinatarios-" + guidconversacion);
        $(paneldestinatarios).slideToggle();

    },
    searchContact: function (e) {
        if (e.keyCode == 13)
            return false;

    }
};

var NewMessage = {

    init: function (config) {
        var self = this;
        this.config = config;
        self.process = 0;
        this.selectedcontacts = [];

        this.messagearea = config.messagearea;
        this.msheader = config.msheader;
        this.msmain = config.msmain;
        this.msfooter = config.msfooter;
        this.searchcontactContainer = config.searchcontactContainer;
        this.newmsContainer = config.newmsContainer;
        this.contactContainer = config.contactContainer;
        this.contactsContainer = config.contactsContainer;
        this.txtmensaje = config.txtmensaje;
        this.$imgContacto = $(this.contactsContainer).find('input:image');
        this.$iconClose = $(this.contactsContainer).find('span');

        self.bindEvents();
        self.setupTemplates();
        self.loadContacts();
        self.initProcessNewMesage();

        /*Procesos en Mensajes*/
        // 1:Nuevo Mensaje.
        // 4:Guardar Mensaje.
    },
    bindEvents: function () {
        var self = NewMessage;
        self.config.txtcontactos.on('focusout', self.endSearchContact);
        self.config.txtcontactos.on('keyup', self.searchContact);
        self.config.btnenviar.on('click', self.saveMessage);
        self.config.btnnuevo.on('click', self.newMessage);
        self.$imgContacto.on('click', self.contactSelected);
        self.$iconClose.on('click', self.contactSelected);

        $(self.txtmensaje).autoResize({
            onResize: function () { $(this).css({ opacity: 0.9 }); }, animateCallback: function () { $(this).css({ opacity: 1 }); }, animateDuration: 200,
            extraSpace: 1
        });
        var options = {
            'maxCharacterSize': 300, 'originalStyle': 'display_info_textarea', 'warningStyle': 'display_warning_textarea',
            'warningNumber': 300, 'displayFormat': '#left caracteres restantes de #max max.'
        };
        $(self.txtmensaje).textareaCount(options);
    },
    searchContact: function (e) {
        if (e.keyCode == 13)
            return false;

    },
    endSearchContact: function (e) {
        var self = NewMessage;
        self.hide(self.config.lblresultado);
    },
    bindContactControls: function () {
        var self = NewMessage;
        $(self.config.txtcontactos).autocomplete({
            source: function (request, response) {
                var results = $.ui.autocomplete.filter(self.sourceautocompletado, request.term);
                if (!results.length) {
                    self.display(self.config.lblresultado);
                } else {
                    self.hide(self.config.lblresultado);
                }
                response(results);
            },
            minLength: 3,
            select: function (event, ui) {

                self.addContact(ui);
                $(this).val('');
                return false;
            }
        });
    },
    display: function (obj) {

        $(obj).css({ 'display': 'block', 'visibility': 'visible' });
    },
    hide: function (obj) {

        $(obj).css({ 'display': 'none', 'visibility': 'hidden' });
    },
    setupTemplates: function () {
        var self = NewMessage;
        self.compiletemplateaddcontact = $(self.config.templateaddcontact).template();
    },
    addContact: function (data) {
        var self = NewMessage;
        var rept = false;
        //NUEVO MENSAJE
        if (self.process === 1) {
            if (self.selectedcontacts.length <= 0) {
                self.selectedcontacts.push(data.item);
            }

            else {
                $.each(self.selectedcontacts, function (index, obj) {
                    if (obj.val === data.item.val) {
                        rept = true;
                        return;
                    }
                });
                if (!rept) {
                    self.selectedcontacts.push(data.item);
                }
            }
            self.fetchContact();
        }
    },
    removeContact: function (contactcontainer) {
        var self = NewMessage;
        var rept = false;
        if (self.process === 1) {
            if (self.selectedcontacts.length > 0) {
                var contact = $(contactcontainer).find('input:hidden');
                $.each(self.selectedcontacts, function (index, obj) {
                    if (obj.val == contact.val()) {
                        delete self.selectedcontacts[index];
                        return;
                    }
                });
                var newselectedcontacts = [];
                $.each(self.selectedcontacts, function (index, obj) {
                    if (obj !== undefined)
                        newselectedcontacts.push(obj);
                });
                self.selectedcontacts = newselectedcontacts;
            }
        }
    },
    fetchContact: function () {
        var self = NewMessage;
        //NUEVO MENSAJE
        if (self.process === 1) {
            self.config.contactContainer.empty();
            var display = $.tmpl(self.compiletemplateaddcontact, self.selectedcontacts);
            display.appendTo($(self.config.contactContainer));
        }
    },
    contactSelected: function (e) {
        var self = NewMessage;
        e.preventDefault();
        //NUEVO MENSAJE
        if (self.process === 1) {
            var contacts = $(this).parent().parent().parent().parent();
            contacts.remove();
            self.removeContact(contacts);
        }
    },
    endProcessNewMessage: function () {
        var self = NewMessage;

        this.selectedcontacts = [];
        self.process = 1;

        $(self.config.contactContainer).empty();
        self.txtmensaje.val('');
    },
    loadContacts: function () {
        var self = NewMessage;
        var apicontactos = new ContactosApi();
        var data = {
            dto: {
                "pagesize": 10,
                "currentpage": 1
            }
        };

        var dataString = $.toJSON(data);
        myApiBlockUI.loading();
        apicontactos.GetContactos({
            success: function (results) {
                if (self.sourcecontacts === undefined) {
                    self.sourcecontacts = results;
                    self.sourceautocompletado = [];
                    $.map(self.sourcecontacts.d, function (item, index) {
                        self.sourceautocompletado.push({ "label": item.screenname, "val": item.usuariosocialid });
                    });
                    self.bindContactControls();
                }
                myApiBlockUI.unblockContainer();
            }
        }, dataString);
    },
    initProcessNewMesage: function () {
        var self = NewMessage;
        //Eliminar los datos anteriores
        $(self.config.contactContainer).empty();
        self.txtmensaje.val('');
        self.selectedcontacts = [];
        self.display(self.newmsContainer);
        self.display(self.searchcontactContainer);
        self.process = 1;
    },
    newMessage: function (e) {
        var self = NewMessage;
        self.process = 1;
        self.initProcessNewMesage();
    },
    saveMessage: function () {
        var self = NewMessage;
        self.process = 4;
        var contenido = self.txtmensaje.val();
        var strcont = '';
        var error = '';
        if (self.selectedcontacts.length <= 0) {
            error += " No se seleccion&oacute; contacto(s) para el mensaje. <br/>";
        }
        if (contenido === '') {
            error += '   ' + " No se encontr&oacute; contenido en el mensaje. ";
        }
        if (error === '') {
            if (self.selectedcontacts.length > 0) {
                $.each(self.selectedcontacts, function (index, obj) {
                    strcont += obj.val + '-';
                });
                var api = new MensajeApi();
                var data = {
                    dto: {
                        contenido: self.txtmensaje.val(), asunto: self.process === 4 ? ' Nuevo Mensaje ' : ' Respuesta ',
                        guidconversacion: self.guidconversacion, destinatariosstring: strcont
                    }
                };
                var dataString = $.toJSON(data);
                api.Send({ success: function (result) { self.endProcessNewMessage(result); self.showDialog("Mensaje enviado", "INFO"); } }, dataString);
                self.process = 0;
            }
        }
        else {
            //Se encontró un error.
            self.showDialog(error, "ERROR");
            self.process = 1;
        }
    },
    showDialog: function (textdialog, type) {
        var self = NewMessage;
        var $dialogquestion = $(self.config.dialogquestion);

        var band = 0;
        var icon = "";
        var classHeader = "modal-header ";
        switch (type) {
            case "ERROR":
                icon = "";
                classHeader += 'color-error';
                break;
            case "WARNING":
                icon = 'glyphicon glyphicon-warning-sign';
                classHeader += 'color-warning';
                break;
            case "INFO":
                icon = 'glyphicon glyphicon-info-sign';
                classHeader += 'color-info';
                break;
            case "OK":
                icon = 'glyphicon glyphicon-ok-sign';
                classHeader += 'color-success';
                break;
            default: band = 1; alert("Tipo de mensaje no soportado");
        }

        $("#modalHeader").attr('class', classHeader);
        var message = textdialog;
        $("#emergentes").html(message);
        var btns = '<button type="button" class="btn-cancel" data-dismiss="modal" id="btnRechazarMensaje" >Cerrar</button>';
        $("#modalFooter").html(btns);
        $('#updPanelMaster').on('shown.bs.modal', function () {
            $('#btnAceptarMensaje').focus()
        });
        $('#updPanelMaster').modal('show');
    }
};