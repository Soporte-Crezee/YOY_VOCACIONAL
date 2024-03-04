<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="POV.Web.Administracion.Auth.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>YOY - ADMINISTRADOR</title>
    <link rel="shortcut icon" href="~/Images/Yoy_Favicon20px.png" />
    <link href="~/Styles/login.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/ui-lightness/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/bootstrap.css" rel="stylesheet" type="text/css" />

    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery-1.12.1.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/bootstrap.js")%>" type="text/javascript"></script>

    <style type="text/css">
        .row.vertical-divider {
            overflow: hidden;
        }

            .row.vertical-divider > div[class^="col-"] {
                text-align: center;
                padding-bottom: 80px;
                margin-bottom: -80px;
                border-left: 1px solid #082d49;
                border-right: 1px solid #082d49;
            }

            .row.vertical-divider div[class^="col-"]:first-child {
                border-left: none;
            }

            .row.vertical-divider div[class^="col-"]:last-child {
                border-right: none;
            }
    </style>

    <script type="text/javascript">
        $(function () {
            $("#<%=TxtNombre.ClientID %>").focus();
        });

        $(document).ready(function () {
            function updateScreen() {
                var contloginheight_ = (window.innerHeight - ((document.getElementById("cabeza").offsetHeight + document.getElementById("pie").offsetHeight)));
                var d = document.getElementsByClassName("bodyadaptable");
                for (var i = 0; i < d.length; i++) {
                    d[i].style.height = contloginheight_ + "px";
                }
            }
            function desplegarForm() {
                if ($("#txtLoginFail").val() != "") {
                    $("#btn_usuario").click();
                    $("#txtLoginFail").val('');
                }
                clearInterval(esperando);
            }
            
            var esperando = setInterval(desplegarForm, 500);
            var actualizando = setInterval(updateScreen, 1);
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="pageContainerLogin">
            <%-- header --%>
            <div class="col-xs-13">
                <!--Header-->
                <div class="col-xs-12">
                    <div id="cabeza" style="margin: 0px; padding: 15px;">
                        <div class="row vertical-divider" style="margin-top: 0px">
                            <div class="col-xs-12 col-sm-4 col-md-3">
                                <img alt="Logo" src="../Images/SOV_VOCAREER1.png" style="margin: 14px 15px 0px; padding: 0px" width="280px" class="img-responsive" />
                            </div>
                            <div class="col-xs-12 col-sm-8 col-md-9">
                                <div class="col-xs-3 col-sm-2 col-md-2 col-lg-1">
                                    <img alt="Logo" src="../Images/SOV_administracion.png" style="margin: 30px 0px 0px 10px; padding: 0px" width="50px" class="img-responsive" />
                                </div>
                                <div class="col-xs-9 col-sm-10 col-md-10 col-lg-11">
                                    <h2 class="pull-left titulo-portal" id="tituloPortal" runat="server" style="margin-top: 30px;">PORTAL ADMINISTRADOR</h2>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!--Content-->
                <div class="bodyadaptable col-xs-12" style="margin:0;padding:0;">
                    <div id="container_login_" class="bodyadaptable container_login col-xs-12 col-sm-12 col-md-12 col-lg-12" style="margin: 0px; padding: 0px">
                        <div class="col-xs-12 col-md-1"></div>
                        <div class="col-xs-12 col-md-10">
                            <!-- Datos -->
                            <div class="bodyadaptable col-xs-6 col-sm-4 col-md-4 col-lg-3 col-xs-offset-6 col-md-offset-8 " style="top: 32px;">
                                <div id="container_login_panel">
                                    <asp:TextBox ID="txtLoginFail" runat="server" class="form-control" Style="display: none"></asp:TextBox>
                                    <div class="btn-group col-xs-12">
                                        <div class="col-xs-12 col-md-12 loginCard" style="width: 270px; margin: 0px 0px 0px 5px;">
                                            <fieldset class="form_login" style="margin: 2px 0px 0px 5px">
                                                <div class="col-xs-12 form-group">
                                                    <asp:Label ID="LblLoginFail" runat="server" Text="" CssClass="error col-sm-12 control-label" ForeColor="Red"></asp:Label>
                                                </div>
                                                <div class="col-xs-12 form-group">
                                                    <div class="col-sm-12">
                                                        <asp:TextBox ID="TxtNombre" runat="server" class="form-control" required="" placeholder="Usuario" MaxLength="50"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-xs-12 form-group">
                                                    <div class="col-sm-12">
                                                        <asp:TextBox ID="TxtPassword" runat="server" TextMode="Password" class="form-control" required="" placeholder="Contrase&ntilde;a" MaxLength="50"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-xs-12 form-group">
                                                    <div class="col-sm-8 col-md-offset-2">
                                                        <asp:Button runat="server" ID="btnEntrar" OnClick="BtnEntrar_Click" CssClass="btn-entrar btn-md btn-block" ToolTip="Entrar al portal" Text="ENTRAR" />
                                                    </div>
                                                </div>
                                            </fieldset>
                                        </div>
                                    </div>
                                    <div class="col-xs-12">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12 col-md-1"></div>
                    </div>
                </div>

                <!--Footer-->
                <div id="pie" class="col-xs-12" style="margin:0px;padding:0px;">
                    <div class="col-xs-12 col-md-12" style="background: #0067ac; height: 10px;"></div>
                    <div class="text-center col-xs-12" style="background-color: #242525;">
                        <div class="col-xs-12 footer">
                            <div class="col-xs-4 col-md-4">
                                <p class="avisos_footer" style="background-color:#242525;border-color:#242525;"><a href="#" target="_self">T&Eacute;RMINOS DE USO.</a></p>
                            </div>
                            <div class="col-xs-4 col-md-4">
                                <p class="avisos_footer" style="color:#0067ac;background-color:#242525;border-color:#242525;">&copy;<asp:Label ID="lblFechaAnio" runat="server"></asp:Label>, Derechos reservados YOY Vocacional. </p>
                            </div>
                            <div class="col-xs-4 col-md-4">
                                <p class="avisos_footer" style="background-color:#242525;border-color:#242525;"><a href="#" target="_self">AVISO DE PRIVACIDAD.</a></p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
