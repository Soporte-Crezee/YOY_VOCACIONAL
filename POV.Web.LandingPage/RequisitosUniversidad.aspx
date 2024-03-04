﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequisitosUniversidad.aspx.cs" Inherits="POV.Web.LandingPage.RequisitosUniversidad" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Vocareer - ORIENTACIÓN VOCACIONAL</title>
    <!-- Bootstrap CSS -->
    <link href="Styles/bootstrap.css" rel="stylesheet" />
    <link href="Styles/bootstrap-theme.css" rel="stylesheet" />
    <link href="Styles/theme.css" rel="stylesheet" />

    <!-- Bootstrap JS -->
    <script src="Scripts/jquery.js"></script>
    <script src="Scripts/bootstrap.js"></script>

    <style>
        body {
            background-color: #f5f5f5;
        }

        .box-shadow {
            box-shadow: 0px 2px 9px 0px rgba(0, 0, 0, 0.35);
        }

            .box-shadow:hover {
                background-color: #fff4d2;
            }

        .page {
            min-height: 450px;
        }

        p {
            text-align: justify;
        }
    </style>

    <script type="text/javascript">
        var ano = (new Date).getFullYear();

        $(document).ready(function () {
            $("#fecha").text(ano);
        });
    </script>
</head>
<body style="background-color:white">

    <!-- Fixed navbar -->
    <nav class="navbar navbar-inverse navbar-fixed-top">
        <div class="container">
            <div class="navbar-header">
                <a class="navbar-brand" id="regresar" runat="server">Regresar</a>
            </div>
            <div id="navbar" class="navbar-collapse collapse">
            </div>
            <!--/.nav-collapse -->
        </div>
    </nav>

    <!-- Header -->
    <div class="jumbutron2">
        <div class="col-xs-12 col-md-3"></div>
        <div class="jumbotron jumbutron2">
            <div class="col-xs-12">
                <div class="col-md-offset-3 col-xs-offset-3">
                    <img alt="" src="../images/SOV_VOCAREER1.png" />
                </div>
                <div class="col-md-4"></div>
                <div class="col-md-5"></div>
            </div>
        </div>
        <div class="col-xs-12 col-md-3"></div>
    </div>
    <!-- Fin Header -->

    <div class="container jumbutron2 theme-showcase page col-xs-12" role="main">
        <!-- Main jumbotron -->
        <div class="jumbotron text-center col-xs-12">
            <div class="col-xs-2"></div>
            <div class="col-xs-8">
                <div class="col-md-6">
                    <h2 style="font-weight: bold; text-align: left;">Bienvenido</h2>
                    <br />
                    <p>
                        Estimada universidad,<br />
                        para formar parte de nuestra plataforma todo interesado debe pasar por un proceso de validaci&oacute;n de su informaci&oacute;n,
                    por lo que se le pide que env&iacute;e los documentos que a continuaci&oacute;n se le solicita al correo:<br />
                    </p>
                </div>
                <div class="col-md-2">
                    <img alt="" src="../images/POV_UNIVERSIDAD.png" style="max-width: 350px;" />
                </div>
            </div>
            <div class="col-xs-2"></div>
        </div>

        <div class="col-xs-12">
            <div class="col-xs-2"></div>
            <div class="col-xs-8  text-center">
                <div class="jumbotron jumbutron2 text-top">
                    <div class="col-xs-12">
                        <p style="border-bottom: solid">
                            sis.pov.2016@gmail.com
                        </p>
                    </div>
                    <br />
                    <p style="color: #77aea9;">
                        Con el Asunto:<br />
                    </p>
                    <div class="col-xs-12">
                        <p style="border-bottom: solid">
                            Quiero formar parte del sistema
                        </p>
                    </div>
                    <br />
                    <br />
                    <p>
                        <br />
                        <b>Documentos que deber&aacute; enviar el interesado:</b>
                    </p>
                    <div class="col-mx-8 requisitos" >
                        <br />
                        <ul class="list">
                            <li>
                                <p>Nombre de la universidad</p>
                            </li>
                            <li>
                                <p>Clave de la universidad</p>
                            </li>
                            <li>
                                <p>Tel&eacute;fono</p>
                            </li>
                            <li>
                                <p>Direcci&oacute;n</p>
                            </li>
                        </ul>
                        <br />
                    </div>
                    <br />

                    <p>
                        <b>Funciones que brinda la plataforma:</b>
                    </p>
                    <ul class="list" style="list-style-type: none; color: black;">
                        <li class="plist">Dar de alta a sus orientadores.</li>
                        <li class="plist">Control de expedientes.</li>
                        <li class="plist">Dar a conocer las carreras que imparte.</li>
                        <li class="plist">Publicar eventos escolares dentro del portal de los aspirantes.</li>
                    </ul>
                    <br />
                    <p>
                        Puede usar el servidor de correo Outlook o el de su mayor preferencia.
                    </p>

                    <div class="col-md-12">
                        <a runat="server" id="enviarCorreo">
                            <div class="col-md-5"></div>
                            <div class="col-md-4">
                            </div>
                            <div class="col-xs-offset-6 col-md-3 btn_entrar">
                                Enviar documentos
                            </div>
                        </a>
                    </div>
                </div>
            </div>

            <div class="col-xs-2"></div>
        </div>

        <!-- Footer jumbotron -->
        <div class="jumbotron text-center col-xs-12">
            <div class="col-xs-2"></div>
            <div class="col-xs-8">
            </div>
            <div class="col-xs-2">
            </div>
        </div>
    </div>
    <!-- /container -->


</body>
</html>