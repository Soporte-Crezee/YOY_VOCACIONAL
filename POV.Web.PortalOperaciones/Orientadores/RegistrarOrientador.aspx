<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegistrarOrientador.aspx.cs" Inherits="POV.Web.PortalOperaciones.Orientadores.RegistrarOrientador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/messages_es.js")%>" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(initPage);

        function initPage() {
            $('.boton').button();
            $("#frmMain").validate(
                {
                    rules: 
                        {
                            <%=fuArchivoDocentes.UniqueID %>:
                        {
                            required: true
                        }
                        },
                    submitHandler: function(form) {
                        DocumentBlockUI();
                        form.submit();
                    }
                });
            }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="bodyadaptable col-xs-13 col-md-13">
        <h3 class="ui-widget-header ui-widget-header-label">
            <asp:HyperLink ID="lnkBack" runat="server"></asp:HyperLink>Importar excel de orientadores</h3>

        <div class="ui-widget-content" style="padding: 5px" id="Contenido" runat="server" visible="false">
            <h2>Informaci&oacute;n</h2>
            <hr />
            <div class="container">
                <div class="alert alert-danger">
                    <strong>Importante!</strong>
                    <p>Para poder dar de alta a los orientadores, es necesario que primero se realice la carga de Escuelas.</p>
                    <strong>Si usted no sigue las indicaciones los orientadores no serán dados de alta en el sistema.</strong>
                </div>
            </div>
            <br />
            <asp:Panel ID="pnlCargaOrientadores" runat="server" Visible="false">
                <h2>Excel  de  orientadores</h2>
                <hr />
                <br />
                <table>
                    <tr>
                        <td class="td-label">
                            <asp:Label ID="lblDocentes" runat="server" Text="Orientadores"></asp:Label>
                        </td>
                        <td style="width: 400px;">
                            <asp:FileUpload ID="fuArchivoDocentes" runat="server" Style="width: 100%" />
                        </td>
                        <td style="width: 70px;">
                            <asp:Button ID="btnCargar" runat="server" Text="Cargar" Style="width: 100%" CssClass="btn-green" OnClick="btnCargar_OnClick" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlErrorDisponibles" runat="server" Visible="false">
                <p style="color: Red;">
                    No se puede realizar la importaci&oacute;n, no hay licencias disponibles
                </p>
            </asp:Panel>

            <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server" style="display: none;" />
        </div>
        <div class="results">
        </div>
    </div>
</asp:Content>
