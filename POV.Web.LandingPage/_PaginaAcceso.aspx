<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="_PaginaAcceso.aspx.cs" Inherits="POV.Web.LandingPage.PaginaAcceso" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta http-equiv="content-type" content="text/html; charset=windows-1252">
    <link rel="shortcut icon" href="Images/Yoy_Favicon20px.png" />
    <title>YOY - ACCESO</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <link href="../Styles/style.css" rel="stylesheet" />
    <link href="Styles/bootstrap.css" rel="stylesheet" />
    <!-- Bootstrap JS -->
    <script src="../Scripts/jquery-1.12.1.min.js"></script>
    <script src="../Scripts/bootstrap.js"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>

    <style>
        body {
            background-color: #f5f5f5;
        }

        .box-shadow {
            box-shadow: 0px 2px 9px 0px rgba(0, 0, 0, 0.35);
        }

            .box-shadow:hover {
                background-color: #f7b234;
            }

        .page {
            min-height: 450px;
        }
    </style>

    <script type="text/javascript">
        var anio = (new Date).getFullYear();

        $(document).ready(function () {
            $("#fecha").text(anio);
        });

        $(function () {
            $('#<%=txtMensaje.ClientID %>').val('');
            $('#<%=txtMensaje.ClientID %>').removeAttr('required');

            $("#btnClose").on("click", function () {
                $('#<%=txtMensaje.ClientID %>').val('');
                $('#<%=txtMensaje.ClientID %>').removeAttr('required');
            });

            $("#btnWhats").button().on("click", function () {
                $('#<%=txtMensaje.ClientID %>').val('');
                $('#<%=txtMensaje.ClientID %>').attr('required', '');
            });
        });
    </script>
