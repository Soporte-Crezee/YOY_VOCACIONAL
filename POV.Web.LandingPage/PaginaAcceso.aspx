<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaginaAcceso.aspx.cs" Inherits="POV.Web.LandingPage.PaginaAcceso1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <title>Página de acceso</title>
    <link rel="shortcut icon" href="Images/Yoy_Favicon20px.png" />
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css?family=Montserrat:700" rel="stylesheet" />
    <link href="/Styles/PaginaAccesoStyle/Style.css" rel="stylesheet" />

    <style type="text/css">
        a {
            color:#fff !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-static-top shadow">
            <div class="container">
                <div class="navbar-header">
                    <a class="navbar-brand" href="#">
                        <img runat="server" src="~/Images/PaginaAccesoImages/logo.png" />
                    </a>
                </div>
                <div id="navbar" class="navbar-collapse collapse">
                    <ul class="nav navbar-nav navbar-right">
                        <li style="margin-top: 40px;" class="aileron-regular fc-4">ACCESO A PORTALES</li>
                    </ul>
                </div>
                <!--/.navbar-collapse -->
            </div>
        </nav>

        <!-- Main jumbotron for a primary marketing message or call to action -->
        <div class="jumbotron imagenprincipal" style="background: url(/Images/PaginaAccesoImages/imagen.jpg)">
            <h2 style="text-align: center" class="monserrat bold fs-38 fc-1">DESCUBRE TUS TALENTOS</h2>
            <h3 style="text-align: center; margin-top: 0px" class="fs-38 fc-1 aileron-light">DIRÍGETE AL ÉXITO PROFESIONAL</h3>
            <div style="text-align: center" id="botonprincipal">
                <a id="A4" runat="server" class="buttoninicial fc-2 fs-24 aileron-regular botonresponsivo">PORTAL ESTUDIANTES</a>
            </div>
        </div>

        <div class="container">
            <!-- Example row of columns -->
            <div class="row" style="margin-top: 120px; margin-bottom: 94px">
                <div class="col-md-12">
                    <h2 style="text-align: center" class="aileron-light fs-33 fc-3">OTROS PORTALES</h2>
                </div>
            </div>
            <div class="row" style="text-align: center">
                <div class="col-md-4">
                    <a runat="server" id="A1" style="text-decoration:none">
                        <img src="~/Images/PaginaAccesoImages/orientador.png" id="orientador" runat="server" />
                        <p class="aileron-bold fs-20 fc-3" style="margin-top: 30px">
                            Portal<br />
                            Orientador
                        </p>
                    </a>
                </div>
                <div class="col-md-4">
                    <a runat="server" id="A2" style="text-decoration:none">
                        <img src="~/Images/PaginaAccesoImages/padres.png" id="padres" runat="server" />
                        <p class="aileron-bold fs-20 fc-3" style="margin-top: 30px">
                            Portal<br />
                            Padres
                        </p>
                    </a>
                </div>
                <div class="col-md-4">
                    <a runat="server" id="A3" style="text-decoration:none">
                        <img src="~/Images/PaginaAccesoImages/escuela.png" id="escuela" runat="server" />
                        <p class="aileron-bold fs-20 fc-3" style="margin-top: 30px">
                            Portal<br />
                            Escuela
                        </p>
                    </a>
                </div>
            </div>
            <div class="row" style="margin-top: 130px; margin-bottom: 50px">
                <div class="col-md-4"></div>
                <div class="col-md-4 col-sm-12 col-xs-12">
                    <ul style="margin: 0 auto; text-align: center">
                        <li>
                            <a href="https://www.facebook.com/Yoy-Orientaci%C3%B3n-Vocacional-1922257968041032/" target="_blank">
                                <img class="facebook" src="~/Images/PaginaAccesoImages/facebook.png" runat="server" />
                            </a>
                        </li>
                        <li>
                            <a href="https://twitter.com/Yoyvocacional" target="_blank">
                                <img class="twitter" src="~/Images/PaginaAccesoImages/twitter.png" runat="server" />
                            </a>
                        </li>
                        <li>
                            <a href="https://www.linkedin.com/company-beta/24783354/" target="_blank">
                                <img class="linkedin" src="~/Images/PaginaAccesoImages/in.png" runat="server" />
                            </a>
                        </li>
                        <li>
                            <a href="http://instagram.com/yoyvocacional" target="_blank">
                                <img class="instagram" src="~/Images/PaginaAccesoImages/instagram.png" runat="server" />
                            </a>
                        </li>

                    </ul>
                </div>
                <div class="col-md-4"></div>
            </div>
            <div class="row">
                <div class="col-md-3">
                </div>
                <div class="col-md-6">
                    <p style="text-align: center" class="aileron-light fs-20 fc-3">
                        Queremos conocerte y apoyarte, comunícate con nosotros y síguenos en nuestras redes sociales
                    </p>
                </div>
                <div class="col-md-3">
                </div>
            </div>
        </div>
        <!-- /container -->
    </form>
    <script src="https://code.jquery.com/jquery-3.2.1.min.js" integrity="sha256-hwg4gsxgFZhOsEEamdOYGBf13FyQuiTwlAQgxVSNgt4=" crossorigin="anonymous"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
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
