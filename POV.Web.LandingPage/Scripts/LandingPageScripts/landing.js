$(document).ready(function () {

    $("#telefono").keydown(function (e) {
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            // Allow: Ctrl/cmd+A
            (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: Ctrl/cmd+C
            (e.keyCode == 67 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: Ctrl/cmd+X
            (e.keyCode == 88 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: home, end, left, right
            (e.keyCode >= 35 && e.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });

    function validatecommSeptEmail(commSeptEmail) {
        var regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,5}$/;
        return (regex.test(commSeptEmail)) ? true : false;
    }

    /*Solicitud de envio de correo*/
    $("#contactMe").click(function () {
        var data = {
            "sendMail": "OK",
            "nombre": $("#nombre").val(),
            "institucion": $("#institucion").val(),
            "cargo": $("#cargo").val(),
            "telefono": $("#telefono").val(),
            "email": $("#email").val(),
            "comonos": $("#comonos").val(),
            "pais": $("#pais").val(),
            "estado": $("#estado").val(),
            "soy": $("#soy").val(),
            "ciudad": $("#ciudad").val(),
            "mensaje":$("#mensaje").val()
        }
        var camposvacios = true
        var msgCampos = 'Por favor existen campos vacíos en el formulario de contacto';
        var i = 0;
        Object.keys(data).forEach(function (key) {

            if (data[key] == "" && (i == 1 || i == 4 || i == 5 || i == 7 || i == 8 || i == 9 || i == 11)) {
                camposvacios = false;
                msgCampos = 'Por favor existen campos vacíos en el formulario de contacto';
                return;
            }
            if (i == 5 && data[key] != "" && !validatecommSeptEmail(data[key])) {
                camposvacios = false;                
                msgCampos = 'Email inválido';
                return;
            }
            i = i+1;
        });

        data["telefono"] = $("#telefono").intlTelInput("getNumber", intlTelInputUtils.numberFormat.E164);

        var url = "../../Default.aspx";

        if (camposvacios) {
            $.ajax({
                type: "POST",
                url: url,
                data: data,
                beforeSend: function () {
                    swal({
                        title: 'Enviando mensaje',
                        type: 'info',
                        html:
                          '<img src=""/><br>¡Se está enviando tu mensaje!',
                        showCloseButton: false,
                        showCancelButton: false,
                        showConfirmButton:false,
                        animation: true
                    })
                },
                success: function (data) {
                    swal({
                        title: '¡Enviado!',
                        type: 'success',
                        html:
                          'Hemos recibido tu mensaje, en breve un asesor se pondrá en contacto contigo',
                        showCancelButton: false,                       
                        confirmButtonText:
                          '<i class="fa fa-thumbs-up"></i> ¡Gracias!',                       
                        animation: true
                    })
                    $("#showformulario").trigger('click');
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {                    
                    swal({
                        title: '¡Error!',
                        type: 'error',
                        html:
                          "Correo de contacto no enviado, favor de intentar nuevamente",
                        showCancelButton: false,
                        confirmButtonText:
                          '<i class="fa fa-thumbs-up"></i> ¡Disculpe!',
                        animation: true
                    })
                    $("#nombre").focus();
                }
            });
        } else {
            swal({
                title: 'Validación',
                type: 'info',
                html:
                  msgCampos,
                showCloseButton: true,
                showCancelButton: false,
                animation: true
            })
        }

    });
    var hidden = $('.hidde');
    var slideClicked = false, slide2Clicked = false, slide3Clicked = false, slide4Clicked = false;
    var portalPadres = $("#portalPadres");
    var portalEstudiantes = $("#portalEstudiantes");
    var portalOrientadores = $("#portalOrientadores");
    var portalEscuelas = $("#portalEscuelas");

    portalPadres.show();
    portalEscuelas.hide();
    portalEstudiantes.hide();
    portalOrientadores.hide();

    hidden.animate({ "left": "0px" }, 1000).addClass('visible');
    //$("#imgMockup").fadeOut(1000).css("content", "url('Images/LandingImages/mockup_portal_estudiante.png')").fadeIn(1000);
    $("#imgMockup").hide('slow').show('slow').attr("src", "Images/LandingImages/mockup_portal_estudiante.png");
    slide3Clicked = true;
    slideClickedIcon();

    $('#slide').click(function () {
        slideClicked = true;
        slide2Clicked = false;
        slide3Clicked = false;
        slide4Clicked = false;
        if (!hidden.is(':animated')) {
            if (hidden.hasClass('visible')) {
                hidden.animate({ "left": "-2000px" }, 1000).removeClass('visible');
                hidden.animate({ "left": "0px" }, 1000).addClass('visible');
                slideClickedIcon();
                $("#imgMockup").hide('slow').show('slow').attr("src", "Images/LandingImages/mockup_portal_padres.png");
            } else {
                slideClickedIcon();
                hidden.animate({ "left": "0px" }, 1000).addClass('visible');
                $("#imgMockup").hide('slow').show('slow').attr("src", "Images/LandingImages/mockup_portal_padres.png");
            }
        }
    })
    $('#slide2').click(function () {
        slide2Clicked = true;
        slideClicked = false;
        slide3Clicked = false;
        slide4Clicked = false;
        if (!hidden.is(':animated')) {
            if (hidden.hasClass('visible')) {
                hidden.animate({ "left": "-1000px" }, 1000).removeClass('visible');
                hidden.animate({ "left": "0px" }, 1000).addClass('visible');
                slideClickedIcon();
                $("#imgMockup").hide('slow').show('slow').attr("src", "Images/LandingImages/mockup_portal_orienytador.png");
            } else {
                slideClickedIcon();
                hidden.animate({ "left": "0px" }, 1000).addClass('visible');
                $("#imgMockup").hide('slow').show('slow').attr("src", "Images/LandingImages/mockup_portal_orienytador.png");
            }
        }
    })
    $('#slide3').click(function () {
        slide3Clicked = true;
        slide2Clicked = false;
        slideClicked = false;
        slide4Clicked = false;
        if (!hidden.is(':animated')) {
            if (hidden.hasClass('visible')) {
                hidden.animate({ "left": "-1000px" }, 1000).removeClass('visible');
                slideClickedIcon();
                hidden.animate({ "left": "0px" }, 1000).addClass('visible');
                $("#imgMockup").hide('slow').show('slow').attr("src", "Images/LandingImages/mockup_portal_estudiante.png");//.fadeIn(1000);
            } else {
                slideClickedIcon();
                hidden.animate({ "left": "0px" }, 1000).addClass('visible');
                $("#imgMockup").hide('slow').show('slow').attr("src", "Images/LandingImages/mockup_portal_estudiante.png");
            }
        }
    })

    $('#slide4').click(function () {
        slide4Clicked = true;
        slide2Clicked = false;
        slide3Clicked = false;
        slideClicked = false;
        if (!hidden.is(':animated')) {
            if (hidden.hasClass('visible')) {
                $("#textIam").html("");
                hidden.animate({ "left": "-1000px" }, 1000).removeClass('visible');
                slideClickedIcon();
                hidden.animate({ "left": "0px" }, 1000).addClass('visible');
                $("#imgMockup").hide('slow').show('slow').attr("src", "Images/LandingImages/mockup_portal_escuela.png");
            } else {
                slideClickedIcon();
                hidden.animate({ "left": "0px" }, 1000).addClass('visible');
                $("#imgMockup").hide('slow').show('slow').attr("src", "Images/LandingImages/mockup_portal_escuela.png");
            }
        }
    })

    function slideClickedIcon() {
        if (slideClicked) {
            $("#img1").attr("src", "Styles/LandingPageStyle/1_hover.png");
            $("#img2").attr("src", "Styles/LandingPageStyle/2.png");
            $("#img3").attr("src", "Styles/LandingPageStyle/3.png");
            $("#img4").attr("src", "Styles/LandingPageStyle/4.png");
            portalPadres.show("slow");
            portalEscuelas.hide("slow");
            portalEstudiantes.hide("slow");
            portalOrientadores.hide("slow");

        } else if (slide2Clicked) {
            $("#img2").attr("src", "Styles/LandingPageStyle/2_hover.png");
            $("#img1").attr("src", "Styles/LandingPageStyle/1.png");
            $("#img3").attr("src", "Styles/LandingPageStyle/3.png");
            $("#img4").attr("src", "Styles/LandingPageStyle/4.png");
            portalPadres.hide("slow");
            portalEscuelas.hide("slow");
            portalEstudiantes.hide("slow");
            portalOrientadores.show("slow");
        } else if (slide3Clicked) {
            $("#img3").attr("src", "Styles/LandingPageStyle/3_hover.png");
            $("#img2").attr("src", "Styles/LandingPageStyle/2.png");
            $("#img1").attr("src", "Styles/LandingPageStyle/1.png");
            $("#img4").attr("src", "Styles/LandingPageStyle/4.png");
            portalPadres.hide("slow");
            portalEscuelas.hide("slow");
            portalEstudiantes.show("slow");
            portalOrientadores.hide("slow");
        } else if (slide4Clicked) {
            $("#img4").attr("src", "Styles/LandingPageStyle/4_hover.png");
            $("#img2").attr("src", "Styles/LandingPageStyle/2.png");
            $("#img3").attr("src", "Styles/LandingPageStyle/3.png");
            $("#img1").attr("src", "Styles/LandingPageStyle/1.png");
            portalPadres.hide("slow");
            portalEscuelas.show("slow");
            portalEstudiantes.hide("slow");
            portalOrientadores.hide("slow");
        }
    }
    // VERIFICA A MUDANÇA DO VALOR DO SELECT DE PAÍS


});

