<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="RegistrarContrato.aspx.cs" Inherits="POV.Web.PortalOperaciones.Contratos.RegistrarContrato" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/registrarcontrato.css" rel="stylesheet" type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/messages_es.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/registrarcontrato.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        <asp:HyperLink ID="lnkBack" runat="server">Volver</asp:HyperLink>/Registrar contrato</h3>
    <div class="ui-widget-content" style="padding: 5px">
        <h2>
            Informaci&oacute;n de la ubicaci&oacute;n</h2>
        <hr />
        <br />
        <table>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblPais" runat="server" Text="País" EnableViewState="False"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cbPais" Name="cbPais" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbPais_SelectedIndexChanged"
                        AppendDataBoundItems="True" ClientIDMode="Static" />
                </td>
                <td class="td-label">
                    <asp:Label ID="lblEstado" runat="server" Text="Estado" EnableViewState="False"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cbEstado" runat="server" AppendDataBoundItems="True" ClientIDMode="Static" />
                </td>
            </tr>
        </table>
        <br />
        <h2>
            Informaci&oacute;n del contrato
        </h2>
        <hr />
        <br />
        <table>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblFechaContrato" runat="server" Text="Fecha de contrato" EnableViewState="False"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFechaContrato" runat="server" ClientIDMode="Static" MaxLength="10"></asp:TextBox>
                </td>
                <td class="td-label">
                    <asp:Label ID="lblClave" runat="server" Text="Clave" EnableViewState="False"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtClave" runat="server" ClientIDMode="Static" MaxLength="10" CssClass="textoClave"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblFechaInicio" runat="server" Text="Fecha de inicio" EnableViewState="False"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFechaInicio" runat="server" ClientIDMode="Static" MaxLength="10"></asp:TextBox>
                </td>
                <td class="td-label">
                    <asp:Label ID="lblFechaFinalizacion" runat="server" Text="Fecha de finalización"
                        EnableViewState="False"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFechaFinalizacion" runat="server" ClientIDMode="Static" MaxLength="10"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblLicencias" runat="server" Text="Licencias ilimitadas" EnableViewState="False"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="chkIlimitadas" runat="server" Text="" ClientIDMode="Static" />
                </td>
                <td class="td-label">
                    <asp:Label ID="lblNumeroLicencias" runat="server" Text="Número de licencias" EnableViewState="False"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNumeroLicencias" runat="server" ClientIDMode="Static" MaxLength="9"
                        CssClass="digits"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <h2>
            Informaci&oacute;n del cliente
        </h2>
        <hr />
        <br />
        <table>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblNombreCte" runat="server" Text="Nombre" EnableViewState="False"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNombreCte" runat="server" ClientIDMode="Static" MaxLength="100"
                        CssClass="textoEnunciado"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblDireccionCte" runat="server" Text="Dirección" EnableViewState="False"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDireccionCte" runat="server" ClientIDMode="Static" MaxLength="150"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblRepresentante" runat="server" Text="Representante" EnableViewState="False"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtRepresentante" runat="server" ClientIDMode="Static" MaxLength="100"
                        CssClass="textoEnunciado"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblTelefono" runat="server" Text="Teléfono" EnableViewState="False"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtTelefono" runat="server" ClientIDMode="Static" MaxLength="20"
                        CssClass="digits"></asp:TextBox>
                </td>
            </tr>
        </table>
        <div class="line">
        </div>
        <div class="">
            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="boton" OnClick="btnGuardar_OnClick"
                ClientIDMode="Static" />
            <asp:HyperLink ID="HpLnkCancelar" Text="Cancelar" runat="server" NavigateUrl="BuscarContrato.aspx"
                CssClass="boton"></asp:HyperLink>
        </div>
        <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
            style="display: none;" />
    </div>
</asp:Content>
