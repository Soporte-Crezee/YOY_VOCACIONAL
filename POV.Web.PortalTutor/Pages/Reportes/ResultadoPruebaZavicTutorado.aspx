<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResultadoPruebaZavicTutorado.aspx.cs" Inherits="POV.Web.PortalTutor.Pages.Reportes.ResultadoPruebaZavicTutorado" %>

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
<asp:Content ID="Content2" ContentPlaceHolderID="page_content" runat="server">
    <div class="panel panel-default">
        <div class="panel-heading">
            B&uacute;squeda por hijos
        </div>
        <div class="panel-body">
            <div class="col-lg-8 col-lg-offset-2">
                <div class="input-group">
                    <span class="hidden-xs hidden-sm input-group-addon btn-addon-find">Nombre del hijo</span>
                    <asp:TextBox ID="txtNombre" MaxLength="30" runat="server" CssClass="form-control text-find" placeholder="Nombre del hijo"></asp:TextBox>
                    <span class="input-group-btn">
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" CssClass="btn btn-green" />
                    </span>
                </div>
            </div>
        </div>
    </div>
    
    <asp:UpdatePanel ID="updAlumnosAllport" runat="server">
        <ContentTemplate>
                <div class="table-responsive">
                <asp:GridView AutoGenerateColumns="false" runat="server" ID="grdAlumnosZavic"
                    RowStyle-CssClass="td"
                    HeaderStyle-CssClass="th"
                    CssClass="table table-bordered table-striped"
                    OnRowCommand="grdAlumnosZavic_RowCommand"
                    OnSorting="grdAlumnosZavic_Sorting" OnRowDataBound="grdAlumnosZavic_DataBound">
                    <HeaderStyle CssClass="th"></HeaderStyle>
                    <RowStyle CssClass="td"></RowStyle>
                    <Columns>
                        <asp:BoundField DataField="NombreAlumno" HeaderText="Nombre" />
                        <asp:BoundField DataField="NombrePrueba" HeaderText="Prueba" HeaderStyle-CssClass="hidden-xs hidden-sm" ItemStyle-CssClass="hidden-sm hidden-xs"/>
                        <asp:BoundField DataField="FechaInicio" HeaderText="Fecha incio" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hidden-xs hidden-sm" ItemStyle-CssClass="hidden-sm hidden-xs"></asp:BoundField>
                        <asp:BoundField HeaderText="Fecha Fin" DataField="FechaFin" HeaderStyle-CssClass="hidden-xs hidden-sm" ItemStyle-CssClass="hidden-sm hidden-xs"/>
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imbAsignarPaquete" runat="server" CommandName="VerReporte" ImageUrl="~/images/VOCAREER_buscar.png"
                                    CommandArgument='<%#DataBinder.Eval(Container.DataItem,"AlumnoID")  %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="ui-state-highlight ui-corner-all">
                            <p>
                                <span class="ui-icon ui-icon-info" style="float: left"></span>La búsqueda no produjo resultados
                            </p>
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
