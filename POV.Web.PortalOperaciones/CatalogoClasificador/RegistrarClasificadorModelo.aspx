<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegistrarClasificadorModelo.aspx.cs" Inherits="POV.Web.PortalOperaciones.CatalogoClasificador.RegistrarClasificadorModelo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/messages_es.js")%>" type="text/javascript"></script>
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
    <div class="bodyadaptable  col-xs-13 col-md-13">
        <h3 class="ui-widget-header ui-widget-header-label">
            <asp:LinkButton ID="lnkBack" runat="server" OnClick="BtnCancelar_OnClick" OnClientClick="CancelValidate()">Volver</asp:LinkButton>/Registrar clasificador de modelo de prueba
        </h3>
        <div class="main_div ui-widget-content" style="padding: 5px;">
            <table>
                <tr>
                    <td class="td-label">
                        <label class="label">Nombre</label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtNombre" ReadOnly="False" Enabled="True" CssClass="required" MaxLength="200" TabIndex="1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td-label-texarea">
                        <label class="label">Descripción</label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDescripcion" ReadOnly="False" Enabled="True" TextMode="MultiLine" MaxLength="500" TabIndex="3">
                        </asp:TextBox>
                    </td>
                </tr>
            </table>
            <br />
            <h2>Descripción de propiedades de los clasificadores</h2>
            <hr />
            <br />
            <%-- BEGIN formulario Propiedades --%>
            <asp:UpdatePanel ID="UpdPanelPropiedades" runat="server">
                <ContentTemplate>
                    <table>
                        <asp:Repeater runat="server" ID="RptPropiedades" OnItemDataBound="RptPropiedades_ItemDataBound" Visible="false">
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="label">
                                        <asp:Label runat="server" ID="lblPropiedadID" Text='<%# Eval("propiedadID") %>' Visible="false"> </asp:Label>
                                    </td>
                                    <td class="td-label-texarea">
                                        <asp:Label runat="server" ID="lblNombreProp" Text='<%# Eval("nombrePropiedad") %>'> </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDescripcionPropiedad" runat="server" TextMode="MultiLine" MaxLength="500"></asp:TextBox>
                                    </td>
                                </tr>

                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <div class="line"></div>
            <%-- END formulario Propiedades --%>
            <table>
                <tr>
                    <td class="td-label"></td>
                    <td class="label">
                        <asp:Button ID="BtnGuardar" Text="Guardar" runat="server" OnClick="BtnGuardar_OnClick"
                            CssClass="btn-green" OnClientClick="ValidateFields()" />
                    </td>
                    <td class="label">
                        <asp:Button ID="BtnCancelar" Text="Cancelar" runat="server" OnClick="BtnCancelar_OnClick"
                            CssClass="btn-cancel" OnClientClick="CancelValidate()" />
                    </td>
                    <td class="td-label"></td>
                </tr>
            </table>

            <span style=""></span>
            <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
                style="display: none;" />
        </div>
    </div>
</asp:Content>