</head>
<body style="background: #000">
    <form runat="server" style="height: 100%;">

        <div class="">
            <div class="">
                <div class="modal fade dialog-md" tabindex="-1"
                    data-keyboard="false" data-backdrop="static"
                    role="dialog" aria-labelledby="ventanaModalLabel" id="ventanaModal">
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                            <div class="modal-header" style="background: #33acfd">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true" style="color: white"><span class="glyphicon glyphicon-remove"></span></button>
                                <h1 style="color: white">Comun&iacute;cate con YOY</h1>
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
                                    <div class="col-md-6 pull-right" style="padding-right: 15px;">
                                        <asp:Button runat="server" ID="BtnEnviar" CssClass="btn btn-green btn-md" Text="Enviar" OnClick="BtnEnviar_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="pageContainer" style="height: auto;">
                <!-- Header -->
                <div class="container" style="margin-top: 0px;">
                    <nav id="inicio" class="navbar navbar-toggleable-md navbar-light bg-faded na" style="background-color: black">

                        <div class="col-xs-6 col-sm-4 col-md-3 bannerimg">
                            <img class="img-responsive" src="Images/SOV_ELEMENTOS_logo_VOCAREER1.png" />
                        </div>
                        <div class="collapse navbar-collapse" id="navbarNav">
                        </div>
                    </nav>
                </div>

                <!-- Content -->
                <div class="">
                    <div id="container" class="">
                        <div class="">
                            <div class="">
                                <div class="col-xs-12 col-md-12" id="backgroundprincipal">
                                    <div class="pull-right" style="text-align: left;">
                                        <table style="text-align: right">
                                            <tr style="text-align: right">
                                                <br />
                                                <td>
                                                    <font color="white" size="20" bold="100%" style="text-align: right">
                                                          DESCUBRE TUS TALENTOS, <br /> DIRÍGETE AL <br /> ÉXITO PROFESIONAL
                                                      </font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <br />
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr style="text-align: center">
                                                <td>
                                                    <img src="../Images/contacto_twitter_ARRIBA.png" />
                                                    &nbsp &nbsp &nbsp &nbsp
                                                      <img src="../Images/contacto_facebook_ARRIBA.png" />&nbsp &nbsp &nbsp &nbsp
                                                      <img src="../Images/contacto_blog_ARRIBA.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr style="text-align: center">
                                                <td colspan="3">
                                                    <font color="white" size="4">SÍGUENOS EN REDES SOCIALES</font>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-md-12" style="background: #082d49; height: 5px;"></div>
                            </div>
                            <div class="">
                                <div class="col-xs-12 col-md-12 container theme-showcase page" style="background: #f2f2f2">
                                    <div class="theme-showcase page">
                                        <div class="col-xs-12 col-md-12 form-group" style="text-align: center">
                                            <div id="encabezado" style="text-align: center;">
                                                <div style="font: bold; font-size: 3em">NUESTROS PORTALES</div>
                                                <div style="font: bold; font-size: 2em; padding-bottom: 10px">DE ORIENTACION</div>
                                                <hr id="lineblack" width="25%" style="padding-bottom: 50px;" />
                                            </div>
                                            <div class="row visible-sm">
                                                <div class="col-xs-12  col-sm-6">
                                                    <a runat="server" id="A5" style="width: 100%">
                                                        <div class="col-xs-12"><font class="font-helv-acces" size="4px">PORTAL ESTUDIANTES</font></div>
                                                        <div class="col-xs-12">
                                                            <img style="padding-top: 20px" src="Images/SOV_ELEMENTOS_portal_universidad.png" />
                                                        </div>
                                                    </a>
                                                </div>
                                                <div class="col-xs-12 col-sm-6 col-lg-3">
                                                    <a runat="server" id="A1" style="width: 100%">
                                                        <div class="col-xs-12"><font class="font-helv-acces">PORTAL ORIENTADOR</font></div>
                                                        <div class="col-xs-12">
                                                            <img style="padding-top: 20px" src="Images/SOV_ELEMENTOS_portal_orientadores.png" />
                                                        </div>
                                                    </a>
                                                </div>
                                                <div class="col-xs-12 col-sm-6 col-lg-3">
                                                    <a runat="server" id="A2" style="width: 100%">
                                                        <div class="col-xs-12"><font class="font-helv-acces">PORTAL PADRES</font></div>
                                                        <div class="col-xs-12">
                                                            <img style="padding-top: 20px" src="Images/SOV_ELEMENTOS_portal_tutores.png" />
                                                        </div>
                                                    </a>
                                                </div>
                                                <div class="col-xs-12 col-sm-6 col-lg-3">
                                                    <a runat="server" id="A3" style="width: 100%">
                                                        <div class="col-xs-12"><font class="font-helv-acces">PORTAL ESCUELA</font></div>
                                                        <div class="col-xs-12">
                                                            <img style="padding-top: 20px" src="Images/SOV_ELEMENTOS_portal_administracion.png" />
                                                        </div>
                                                    </a>
                                                </div>
                                                <div class="col-xs-12 col-sm-12 col-lg-3">
                                                    <a runat="server" id="A4" style="width: 100%">
                                                        <div class="col-xs-12"><font class="font-helv-acces">PORTAL OPERACIONES</font></div>
                                                        <div class="col-xs-12">
                                                            <img style="padding-top: 20px" src="Images/SOV_ELEMENTOS_portal_operaciones.png" />
                                                        </div>
                                                    </a>
                                                </div>
                                            </div>

                                            <div class="row hidden-sm">
                                                <div class="col-xs-12 form-group">
                                                    <div class="col-xs-12">
                                                        <a runat="server" id="portalSocial" style="width: 100%">
                                                            <div class="col-xs-12"><font class="font-helv-acces fuente" size="4px">PORTAL ESTUDIANTES</font></div>
                                                            <div class="col-xs-12">
                                                                <img style="padding-top: 20px" src="Images/SOV_ELEMENTOS_portal_universidad.png" />
                                                            </div>
                                                        </a>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row hidden-sm">
                                                <div class="col-xs-12 form-group">
                                                    <div class="col-xs-12 col-md-3 col-lg-3">
                                                        <a runat="server" id="portalOrientadores" style="width: 100%">
                                                            <div class="col-xs-12"><font class="font-helv-acces fuente">PORTAL ORIENTADOR</font></div>
                                                            <div class="col-xs-12">
                                                                <img style="padding-top: 20px" src="Images/SOV_ELEMENTOS_portal_orientadores.png" />
                                                            </div>
                                                        </a>
                                                    </div>
                                                    <div class="col-xs-12  col-md-3 col-lg-3">
                                                        <a runat="server" id="portalTutor" style="width: 100%">
                                                            <div class="col-xs-12"><font class="font-helv-acces fuente">PORTAL PADRES</font></div>
                                                            <div class="col-xs-12">
                                                                <img style="padding-top: 20px" src="Images/SOV_ELEMENTOS_portal_tutores.png" />
                                                            </div>
                                                        </a>
                                                    </div>
                                                    <div class="col-xs-12 col-md-3 col-lg-3">
                                                        <a runat="server" id="portalUniversidad" style="width: 100%">
                                                            <div class="col-xs-12"><font class="font-helv-acces fuente">PORTAL ESCUELA</font></div>
                                                            <div class="col-xs-12">
                                                                <img style="padding-top: 20px" src="Images/SOV_ELEMENTOS_portal_administracion.png" />
                                                            </div>
                                                        </a>
                                                    </div>
                                                    <div class="col-xs-12 col-md-3 col-lg-3">
                                                        <a runat="server" id="portalOperaciones" style="width: 100%">
                                                            <div class="col-xs-12"><font class="font-helv-acces fuente">PORTAL OPERACIONES</font></div>
                                                            <div class="col-xs-12">
                                                                <img style="padding-top: 20px" src="Images/SOV_ELEMENTOS_portal_operaciones.png" />
                                                            </div>
                                                        </a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-md-12" style="background: #082d49; height: 5px; text-align: center">
                                    <a href="PaginaAcceso.aspx" title="">
                                        <img style="padding-bottom: .5em; cursor: pointer; float: inherit; margin-top: -30px" src="Images/SOV_ELEMENTOS_boton_arriba.png" />
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="contacto" style="width: 100%; height: 40%; text-align: center; background: black;">
                <div>
                    <label style="height: 120px; padding-top: 30px;" class="font-helvb">CONTÁCTANOS</label>
                </div>
                <div>
                    <div class="col-xs-12 form-group">
                        <div class="col-xs-12 col-lg-6">
                            <label class="font-helv">Queremos conocerte y apoyarte, comunícate con nosotros y síguenos en nuestras redes sociales</label>
                        </div>
                        <div class="col-xs-12 col-lg-6">
                            <div class="row">
                                <div class="col-lg-3 col-md-3"></div>
                                <div class="col-xs-4 col-lg-2 col-md-2" style="text-align:center">
                                    <a href="https://www.facebook.com/Yoy-Orientaci%C3%B3n-Vocacional-1922257968041032/">
                                        <img src="Images/LANDIN CAMPUSOV4.png" class="contact" />
                                    </a>
                                </div>
                                <div class="col-xs-4 col-lg-2 col-md-2" style="text-align:center">
                                    <a href="https://twitter.com/YoyOrientacion">
                                        <img src="Images/LANDIN CAMPUSOV5.png" class="contact" />
                                    </a>
                                </div>
                                <div class="col-xs-4 col-lg-2 col-md-2" style="text-align:center">
                                    <a href="https://www.linkedin.com/company-beta/24783354/">
                                        <img src="Images/LANDIN CAMPUSOV.png" class="ontact" />
                                    </a>
                                </div>
                                <div class="col-lg-3 col-md-3"></div>
                            </div>
                            <div class="row">
                                <div class="col-xs-2 col-lg-4 col-md-4"></div>
                                <div class="col-xs-4 col-lg-2 col-md-2" style="text-align:center">
                                    <a href="http://instagram.com/yoyorientacion" >
                                        <img src="Images/LANDIN CAMPUSOV3.png" class="contact" />
                                        <%--Falta El icono de instragam--%>
                                    </a>
                                </div>
                                <div class="col-xs-4 col-lg-2 col-md-2" style="text-align:center">
                                    <a href="#" data-toggle="modal" data-target=".dialog-md" id="btnWhats">
                                        <img src="Images/LANDIN CAMPUSOV2.png" class="contact" />
                                    </a>
                                </div>
                                <div class=" col-xs-2 col-lg-4 col-md-4"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- footer -->
            <div class="col-xs-12">
                <div id="footer" class="page_container text-center">
                    <p class="avisos_footer" style="color:white">
                        <strong id="fecha">2016</strong>, Todos los derechos reservados, YOY Vocacional
			            <br>
                        <a href="#">Términos y Condiciones.</a>&nbsp;&nbsp;
                        <a href="#">Acerca de.</a>&nbsp;&nbsp;
                    </p>
                </div>
            </div>
        </div>
    </form>
</body>

</html>
