<%@ Page Title="" Language="C#" MasterPageFile="~/Content/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="BuscarAlumnosSACKSUI.aspx.cs" Inherits="POV.Web.HerramientasDocente.Pages.Pruebas.BuscarAlumnosSACKSUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Content/Styles/")%>gridview.css" rel="stylesheet" type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Content/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Content/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            $(".boton").button();
            $("#frmMain").validate();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="bodyadaptable">
        <div class="panel panel-default" style="margin-top: 10px;">
            <div class="panel-heading">
                <asp:Label runat="server" ID="lblSubtitulo1" Text="Alumnos con pruebas SACKS contestadas"></asp:Label>
            </div>
            <div class="panel-body">
                <div class="col-md-offset-1 col-md-10 col-lg-offset-2 col-lg-8">
                    <div class="input-group">
                        <span class="hidden-xs hidden-sm input-group-addon">Nombre del estudiante</span>
                        <asp:TextBox ID="txtNombre" MaxLength="30" runat="server" CssClass="form-control" placeholder="Nombre del estudiante" Style="height: 40px;"></asp:TextBox>
                        <span class="input-group-btn">
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-green button_clip_39215E" OnClick="btnBuscar_Click" />
                        </span>
                    </div>
                </div>
            </div>
        </div>
        <div class="hidden-lg hidden-md" style="margin-bottom: 20px;"></div>
    </div>
    <div class="results" style="margin-bottom: 20px">
        <asp:UpdatePanel ID="updAlumnosSACKS" runat="server">
            <ContentTemplate>
                <asp:GridView ID="grdAlumnosSACKS" runat="server" CssClass="table table-striped table-bordered" RowStyle-CssClass="td"
                    HeaderStyle-CssClass="th" AutoGenerateColumns="False" AllowPaging="True"
                    Width="100%" EnableSortingAndPagingCallbacks="True" AllowSorting="True" OnRowCommand="grdAlumnosSACKS_RowCommand"
                    OnSorting="grdAlumnosSACKS_Sorting" OnRowDataBound="grdAlumnosSACKS_DataBound" OnPageIndexChanging="grdAlumnosSACKS_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="NombreAlumno" HeaderText="Estudiante"></asp:BoundField>
                        <asp:BoundField DataField="NombrePrueba" HeaderText="Prueba">
                            <HeaderStyle CssClass="hidden-sm hidden-xs" />
                            <ItemStyle CssClass="hidden-sm hidden-xs" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FechaInicio" HeaderText="Fecha incio">
                            <HeaderStyle CssClass="hidden-sm hidden-xs" />
                            <ItemStyle CssClass="hidden-sm hidden-xs" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Fecha Fin" DataField="FechaFin">
                            <HeaderStyle CssClass="hidden-sm hidden-xs" />
                            <ItemStyle CssClass="hidden-sm hidden-xs" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkCompletar" runat="server" CommandName="completar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "AlumnoID")%>'
                                    Text="Sumario general prueba" ToolTip="Completar prueba SACKS"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="ui-state-highlight ui-corner-all">
                            <p>
                                <span class="ui-icon ui-icon-info"></span>La búsqueda no produjo
                                resultados
                            </p>
                        </div>
                    </EmptyDataTemplate>
                    <HeaderStyle CssClass="th" />
                    <RowStyle CssClass="td" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
