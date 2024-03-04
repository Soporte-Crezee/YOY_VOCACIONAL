<%@ Page Title="" Language="C#" MasterPageFile="~/Content/MasterPages/PruebaMaster.Master" AutoEventWireup="true" CodeBehind="AccesoPrueba.aspx.cs" Inherits="POV.Web.Portal.Pruebas.Pages.AccesoPrueba" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>ui-lightness/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/Layout.css" rel="stylesheet" type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.1.7.2.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api.dialogs.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            initPageClient();
        });
        function initPageClient() {
            var apimess = new MessageApi();
            apimess.PrepareDialog();
            if ($("#<%=hdnShowMessage.ClientID%>").val() == "1") { mostrarError(); }
            if ($("#<%=hdnShowMessage.ClientID%>").val() == "2") { mostrarWarning(); }
            if ($("#<%=hdnShowMessage.ClientID%>").val() == "3") { mostrarInfo(); }
        }
        function mostrarError() {
            var api = new MessageApi();
            var text = $("#<%=hdnLastMessage.ClientID %>").val();
            api.CreateMessage(text, "ERROR");
            api.Show();
            resetHiden();
        }
        function mostrarWarning() {
            var api = new MessageApi();
            var text = $("#<%=hdnLastMessage.ClientID %>").val();
            api.CreateMessage(text, "WARNING");
            api.Show();
            resetHiden();
        }
        function mostrarInfo() {
            var api = new MessageApi();
            var text = $("#<%=hdnLastMessage.ClientID %>").val();
            api.CreateMessage(text, "INFO");
            api.Show();
            resetHiden();
        }
        function resetHiden() {
            $("#<%=hdnShowMessage.ClientID %>").val("0");
            $("#<%=hdnLastMessage.ClientID %>").val("");
        }
        var hdnmessageinputid = "#<%=hdnLastMessage.ClientID %>";
        var hdnmessagetypeinputid = "#<%=hdnShowMessage.ClientID %>";      
    </script>
    <title>Prueba</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <div style="display: none;">
        <div class="border_div_contenedor background_div_contenedor alinear_contenido_centro">
            <div>
                <div id="ContenidoImagenSaludo" style="margin-left: 0px; margin-top: -30px">
                </div>
                <br />

                <br />
                <div id="SolicitaMensaje" class="texto_normal_titulo" style="display: none;">
                    <asp:Label runat="server" ID="lblSolicitaMensaje">Si aún no tienes tu código, solicitaselo a tu profesor</asp:Label>
                </div>
                <div id="NombreAlumnoMensaje" class="texto_bold_titulo" style="display: none;">
                    <asp:Label runat="server" ID="lblNombreAlumnoMensaje"></asp:Label>
                </div>
                <br />

                <div id="AccesoMensaje" class="texto_normal_titulo" style="display: none;">
                    <asp:Label runat="server" ID="lblMensajeAcceso"></asp:Label>
                </div>
                <br />

                <asp:TextBox ID="txtAccesoPrueba" CssClass="texto_label_campo_contenido" runat="server" Height="26px" Width="260px" Style="display: none;"></asp:TextBox>
                <div id="AccesoFail" style="display: none;">
                    <asp:Label ID="lblCodigoFail" runat="server" CssClass="error"></asp:Label>
                </div>
                <br />
            </div>
        </div>
    </div>
    <div>
        <div style="height: 75px"></div>
        <div class="border_div_contenedor background_div_contenedor alinear_contenido_centro" style="min-height:454px;">
            <div id="ContenidoAcceso">
                <div class="container">
                    <div class="panel panel-default">
                        <div class="panel-body">
                            <img alt="404" src="../Content/Images/errorPrueba.png" class="img-responsive center-block" />
                        </div>
                        <div id="acceso_prueba">
                            <asp:Button ID="btnAccesoPrueba" CssClass="boton_siguiente_iniciar_sesion" Text="Entrar"
                                runat="server" OnClick="btnAccesoPrueba_Click" />
                        </div>
                    </div>
                </div>
                <div id="dialog" title="INFORME">
                    <span id="message"></span>
                </div>
                <div id="dialog-question" title="CONFIRMAR">
                    <span id="message_question"></span>
                </div>
                <asp:HiddenField ID="hdnLastMessage" runat="server" />
                <asp:HiddenField ID="hdnShowMessage" runat="server" Value="0" />
                <asp:TextBox ID="txtRedirect" ClientIDMode="Static" runat="server" Style="display: none;"></asp:TextBox>
            </div>
        </div>
    </div>
</asp:Content>
