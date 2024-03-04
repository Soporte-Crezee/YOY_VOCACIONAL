<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="POV.Web.PortalTutor.Auth.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>YOY - PADRES</title>
    <meta name="viewport" content="width=device-width" />

    <link rel="shortcut icon" href="../images/Yoy_Favicon20px.png" />
    <link href="~/Styles/Default.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/Login.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/ui-lightness/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/bootstrap.css" rel="stylesheet" type="text/css" />

    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>


    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery-1.12.1.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/bootstrap.js")%>" type="text/javascript"></script>

    <style type="text/css">
        #divContainer {
            background: url('../images/backgroundlogintutores.jpg');
            background-position-x: 10%;
            background-size: cover;
            min-height: 522px;
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
        html {
            position: relative;
            min-height: 100%;
        }

        body {
            /* Margin bottom by footer height */
            margin-bottom: 40px;
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

        #btnRegistrar{
            text-decoration:none;
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
        .navbar-static-top {
            border-width:0px 0px 0px !important;
        }
    </style>

    <script type="text/javascript">
        $(function () {
            $("#<%=TxtNombre.ClientID %>").focus();
        });

        $(document).ready(function () {

            function desplegarForm() {
                if ($("#txtLoginFail").val() != "") {
                    $("#btn_usuario").click();
                    $("#txtLoginFail").val('');
                }
                clearInterval(esperando);
            }
            var esperando = setInterval(desplegarForm, 500);
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <%-- header --%>
        <!--Header-->
        <div class="navbar navbar-default navbar-static-top">
                <div class="col-md-12 col-sm-12 col-xs-12 text-center" id="headerLogin">                    
                    <div class="col-xs-12 col-sm-4 col-md-3 col-lg-3" style="text-align:center">
                        <img alt="Logo" src="../Images/SOV_VOCAREER1.png" style="padding-top:5px; margin-top:10px;margin-bottom:10px;" />
                    </div>
                    <div class="col-xs-3 col-sm-2 col-md-1 col-lg-1">
                        <img alt="Logo" src="../Images/SOV_tutores.png" style="margin: 20px 0px 0px 10px; padding: 0px" width="50px" />
                    </div>
                    <div class="col-xs-8 col-sm-6 col-md-5 col-lg-7">
                        <h2 class="pull-left titulo-portal" id="tituloPortal" runat="server" style="padding-top:10px;">PORTAL PADRES</h2>
                    </div>
                </div>     
                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12" id="underheader">

                </div>
            </div>

        <div id="divContainer" class="container-fluid">
            <div class="col-md-4 col-md-offset-7">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <strong>Inicio de sesión</strong>
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <asp:Label ID="LblLoginFail" runat="server" Text="" CssClass="error col-sm-12 control-label" ForeColor="Red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <asp:TextBox ID="TxtNombre" runat="server" class="form-control" required="" placeholder="Usuario" MaxLength="50"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <asp:TextBox ID="TxtPassword" runat="server" TextMode="Password" class="form-control" required="" placeholder="Contrase&ntilde;a" MaxLength="50"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group last">
                                <div class="col-xs-offset-3 col-xs-6">
                                    <asp:Button runat="server" ID="btnEntrar" OnClick="BtnEntrar_Click" CssClass="btn-entrar btn-md btn-block" ToolTip="Entrar al portal" Text="Entrar" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel-footer text-center">
                        <p>
                            <asp:LinkButton runat="server" CssClass="" ID="btnOlvidasteContrasena" PostBackUrl="RecuperarPassword.aspx">¿Olvidaste tu contraseña?</asp:LinkButton>
                        </p>
                        <p>
                            <asp:Label ID="LblNewUser" CssClass="col-sm-12 control-label" runat="server" Text="¿Eres nuevo usuario?"></asp:Label>
                        </p>
                        <p>
                            <asp:LinkButton runat="server" ID="btnRegistrar" TabIndex="9" Text="REGÍSTRATE AHORA" Width="170px" CssClass="btn-entrar btn-md btn-block" OnClick="btnRegistrar_Click"></asp:LinkButton>
                        </p>
                    </div>
                </div>
            </div>

        </div>
        <div class="footer">
            <div class="container">
                <div class="row firstFooter" >
                    <div class="col-xs-4 col-sm-4 col-md-4 text-center">
                        <p class="avisos_footer_login"><a href="/pdf/terminoscondicionesyoy.pdf" target="_blank">T&Eacute;RMINOS DE USO.</a></p>
                    </div>                    
                    <div class="col-xs-4 col-sm-4 col-md-4 ">
                        <p class="avisos_footer_login"><a href="/pdf/avisoprivacidadyoy.pdf" target="_blank">AVISO DE PRIVACIDAD.</a></p>
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
