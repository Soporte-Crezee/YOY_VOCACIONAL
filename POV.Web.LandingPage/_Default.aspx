<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="_Default.aspx.cs" Inherits="POV.Web.LandingPage.Default" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta name="author" content="Script Tutorials" />
    <title>CAMPUSOV Landing page</title>
    <link rel="shortcut icon" href="Images/Yoy_Favicon20px.png" />
    <meta name="viewport" content="width=device-width, height=device-height, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <!-- attach CSS styles -->
    <link href="~/Styles/style.css" rel="stylesheet" />
    <link href="../Sliders/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Styles/bootstrap.css" rel="stylesheet" />
    <link href="../Styles/modal.css" rel="stylesheet" />

    <script src="../Scripts/jquery-1.12.1.min.js"></script>
    
    <script src="../Sliders/js/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-1.8.2.min.js"></script>
    <script src="../Scripts/jquery.scrollTo-1.4.3.1.min.js"></script>
    <script src="../Scripts/jquery.easing.1.3.js"></script>
    <script src="../Scripts/jquery.scrollorama.js"></script>
    <script src="../Scripts/jquery.scrolldeck.js"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>

    <script>
        $(document).ready(function () {
            function updateScreen() {
                var contloginheight_ = (document.getElementById("barranavegable").offsetHeight);
                var d = document.getElementsByClassName("divadaptable");
                for (var i = 0; i < d.length; i++) {
                    d[i].style.marginTop = contloginheight_ + "px";
                }
            }

            $('.about1').click(function () {
                document.getElementById('about').style.display = 'none';
                document.getElementById('divRolloMain').style.display = 'block';
                document.getElementById('about1').style.display = 'block';
                document.getElementById('about2').style.display = 'none';
                document.getElementById('about3').style.display = 'none';
                document.getElementById('about4').style.display = 'none';
            });
            $('.about2').click(function () {
                document.getElementById('about').style.display = 'none';
                document.getElementById('divRolloMain').style.display = 'block';
                document.getElementById('about1').style.display = 'none';
                document.getElementById('about2').style.display = 'block';
                document.getElementById('about3').style.display = 'none';
                document.getElementById('about4').style.display = 'none';
            });
            $('.about3').click(function () {
                document.getElementById('about').style.display = 'none';
                document.getElementById('divRolloMain').style.display = 'block';
                document.getElementById('about1').style.display = 'none';
                document.getElementById('about2').style.display = 'none';
                document.getElementById('about3').style.display = 'block';
                document.getElementById('about4').style.display = 'none';
            });
            $('.about4').click(function () {
                document.getElementById('about').style.display = 'none';
                document.getElementById('divRolloMain').style.display = 'block';
                document.getElementById('about1').style.display = 'none';
                document.getElementById('about2').style.display = 'none';
                document.getElementById('about3').style.display = 'none';
                document.getElementById('about4').style.display = 'block';
            });

            var deck = new $.scrolldeck({
                buttons: '.nav-link',
                easing: 'easeInOutExpo'
            });

            var srcPortales = [
                {
                    "imgPortal": 'Images/SOV_ELEMENTOS_portal_universidad.png',
                    "imgMockup": 'Images/PORTAL_MOCKUP-aspirantes.png',
                    "titlePortal": 'PORTAL ESTUDIANTES'
                },
                {
                    "imgPortal": 'Images/SOV_ELEMENTOS_portal_orientadores.png',
                    "imgMockup": 'Images/PORTAL_MOCKUP-orientador.png',
                    "titlePortal": 'PORTAL ORIENTADOR'
                },
                {
                    "imgPortal": 'Images/SOV_ELEMENTOS_portal_tutores.png',
                    "imgMockup": 'Images/PORTAL_MOCKUP-tutores.png',
                    "titlePortal": 'PORTAL PADRES'
                },
                {
                    "imgPortal": 'Images/SOV_ELEMENTOS_portal_administracion.png',
                    "imgMockup": 'Images/PORTAL_MOCKUP-admon.png',
                    "titlePortal": 'PORTAL ESCUELA'
                }
            ]
            $('.invento').each(function (i) {
                $(this).hover(
                    function () {
                        $(this).css('background', 'black');
                        $('.titulo').eq(i).addClass('title-card');
                        $('.cuerpo').eq(i).addClass('body-card');

                        document.getElementById('imgPortal').src = srcPortales[i].imgPortal;
                        document.getElementById('imgMockup').src = srcPortales[i].imgMockup;
                        document.getElementById('titlePortal').textContent = srcPortales[i].titlePortal;
                    },
                    function () {
                        var seleccion = $(this).hasClass('select-card');
                        if (!seleccion) {
                            $(this).css('background', 'white');
                            $('.titulo').eq(i).removeClass('title-card');
                            $('.cuerpo').eq(i).removeClass('body-card');
                        }
                    }
                )
            });
            var actualizando = setInterval(updateScreen, 100);
        });
        function divShow(bloq, desplega) {
            obj = document.getElementById(bloq);
            obj.style.display = desplega;
        }

        $(function () {
            $('#<%=txtMensaje.ClientID %>').val('');
            $('#<%=txtMensaje.ClientID %>').removeAttr('required');

            $("#btnClose").on("click", function () {
                $('#<%=txtMensaje.ClientID %>').val('');
                $('#<%=txtMensaje.ClientID %>').removeAttr('required');
            });

            $("#btnWhats").on("click", function () {
                $('#<%=txtMensaje.ClientID %>').val('');
                $('#<%=txtMensaje.ClientID %>').attr('required', '');
            });
        });
    </script>

    <style>
        @media screen and (min-width: 320px) and (max-width: 450px) {
            .col-xs-1 {
                width: 25%;
                *width: 25%;
                padding-top: 20px;
            }

            .roboto-medium_38 {
                font-family: Roboto-Medium;
                font-size: 15pt;
                Color: #0067ac;
            }

            .roboto-regular_23 {
                font-family: Roboto-Regular;
                font-size: 10pt;
                color: black;
            }

            #lineblack {
                width: 75%;
            }

            #proceso {
                padding-top: 20px;
            }

            .leyendaproc {
                color: white;
                font-size: 15px;
            }

            .showflecha {
                display: none;
            }

            .myimg {
                min-height: 51px;
                min-width: 51px;
            }

            .ltrproceso {
                font-family: Roboto-Regular;
                font-size: 8pt;
                color: white;
                padding: 0;
            }

            .roboto-regular_17 {
                font-family: Roboto-Regular;
                font-size: 10pt;
                color: white !important;
            }

            .roboto-regular_17_black {
                font-family: Roboto-Regular;
                font-size: 12pt;
            }

            .margenlistaul {
                margin-top: 0px;
                margin-right: 50px;
            }

            .margentiraop {
                margin-top: 5px;
                margin-bottom: 5px;
            }
        }

        @media screen and (min-width: 451px) and (max-width:768px) {
            .col-xs-1 {
                width: 14.285714285714285714285714285714%;
                *width: 14.285714285714285714285714285714%;
                padding-left: 5px;
                padding-right: 5px;
                padding-top: 50px;
            }

            .roboto-medium_38 {
                font-family: Roboto-Medium;
                font-size: 20pt;
                Color: #0067ac;
            }

            .roboto-regular_23 {
                font-family: Roboto-Regular;
                font-size: 15pt;
                color: black;
            }

            #lineblack {
                width: 50%;
            }

            #proceso {
                padding-top: 45px;
            }

            .leyendaproc {
                color: white;
                font-size: 30px;
            }

            .showflecha {
                display: initial;
            }

            .ltrproceso {
                font-family: Roboto-Regular;
                font-size: 8pt;
                color: white;
                padding: 0;
            }

            .roboto-regular_17 {
                font-family: Roboto-Regular;
                font-size: 12pt;
                color: white !important;
            }

            .margenlistaul {
                margin-top: 0px;
                margin-right: 50px;
            }

            .margentiraop {
                margin-top: 5px;
                margin-bottom: 5px;
            }
        }

        @media screen and (min-width: 769px) and (max-width: 991px) {
            .col-xs-1 {
                width: 14.285714285714285714285714285714%;
                *width: 14.285714285714285714285714285714%;
                padding-top: 50px;
            }

            .roboto-medium_38 {
                font-family: Roboto-Medium;
                font-size: 38pt;
                Color: #0067ac;
            }

            .roboto-regular_23 {
                font-family: Roboto-Regular;
                font-size: 23pt;
                color: black;
            }

            #lineblack {
                width: 50%;
            }

            #proceso {
                padding-top: 90px;
            }

            .leyendaproc {
                color: white;
                font-size: 60px;
            }

            .showflecha {
                display: initial;
            }

            .ltrproceso {
                font-family: Roboto-Regular;
                font-size: 15pt;
                color: white;
            }

            .roboto-regular_17 {
                font-family: Roboto-Regular;
                font-size: 12pt;
                color: white !important;
            }

            .margenlistaul {
                margin-top: 0px;
                margin-right: 50px;
            }

            .margentiraop {
                margin-top: 5px;
                margin-bottom: 5px;
            }
        }

        @media screen and (min-width: 992px) and (max-width: 1024px) {
            .col-xs-1 {
                width: 14.285714285714285714285714285714%;
                *width: 14.285714285714285714285714285714%;
                padding-top: 50px;
            }

            .roboto-medium_38 {
                font-family: Roboto-Medium;
                font-size: 38pt;
                Color: #0067ac;
            }

            .roboto-regular_23 {
                font-family: Roboto-Regular;
                font-size: 23pt;
                color: black;
            }

            #lineblack {
                width: 50%;
            }

            #proceso {
                padding-top: 90px;
            }

            .leyendaproc {
                color: white;
                font-size: 60px;
            }

            .showflecha {
                display: initial;
            }

            .ltrproceso {
                font-family: Roboto-Regular;
                font-size: 15pt;
                color: white;
            }

            .margenlistaul {
                margin-top: 20px;
                margin-right: 50px;
            }
        }

        @media screen and (min-width: 1025px) and (max-width: 1291px) {
            .col-xs-1 {
                width: 14.285714285714285714285714285714%;
                *width: 14.285714285714285714285714285714%;
                padding-top: 50px;
            }

            .roboto-medium_38 {
                font-family: Roboto-Medium;
                font-size: 38pt;
                Color: #0067ac;
            }

            .roboto-regular_23 {
                font-family: Roboto-Regular;
                font-size: 23pt;
                color: black;
            }

            #lineblack {
                width: 50%;
            }

            #proceso {
                padding-top: 90px;
            }

            .leyendaproc {
                color: white;
                font-size: 60px;
            }

            .showflecha {
                display: initial;
            }

            .ltrproceso {
                font-family: Roboto-Regular;
                font-size: 15pt;
                color: white;
            }

            .margenlistaul {
                margin-top: 20px;
                margin-right: 50px;
            }
        }

        @media screen and (min-width: 1292px) and (max-width: 1920px) {
            .col-xs-1 {
                width: 14.285714285714285714285714285714%;
                *width: 14.285714285714285714285714285714%;
                padding-top: 50px;
            }

            .roboto-medium_38 {
                font-family: Roboto-Medium;
                font-size: 38pt;
                Color: #0067ac;
            }

            .roboto-regular_23 {
                font-family: Roboto-Regular;
                font-size: 23pt;
                color: black;
            }

            #lineblack {
                width: 50%;
            }

            #proceso {
                padding-top: 90px;
            }

            .leyendaproc {
                color: white;
                font-size: 60px;
            }

            .showflecha {
                display: initial;
            }

            .ltrproceso {
                font-family: Roboto-Regular;
                font-size: 15pt;
                color: white;
            }

            .margenlistaul {
                margin-top: 20px;
                margin-right: 50px;
            }
        }

        .divproc {
            padding-bottom: 1em;
        }

        ul {
            list-style: none;
            margin: 0;
            padding: 0;
            width: 100%;
            text-align: justify;
        }

            ul:after {
                content: ".";
                display: inline-block;
                width: 100%;
                height: 0;
                visibility: hidden;
            }

            ul li {
                display: inline-block;
            }
    </style>
