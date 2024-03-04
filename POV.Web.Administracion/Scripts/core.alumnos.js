

var CoreAlumnos = {};

CoreAlumnos.ValidacionesAlumno = {

    init: function (elements) {
        var self = this;
        this.elements = elements;
        self.bindEvents();

    },
    onValidateAlumnoAsignadoEscuela: function (result, event) {
        var self = CoreAlumnos.ValidacionesAlumno;
        event.preventDefault();

        var stresult = $('<p/>', { text: 'El alumno CURP:' + result.curp + ',      ' + 'Nombre:' + result.nombrecompleto });
        $(self.elements.textdialog).html(stresult);


        //Desplegar Dialogo
        $(self.elements.dialog).dialog({ modal: true, resizable: false,
            buttons: [{ text: "Ok", click: function () { $(this).dialog("close"); $(self.elements.btnguardar).unbind(event).click(); } },
                          { text: "Cancelar", click: function () { $(this).dialog("close"); } }]
        });


    },
    validateAlumnoAsignadoEscuela: function (e) {
        var self = CoreAlumnos.ValidacionesAlumno;
        e.preventDefault();

        var curp = $(self.elements.txtalumcurp).val();

        if (curp.length <= 0) {
            $(self.elements.btnguardar).unbind(e).click();
            return;
        }
        var data = { dto: { curp: curp} };

        var datastring = $.toJSON(data);
        var apialumnos = new AlumnosApi();

        apialumnos.ValidateAlumnoAsignadoEscuela({ success: function (result) {
            if (result == null || result.d == null) {
                $(self.elements.btnguardar).unbind(e).click();
            } else {
                if (result) {

                    self.onValidateAlumnoAsignadoEscuela(result.d, e);
                }
            }

        }
        }, datastring);
    },

    validateUsuario: function () {
        var self = CoreAlumnos.ValidacionesAlumno;
        //e.preventDefault();

        var nombreusuario = $(self.elements.txtusername).val();

        //if (nombreusuario.length <= 0) {
        //    $(self.elements.btnguardar).unbind(e).click();
        //    return;
        //}
        var data = { dto: { nombreusuario: nombreusuario } };

        var datastring = $.toJSON(data);
        var apialumnos = new AlumnosApi();

        apialumnos.ValidateAspirante({
            success: function (result) {
                var id = $("#userimg").length;
                if (id > 0) {
                    $("#userimg").remove();
                }

                if (result != null && result.d != null && result.d.usuarioid != null) {                   
                    $(".username").append('<img src="../images/hr.gif" id="userimg" title="nombre de usuario no disponible" >');
                    alertify.set('notifier', 'position', 'bottom-left');
                    alertify.set('notifier', 'delay', 5);
                    alertify.error("El nombre de usuario está en uso");
                    $(".btnAcces").prop('disabled', true);
                }
                else {
                    $(".username").append('<img src="../images/tick.png"  id="userimg" title="nombre de usuario disponible" >');
                    $(".btnAcces").prop('disabled', false);
                }
            }
        }, datastring);
    },

    bindEvents: function () {
        var self = CoreAlumnos.ValidacionesAlumno;

        $(self.elements.buttons).button();
        //$(self.elements.txtfechanacimiento).datepicker({ yearRange: '-100:+0', changeYear: true, changeMonth:true, dateFormat: "dd/mm/yy" });
        $(self.elements.btnguardar).on('click', function (e, sender) {
            //self.validateAlumnoAsignadoEscuela(e);
            //self.validateUsuario(e);
        });
    }
};