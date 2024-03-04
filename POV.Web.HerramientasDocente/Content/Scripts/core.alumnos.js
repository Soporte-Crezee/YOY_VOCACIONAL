

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
    bindEvents: function () {
        var self = CoreAlumnos.ValidacionesAlumno;

        $(self.elements.buttons).button();
        $(self.elements.txtfechanacimiento).datepicker({ yearRange: '-100:+0', changeYear: true, changeMonth:true, dateFormat: "dd/mm/yy" });
        $(self.elements.btnguardar).on('click', function (e, sender) {
            self.validateAlumnoAsignadoEscuela(e);

        });
    }
};