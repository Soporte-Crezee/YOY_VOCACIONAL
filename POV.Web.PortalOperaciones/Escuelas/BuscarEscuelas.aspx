<%@ Page Title="Vocarrer - Gestión de escuelas" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="BuscarEscuelas.aspx.cs" Inherits="POV.Web.PortalOperaciones.Escuelas.BuscarEscuelas" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/gridview.css" rel="stylesheet" type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/messages_es.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);

        function initPage() {
            $(".boton_new").button();
            $(".boton_search").button({ icons: {
                primary: "ui-icon-search"
            }
            });
            $("#frmMain").validate();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        Cat&aacute;logo de escuelas
    </h3>
    <div class="finder ui-widget-content">
        <h2>
            Informaci&oacute;n del contrato
        </h2>
        <hr />
        <br />
        <table class="finder">
            <tr>
                <td class="label">
                    Clave contrato
                </td>
                <td colspan="4">
                    <asp:DropDownList ID="cbContrato" runat="server" AppendDataBoundItems="True" />
                </td>
            </tr>
        </table>
        <br />
        <h2>
            Informaci&oacute;n de la ubicaci&oacute;n
        </h2>
        <hr />
        <br />
        <table class="finder">
            <tr>
                <td class="label">
                    <asp:Label runat="server" ID="lblPais" Text="País"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel runat="server" ID="updPais">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="CbPais" TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="CbPais_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td class="label">
                    <asp:Label runat="server" ID="lblEstado" Text="Estado"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="updEstado" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="CbEstado" TabIndex="2" AutoPostBack="True" OnSelectedIndexChanged="CbEstado_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td class="label">
                    <asp:Label runat="server" ID="lblMunicipio" Text="Municipio"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="updCiudad" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="CbMunicipio" TabIndex="3" AutoPostBack="True"
                                OnSelectedIndexChanged="CbMunicipio_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td class="label">
                    <asp:Label runat="server" ID="lblLocalidad" Text="Localidad"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="updLocalidad" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="CbLocalidad" TabIndex="4" AutoPostBack="True"
                                OnSelectedIndexChanged="CbLocalidad_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td class="label">
                    <asp:Label runat="server" ID="lblZona" Text="Zona"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel runat="server" ID="updZona">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="CbZona">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <br />
        <h2>
            Informaci&oacute;n de la escuela
        </h2>
        <hr />
        <br />
        <table class="finder">
            <tr>
                <td class="label">
                    <asp:Label runat="server" ID="lblClaveEscuela" Text="Clave escuela"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtClaveEscuela" ReadOnly="False" Enabled="True"
                        CssClass="textoEnunciado" MaxLength="50"></asp:TextBox>
                </td>
                <td class="label">
                    <asp:Label runat="server" ID="lblTurno" Text="Turno"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel runat="server" ID="updTurno">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="CbTurno" TabIndex="5">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td class="label">
                    <asp:Label runat="server" ID="lblNombreEscuela" Text="Nombre"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtNombreEscuela" ReadOnly="False" Enabled="True"
                        CssClass="textoEnunciado" MaxLength="50"></asp:TextBox>
                </td>
                <td class="label">
                    <asp:Label runat="server" ID="lblAmbito" Text="Ámbito"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel runat="server" ID="updAmbito">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="CbAmbito" TabIndex="6">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td class="label">
                    <asp:Label runat="server" ID="lblTipoNivelEducativo" Text="Tipo nivel educativo"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel runat="server" ID="updTipoNivelEducativo">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="CbTipoNivelEducativo" AutoPostBack="True" OnSelectedIndexChanged="CbTipoNivelEducativo_SelectedIndexChanged" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td class="label">
                    <asp:Label runat="server" ID="lblNivel" Text="Nivel educativo"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel runat="server" ID="updNivel">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="CbNivel" AutoPostBack="True" OnSelectedIndexChanged="CbNivel_SelectedIndexChanged" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
               
                <td class="label">
                    <asp:Label runat="server" ID="lblTipoServicio" Text="Tipo servicio"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel runat="server" ID="updServicio">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="CbTipoServicio" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                 <td class="label">
                    <asp:Label runat="server" ID="lblControl" Text="Control"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel runat="server" ID="updControl">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="CbControl" TabIndex="7">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton_search" OnClick="btnBuscar_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div class="results">
        <div id="PnlCreate" class="nuevo" runat="server" visible="false">
            <a href="NuevaEscuela.aspx" id="lnkNuevaEscuela" class="boton_new"><span class="ui-icon ui-icon-circle-plus"
                style="display: inline-block; vertical-align: middle; margin-top: -5px;"></span>
                <asp:Label ID="lblNuevaEscuela" runat="server" Text="Agregar nueva escuela"></asp:Label>
            </a>
        </div>
        <asp:UpdatePanel ID="updEscuela" runat="server">
            <ContentTemplate>
                <asp:GridView ID="grdEscuela" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                    HeaderStyle-CssClass="th" AutoGenerateColumns="False" PageSize="10" AllowPaging="True"
                    OnRowCommand="grdEscuela_RowCommand" Visible="false" OnRowDataBound="grdEscuela_DataBound"
                    Width="100%">
                    <Columns>
                        <asp:BoundField DataField="ClaveContrato" HeaderText="Clave contrato" SortExpression="ClaveContrato" />
                        <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" />
                        <asp:BoundField DataField="NombreEscuela" HeaderText="Nombre" SortExpression="Nombre" />
                        <asp:BoundField DataField="Turno" HeaderText="Turno" SortExpression="Turno" />
                        <asp:BoundField DataField="TipoServicio" HeaderText="Tipo de servicio" SortExpression="TipoServicio" />
                        <asp:BoundField DataField="Ambito" HeaderText="Ámbito" SortExpression="Ambito" />
                        <asp:BoundField DataField="Estado" HeaderText="Estado" SortExpression="Estado" />
                        <asp:BoundField DataField="Ciudad" HeaderText="Municipio" SortExpression="Ciudad" />
                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnUpdateLicencia" runat="server" CommandName="updLicencia"
                                    CommandArgument='<%#DataBinder.Eval(Container.DataItem, "LicenciaEscuelaID") %>'
                                    ImageUrl="../images/arrow_refresh.png" ToolTip="Actualizar licencia" Visible="false" />
                                <asp:ImageButton ID="btnEdit" runat="server" CommandName="editar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "LicenciaEscuelaID") %>'
                                    ImageUrl="../images/edit-button.png" ToolTip="Editar" Visible="false" />
                                <asp:ImageButton ID="btnDelete" runat="server" CommandName="eliminar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "LicenciaEscuelaID")%>'
                                    ImageUrl="../images/minus-button.png" ToolTip="Eliminar" OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');"
                                    Visible="false" />
                            </ItemTemplate>
                            <HeaderStyle Wrap="False" Width="40" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="ui-state-highlight ui-corner-all">
                            <p>
                                <span class="ui-icon ui-icon-info" style="float: left;"></span>La b&uacute;squeda
                                no produjo resultados
                            </p>
                        </div>
                    </EmptyDataTemplate>
                    <PagerTemplate>
                        <asp:GridViewPager runat="server" ID="grdViewPager" SessionName="escuelasCDS" DataSourceType="DataSet" />
                    </PagerTemplate>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
