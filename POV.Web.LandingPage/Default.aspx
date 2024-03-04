<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="POV.Web.LandingPage.Default1" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>YOY</title>
    <link rel="shortcut icon" href="Images/Yoy_Favicon20px.png" />
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css?family=Montserrat:700" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css?family=Roboto:500" rel="stylesheet" />
    <link href="/Styles/LandingPageStyle/Style.css" rel="stylesheet" />
    <link href="/Scripts/LandingPageScripts/telefonoinput/css/intlTelInput.css" rel="stylesheet" />
    <link href="/Scripts/LandingPageScripts/boxslider/jquery.bxslider.css" rel="stylesheet" />
    <link href="/Styles/LandingPageStyle/Sweetalert2.css" rel="stylesheet" />


    <!-- SLIDER REVOLUTION 4.x CSS SETTINGS -->
    <link rel="stylesheet" type="text/css" href="/Scripts/revolutionslider/css/settings.css" media="screen" />
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-fixed-top shadow">
            <div class="container">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">
                        <span class="sr-only">Toggle navigation</span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <a class="navbar-brand" href="#">
                        <img runat="server" src="~/Images/PaginaAccesoImages/logo.png" />
                    </a>
                </div>
                <div id="navbar" class="navbar-collapse collapse">
                    <ul class="nav navbar-nav navbar-right menubaryoy">
                        <li style="margin-top: 25px;" class="aileron-regular fc-4"><a data-id="acercade" class="menu" href="javascript:void(0)">ACERCA DE</a></li>
                        <li style="margin-top: 25px;" class="aileron-regular fc-4"><a data-id="portales" class="menu" href="javascript:void(0)">PORTALES</a></li>
                        <li style="margin-top: 25px;" class="aileron-regular fc-4"><a data-id="proceso" class="menu" href="javascript:void(0)">PROCESO</a></li>
                        <li style="margin-top: 25px;" class="aileron-regular fc-4"><a data-id="historia" class="menu" href="javascript:void(0)">HISTORIAS</a></li>
                        <li style="margin-top: 25px;" class="aileron-regular fc-4"><a data-id="contacto" class="menu" href="javascript:void(0)">CONTACTO</a></li>
                        <li style="margin-top: 25px;" class="aileron-regular fc-4"><a class="menu" target="_blank" href="http://testpov.grupoplenum.com/Online/PortalSocial/LandingPage/PaginaAcceso.aspx">LO QUIERO</a></li>
                    </ul>
                </div>
                <!--/.navbar-collapse -->
            </div>
        </nav>

        <!-- Main jumbotron for a primary marketing message or call to action -->
        <div class="jumbotron imagenprincipal" style="margin-top: 100px !important">

            <div class="tp-banner-container">
                <div class="tp-banner">
                    <ul>
                        <!-- SLIDE  -->
                        <li data-transition="fade" data-slotamount="7" data-masterspeed="1500">
                            <!-- MAIN IMAGE -->
                            <img src="/Images/LandingImages/slider1.jpg" alt="Principal" data-bgfit="cover" data-bgposition="left top" data-bgrepeat="no-repeat" />
                            <!-- LAYERS -->

                            <!-- LAYER NR. 3 -->
                            <div class="tp-caption skewfromrightshort customout"
                                data-x="right"
                                data-y="90"
                                data-customout="x:0;y:0;z:0;rotationX:0;rotationY:0;rotationZ:0;scaleX:0.75;scaleY:0.75;skewX:0;skewY:0;opacity:0;transformPerspective:600;transformOrigin:50% 50%;"
                                data-speed="500"
                                data-start="800"
                                data-easing="Back.easeOut"
                                data-endspeed="500"
                                data-endeasing="Power4.easeIn"
                                data-captionhidden="on"
                                style="z-index: 4">
                                <div class="fs-40 fc-2 monserrat textoslider" style="text-align: center">
                                    EL PROCESO DE ORIENTACIÓN<br />
                                    MÁS COMPLETO
                                </div>
                                <div style="text-align: center;" class="fs-35 fc-2 aileron-thin textoslidersecundario">A MI PROPIO RITMO Y ONLINE</div>
                                <div style="width: 100%; margin-top: 36px" class="ajusteredes">
                                    <div class="redessociales" style="margin: 0 auto; text-align: center">
                                        <button class="onlinebtn fs-30" id="btnOnline" type="button" onclick="parent.open('http://testpov.grupoplenum.com/Online/PortalSocial/LandingPage/PaginaAcceso.aspx')">DESC&Uacute;BRELO AHORA</button>
                                    </div>
                                    <%-- Se amntiene el codigo solo para amnterner el orden del slide--%>
                                    <ul style="margin: 0 auto; text-align: center; display: none;">
                                        <li class="redessociales" style="display: none;">
                                            <a href="https://twitter.com/Yoyvocacional" target="_blank" style="text-decoration-color: none;">
                                                <img class="twitterslider" src="~/Images/LandingImages/twslider.png" runat="server" />
                                            </a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </li>
                        <li data-transition="fade" data-slotamount="7" data-masterspeed="1500">
                            <!-- MAIN IMAGE -->
                            <img src="/Images/LandingImages/slider2.jpg" alt="Principal" data-bgfit="cover" data-bgposition="left top" data-bgrepeat="no-repeat" />
                            <!-- LAYERS -->

                            <!-- LAYER NR. 3 -->
                            <div class="tp-caption skewfromrightshort customout"
                                data-x="right"
                                data-y="90"
                                data-customout="x:0;y:0;z:0;rotationX:0;rotationY:0;rotationZ:0;scaleX:0.75;scaleY:0.75;skewX:0;skewY:0;opacity:0;transformPerspective:600;transformOrigin:50% 50%;"
                                data-speed="500"
                                data-start="800"
                                data-easing="Back.easeOut"
                                data-endspeed="500"
                                data-endeasing="Power4.easeIn"
                                data-captionhidden="on"
                                style="z-index: 4">
                                <div class="fs-40 fc-2 monserrat textoslider" style="text-align: center">
                                    AHORA PUEDO ACOMPAÑAR
                                    <br />
                                    A MIS HIJOS,
                                </div>
                                <div style="text-align: center;" class="fs-35 fc-2 aileron-thin textoslidersecundario">EN SU PROCESO DE ORIENTACIÓN</div>
                                <div style="width: 100%; margin-top: 36px" class="ajusteredes">
                                    <ul style="margin: 0 auto; text-align: center">
                                        <li class="redessociales">
                                            <a href="https://www.facebook.com/Yoy-Orientaci%C3%B3n-Vocacional-1922257968041032/" target="_blank">
                                                <img class="facebookslider" src="~/Images/LandingImages/fbslider.png" runat="server" />
                                            </a>
                                        </li>
                                        <li class="redessociales">
                                            <a href="https://twitter.com/Yoyvocacional" target="_blank">
                                                <img class="twitterslider" src="~/Images/LandingImages/twslider.png" runat="server" />
                                            </a>
                                        </li>
                                        <li class="redessociales">
                                            <a href="https://mx.linkedin.com/company/yoyorientacionvocacional" target="_blank">
                                                <img class="linkedinslider" src="~/Images/LandingImages/inslider.png" runat="server" />
                                            </a>
                                        </li>
                                        <li class="redessociales">
                                            <a href="https://www.instagram.com/yoyvocacional/" target="_blank">
                                                <img class="instagramslider" src="~/Images/LandingImages/instaslider.png" runat="server" />
                                            </a>
                                        </li>
                                    </ul>
                                </div>
                                <div style="text-align: center; margin-top: 6%" class="fc-2 fs-18 roboto-medium ajustetexto">
                                    SÍGUENOS EN REDES SOCIALES
                                </div>

                            </div>
                        </li>
                        <li data-transition="fade" data-slotamount="7" data-masterspeed="1500">
                            <!-- MAIN IMAGE -->
                            <img src="/Images/LandingImages/slider3.jpg" alt="Principal" data-bgfit="cover" data-bgposition="left top" data-bgrepeat="no-repeat" />
                            <!-- LAYERS -->

                            <!-- LAYER NR. 3 -->
                            <div class="tp-caption skewfromrightshort customout"
                                data-x="right"
                                data-y="90"
                                data-customout="x:0;y:0;z:0;rotationX:0;rotationY:0;rotationZ:0;scaleX:0.75;scaleY:0.75;skewX:0;skewY:0;opacity:0;transformPerspective:600;transformOrigin:50% 50%;"
                                data-speed="500"
                                data-start="800"
                                data-easing="Back.easeOut"
                                data-endspeed="500"
                                data-endeasing="Power4.easeIn"
                                data-captionhidden="on"
                                style="z-index: 4">
                                <div class="fs-40 fc-2 monserrat textoslider" style="text-align: center">
                                    ¡DEJA ATRÁS LA CALIFICACIÓN<br />
                                    DE LAS PRUEBAS! 
                                </div>
                                <div style="text-align: center; text-transform: uppercase" class="fs-35 fc-2 aileron-thin textoslidersecundario">Y concéntrate en tus estudiantes</div>
                                <div style="width: 100%; margin-top: 36px" class="ajusteredes">
                                    <ul style="margin: 0 auto; text-align: center">
                                        <li class="redessociales">
                                            <a href="https://www.facebook.com/Yoy-Orientaci%C3%B3n-Vocacional-1922257968041032/" target="_blank">
                                                <img class="facebookslider" src="~/Images/LandingImages/fbslider.png" runat="server" />
                                            </a>
                                        </li>
                                        <li class="redessociales">
                                            <a href="https://twitter.com/Yoyvocacional" target="_blank">
                                                <img class="twitterslider" src="~/Images/LandingImages/twslider.png" runat="server" />
                                            </a>
                                        </li>
                                        <li class="redessociales">
                                            <a href="https://mx.linkedin.com/company/yoyorientacionvocacional" target="_blank">
                                                <img class="linkedinslider" src="~/Images/LandingImages/inslider.png" runat="server" />
                                            </a>
                                        </li>
                                        <li class="redessociales">
                                            <a href="https://www.instagram.com/yoyvocacional/" target="_blank">
                                                <img class="instagramslider" src="~/Images/LandingImages/instaslider.png" runat="server" />
                                            </a>
                                        </li>
                                    </ul>
                                </div>
                                <div style="text-align: center; margin-top: 6%" class="fc-2 fs-18 roboto-medium ajustetexto">
                                    SÍGUENOS EN REDES SOCIALES
                                </div>

                            </div>
                        </li>

                        <li data-transition="fade" data-slotamount="7" data-masterspeed="1500">
                            <!-- MAIN IMAGE -->
                            <img src="/Images/LandingImages/slider4.jpg" alt="Principal" data-bgfit="cover" data-bgposition="left top" data-bgrepeat="no-repeat" />
                            <!-- LAYERS -->

                            <!-- LAYER NR. 3 -->
                            <div class="tp-caption skewfromrightshort customout"
                                data-x="right"
                                data-y="140"
                                data-customout="x:0;y:0;z:0;rotationX:0;rotationY:0;rotationZ:0;scaleX:0.75;scaleY:0.75;skewX:0;skewY:0;opacity:0;transformPerspective:600;transformOrigin:50% 50%;"
                                data-speed="500"
                                data-start="800"
                                data-easing="Back.easeOut"
                                data-endspeed="500"
                                data-endeasing="Power4.easeIn"
                                data-captionhidden="on"
                                style="z-index: 4">
                                <div class="fs-40 fc-2 monserrat textoslider" style="text-align: center">
                                    DESCUBRE TUS TALENTOS, 
                                </div>
                                <div style="text-align: center;" class="fs-35 fc-2 aileron-thin textoslidersecundario">DIRÍGETE AL ÉXITO PROFESIONAL</div>
                                <div style="width: 100%; margin-top: 36px" class="ajusteredes">
                                    <ul style="margin: 0 auto; text-align: center">
                                        <li class="redessociales">
                                            <a href="https://www.facebook.com/Yoy-Orientaci%C3%B3n-Vocacional-1922257968041032/" target="_blank">
                                                <img class="facebookslider" src="~/Images/LandingImages/fbslider.png" runat="server" />
                                            </a>
                                        </li>
                                        <li class="redessociales">
                                            <a href="https://twitter.com/Yoyvocacional" target="_blank">
                                                <img class="twitterslider" src="~/Images/LandingImages/twslider.png" runat="server" />
                                            </a>
                                        </li>
                                        <li class="redessociales">
                                            <a href="https://mx.linkedin.com/company/yoyorientacionvocacional" target="_blank">
                                                <img class="linkedinslider" src="~/Images/LandingImages/inslider.png" runat="server" />
                                            </a>
                                        </li>
                                        <li class="redessociales">
                                            <a href="https://www.instagram.com/yoyvocacional/" target="_blank">
                                                <img class="instagramslider" src="~/Images/LandingImages/instaslider.png" runat="server" />
                                            </a>
                                        </li>
                                    </ul>
                                </div>
                                <div style="text-align: center; margin-top: 6%" class="fc-2 fs-18 roboto-medium ajustetexto">
                                    SÍGUENOS EN REDES SOCIALES
                                </div>

                            </div>
                        </li>

                    </ul>
                </div>
            </div>

        </div>

        <div class="container" id="acercade">
            <!-- Example row of columns -->
            <div class="row" style="margin-top: 65px;">
                <div class="col-md-12">
                    <h2 style="text-align: center; margin-bottom: 30px" class="aileron-light fs-44 fc-6 aileron-semibold">¿Qué es YOY?</h2>
                </div>
            </div>
            <div class="row" style="margin-bottom: 100px">
                <div class="col-md-12 fc-5 fs-26 aileron-light" style="text-align: center; line-height: 55px; letter-spacing: 3px">
                    El &uacute;nico ecosistema integral para la orientaci&oacute;n y tutor&iacute;a de j&oacute;venes, que adem&aacute;s de guiar y ofrecer informaci&oacute;n para la 
                    toma de decisiones, permite que el equipo psicopedag&oacute;gico y los padres interact&uacute;en en el proceso y optimicen recursos.
                </div>
            </div>
        </div>
        <div class="jumbotron" style="background: #f4f4f4; padding-bottom: 80px; margin: 0px">
            <div class="container">
                <div class="row" style="margin-top: 73px; margin-bottom: 50px">
                    <div class="col-md-12">
                        <h2 style="text-align: center" class="aileron-semibold fc-6 fs-44">Beneficios</h2>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-1"></div>
                    <div class="col-md-3" style="text-align: center">
                        <img src="~/Images/LandingImages/beneficios1.png" runat="server" />
                        <p style="margin-top: 30px" class="aileron-light fc-5 fs-20">
                            Identifica talento y aptitudes de los estudiantes
                        </p>
                    </div>
                    <div class="col-md-4" style="text-align: center">
                        <img src="~/Images/LandingImages/beneficios2.png" runat="server" />
                        <p style="margin-top: 30px" class="aileron-light fc-5 fs-20">
                            Optimiza tiempos del 
