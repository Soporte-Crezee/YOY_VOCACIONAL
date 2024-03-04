<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResultadoPruebaRotterTutorado.aspx.cs" Inherits="POV.Web.PortalTutor.Pages.Reportes.ResultadoPruebaRotterTutorado" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function showMessage(text) {
            var api = new MessageApi();
            api.CreateMessage(text, "ERROR");
            resetHiden();
            api.Show();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="page_content" runat="server">
    <div class="panel panel-default">
        <div class="panel-heading">
            B&iacute;squeda por hijo
        </div>
        <div class="panel-body">
            <div class="col-lg-8 col-lg-offset-2">
                <div class="input-group">
                    <span class="hidden-xs hidden-sm input-group-addon btn-addon-find">Nombre del hijo</span>
                    <asp:TextBox ID="txtNombreTutorado" runat="server" CssClass="form-control text-find" placeholder="Nombre del hijo"></asp:TextBox>
                    <span class="input-group-btn">
                        <asp:Button ID="btnBuscarHijo" runat="server" Text="Buscar" OnClick="btnBuscarHijo_Click" CssClass="btn btn-green" />
                    </span>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="updHijosRotter" runat="server">
        <ContentTemplate>
            <div class="table-responsive">
                <asp:GridView AutoGenerateColumns="false" runat="server" ID="grdHijosRotter" RowStyle-CssClass="td" HeaderStyle-CssClass="th"
                    CssClass="table table-bordered table-striped" OnRowCommand="grdHijosRotter_RowCommand" OnRowDataBound="grdHijosRotter_RowDataBound">
                    <HeaderStyle CssClass="th"></HeaderStyle>
                    <RowStyle CssClass="td"></RowStyle>
                    <Columns>
                        <asp:BoundField DataField="NombreAlumno" HeaderText="Nombre" />
                        <asp:BoundField DataField="NombrePrueba" HeaderText="Prueba" HeaderStyle-CssClass="hidden-xs hidden-sm" ItemStyle-CssClass="hidden-sm hidden-xs"/>
                        <asp:BoundField DataField="FechaInicio" HeaderText="Fecha incio" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hidden-xs hidden-sm" 
                            ItemStyle-CssClass="hidden-sm hidden-xs"></asp:BoundField>
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