<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegistrarAlumno.aspx.cs" Inherits="POV.Web.PortalOperaciones.Alumnos.RegistrarAlumno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/gridview.css" rel="stylesheet" type="text/css" />
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
                            <%=fuArchivoAlumnos.UniqueID %>:
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
            <asp:HyperLink ID="lnkBack" runat="server"></asp:HyperLink>Importar excel de estudiantes</h3>
        <div class="ui-widget-content" style="padding: 5px" id="Contenido" runat="server" visible="false">
            <h2>Informaci&oacute;n</h2>
            <hr />
            <div class="container">
                <div class="alert alert-danger">
                    <strong>Importante!</strong>
                    <p>
                        Para poder dar de alta a los estudiantes, es necesario que primero realices la carga de 
                las Escuelas, seguido de los Orientadores, continuando con los Padres y por el último los estudiantes.
                    </p>
                    <strong>Si usted no sigue estos pasos los estudiantes no serán dados de alta en el sistema.</strong>
                </div>
            </div>
            <br />
            <asp:Panel ID="pnlCargarAlumnos" runat="server" Visible="false">
                <h2>Excel de estudiantes</h2>
                <hr />
                <br />
                <table>
                    <tr>
                        <td class="td-label">
                            <asp:Label ID="lblAlumnos" runat="server" Text="Estudiantes"></asp:Label>
                        </td>
                        <td style="width: 400px;">
                            <asp:FileUpload ID="fuArchivoAlumnos" runat="server" Style="width: 100%" />
                        </td>
                        <td style="width: 70px;">
                            <asp:Button ID="btnCargar" runat="server" Text="Cargar" Style="width: 100%" CssClass="btn-green"
                                OnClick="btnCargar_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlErrorDisponibles" runat="server" Visible="false">
                <p style="color: Red;">
                    No se puede realizar la importaci&oacute;n, no hay licencias disponibles
                </p>
            </asp:Panel>
            <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
                style="display: none;" />
            <div class="results">
            </div>
        </div>
    </div>
</asp:Content>
