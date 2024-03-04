<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LiberarPrueba.aspx.cs" Inherits="POV.Web.PortalOperaciones.Pruebas.LiberarPrueba" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet"
        type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            $(".boton").button();
        }

        function ValidateFields() {
            $("#frmMain").validate();
        }

        function CancelValidate() {
            var form = $('#frmMain').get(0);
            $.removeData(form, 'validator');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="bodyadaptable col-xs-13 col-md-13">
        <h3 class="ui-widget-header ui-widget-header-label">
            <asp:LinkButton ID="lnkBack" runat="server" OnClick="BtnCancelar_OnClick" OnClientClick="CancelValidate()">Volver</asp:LinkButton>/Liberar
        prueba</h3>
        <div class="ui-widget-content" style="padding: 5px;">
            <%-- BEGIN formulario  principal--%>
            <h2>
                <label>
                    Informaci&oacute;n de la prueba</label>
            </h2>
            <div class="line"></div>
            <table>
                <tr>
                    <td class="td-label">
                        <label>
                            Clave</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtClave" runat="server" CssClass="required" MaxLength="30" Enabled="false"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td class="td-label">
                        <label>
                            Nombre</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="required" MaxLength="100" Enabled="false"></asp:TextBox>
                    </td>
                    <td style="text-align: right">
                        <asp:CheckBox runat="server" ID="chkPruebaPremium" Text=" Prueba PREMIUM" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td class="td-label-texarea">
                        <label>
                            Instrucciones</label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtInstrucciones" runat="server" TextMode="MultiLine" Columns="40"
                            Rows="5" CssClass="required" MaxLength="1000" Enabled="false" Width="500" Height="100"> </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td-label-texarea">
                        <asp:Label ID="lblModelo" runat="server" Text="Modelo de prueba"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:UpdatePanel ID="updPanelLabels" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlModelo" runat="server" OnSelectedIndexChanged="ddlModelo_SelectedIndexChanged"
                                    AutoPostBack="true" CssClass="required" Enabled="false">
                                </asp:DropDownList>
                                <asp:Label ID="lblNombreMetodo" runat="server" Text="Método de calificación: "></asp:Label>
                                <asp:Label ID="lblMetodoCalificacion" runat="server" Text="SELECCIONE UN MODELO"
                                    Font-Size="11px"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlModelo" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:UpdatePanel ID="updPanelModelo" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td colspan="2"></td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlModelo" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td class="td-label">Presentación de la prueba</td>
                    <td>
                        <asp:DropDownList ID="DDLTipoPruebaPresentacion" runat="server" Enabled="false">
                            <asp:ListItem Text="SELECCIONE..." Value=""></asp:ListItem>
                            <asp:ListItem Text="Dinamica" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Habitos de estudio" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Dominós" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Terman Merrill" Value="6"></asp:ListItem>
                            <asp:ListItem Text="Kuder" Value="7"></asp:ListItem>
                            <asp:ListItem Text="Allport" Value="8"></asp:ListItem>
                            <asp:ListItem Text="Frases incompletas de Sacks" Value="9"></asp:ListItem>
                            <asp:ListItem Text="Cleaver" Value="10"></asp:ListItem>
                            <asp:ListItem Text="Chaside" Value="11"></asp:ListItem>
                            <asp:ListItem Text="Rotter" Value="12"></asp:ListItem>
                            <asp:ListItem Text="Frases incompletas Vocacionales" Value="13"></asp:ListItem>
                            <asp:ListItem Text="Zavic" Value="14"></asp:ListItem>
                            <asp:ListItem Text="Raven" Value="15"></asp:ListItem>
                            <asp:ListItem Text="Kostick" Value="16"></asp:ListItem>
                            <asp:ListItem Text="Bullying" Value="17"></asp:ListItem>
                            <asp:ListItem Text="Socioeconomico" Value="18"></asp:ListItem>
                            <asp:ListItem Text="Autoconcepto" Value="19"></asp:ListItem>
                            <asp:ListItem Text="Actitudes" Value="20"></asp:ListItem>
                            <asp:ListItem Text="Empatía" Value="21"></asp:ListItem>
                            <asp:ListItem Text="Humor" Value="22"></asp:ListItem>
                            <asp:ListItem Text="Victimización" Value="23"></asp:ListItem>
                            <asp:ListItem Text="Ciberbullying" Value="24"></asp:ListItem>
                            <asp:ListItem Text="Violencia" Value="25"></asp:ListItem>
                            <asp:ListItem Text="Comunicación" Value="26"></asp:ListItem>
                            <asp:ListItem Text="Imagen coporal" Value="27"></asp:ListItem>
                            <asp:ListItem Text="Ansiedad" Value="28"></asp:ListItem>
                            <asp:ListItem Text="Depresión" Value="29"></asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
            </table>
            <div class="failureNotification">
                <asp:Label ID="lblMsgError" runat="server"></asp:Label>
            </div>
            <%-- END formulario  principal--%>
            <br />
            <div class="line"></div>
            <asp:Button ID="BtnGuardar" Text="Liberar" runat="server" OnClick="BtnGuardar_OnClick"
                CssClass="btn-green" OnClientClick="ValidateFields()" Enabled="false" />
            <span style="">
                <asp:Button ID="BtnCancelar" Text="Cancelar" runat="server" OnClick="BtnCancelar_OnClick"
                    CssClass="btn-cancel" OnClientClick="CancelValidate()" />
            </span>
            <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
                style="display: none;" />
        </div>
    </div>
</asp:Content>
