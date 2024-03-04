<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="BuscarProgramaEducativo.aspx.cs" Inherits="POV.Web.PortalOperaciones.CatalogoAsignaturas.BuscarProgramaEducativo" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet"
        type="text/css" />
    <script src="<%=Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<%=Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            $(".boton_new").button();
            $(".boton").button({ icons: {
                primary: "ui-icon-search"
            }
            });

            $('#<%=txtVigenciaInicio.ClientID %>').datepicker({ changeYear: true, changeMonth: true, dateFormat: "dd/mm/yy" });
            $('#<%=txtVigenciaFin.ClientID %>').datepicker({ changeYear: true, changeMonth: true, dateFormat: "dd/mm/yy" });



        }

        function ValidarFormBusqueda() {
            $("#frmMain").validate({
                rules: {
                    '<%=txtIDPlan.UniqueID %>': {
                        maxlength: 2
                    },
                    '<%=txttituloPlan.UniqueID %>': {
                        maxlength: 50
                    },
                    '<%=txtDescripcion.UniqueID %>': {
                        maxlength: 200
                    }
                }
                , submitHandler: function (form) {
                    $(form).block();
                    form.submit();
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        Cat&aacute;logo de planes educativos
    </h3>
    <div class="finder ui-widget-content">
        <asp:UpdatePanel ID="updGestionPrograma" runat="server">
            <ContentTemplate>
                <table class="finder">
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblIDPlan" runat="server" Text="ID"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtIDPlan" runat="server" MaxLength="2"></asp:TextBox>
                        </td>
                        <td class="label">
                            <asp:Label ID="lbltituloPlan" runat="server" Text="T&iacute;tulo"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txttituloPlan" runat="server" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblDescripcion" runat="server" Text="Descripci&oacute;n"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescripcion" runat="server" MaxLength="200"></asp:TextBox>
                        </td>
                        <td class="label">
                            <asp:Label ID="lblNivelEducativo" runat="server" Text="Nivel educativo"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="cbNivelEducativo" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblVigenciaInicio" runat="server" Text="Inicia vigencia"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVigenciaInicio" runat="server" MaxLength="100"></asp:TextBox>
                        </td>
                        <td class="label">
                            <asp:Label ID="lblVigenciaFin" runat="server" Text="Termina vigencia"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVigenciaFin" runat="server" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblEstatus" runat="server" Text="Estatus"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="cbEstatus" runat="server">
                                <asp:ListItem Value="" Text="Todos" Selected="true"></asp:ListItem>
                                <asp:ListItem Value="true" Text="Activos"></asp:ListItem>
                                <asp:ListItem Value="false" Text="Inactivos"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton" OnClick="btnBuscar_Click"
                                OnClientClick="ValidarFormBusqueda();" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="results">
        <div id="PnlCreate" class="nuevo" runat="server" visible="false">
            <a href="RegistrarProgramaEducativo.aspx" id="lnkNuevoProgramaEducativo" class="boton_new">
                <span class="ui-icon ui-icon-circle-plus" style="display: inline-block; vertical-align: middle;
                    margin-top: -5px;"></span>
                <asp:Label ID="lblNuevoProgramaEducativo" runat="server" Text="Agregar nuevo plan educativo"></asp:Label>
            </a>
        </div>
        <asp:UpdatePanel ID="updPlanEducativo" runat="server">
            <ContentTemplate>
                <asp:GridView ID="grdProgramaEducativo" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                    HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                    Width="100%" OnRowCommand="grdProgramaEducativo_RowCommand" OnRowDataBound="grdProgramaEducativo_DataBound"
                    OnSorting="grdProgramaEducativo_Sorting" Visible="false">
                    <Columns>
                        <asp:BoundField DataField="PlanEducativoID" HeaderText="ID" SortExpression="PlanEducativoID" />
                        <asp:BoundField DataField="Titulo" HeaderText="Título" SortExpression="Titulo" />
                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
                        <asp:BoundField DataField="ValidoDesde" HeaderText="Válido desde" SortExpression="ValidoDesde"
                            DataFormatString="{0:d}" />
                        <asp:BoundField DataField="ValidoHasta" HeaderText="Válido hasta" SortExpression="ValidoHasta"
                            DataFormatString="{0:d}" />
                        <asp:TemplateField HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left">
                            <HeaderTemplate>
                                <label>
                                    Estado</label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# (bool)DataBinder.Eval(Container, "DataItem.Estatus") == true ? "Activo" : "Inactivo"%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" CommandName="editar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "PlanEducativoID") %>'
                                    ImageUrl="../images/edit-button.png" ToolTip="Editar" Visible="false" />
                                <asp:ImageButton ID="btnDelete" runat="server" CommandName="eliminar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "PlanEducativoID")%>'
                                    ImageUrl="../images/minus-button.png" ToolTip="Eliminar" OnClientClick="return confirm('¿Está seguro que desea desactivar este plan?');"
                                    Visible="false" />
                            </ItemTemplate>
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
                        <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="PlanesEducativos"
                            DataSourceType="DataSet" />
                    </PagerTemplate>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(loadControls);

        function loadControls() {

            var ddate = new Date();
            var maxddate = new Date(ddate.getFullYear(), -1, 1);

            $('<%=txtVigenciaInicio.ClientID %>').datepicker({ maxDate: maxddate, changeYear: true });
            $('<%=txtVigenciaFin.ClientID %>').datepicker({ maxDate: maxddate, changeYear: true });
        }
    </script>
</asp:Content>
