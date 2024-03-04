<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecuperarPassword.aspx.cs" Inherits="POV.Web.PortalUniversidad.Auth.RecuperarPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>YOY - ESCUELA</title>
    <link rel="shortcut icon" href="../images/Yoy_Favicon20px.png" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, user-scalable=no" />
    <meta name="viewport" content="width=device-width" />
    <link href="~/Styles/Default.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/Login.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/ui-lightness/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/Login-responsive.css" rel="stylesheet" />
    <link href="~/Styles/bootstrap.css" rel="stylesheet" type="text/css" />

    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery-1.12.1.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/bootstrap.js")%>" type="text/javascript"></script>
    <style type="text/css">
        html, body, #form1 {
            height: 100%;
        }

        #divContainer {
            background: url('../images/backgorundloginescuela.jpg');
            background-position-x: 10%;
            background-size: cover;
            padding-top: 30px;
            height: 100%;
        }

        .panel-default {
            opacity: 0.9;
        }

        .form-group.last {
            margin-bottom: 0px;
        }

        .page_container {
            width: 1000px;
            height: auto;
            margin: 0 auto;
        }
        /* Sticky footer styles
-------------------------------------------------- */

        body {
            /* Margin bottom by footer height */
            margin-bottom: 45px;
        }

        .footer {
            bottom: 0;
            width: 100%;
            /* Set the fixed height of the footer here */
            height: 45px;
            background-color: #f5f5f5;
        }


        /* Custom page CSS
-------------------------------------------------- */
        /* Not required for template or sticky footer method. */

         .container {
            width: auto;
            /*max-width: 680px;*/
            padding: 0 15px;
        }

        .navbar {
            margin-bottom: 0px !important;
        }

        .avisos_footer_login {
            margin-bottom: 6px !important;
        }

        .navbar {
            margin-bottom: 0px !important;
            background-image:url('../images/headerpattern.png') !important;
            background-repeat:repeat;
            min-height:92px;            
        }

        .avisos_footer_login {
            margin-bottom: 4px !important;            
            padding-top:8px;
        }

        #underheader {
            background-color:#19abd8;
            border-bottom:6px solid #646464;
            min-height:40px;            
        }

        .panel-heading {
            background-color:white !important;
            color: #787877;
            border-top-left-radius: 2em !important; 
            border-top-right-radius:2em !important; 
            border:none !important;
            text-align:center;
        }

        .panel-footer {
            background-color:white !important;
            border-radius:5px !important;
            border:none !important;
        }

        .panel-body {
            background-color:#f3f3f3;
            color:#787877;
        }

        #btnOlvidasteContrasena {
            text-decoration:none;
            color:#8f8e8e;
        }

        #tituloPortal {
            color:#19abd8 !important;
            font-size:24px !important;
            letter-spacing:2px;
        }
        .firstFooter a {
            text-decoration:none;
            color:#8f8e8e !important;
            font-size:14px;            
        }
        .firstFooter {
            background-color:white;
            min-height:50px;
            border-top:10px solid;
            border-top-color:#ff9d12;
            color:#8f8e8e !important;
            font-size:13px;
        }
        .secondFooter {
            background-image:url('../images/footerpattern.png');
            background-repeat:repeat;
            min-height:70px;
            font-size:14px;
        }

        #headerLogin {
            box-shadow: 0px 2px 10px #808080;
            z-index:999;
        }

        #linkRegresar {
            text-decoration:none;
            color:#8f8e8e;
        }
        .navbar-static-top {
            border-width:0px 0px 0px !important;
        }
        #LblUsuario {
            text-align: left !important;
            word-wrap: break-word !important;
            display: inherit !important;
        }
        #paso2Correo {
            margin-left: 0px !important;
            margin-right: 0px !important;
        }
        #paso2SinCorreo {
            margin-left: 0px !important;
            margin-right: 0px !important;
            padding-left: 0px !important;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {           

            $("#btnSiguiente").click(function () {
                if ($("#rbSi").is(":checked")) {
                    $("#paso1").hide("slow", function () {
                        $("#paso2Correo").show("slow");
                        $("#<%=txtCorreo.ClientID %>").focus();
                    });
                }
                if ($("#rbNo").is(":checked")) {
                    $("#paso1").hide("slow", function () {
                        $("#paso2SinCorreo").show("slow");

                    });
                }

                return false;
            });
            $("#btnRecuperar").click(function () {

                var el_correo = $("#txtCorreo").val(); //validar correo
                var er_email = /^(.+\@.+\..+)$/; // expresion regular para validar formato de correo
                if (!er_email.test(el_correo) || el_correo.trim().length > 100) {
                    error_registro = true;
                    $("#correoInvalido").css("display", "block");
                    return false;
                }
                else {
                    var alto = $(document).height(); //alto del documento
                    $("#ventanaModal").css("height", alto); // agregamos el alto a la ventana modal
                    $("#ventanaModal").css("z-index", "9999"); // agregamos el alto a la ventana modal
                    $("#ventanaModal").show();
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="ventanaModal" class="ventana_modal" runat="server">
            <div id="contenedorModal" class="contenedor_modal">
                <h3>Espere un momento.</h3>
                <p>Estamos tratando de enviarle un correo con sus datos de acceso.</p>
            </div>
        </div>
        <asp:Label ID="correoEncontrado" runat="server" Text=""></asp:Label>
        <%-- header --%>
        <!--Header-->
       <div class="navbar navbar-default navbar-static-top">
                <div class="col-md-12 col-sm-12 col-xs-12 text-center" id="headerLogin">                    
                    <div class="col-xs-12 col-sm-4 col-md-3 col-lg-3" style="text-align:center">
                        <img alt="Logo" src="../Images/SOV_VOCAREER1.png" style="padding-top:5px; margin-top:10px;margin-bottom:10px;" />
                    </div>
                    <div class="col-xs-3 col-sm-2 col-md-1 col-lg-1">
                        <img alt="Logo" src="../Images/SOV_universidad.png" style="margin: 20px 0px 0px 10px; padding: 0px" width="50px" />
                    </div>
                    <div class="col-xs-8 col-sm-6 col-md-5 col-lg-7">
                        <h2 class="pull-left titulo-portal" id="tituloPortal" runat="server" style="padding-top:10px;">PORTAL ESCUELA</h2>
                    </div>
                </div>     
                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12" id="underheader">

                </div>
            </div>

        <div id="divContainer" class="container-fluid">
            <div class="col-md-4 col-md-offset-7">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <strong>Recuperar contraseña</strong>
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal" role="form">
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <asp:Label ID="LblLoginFail" runat="server" Text="" CssClass="error col-sm-12 control-label" ForeColor="Red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="LblUsuario" CssClass="col-sm-12 control-label" runat="server" Text="Aqu&iacute; le ayudaremos a recuperar su contrase&ntilde;a."></asp:Label>
                            </div>
                            <div class="form-group" id="paso1">
                                <div class="col-md-12">
                                    <h4 style="color:#000">Paso 1:</h4>

                                    <p>¿Tiene un correo registrado en su cuenta?</p>
                                    <asp:RadioButton runat="server" ID="rbSi" Text="Sí" GroupName="preguntaPaso1" />
                                    <asp:RadioButton runat="server" ID="rbNo" Text="No" GroupName="preguntaPaso1" />
                                </div>
                                <div class="clearfix"></div>
                                <div style="margin-bottom:10px;"></div>
                                <div class="col-xs-11">
                                    <asp:LinkButton runat="server" ID="btnSiguiente" Text="Siguiente"
                                        CssClass="btn-entrar btn-md btn-block" ToolTip="Siguiente"
                                        OnClick="btnSiguiente_Click"></asp:LinkButton>
                                </div>
                            </div>
                            <div class="form-group" id="paso2Correo" style="display: none">
                                <h4 style="color:#000">Paso 2:</h4>
                                <p>Ingrese su correo electr&oacute;nico.</p>
                                <asp:TextBox ID="txtCorreo" runat="server" MaxLength="100" CssClass="form-control"></asp:TextBox>
                                <asp:Label ID="correoInvalido" CssClass="error_recuperar" runat="server" Text="Ingresa un correo v&aacute;lido"></asp:Label>
                                <br />
                                <div class="">
                                    <asp:LinkButton runat="server" OnClick="btnRecuperar_Click" ID="btnRecuperar" Width="150px"
                                        CssClass="btn-entrar btn-md btn-block" ToolTip="Siguiente"
                                        PostBackUrl="RecuperarPassword.aspx?enviar=true" Text="Recuperar">
                                    </asp:LinkButton>
                                </div>
                            </div>
                            <div class="form-group error" id="paso2SinCorreo" style="display: none;">
                                <h4>Error:</h4>
                                <p>No es posible recuperar la contrase&ntilde;a.</p>
                                <p>Es necesario contar con un correo electr&oacute;nico registrado.</p>
                            </div>
                        </div>
                    </div>
                    <div class="panel-footer text-center">
                        <a href="Login.aspx" runat="server" id="linkRegresar" target="_self">Regresar</a>
                    </div>
                </div>
            </div>
        </div>
        <!-- Footer-->
        <div class="footer">
            <div class="container">
                <div class="row firstFooter">
                    <div class="col-xs-4 col-sm-4 col-md-4 text-center">
                        <p class="avisos_footer_login"><a href="#" target="_self">T&Eacute;RMINOS DE USO.</a></p>
                    </div>
                    <div class="col-xs-4 col-sm-4 col-md-4 ">
                        <p class="avisos_footer_login"><a href="#" target="_self">AVISO DE PRIVACIDAD.</a></p>
                    </div>
                </div>
                <div class="text-center row secondFooter">
                    <div class="col-xs-12">
                        <p class="avisos_footer pull-right" style="color: white;">
                            &copy;<asp:Label ID="lblFechaAnio" runat="server"></asp:Label>, Derechos reservados YOY Vocacional.
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