</head>
<body>
    <form runat="server">
        <%--<div class="col-xs-13 col-sm-13 col-md-13 col-lg-13">--%>
        <!-- navigation panel -->
        <div id="inicio" class="col-xs-12 col-sm-12 col-md-12 col-lg-12" style="background-color: black">
            <div class="container">
                <div class="modal fade dialog-md" tabindex="-1"
                    data-keyboard="false" data-backdrop="static"
                    role="dialog" aria-labelledby="ventanaModalLabel" id="ventanaModal">
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                            <div class="modal-header" style="background: #33acfd">
                                <h1 class="col-xs-11" style="color: white">Comun&iacute;cate con YOY</h1>
                                <button type="button" class="close col-xs-1" data-dismiss="modal" aria-hidden="true" style="color: white"><span class="glyphicon glyphicon-remove"></span></button>                                
                            </div>
                            <div class="modal-body">
                                <div class="">
                                    <div class="row bs-wizard" style="border-bottom: 0;">
                                        <div class="col-md-12" style="padding: 5px 0px 0px 0px">
                                            <div class="col-xs-12">
                                                <div class="col-xs-12 form-group">
                                                    <label style="padding: 0px 0px 10px 0px">¡Escr&iacute;benos! Por favor indicanos cu&aacute;l es tu inquietud</label>
                                                    <asp:TextBox ID="txtMensaje" runat="server" placeholder="Mensaje" CssClass="form-control"
                                                        TextMode="MultiLine" Rows="4"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <div class="form-group">
                                    <div class="col-md-6" style="padding-right: 15px;">
                                        <asp:Button runat="server" ID="BtnEnviar" CssClass="btn btn-green btn-md" Text="Enviar" OnClick="BtnEnviar_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div style="background-color: black">
                <div style="margin-left: 120px; margin-right: 120px">
                    <nav id="barranavegable" class="navbar navbar-right navbar-toggleable-md navbar-light navbar-inverse navbar-fixed-top" style="margin-bottom: 0px !important; background-color: black;">
                        <button class="navbar-toggler navbar-toggler-right" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                            <span style="color: white" class="navbar-toggler-icon"></span>
                        </button>
                        <div class="col-xs-3">
                            <img class="img-responsive" src="Images/SOV_ELEMENTOS_logo_VOCAREER1.png" />
                        </div>
                        <div class="col-xs-9">
                            <div class="collapse navbar-collapse" id="navbarNav">
                                <ul class="margenlistaul">
                                    <li class="nav-item margentiraop">
                                        <a class="nav-link roboto-regular_17" style="padding: 0px;" href="#acercade">ACERCA DE
                                        </a>
                                    </li>
                                    <li class="nav-item margentiraop">
                                        <a class="nav-link roboto-regular_17" style="padding: 0px;" href="#portales">PORTALES
                                        </a>
                                    </li>
                                    <li class="nav-item margentiraop">
                                        <a class="nav-link roboto-regular_17" style="padding: 0px;" href="#proceso">PROCESO
                                        </a>
                                    </li>
                                    <li class="nav-item margentiraop">
                                        <a class="nav-link roboto-regular_17" style="padding: 0px;" href="#historias">HISTORIAS
                                        </a>
                                    </li>
                                    <li class="nav-item margentiraop">
                                        <a class="nav-link roboto-regular_17" style="padding: 0px;" href="#contacto">CONTACTO
                                        </a>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </nav>
                </div>
            </div>
        </div>
        <!-- /navigation panel -->


        <!-- first section - Home -->
        <div id="home" class="container" style="margin: 0px; padding: 0px">
            <div id="casa" class="carousel slide" data-ride="carousel">
                <ol class="carousel-indicators">
                    <li data-target="#casa" data-slide-to="0" class="active"></li>
                    <li data-target="#casa" data-slide-to="1"></li>
                    <li data-target="#casa" data-slide-to="2"></li>
                    <li data-target="#casa" data-slide-to="3"></li>
                </ol>
                <div class="carousel-inner" role="listbox" style="height: 100%;">
                    <div class="carousel-item active pull-right" style="height: 100%">
                        <div class="divadaptable divCarrusel" style="background-image: url(../images/LANDING_CAMPUSOV1.jpg)">
                            <div class="divContentSlider col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                <div id="divSlider1" class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div style="text-align: center">
                                        <div style="text-align: center;" class="leyenda col-xs-9 col-xs-offset-4 col-sm-7 col-sm-offset-5 col-md-7 col-md-offset-5 col-lg-6 col-lg-offset-5">
                                            <p class="fs-type-ltr-msg">
                                                DESCUBRE TUS TALENTOS, DIRÍGETE AL ÉXITO PROFESIONAL
                                            </p>
                                        </div>
                                        <div class="configiconredsocial col-xs-9 col-xs-offset-5 col-sm-6 col-sm-offset-6 col-md-6 col-md-offset-6 col-lg-5 col-lg-offset-6" style="text-align: center;">
                                            <div class="col-xs-3 col-sm-3">
                                                <img src="../Images/contacto_twitter_ARRIBA.png" class="img-responsive" style="min-height: 30px; min-width: 30px; display: initial" />
                                            </div>
                                            <div class="col-xs-3 col-sm-3">
                                                <img src="../Images/contacto_facebook_ARRIBA.png" class="img-responsive" style="min-height: 30px; min-width: 30px; display: initial" />
                                            </div>
                                            <div class="col-xs-3 col-sm-3">
                                                <img src="../Images/contacto_blog_ARRIBA.png" class="img-responsive" style="min-height: 30px; min-width: 30px; display: initial" />
                                            </div>
                                            <div class="col-xs-9 col-sm-9">
                                                <p class="configtxtredsocial roboto-medium_13">SÍGUENOS EN REDES SOCIALES</p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="carousel-item pull-right" style="height: 100%">
                        <div class="divadaptable divCarrusel" style="background-image: url(../images/LANDING_CAMPUSOV2.jpg)">
                            <div class="divContentSlider col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                <div id="divSlider2" class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div style="text-align: center">
                                        <div style="text-align: center;" class="leyenda col-xs-9 col-xs-offset-4 col-sm-9 col-sm-offset-5 col-md-12 col-lg-6 col-lg-offset-5">
                                            <p class="fs-type-ltr-msg">
                                                YOY MEJORA MIS TIEMPOS Y ESTOY MÁS CON MIS ESTUDIANTES
                                            </p>
                                        </div>
                                        <div class="configiconredsocial col-xs-9 col-xs-offset-5 col-sm-9 col-sm-offset-5 col-md-12 col-md-offset-3 col-lg-5 col-lg-offset-6" style="text-align: center;">
                                            <div class="col-xs-3 col-sm-3">
                                                <img src="../Images/contacto_twitter_ARRIBA.png" class="img-responsive" style="min-height: 30px; min-width: 30px; display: initial" />
                                            </div>
                                            <div class="col-xs-3 col-sm-3">
                                                <img src="../Images/contacto_facebook_ARRIBA.png" class="img-responsive" style="min-height: 30px; min-width: 30px; display: initial" />
                                            </div>
                                            <div class="col-xs-3 col-sm-3">
                                                <img src="../Images/contacto_blog_ARRIBA.png" class="img-responsive" style="min-height: 30px; min-width: 30px; display: initial" />
                                            </div>
                                            <div class="col-xs-9 col-sm-9">
                                                <p class="configtxtredsocial roboto-medium_13">SÍGUENOS EN REDES SOCIALES</p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="carousel-item pull-right" style="height: 100%">
                        <div class="divadaptable divCarrusel" style="background-image: url(../images/LANDING_CAMPUSOV3.jpg)">
                            <div class="divContentSlider col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                <div id="divSlider3" class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div style="text-align: center">
                                        <div style="text-align: center;" class="leyenda col-xs-9 col-xs-offset-4 col-sm-9 col-sm-offset-5 col-md-12 col-lg-6 col-lg-offset-5">
                                            <p class="fs-type-ltr-msg">
                                                EL PROCESO DE ORIENTACIÓN MÁS COMPLETO Y A MI PROPIO RITMO
                                            </p>
                                        </div>
                                        <div class="configiconredsocial col-xs-9 col-xs-offset-5 col-sm-9 col-sm-offset-5 col-md-12 col-md-offset-3 col-lg-5 col-lg-offset-6" style="text-align: center;">
                                            <div class="col-xs-3 col-sm-3">
                                                <img src="../Images/contacto_twitter_ARRIBA.png" class="img-responsive" style="min-height: 30px; min-width: 30px; display: initial" />
                                            </div>
                                            <div class="col-xs-3 col-sm-3">
                                                <img src="../Images/contacto_facebook_ARRIBA.png" class="img-responsive" style="min-height: 30px; min-width: 30px; display: initial" />
                                            </div>
                                            <div class="col-xs-3 col-sm-3">
                                                <img src="../Images/contacto_blog_ARRIBA.png" class="img-responsive" style="min-height: 30px; min-width: 30px; display: initial" />
                                            </div>
                                            <div class="col-xs-9 col-sm-9">
                                                <p class="configtxtredsocial roboto-medium_13">SÍGUENOS EN REDES SOCIALES</p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="carousel-item pull-right" style="height: 100%">
                        <div class="divadaptable divCarrusel" style="background-image: url(../images/LANDING_CAMPUSOV4.jpg)">
                            <div class="divContentSlider col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                <div id="divSlider4" class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div style="text-align: center">
                                        <div style="text-align: center;" class="leyenda col-xs-9 col-xs-offset-4 col-sm-9 col-sm-offset-5 col-md-12 col-lg-6 col-lg-offset-5">
                                            <p class="fs-type-ltr-msg">
                                                AHORA PUEDO ACOMPAÑAR A MIS HIJOS EN SU PROCESO DE ORIENTACIÓN
                                            </p>
                                        </div>
                                        <div class="configiconredsocial col-xs-9 col-xs-offset-5 col-sm-9 col-sm-offset-5 col-md-12 col-md-offset-3 col-lg-5 col-lg-offset-6" style="text-align: center;">
                                            <div class="col-xs-3 col-sm-3">
                                                <img src="../Images/contacto_twitter_ARRIBA.png" class="img-responsive" style="min-height: 30px; min-width: 30px; display: initial" />
                                            </div>
                                            <div class="col-xs-3 col-sm-3">
                                                <img src="../Images/contacto_facebook_ARRIBA.png" class="img-responsive" style="min-height: 30px; min-width: 30px; display: initial" />
                                            </div>
                                            <div class="col-xs-3 col-sm-3">
                                                <img src="../Images/contacto_blog_ARRIBA.png" class="img-responsive" style="min-height: 30px; min-width: 30px; display: initial" />
                                            </div>
                                            <div class="col-xs-9 col-sm-9">
                                                <p class="configtxtredsocial roboto-medium_13">SÍGUENOS EN REDES SOCIALES</p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- /first section -->

        <!-- second section - About -->
        <div id="acercade" style="">
            <div id="about" class="configzoom">
                <div id="divHeaderAbout" style="text-align: center">
                    <p class="roboto-medium_38">CONOCE MÁS ACERCA DE NOSOTROS</p>
                    <hr width="40%" style="color: black !important; margin-bottom: 50px" />
                    <div id="divAboutMain" style="text-align: center;" class="col-xs-12 col-sm-12 col-md-12 col-lg-12  ">
                        <a class="about1" style="cursor: pointer">
                            <div id="divAbout1" class="col-xs-12 col-sm-12 col-md-6 col-lg-3  ">
                                <div style="margin-bottom: 30px">
                                    <input type="button" class="btnPerfilaciones" />
                                </div>
                                <p style="color: black !important" class="roboto-regular_17">¿QUÉ ES YOY?</p>
                                <p style="color: black !important" class="leyenda-hiden roboto-light_15">Es el único ecosistema integral para la orientación vocacional de jóvenes...</p>
                            </div>
                        </a>
                        <a class="about2" style="cursor: pointer">
                            <div id="divAbout2" class="col-xs-12 col-sm-12 col-md-6 col-lg-3  ">
                                <div style="margin-bottom: 30px">
                                    <input type="button" class="btnApoyoEscolar" />
                                </div>
                                <p style="color: black !important" class="roboto-regular_17">¿PARA QUIÉN ES?</p>
                                <p style="color: black !important" class="leyenda-hiden roboto-light_15">La plataforma está diseña para el trabajo de los tres principales actores educativos ...</p>
                            </div>
                        </a>
                        <a class="about3" style="cursor: pointer">
                            <div id="divAbout3" class="col-xs-12 col-sm-12 col-md-6 col-lg-3  ">
                                <div style="margin-bottom: 30px">
                                    <input type="button" class="btnOrientadores" />
                                </div>
                                <p style="color: black !important" class="roboto-regular_17">RECURSOS</p>
                                <p style="color: black !important" class="leyenda-hiden roboto-light_15">La plataforma YOY cuenta con los principales Test psicométricos estandarizados, entre los que se encuentran ...</p>
                            </div>
                        </a>
                        <a class="about4" style="cursor: pointer">
                            <div id="divAbout4" class="col-xs-12 col-sm-12 col-md-6 col-lg-3  ">
                                <div style="margin-bottom: 30px">
                                    <input type="button" class="btnComunidad" />
                                </div>
                                <p style="color: black !important" class="roboto-regular_17">BENEFICIOS</p>
                                <p style="color: black !important" class="leyenda-hiden roboto-light_15">Permite a los estudiantes identificar sus talentos y aptitudes, y lo mejor es que ...</p>
                            </div>
                        </a>
                    </div>
                </div>
            </div>
            <div id="divRolloMain" style="background-color: black; display: none">
                <div class="configzoom">
                    <div id="about1" style="background-color: black; text-align: center; display: none">
                        <div id="divImagenes">
                            <div class="col-xs-12 col-sm-12 col-md-6 col-lg-3  ">
                                <input type="button" class="about1 btnPerfilaciones" style="background-image: url(../Images/SOV_ELEMENTOS_perfilaciones_on.png)" /><br />
                                <p class="roboto-regular_15">¿QUÉ ES YOY?</p>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-6 col-lg-3  ">
                                <input type="button" class="about2 btnApoyoEscolar" />
                                <p style="color: #33acff" class="roboto-regular_15">¿PARA QUIEN ES?</p>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-6 col-lg-3  ">
                                <input type="button" class="about3 btnOrientadores" />
                                <p style="color: #33acff" class="roboto-regular_15">RECURSOS</p>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-6 col-lg-3  ">
                                <input type="button" class="about4 btnComunidad" />
                                <p style="color: #33acff" class="roboto-regular_15">COMUNIDAD</p>
                            </div>
                        </div>
                        <br />
                        <div id="divRollo">
                            <p class="roboto-bold_39 col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                ¿QUÉ ES YOY?<p />
                            <p class="roboto-light_23" style="text-align: justify">Es el único ecosistema integral para la orientación vocacional de jóvenes, que además de guiar y ofrecer información de carreras universitarias, permite que el equipo psicopedagógico y los padres interactúen en el proceso.</p>
                        </div>
                    </div>
                    <div id="about2" style="background-color: black; text-align: center; display: none">
                        <div id="divImagenes2">
                            <div class="col-xs-12 col-sm-12 col-md-6 col-lg-3  ">
                                <input type="button" class="about1 btnPerfilaciones" /><br />
                                <p style="color: #33acff" class="roboto-regular_15">¿QUÉ ES YOY?</p>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-6 col-lg-3  ">
                                <input type="button" class="about2 btnApoyoEscolar" style="background-image: url(../Images/SOV_ELEMENTOS_apoyoescolar_on.png)" />
                                <p class="roboto-regular_15">¿PARA QUIEN ES?</p>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-6 col-lg-3  ">
                                <input type="button" class="about3 btnOrientadores" />
                                <p style="color: #33acff" class="roboto-regular_15">RECURSOS</p>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-6 col-lg-3  ">
                                <input type="button" class="about4 btnComunidad" />
                                <p style="color: #33acff" class="roboto-regular_15">COMUNIDAD</p>
                            </div>
                        </div>
                        <br />
                        <div id="divRollo2">
                            <p class="roboto-bold_39">
                                ¿PARA QUIÉN ES?<p />
                            <p class="roboto-light_23" style="text-align: justify">La plataforma está diseña para el trabajo de tres principales actores educativos: estudiantes, personal educativo y padres de familia. Para jóvenes de bachillerato de entre 14 y 19 años que disfrutan del uso de la tecnología y que quieren conocer su verdadera vocación. Para psicólogos orientadores y personal de los departamentos psicopedagógicos del nivel medio superior, que quieren optimizar su labor profesional. Para padres de familia de jóvenes de bachillerato que quieren acompañar a sus hijos en el proceso.</p>
                        </div>
                    </div>
                    <div id="about3" style="background-color: black; text-align: center; display: none">
                        <div id="divImagenes3">
                            <div class="col-xs-12 col-sm-12 col-md-6 col-lg-3  ">
                                <input type="button" class="about1 btnPerfilaciones" /><br />
                                <p style="color: #33acff" class="roboto-regular_15">¿QUÉ ES YOY?</p>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-6 col-lg-3  ">
                                <input type="button" class="about2 btnApoyoEscolar" />
                                <p style="color: #33acff" class="roboto-regular_15">¿PARA QUIEN ES?</p>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-6 col-lg-3  ">
                                <input type="button" class="about3 btnOrientadores" style="background-image: url(../Images/SOV_ELEMENTOS_orientadores_on.png)" />
                                <p class="roboto-regular_15">RECURSOS</p>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-6 col-lg-3  ">
                                <input type="button" class="about4 btnComunidad" />
                                <p style="color: #33acff" class="roboto-regular_15">COMUNIDAD</p>
                            </div>
                        </div>
                        <div id="divRollo3">
                            <p class="roboto-bold_39">
                                RECURSOS<p />
                            <p class="roboto-light_23" style="text-align: justify">La plataforma YOY cuenta con los principales Test psicométricos estandarizados, entre los que se encuentran los de Inteligencia (2), Intereses vocacionales (3), Personalidad (2), Hábitos de estudio y Valores; los Test están sistematizados para la calificación automática y la generación de reportes de resultados. Así mismo, Los estudiantes pueden comunicarse con otros estudiantes que comparten sus intereses y conocer más acerca del estatus de las principales carreras profesionales de la actualidad. Además, los orientadores pueden crear actividades complementarias en el sistema y agendar asesorías, para profundizar aún más en el proceso.</p>
                        </div>
                    </div>
                    <div id="about4" style="background-color: black; text-align: center; display: none">
                        <div id="divImagenes4">
                            <div class="col-xs-12 col-sm-12 col-md-6 col-lg-3  ">
                                <input type="button" class="about1 btnPerfilaciones" /><br />
                                <p style="color: #33acff" class="roboto-regular_15">¿QUÉ ES YOY?</p>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-6 col-lg-3  ">
                                <input type="button" class="about2 btnApoyoEscolar" />
                                <p style="color: #33acff" class="roboto-regular_15">¿PARA QUIEN ES?</p>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-6 col-lg-3  ">
                                <input type="button" class="about3 btnOrientadores" />
                                <p style="color: #33acff" class="roboto-regular_15">RECURSOS</p>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-6 col-lg-3  ">
                                <input type="button" class="about4 btnComunidad" style="background-image: url(../Images/SOV_ELEMENTOS_comunidad_on.png)" />
                                <p class="roboto-regular_15">COMUNIDAD</p>
                            </div>
                        </div>
                        <div id="divRollo4">
                            <p class="roboto-bold_39">
                                BENEFICIOS<p />
                            <p class="roboto-light_23" style="text-align: justify">Permite que los estudiantes identifiquen sus talentos y aptitudes, y lo mejor es que cada quien puede ir a su propio ritmo; esto incrementa la certeza del estudiante en su decisión vocacional. Optimiza los tiempos del orientador en el proceso vocacional, al elimina los tiempos de calificación de pruebas y mejora los tiempos de interpretación y análisis con sus estudiantes. Además, permite que los padres de familia estén al tanto del proceso.</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- /second section -->

        <!-- third section - Portales -->
        <div id="portales" class=" col-xs-12 col-sm-12 col-md-12 col-lg-12" style="padding-top: 90px">
            <div class="margin120" style="text-align: center;">
                <p class="roboto-medium_38" style="color: #072c48;">NUESTROS PORTALES</p>
                <p class="roboto-regular_23" style="color: #072c48; margin-top: 0px">DE ORIENTACION</p>
                <hr width="40%" style="color: #072c48 !important" />
                <div class="form-group col-xs-13 col-sm-13 col-md-13 col-lg-13">
                    <div class="form-group col-xs-12 col-sm-12 col-md-12 col-lg-6">
                        <div id="divEstudiantes" class="invento card col-xs-12 col-sm-12 col-md-12 col-lg-12" style="border: none !important">
                            <div class="card-block" style="border-bottom: solid 1px;">
                                <div style="text-align: left; font-weight: bold" class="roboto-regular_17_black titulo">
                                    PORTAL ESTUDIANTES
                                </div>
                                <div style="text-align: justify" class="leyenda-hiden roboto-light_15_black cuerpo">
                                    <p />
                                    Los alumnos pueden detectar sus características personales mediante una serie de recursos psicopedagógicos. Además pueden comunicarse con su orientador y con otros compañeros.
                                </div>
                            </div>
                        </div>
                        <div id="divOrientador" class="invento card col-xs-12 col-sm-12 col-md-12 col-lg-12" style="border: none !important">
                            <div class="card-block" style="border-bottom: solid 1px; border-top: solid 1px;">
                                <div style="text-align: left; font-weight: bold" class="roboto-regular_17_black titulo">
                                    PORTAL ORIENTADOR
                                </div>
                                <div style="text-align: justify" class="leyenda-hiden roboto-light_15_black cuerpo">
                                    <p />
                                    Aquí los docentes crean y asignan actividades, pueden consultar los expedientes de los estudiantes con los  reportes automatizados y calendarizar sesiones de trabajo con los estudiantes.
                                </div>
                            </div>
                        </div>
                        <div id="divPadres" class="invento card col-xs-12 col-sm-12 col-md-12 col-lg-12" style="border: none !important">
                            <div class="card-block" style="border-bottom: solid 1px; border-top: solid 1px;">
                                <div style="text-align: left; font-weight: bold" class="roboto-regular_17_black titulo">
                                    PORTAL PADRES
                                </div>
                                <div style="text-align: justify" class="leyenda-hiden roboto-light_15_black cuerpo">
                                    <p />
                                    Los padres de familia de cada estudiante pueden darle seguimiento al proceso vocacional que realizan sus hijos, mediante el ingreso al apartado de reportes.
                                </div>
                            </div>
                        </div>
                        <div id="divEscuela" class="invento card col-xs-12 col-sm-12 col-md-12 col-lg-12" style="border: none !important">
                            <div class="card-block" style="border-top: solid 1px;">
                                <div style="text-align: left; font-weight: bold" class="roboto-regular_17_black titulo">
                                    <p />
                                    PORTAL ESCUELA
                                </div>
                                <div style="text-align: justify" class="leyenda-hiden roboto-light_15_black cuerpo">
                                    <p />
                                    Este apartado es para que los directivos del bachillerato utilicen un super usuario para darle seguimiento a la orientación vocacional que realiza el equipo psicopedagógico; puede cargar nuevos orientadores y crear publicaciones para los estudiantes.
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group col-xs-12 col-sm-12 col-md-12 col-lg-6">
                        <table id="Table1" style="margin-left: 20px">
                            <tr>
                                <td style="font-weight: bold; font-size: xx-large; font-family: Tahoma">
                                    <img id="imgPortal" style="height: 90px; width: 90px" src="../Images/SOV_ELEMENTOS_portal_universidad.png" />
                                    <p id="titlePortal" style="color: #072c48" class="roboto-bold_25">PORTAL ESTUDIANTES</p>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp</td>
                            </tr>
                            <tr>
                                <td>&nbsp</td>
                            </tr>
                            <tr>
                                <td>
                                    <img id="imgMockup" class="img-responsive" src="../Images/PORTAL_MOCKUP-aspirantes.png" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

            </div>
        </div>
        <!-- /third section -->

        <!-- Fourth section - Proceso -->
        <div id="proceso" class="col-xs-12">
            <div style="text-align: center">
                <div id="encabezado" class="col-xs-12">
                    <p class="roboto-medium_38" style="color: white; margin: 0px 0px 0px 0px">NUESTROS PROCESO</p>
                    <p class="roboto-regular_23" style="color: white; margin: 0px 0px 0px 0px">EN 4 ETAPAS</p>
                    <hr id="lineblack" width="25%" />
                </div>

                <div id="cuerpo1" style="text-align: center;" class="col-xs-12">
                    <div class="col-xs-1" style="display: initial">
                        <img src="Images/SOV_ELEMENTOS_ landing_plan.png" class="img-responsive myimg" style="display: initial" />
                    </div>
                    <div class="col-xs-1 showflecha">
                        <img src="Images/SOV_ELEMENTOS_flecha.png" class="img-responsive myimg" style="display: initial" />
                    </div>
                    <div class="col-xs-1" style="display: initial">
                        <img src="Images/SOV_ELEMENTOS_ landing_opciones.png" class="img-responsive myimg" style="display: initial" />
                    </div>
                    <div class="col-xs-1 showflecha">
                        <img src="Images/SOV_ELEMENTOS_flecha.png" class="img-responsive myimg" style="display: initial" />
                    </div>
                    <div class="col-xs-1" style="display: initial">
                        <img src="Images/SOV_ELEMENTOS_ landing_ asesoria.png" class="img-responsive myimg" style="display: initial" />
                    </div>
                    <div class="col-xs-1 showflecha">
                        <img src="Images/SOV_ELEMENTOS_flecha.png" class="img-responsive myimg" style="display: initial" />
                    </div>
                    <div class="col-xs-1" style="display: initial">
                        <img src="Images/SOV_ELEMENTOS_ landing_realizarlo.png" class="img-responsive myimg" style="display: initial" />
                    </div>
                </div>
                <div id="cuerpo2" style="text-align: center;" class="col-xs-12">
                    <div class="col-xs-1 ltrproceso" style="display: initial; padding-top: 15px">
                        <p>CREA TU PERFIL</p>
                    </div>
                    <div class="col-xs-1 showflecha" style="padding-top: 15px"></div>
                    <div class="col-xs-1 ltrproceso" style="display: initial; padding-top: 15px">
                        <p>REALIZA LAS ACTIVIDADES</p>
                    </div>
                    <div class="col-xs-1 showflecha" style="padding-top: 15px"></div>
                    <div class="col-xs-1 ltrproceso" style="display: initial; padding-top: 15px">
                        <p>EXPLORA TUS OPCIONES</p>
                    </div>
                    <div class="col-xs-1 showflecha" style="padding-top: 15px"></div>
                    <div class="col-xs-1 ltrproceso" style="display: initial; padding-top: 15px">
                        <p>¡LLÉVALO A CABO!</p>
                    </div>
                </div>
                <div id="cuerpo3" style="text-align: center;" class="col-xs-12 showflecha">
                    <div class="col-xs-1 ltrproceso" style="display: initial; padding-top: 15px; color: black">
                        <p>Ingresa con tu usuario y completa tu perfil con una foto y tus datos generales. ¡Es muy sencillo!</p>
                    </div>
                    <div class="col-xs-1 showflecha" style="padding-top: 15px"></div>
                    <div class="col-xs-1 ltrproceso" style="display: initial; padding-top: 15px; color: black">
                        <p>Tu orientador te asignará una serie de recursos psicopedagógicos para identificar tus características personales y tus talentos naturales</p>
                    </div>
                    <div class="col-xs-1 showflecha" style="padding-top: 15px"></div>
                    <div class="col-xs-1 ltrproceso" style="display: initial; padding-top: 15px; color: black">
                        <p>Una vez que completas el proceso, podrás explorar con el orientador las mejores opciones profesionales de acuerdo a tu perfil</p>
                    </div>
                    <div class="col-xs-1 showflecha" style="padding-top: 15px"></div>
                    <div class="col-xs-1 ltrproceso" style="display: initial; padding-top: 15px; color: black">
                        <p>Ya tienes todos los elementos para una decisión consciente, sólo falta dar el paso hacia la felicidad y el éxito profesional</p>
                    </div>
                </div>

                <div id="pie" style="position: static; bottom: 0 !important">
                    <p class="leyendaproc">¡Comienza ya a utilizar nuestra herramienta!</p>
                </div>
            </div>
        </div>
        <!-- /Fourth section -->

        <!-- Fifth section - Historias -->
        <div id="historias" class="col-xs-12 contenthistorias" style="">
            <div style="background: #072c48;">
                <div class="confighistorias" style="">
                    <div id="stories" class="carousel slide" data-ride="carousel" style="text-align: center; background: rgb(7, 44, 72);">
                        <div style="width: 100%; padding-bottom: 30px">
                            <p class="cooperhewitt-light_31" style="margin: 0px 0px 0px 10px">¿TE IMAGINAS QUÉ PASARÍA SI PUDIERAS CONOCER TUS VERDADEROS TALENTOS?</p>
                            <p class="cooperhewitt-light_28" style="margin: 0px 0px 0px 0px;">Historias de éxito de nuestros usuarios</p>
                            <div style="background-color: white; height: 5px; width: 50%;" class="col-xs-offset-3"></div>
                        </div>
                        <div class="carousel-inner estilocarrusel" role="listbox" style="">
                            <div class="carousel-item active">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <div class="col-lg-2 col-lg-offset-0 col-md-3 col-md-offset-0 col-sm-6 col-sm-offset-3 col-xs-6 col-xs-offset-3 frameimghist">
                                        <img src="Images/SOV_ELEMENTOS_foto testimonio1.png" class="img img-responsive" style="display: initial" />
                                    </div>
                                    <div class="roboto-light_14 col-lg-9 col-md-9 col-sm-12 col-xs-12 frameltrhist">
                                        <p class="txtshort">
                                            Utilicé la plataforma en mi escuela y el proceso fue muy sencillo.
                                        </p>
                                        <p class="txtlong">
                                            Utilicé la plataforma en mi escuela y el proceso fue muy sencillo. El psicólogo nos asignaba actividades desde la plataforma y veíamos el avance del proceso de orientación. Al finalizar  ya tenía claras mis aptitudes y capacidades, con lo que la elección de mi carrera fue muy simple. Ahora me siento muy contenta estudiando mi carrera.
                                        </p>
                                        <p>
                                            Marilu Pérez,<br />
                                        </p>
                                        <p>
                                            Yucatán, México.
                                        </p>
                                    </div>
                                </div>
                            </div>
                            <div class="carousel-item">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <div class="col-lg-2 col-lg-offset-0 col-md-3 col-md-offset-0 col-sm-6 col-sm-offset-3 col-xs-6 col-xs-offset-3 frameimghist">
                                        <img src="Images/SOV_ELEMENTOS_foto testimonio2.png" class="img img-responsive" style="display: initial" />
                                    </div>
                                    <div class="roboto-light_14 col-lg-9 col-md-9 col-sm-12 col-xs-12 frameltrhist">
                                        <p class="txtshort">
                                            Cuando inicié la prepa me sentía muy confundido, no sabía qué iba a hacer en mi vida.
                                        </p>
                                        <p class="txtlong">
                                            Cuando inicié la prepa me sentía muy confundido, no sabía qué iba a hacer en mi vida. Al ver que mis compañeros decidían lo que querían estudiar comencé a sentirme desesperado. Traté por varios medios pero no me sentía seguro con las respuestas, hasta que usé la plataforma YOY. Ahora me siento seguro de mi decisión, y ya quiero iniciar mi carrera.
                                        </p>
                                        <p>
                                            Carlos Barrera,<br />
                                        </p>
                                        <p>
                                            Campeche, México.
                                        </p>
                                    </div>
                                </div>
                            </div>
                            <div class="carousel-item">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <div class="col-lg-2 col-lg-offset-0 col-md-3 col-md-offset-0 col-sm-6 col-sm-offset-3 col-xs-6 col-xs-offset-3 frameimghist">
                                        <img src="Images/SOV_ELEMENTOS_foto testimonio3.png" class="img img-responsive" style="display: initial" />
                                    </div>
                                    <div class="roboto-light_14 col-lg-9 col-md-9 col-sm-12 col-xs-12 frameltrhist">
                                        <p class="txtshort">
                                            Me gustó mucho la experiencia de utilizar la plataforma YOY.
                                        </p>
                                        <p class="txtlong">
                                            Me gustó mucho la experiencia de utilizar la plataforma YOY, ya que además de las actividades que realizaba podía hablar constantemente tanto con mi orientador como con otros chicos y chicas que también estaban en el proceso de búsqueda de su vocación en la vida. Se siente bien compartir con personas que están en la misma etapa que yo.
                                        </p>
                                        <p>
                                            Sofía Moncada,<br />
                                        </p>
                                        <p>
                                            Puebla, México.
                                        </p>
                                    </div>
                                </div>
                            </div>
                            <a class="carousel-control-prev" href="#stories" role="button" data-slide="prev">
                                <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                                <span class="sr-only">Previous</span>
                            </a>
                            <a class="carousel-control-next" href="#stories" role="button" data-slide="next">
                                <span class="carousel-control-next-icon" aria-hidden="true"></span>
                                <span class="sr-only">Next</span>
                            </a>


                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- /Fifth section -->

        <div class="col-xs-12" style="position: relative; background-color: #0067ac; height: 5px; text-align: center"></div>

        <!-- Sixth section - Contacto -->
        <div id="contacto" class="col-xs-12" style="background: black;">
            <div style="text-align: center; background: black">
                <div class="col-xs-12 estilotitulocontacto">
                    <p class="font-helvb">CONTÁCTANOS</p>
                </div>
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <div class="col-lg-6 col-md-12 col-sm-12 col-xs-12">
                        <p class="font-helv">Queremos conocerte y apoyarte, comunícate con nosotros y síguenos en nuestras redes sociales</p>
                    </div>
                    <div class="col-xs-12 col-lg-6">
                        <div class="col-xs-4 col-lg-2">
                            <a href="https://www.facebook.com/Yoy-Orientaci%C3%B3n-Vocacional-1922257968041032/">
                                <img src="Images/LANDIN CAMPUSOV4.png" class="img-responsive contact" />
                            </a>
                        </div>
                        <div class="col-xs-4 col-lg-2">
                            <a href="https://twitter.com/YoyOrientacion">
                                <img src="Images/LANDIN CAMPUSOV5.png" class="img-responsive contact" />
                            </a>
                        </div>
                        <div class="col-xs-4 col-lg-2">
                            <a href="https://www.linkedin.com/company-beta/24783354/">
                                <img src="Images/LANDIN CAMPUSOV.png" class="img-responsive contact" />
                            </a>

                        </div>
                        <div class="col-xs-4 col-xs-offset-2 col-lg-2 col-lg-offset-0">
                            <a href="http://instagram.com/yoyorientacion">
                                <img src="Images/LANDIN CAMPUSOV3.png" class="img-responsive contact" />
                                <%--Falta El icono de instragam--%>
                            </a>
                        </div>
                        <div class="col-xs-4 col-lg-2">
                            <a href="#" data-toggle="modal" data-target=".dialog-md" id="btnWhats">
                                <img src="Images/LANDIN CAMPUSOV2.png" class="img-responsive contact" />
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- /Sixth section -->
    </form>
</body>
</html>