orientador en el proceso 
vocacional y mejora tiempos de interpretación y análisis
                        </p>
                    </div>
                    <div class="col-md-3" style="text-align: center">
                        <img src="~/Images/LandingImages/beneficios3.png" runat="server" />
                        <p style="margin-top: 30px" class="aileron-light fc-5 fs-20">
                            Permite a los padres de familia 
estar al tanto del proceso
                        </p>
                    </div>
                    <div class="col-md-1"></div>
                </div>
                <div class="row" style="margin-top: 60px">
                    <div class="col-md-1"></div>
                    <div class="col-md-3" style="text-align: center">
                        <img src="~/Images/LandingImages/beneficios4.png" runat="server" />
                        <p style="margin-top: 30px" class="aileron-light fc-5 fs-20">
                            + 10 TEST psicométricos 
estandarizados y
personalizados
                        </p>
                    </div>
                    <div class="col-md-4" style="text-align: center">
                        <img src="~/Images/LandingImages/beneficios5.png" runat="server" />
                        <p style="margin-top: 30px" class="aileron-light fc-5 fs-20">
                            Reportes de resultados
                        </p>
                    </div>
                    <div class="col-md-3" style="text-align: center">
                        <img src="~/Images/LandingImages/beneficios6.png" runat="server" />
                        <p style="margin-top: 30px" class="aileron-light fc-5 fs-20">
                            Creación de actividades
                        </p>
                    </div>
                    <div class="col-md-1"></div>
                </div>
            </div>
        </div>
        <div class="jumbotron seccionparaquienes" style="background: url(/Images/LandingImages/fondoestudiantes.jpg)">
            <div class="container">
                <div class="row" style="margin-bottom: 90px">
                    <div class="col-lg-6 col-md-offset-6 fs-18" id="paraquien">
                        <h2 style="margin-top: 65px; margin-bottom: 57px" class="textoderecha aileron-semibold fs-44 fc-6">¿Para quién es?</h2>
                        <p style="text-align: justify; letter-spacing: 2px" class="aileron-light fc-5 fs-20">
                            La plataforma está diseñada para el trabajo de tres principales actores educativos: estudiantes, personal educativo y padres de familia.
                        </p>

                        <div style="text-align: justify; letter-spacing: 2px">
                            <ul class="vinietas">
                                <li style="margin-top: 65px" class="aileron-light fc-5 fs-20">Para psicólogos orientadores y personal de los departamentos psicopedagógicos del nivel medio superior, que quieren optimizar su labor profesional. 
                                </li>
                                <li style="margin-top: 65px" class="aileron-light fc-5 fs-20">Para jóvenes de bachillerato de entre 14 y 19 años que disfrutan del uso de la tecnología y que quieren conocer su verdadera vocación.
                                </li>
                                <li style="margin-top: 65px" class="aileron-light fc-5 fs-20">Para padres de familia de jóvenes de bachillerato que quieren acompañar a sus hijos en el proceso
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="container" id="portales">
            <div class="row" style="margin-top: 85px">
                <div class="col-md-12">
                    <h2 style="text-align: center; margin: 0px" class="aileron-semibold fs-44 fc-6">Nuestros portales
                    </h2>
                    <h3 style="text-align: center; margin: 0px" class="aileron-light fs-40 fc-6">de orientación
                    </h3>
                </div>
            </div>
            <div class="row" style="margin-top: 85px; margin-bottom: 100px">
                <div class="col-sm-6 col-xs-12 col-md-6 hidde">
                    <div class="row" id="portalPadres">
                        <h2 class="aileron-bold fs-30">PORTAL PADRES</h2>
                        <ul class="portales" style="margin-top: 40px">
                            <li class="aileron-light fc-5 fs-14">
                                <img runat="server" src="~/Styles/LandingPageStyle/familyicon.png" style="padding-right: 20px;" />
                                <div class="text-mockup">Mejora en el acompañamiento de los hijos. </div>
                            </li>
                            <li class="aileron-light fc-5 fs-14">
                                <img runat="server" src="~/Styles/LandingPageStyle/brainicon.png" style="padding-right: 20px;" />
                                <div class="text-mockup">Aumento en el rendimiento académico. </div>
                            </li>
                            <li class="aileron-light fc-5 fs-14">
                                <img runat="server" src="~/Styles/LandingPageStyle/checkicon.png" style="padding-right: 20px;" />
                                <div class="text-mockup">Seguimiento positivo  de los hijos por parte de los padres. </div>
                            </li>
                            <li class="aileron-light fc-5 fs-14">
                                <img runat="server" src="~/Styles/LandingPageStyle/pigicon.png" style="padding-right: 20px;" />
                                <div class="text-mockup">Ahorros al disminuir el riesgo por deserción escolar o cambios de carrera. </div>
                            </li>
                        </ul>
                    </div>
                    <div class="row" id="portalEstudiantes">
                        <h2 class="aileron-bold fs-30">PORTAL ESTUDIANTE</h2>
                        <ul class="portales" style="margin-top: 40px">
                            <li class="aileron-light fc-5 fs-14">
                                <img id="Img5" runat="server" src="~/Styles/LandingPageStyle/estudiante(1).png" style="padding-right: 20px;" />
                                <div class="text-mockup">Interacción con jóvenes que comparten tus intereses</div>
                            </li>
                            <li class="aileron-light fc-5 fs-14">
                                <img id="Img6" runat="server" src="~/Styles/LandingPageStyle/estudiante(2).png" style="padding-right: 20px;" />
                                <div class="text-mockup">Comunidad segura.</div>
                            </li>
                            <li class="aileron-light fc-5 fs-14">
                                <img id="Img7" runat="server" src="~/Styles/LandingPageStyle/estudiante(3).png" style="padding-right: 20px;" />
                                <div class="text-mockup">Identificar las características personales y aptitudes. </div>
                            </li>
                            <li class="aileron-light fc-5 fs-14">
                                <img id="Img8" runat="server" src="~/Styles/LandingPageStyle/estudiante(4).png" style="padding-right: 20px;" />
                                <div class="text-mockup">Acceso a la plataforma desde cualquier lugar y a tu propio ritmo. </div>
                            </li>
                        </ul>
                    </div>
                    <div class="row" id="portalOrientadores">
                        <h2 class="aileron-bold fs-30">PORTAL ORIENTADOR</h2>
                        <ul class="portales" style="margin-top: 40px">
                            <li class="aileron-light fc-5 fs-14">
                                <img id="Img9" runat="server" src="~/Styles/LandingPageStyle/orientador(1).png" style="padding-right: 20px;" />
                                <div class="text-mockup">Aumento de la eficiencia (más alumnos y más actividades en menos tiempo).</div>
                            </li>
                            <li class="aileron-light fc-5 fs-14">
                                <img id="Img10" runat="server" src="~/Styles/LandingPageStyle/orientador(2).png" style="padding-right: 20px;" />
                                <div class="text-mockup">Confianza en utilizar instrumentos estandarizados.</div>
                            </li>
                            <li class="aileron-light fc-5 fs-14">
                                <img id="Img11" runat="server" src="~/Styles/LandingPageStyle/orientador(3).png" style="padding-right: 20px;" />
                                <div class="text-mockup">Responsabilidad compartida (con los padres y la escuela). </div>
                            </li>
                            <li class="aileron-light fc-5 fs-14">
                                <img id="Img12" runat="server" src="~/Styles/LandingPageStyle/orientador(4).png" style="padding-right: 20px;" />
                                <div class="text-mockup">Detección de necesidades socioemocionales. </div>
                            </li>
                        </ul>
                    </div>
                    <div class="row" id="portalEscuelas">
                        <h2 class="aileron-bold fs-30">PORTAL ESCUELA</h2>
                        <ul class="portales" style="margin-top: 40px">
                            <li class="aileron-light fc-5 fs-14">
                                <img id="Img13" runat="server" src="~/Styles/LandingPageStyle/escuela_(1).png" style="padding-right: 20px;" />
                                <div class="text-mockup">Aumento del enfoque de los estudiantes.</div>
                            </li>
                            <li class="aileron-light fc-5 fs-14">
                                <img id="Img14" runat="server" src="~/Styles/LandingPageStyle/escuela_(2).png" style="padding-right: 20px;" />
                                <div class="text-mockup">Aumento de la eficiencia terminal.</div>
                            </li>
                            <li class="aileron-light fc-5 fs-14">
                                <img id="Img15" runat="server" src="~/Styles/LandingPageStyle/escuela_(3).png" style="padding-right: 20px;" />
                                <div class="text-mockup">Direccionamiento de los esfuerzos.</div>
                            </li>
                            <li class="aileron-light fc-5 fs-14">
                                <img id="Img16" runat="server" src="~/Styles/LandingPageStyle/escuela_(4).png" style="padding-right: 20px;" />
                                <div class="text-mockup">Optimización de recursos humanos y materiales. </div>
                            </li>
                        </ul>
                    </div>
                </div>
                <div class="right col-sm-6 col-xs-12 col-md-6">
                    <img id="imgMockup" class="img-responsive" />
                </div>
                <div class="clear-fix"></div>
                <div class="col-xs-12 col-sm-12 col-md-6 col-md-offset-3">
                    <div class="col-xs-3 col-md-3">
                        <a href="javascript:void(0)" id="slide3">
                            <img class="img-responsive center-block" id="img3" alt="Portal Estudiante" /></a>
                    </div>
                    <div class="col-xs-3 col-md-3">
                        <a href="javascript:void(0)" id="slide2">
                            <img class="img-responsive center-block" id="img2" alt="Portal Orientador" /></a>
                    </div>
                    <div class="col-xs-3 col-md-3">
                        <a href="javascript:void(0)" id="slide">
                            <img class="img-responsive center-block" id="img1" alt="Portal Tutor" /></a>
                    </div>
                    <div class="col-xs-3 col-md-3">
                        <a href="javascript:void(0)" id="slide4">
                            <img class="img-responsive center-block" id="img4" alt="Portal Escuela" /></a>
                    </div>
                </div>

            </div>
        </div>
        <div class="jumbotron seccionproceso" id="proceso" style="background: url(/Images/LandingImages/nuestroproceso.jpg); padding-top: 70px">
            <h2 style="text-align: center; margin: 0px" class="aileron-semibold fs-44 fc-2">Nuestro proceso
            </h2>
            <h3 style="text-align: center; margin: 0px" class="aileron-light fs-40 fc-2">en 4 pasos
            </h3>
            <div class="container">
                <div class="row">
                    <div class="col-sm-2 col-xs-1"></div>
                    <div class="col-md-5 col-sm-8 col-xs-10 col-md-offset-7">
                        <img style="width: 100%" src="~/Images/LandingImages/proceso.png" runat="server" />
                    </div>
                    <div class="col-sm-2 col-xs-1"></div>
                </div>
                <div class="row" style="margin-bottom: 50px">
                    <div class="col-sm-2 col-xs-1"></div>
                    <div class="col-md-5 col-sm-8 col-xs-10 col-md-offset-7 fs-22 fc-2 aileron-semibold">
                        ¡Comienza ya a utilizar nuestra herramienta!
                    </div>
                    <div class="col-sm-2 col-xs-1"></div>
                </div>
            </div>
        </div>
        <div class="container" id="contacto">
            <div class="row" style="margin-bottom: 125px">
                <div class="col-md-12" style="text-align: center">
                    <div class="row">
                        <div class="col-md-12">
                            <h2 style="margin-top: 100px; margin-bottom: 65px;" class="fs-35 fc-5 aileron-regular">¿Quiéres saber más sobre la plataforma YOY?</h2>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="col-md-4" style="margin-bottom: 10px;">
                                <button class="linksyoybtn fs-30" id="linkActivacion" type="button" onclick="parent.open('http://testpov.grupoplenum.com/Activacion/Administracion/Aspirantes/NuevoAspirante.aspx')">TEST GRATIS</button>
                                <p class="fc-5 aileron-regular text-contacto">
                                    Toma una prueba sin costo
                                </p>
                            </div>
                            <div class="col-md-4" style="margin-bottom: 10px;">
                                <button class="linksyoybtn fs-30" id="linkOnline" type="button" onclick="parent.open('http://testpov.grupoplenum.com/Online/PortalSocial/LandingPage/PaginaAcceso.aspx')">YOY ONLINE</button>
                                <p class="fc-5 aileron-regular text-contacto">
                                    Por s&oacute;lo $158 MXN/mes
                                </p>
                            </div>
                            <div class="col-md-4" style="margin-bottom: 10px;">
                                <button class="linksyoybtn fs-30" id="showformulario" type="button" style="margin-bottom: 5px;">¿M&Aacute;S INFO?</button>
                                <p class="fc-5 aileron-regular text-contacto">
                                    M&aacute;ndanos un mensaje
                                </p>
                            </div>
                        </div>
                    </div>


                </div>
            </div>
        </div>
        <div id="formulario" style="display: none">
            <div class="container">
                <div class="row" id="formcontacto">
                    <div class="modal-header">
                        <button id="hideformulario" type="button" class="close" data-dismiss="modal" aria-label="Close" style="color: #FFF; opacity: 1; filter: none;">
                            <span aria-hidden="true" style="font-size: 25px; opacity: 1;">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="col-md-12">
                                <input type="text" class="form-control custominput" id="nombre" placeholder="Nombre completo*" required="required" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <select class="form-control custominput" id="soy">
                                    <option value="NA">Motivo de contacto*</option>
                                    <option value="Soy parte de una institución educativa y me gustaría probarlo">Soy parte de una institución educativa y me gustaría probarlo</option>
                                    <option value="Me gustaría saber más para distribuirla">Me gustaría saber más para distribuirla</option>
                                    <option value="Otro">Otro</option>
                                </select>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-6">
                                <input type="text" class="form-control custominput" id="institucion" placeholder="Institución" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-6">
                                <input type="text" class="form-control custominput" id="cargo" placeholder="Cargo" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4">
                                <select class="form-control custominput" id="pais" onchange="getEstado(this.value)">
                                    <option value="">País*</option>
                                </select>
                            </div>
                            <div class="col-md-4">
                                <select class="form-control custominput" id="estado">
                                    <option value="">Estado*</option>
                                </select>
                            </div>
                            <div class="col-md-4">
                                <input type="text" class="form-control custominput" id="ciudad" placeholder="Ciudad*" required="required" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-6">
                                <input type="tel" class="form-control custominput" id="telefono" maxlength="10" placeholder="Teléfono*" required="required" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-6">
                                <input type="email" class="form-control custominput" id="email" placeholder="E-mail*" required="required" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <select class="form-control custominput" id="comonos">
                                    <option value="NA">¿Cómo supiste de nosotros?</option>
                                    <option value="Facebook">Facebook</option>
                                    <option value="Twitter">Twitter</option>
                                    <option value="LinkedIn">LinkedIn</option>
                                    <option value="Recomendación">Recomendación</option>
                                    <option value="Google">Google</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <textarea class="form-control custominput" id="mensaje" placeholder="Mensaje*" rows="3"></textarea>
                        </div>
                        <div class="col-md-12">
                            <button id="contactMe" type="button" style="padding: 8px 40px; float: right" class="custominput">Enviar</button>
                        </div>
                        <div class="col-md-12" style="color: #fff; font-style: italic; font-size: 16px">
                            <span>si lo prefieres envianos un correo a</span> <span style="color: #ff9e16;">info@yoyvocacional.com</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="jumbotron" id="historia">
            <div class="container">
                <div class="row" style="margin-top: 100px">
                    <div class="col-md-12">
                        <h2 class="aileron-semibold fs-22 fc-5" style="text-align: center">¿TE IMAGINAS QUÉ PASARÍA SI PUDIERAS CONOCER TUS VERDADEROS TALENTOS?</h2>
                        <h3 class="aileron-light fs-24 fc-5" style="text-align: center; margin: 0px">Historias de éxito de nuestros usuarios                            
                        </h3>
                        <div class="foot"></div>
                    </div>
                </div>
                <ul class="bxslider" style="margin-top: 25px">
                    <li>
                        <div class="col-lg-1 col-md-1 col-sm-1"></div>
                        <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12 text-center">
                            <div class="reproductor fondovideo text-center" style="height: 350px;">
                                <video controls>
                                    <source src="Video/ileana.mp4" type="audio/mp4" />
                                    Tu navegador no soporta los video HTML5
                                </video>
                            </div>
                        </div>
                        <div class="roboto-light_14 col-lg-7 col-md-7 col-sm-12 col-xs-12 centerimage textomargin" style="height: 320px;">
                            <div class="txtlong aileron-light fs-18 fc-5" style="margin-bottom: 12px">
                                <br />
                                Yo us&eacute; la plataforma YOY Vocacional, muy recomendable, es una plataforma que te ayudar&aacute; a identificar todas tus habilidades y todo tu
                                potencial, no pierdas oportunidad de usarla.
                            </div>
                            <div class="aileron-light fs-16 fc-5" style="margin-bottom: 12px">
                                <b>- Ileana Rivera,</b><br />
                            </div>
                            <div class="aileron-light fs-14 fc-5" style="margin-bottom: 16px">
                                Coordinadora de tutor&iacute;as.
                            </div>
                        </div>
                        <div class="col-lg-1 col-md-1 col-sm-1"></div>
                    </li>
                    <li>
                        <div class="col-lg-1 col-md-1"></div>
                        <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12 text-center">
                            <div class="reproductor fondovideo text-center" style="height: 350px;">
                                <video controls>
                                    <source src="Video/luisa.mp4" type="audio/mp4" />
                                    Tu navegador no soporta los video HTML5
                                </video>
                            </div>
                        </div>
                        <div class="roboto-light_14 col-lg-7 col-md-7 col-sm-12 col-xs-12 centerimage textomargin" style="height: 400px;">
                            <div class="txtlong aileron-light fs-18 fc-5" style="margin-bottom: 12px">
                                <br />
                                YOY es una herramienta de mucha utilidad para todos los estudiantes para reconocer e identificar sus &aacute;reas de oportunidad y crecimiento en el
                                desarrollo de su carrera profesional. &Uacute;senla, benef&iacute;ciense y optim&iacute;cenla.
                            </div>
                            <div class="aileron-light fs-16 fc-5" style="margin-bottom: 16px">
                                <b>- Ma. Luisa Sanchez,</b><br />
                            </div>
                            <div class="aileron-light fs-14 fc-5" style="margin-bottom: 16px">
                                Jefe desarrollo acad&eacute;mico.
                            </div>
                        </div>
                        <div class="col-lg-1 col-md-1"></div>
                    </li>
                    <li>
                        <div class="col-lg-1 col-md-1"></div>
                        <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12 text-center">
                            <div class="reproductor fondovideo text-center" style="height: 350px;">
                                <video controls>
                                    <source src="Video/sharon.mp4" type="audio/mp4" />
                                    Tu navegador no soporta los video HTML5
                                </video>
                            </div>
                        </div>
                        <div class="roboto-light_14 col-lg-7 col-md-7 col-sm-12 col-xs-12 centerimage textomargin" style="height: 400px;">
                            <div class="txtlong aileron-light fs-18 fc-5" style="margin-bottom: 12px">
                                <br />
                                La plataforma me sirvi&oacute; de mucho, mediante de ella descubr&iacute; mis habilidad y debilidades, la recomiendo porque nos permite antes de 
                                iniciar la universidad descubrir en que nos podemos enfocar para desarrollarnos de una mejor manera.
                            </div>
                            <div class="aileron-light fs-16 fc-5" style="margin-bottom: 12px">
                                <b>- Sharon Mart&iacute;nez,</b><br />
                            </div>
                            <div class="aileron-light fs-14 fc-5" style="margin-bottom: 16px">
                                Estudiante CdMx.
                            </div>
                        </div>
                        <div class="col-lg-1 col-md-1"></div>
                    </li>
                </ul>
            </div>
        </div>
        <div class="cintoazul">
            <a href="javascript:void(0)" class="menu" data-id="top">
                <img src="~/Images/LandingImages/arribaicon.png" runat="server" class="arribaicon" />
            </a>
        </div>

        <div class="footer">
            <div class="container">
                <div class="col-md-7 aileron-light  fs-22 fc-2" id="textofooter">
                    Queremos conocerte y apoyarte, comunícate con nosotros y síguenos en nuestras redes sociales
                </div>
                <div class="col-md-2"></div>
                <div class="col-md-5">
                    <ul style="margin: 0 auto; text-align: center" id="redesfooter">
                        <li class="redessociales">
                            <a href="https://www.facebook.com/Yoy-Orientaci%C3%B3n-Vocacional-1922257968041032/" target="_blank">
                                <img class="facebook" src="~/Images/PaginaAccesoImages/facebook.png" runat="server" />
                            </a>
                        </li>
                        <li class="redessociales">
                            <a href="https://twitter.com/Yoyvocacional" target="_blank">
                                <img class="twitter" src="~/Images/PaginaAccesoImages/twitter.png" runat="server" />
                            </a>
                        </li>
                        <li class="redessociales">
                            <a href="https://mx.linkedin.com/company/yoyorientacionvocacional" target="_blank">
                                <img class="linkedin" src="~/Images/PaginaAccesoImages/in.png" runat="server" />
                            </a>
                        </li>
                        <li class="redessociales">
                            <a href="http://instagram.com/yoyvocacional/" target="_blank">
                                <img class="instagram" src="~/Images/PaginaAccesoImages/instagram.png" runat="server" />
                            </a>
                        </li>

                    </ul>
                </div>
            </div>
        </div>
        <!-- /container -->
    </form>
    <script src="https://code.jquery.com/jquery-2.2.4.min.js" type="text/javascript" integrity="sha256-BbhdlvQf/xTY9gja0Dq3HiwQF8LaCRTXxZKRutelT44=" crossorigin="anonymous"></script>
    <script src="/Styles/LandingPageStyle/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="/Scripts/LandingPageScripts/boxslider/jquery.bxslider.js" type="text/javascript"></script>
    <%--<script src="/Scripts/LandingPageScripts/boxslider/vendor/jquery.fitvids.js" type="text/javascript"></script>--%>
    <!-- SLIDER REVOLUTION 4.x SCRIPTS  -->
    <script type="text/javascript" src="/Scripts/revolutionslider/js/jquery.themepunch.plugins.min.js"></script>
    <script type="text/javascript" src="/Scripts/revolutionslider/js/jquery.themepunch.revolution.min.js"></script>
    <script src="/Scripts/LandingPageScripts/landing.js" type="text/javascript"></script>
    <script src="/Scripts/LandingPageScripts/telefonoinput/js/intlTelInput.js" type="text/javascript"></script>
    <script src="/Scripts/city_state.js" type="text/javascript"></script>
    <script src="/Scripts/LandingPageScripts/Sweetalert2.js" type="text/javascript"></script>
    <!-- LOOK THE DOCUMENTATION FOR MORE INFORMATIONS -->
    <script type="text/javascript">

        var revapi;
        var testimonios;

        jQuery(document).ready(function () {

            revapi = jQuery('.tp-banner').revolution(
             {
                 delay: 9000,
                 startwidth: 1170,
                 startheigth: 500,
                 hideThumbs: 10,
                 fullWidth: "on"
             });

            $('.bxslider').bxSlider({
                pager: false
            });

            //$("#reproductor").fitVids();
            //src = "https://www.youtube.com/watch?v=k6NxH_R-xe0"
            // $('iframe[src*="youtube"]').parent().fitVids();
            // Basic FitVids Test
            //$(".reproductor").fitVids();
            // Custom selector and No-Double-Wrapping Prevention Test
            // $(".reproductor").fitVids({ customSelector: "iframe[src^='https://www.youtube.com']" });

            $(".menu").click(function () {
                var seccion = $(this).data("id");
                if (seccion == "top") {

                    $('html, body').animate({
                        scrollTop: $("html, body").offset().top
                    }, 1000);

                } else {

                    $('html, body').animate({
                        scrollTop: ($("#" + seccion).offset().top) - 70
                    }, 1000);

                }


            })

            $("#showformulario").click(function () {

                if (!$("#contacto").is(":visible")) {

                    $("#formulario").slideUp(500);

                    setTimeout(function () {
                        $("#contacto").slideDown();
                    }, 700)
                } else {
                    $("#contacto").slideUp(500);

                    setTimeout(function () {
                        $("#formulario").slideDown();
                    }, 700)

                }

            })

            //$("#showformulario").click(function () {

            //    if (!$("#contacto").is(":visible")) {

            //        $("#formulario").slideUp(500);

            //        setTimeout(function () {
            //            $("#contacto").slideDown();
            //        }, 700)
            //    } else {
            //        $("#contacto").slideUp(500);

            //        setTimeout(function () {
            //            $("#formulario").slideDown();
            //        }, 700)

            //    }

            //})

            $("#hideformulario").click(function () {

                if (!$("#contacto").is(":visible")) {

                    $("#formulario").slideUp(500);

                    setTimeout(function () {
                        $("#contacto").slideDown();
                    }, 700)
                } else {
                    $("#contacto").slideUp(500);

                    setTimeout(function () {
                        $("#formulario").slideDown();
                    }, 700)

                }

            })

            $("#telefono").intlTelInput({
                // allowDropdown: false,
                // autoHideDialCode: false,
                // autoPlaceholder: "off",
                // dropdownContainer: "body",
                // excludeCountries: ["us"],
                // formatOnDisplay: false,
                // geoIpLookup: function(callback) {
                //   $.get("http://ipinfo.io", function() {}, "jsonp").always(function(resp) {
                //     var countryCode = (resp && resp.country) ? resp.country : "";
                //     callback(countryCode);
                //   });
                // },
                // hiddenInput: "full_number",
                // initialCountry: "auto",
                // nationalMode: false,
                // onlyCountries: ['us', 'gb', 'ch', 'ca', 'do'],
                // placeholderNumberType: "MOBILE",
                // preferredCountries: ['cn', 'jp'],
                // separateDialCode: true,
                utilsScript: "/Scripts/LandingPageScripts/telefonoinput/js/utils.js",
                preferredCountries: ["mx", "AR", "co", "ve", "cl"]
            });


        });	//ready



        /*var deck = new $.scrolldeck({
            buttons: '.nav-link',
            easing: 'easeInOutExpo'
        });*/

    </script>
    <script>
        (function (i, s, o, g, r, a, m) {
            i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                (i[r].q = i[r].q || []).push(arguments)
            }, i[r].l = 1 * new Date(); a = s.createElement(o),
            m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
        })(window, document, 'script', 'https://www.google-analytics.com/analytics.js', 'ga');

        ga('create', 'UA-105020357-1', 'auto');
        ga('send', 'pageview');

    </script>
</body>
</html>
