<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequisitosOrientador.aspx.cs" Inherits="POV.Web.LandingPage.RequisitosOrientador" %>

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
<body style="background-color: white">
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
    <div class="container jumbutron2 theme-showcase page col-xs-12" role="main">
        <!-- Main jumbotron -->
        <div class="jumbotron text-center col-xs-12">
            <div class="col-xs-2"></div>
            <div class="col-xs-8">
                <div class="col-md-6">
                    <h2 style="font-weight: bold; text-align: left;">Bienvenido</h2>
                    <br />
                    <p>
                        Estimado orientador,<br />
                        para formar parte de nuestra plataforma todo interesado debe pasar por un proceso de validaci&oacute;n de su informaci&oacute;n,
                    por lo que se le pide que env&iacute;e los documentos que a continuaci&oacute;n se le solicita al correo:<br />
                    </p>
                </div>
                <div class="col-md-2">
                    <img alt="" src="../images/POV_ORIENTADOR.png" style="max-width: 300px;" />
                </div>
            </div>
            <div class="col-xs-2">
            </div>
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
                            Quiero ser orientador
                        </p>
                    </div>
                    <br />
                    <br />
                    <p>
                        <br />
                        <b>Documentos que deber&aacute; enviar el interesado:</b>
                    </p>
                    <div class="col-mx-8 requisitos">
                        <br />
                        <ul class="list">
                            <li>
                                <p>Curriculum Vitae</p>
                            </li>
                            <li>
                                <p>Titulo</p>
                            </li>
                            <li>
                                <p>Cedula Profesional</p>
                            </li>
                            <li>
                                <p>CURP</p>
                            </li>
                            <li>
                                <p>Constancia Laboral</p>
                            </li>
                        </ul>
                        <br />
                    </div>
                    <br />

                    <p>
                        <b>Funciones del orientador:</b>
                    </p>
                    <ul class="list" style="list-style-type: none; color: black;">
                        <li class="plist">Orientar a los estudiantes en su desarrollo acad&eacute;mico, personal y de elecci&oacute;n vocacional.</li>
                        <li class="plist">Aplicar pruebas psicopedag&oacute;gicas.</li>
                        <li class="plist">Detectar &aacute;reas de oportunidad y tácticas de abordaje, intervención en Crisis.</li>
                        <li class="plist">Planificar y coordinar las actividades de su &aacute;rea.</li>
                        <li class="plist">Docencia con uso de TI’s.</li>
                        <li class="plist">Gestión de procesos administrativos.</li>
                        <li class="plist">Log&iacute;stica de eventos.</li>
                        <li class="plist">Atenci&oacute;n a docentes, estudiantes y padres de familia.</li>
                        <li class="plist">Control de expedientes.</li>
                    </ul>
                    <br />
                    <p>
                        Puede usar el servidor de correo Outlook o el de su mayor preferencia.
                    </p>

                    <div class="col-md-12">
                        <a runat="server" id="enviarCorreo">
                            <div class="col-md-5"></div>
                            <div class="col-md-4"></div>
                            <div class="col-xs-offset-6 col-md-3  btn_entrar">
                                Enviar documentos
                            </div>
                        </a>
                    </div>
                </div>
            </div>

            <div class="col-xs-2"></div>
        </div>
    </div>
    <!-- /container -->
</body>
</html>
