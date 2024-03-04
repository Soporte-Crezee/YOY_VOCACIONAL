var intro;
$(function () {
  intro = $.hemiIntro({
    debug: false,
    steps: [
          {
            selector: ".tmuro",
            placement: "bottom",
            content: "En este apartado puedes ver tus publicaciones y las publicaciones de otros usuarios",
          },
          {
              selector: ".tcontactos",
              placement: "bottom",
              content: "En este apartado puedes visualizar los apuntes de tus orientadores, también podrás visualizar tus contactos con intereses en común",
          },
          {
            selector: ".tusuario",
  	        placement: "bottom",
  	        content: "Este es tu nombre de usuario",
  	        offsetTop: 100
          },
          {
              selector: ".tmensajes",
              placement: "bottom",
              content: "En este apartado puedes consultar tus mensajes.",
              offsetTop: 100
          },
          {
              selector: ".tnotificaciones",
              placement: "bottom",
              content: "En este apartado puedes consultar tus notificaciones.",
              offsetTop: 100
          },
          {
              selector: ".miCuenta",
              placement: "bottom",
              content: "Aquí puedes cambiar tu contraseña, tu estado y actualizar tu foto de perfil.",
              offsetTop: 100
          },
          {
  	        selector: ".yo",
  	        placement: "right",
  	        content: "En este apartado puedes consultar tu estado actual"
          },
          //{
          //	selector: ".tGuardar",
          //	placement: "top",
          //	content: "Registrar los datos proporcionados"
          //},
          //{
          //	selector: ".tCancelar",
          //	placement: "top",
          //	content: "Cancelar registro"
          //},
    ],
    startFromStep: 0,
    backdrop: {
      element: $("<div>"),
      class: "hemi-intro-backdrop"
    },
    popover: {
      template: '<div class="popover hemi-intro-popover" role="tooltip"><div class="arrow"></div><h3 class="popover-title"></h3><div class="popover-content"></div></div>'
    },
    buttons: {
        holder: {
            element: $("<div>"),
            class: "hemi-intro-buttons-holder"
        },
        prev: {
            element: $("<input type='button' id='prev' hidden='true' value='Atras' />"),
            class: "btn btn-mini btn-warning"
        },
        next: {
            element: $("<input type='button' id='next' value='Siguiente' />"),
            class: "btn btn-mini btn-success"
        },
        finish: {
            element: $("<input type='button' id='finish' value='Terminar' />"),
            class: "btn btn-mini btn-danger"
        }
    },
    welcomeDialog: {
      show: true,
      selector: "#welcomModal"
    },
    scroll: {
      anmationSpeed: 500
    },
    currentStep: {
      selectedClass: "hemi-intro-selected"
    },
    init: function (plugin) {
      //console.log("init:");
    },
    onLoad: function (plugin) {
      //console.log("onLoad:");
    },
    onStart: function (plugin) {
      //console.log("onStart:");
    },
    onBeforeChangeStep: function () {
        //console.log("onBeforeChangeStep:");        
    },
    onAfterChangeStep: function () {
        //console.log("onAfterChangeStep:");  
        if ($(".tmuro").hasClass('hemi-intro-selected')) {
            $(".tmuro").attr({
                style: "background-color: #39215e;"
            });
        } else {
            if ($('.tmuro').is('[style]')) {
                $(".tmuro").removeAttr('style');
            }
        }

        if ($(".tcontactos").hasClass('hemi-intro-selected')) {
            $(".tcontactos").attr({
                style: "background-color: #39215e;"
            });
        } else {
            if ($('.tcontactos').is('[style]')) {
                $(".tcontactos").removeAttr('style');
            }
        }
    },
    onShowModalDialog: function (plugin, modal) {
      //console.log("onShowModalDialog:");
    },
    onHideModalDialog: function (plugin, modal) {
      //console.log("onHideModalDialog:");
    },
    onComplete: function (plugin) {
      //console.log("onComplete:");
    }
  });
  //setTimeout(function () {
  //  intro.start();
  //}, 2500);
})