<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ExpedienteAlumnoTutor.aspx.cs" Inherits="POV.Web.PortalTutor.Pages.ExpedienteAlumnoTutor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

        });

        $(function () {
            $.blockUI.defaults.overlayCSS.backgroundColor = "white";
            $.blockUI.defaults.message = '<h1 style="font-size:20px;">Registrando, por favor espere...</h1>';

        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="page_content" runat="server">
    <div id="info_alumno" class="breadcrumb">
        <asp:Label ID="LblNombreUsuario" runat="server" Text="" Visible="false"></asp:Label>
        <h1 class="" visible="false">Expediente del estudiante</h1>
        <asp:Label ID="LblNombreGrupo" runat="server" Text="" Visible="false"></asp:Label>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            B&uacute;squeda por hijo
        </div>
        <div class="panel-body">
            <div class="col-lg-8 col-lg-offset-2">
                <div class="input-group">
                    <span class="hidden-xs hidden-sm input-group-addon btn-addon-find">Nombre del hijo</span>
                    <asp:TextBox ID="txtAlumno" runat="server" CssClass="form-control text-find" placeholder="Nombre del hijo"></asp:TextBox>
                    <span class="input-group-btn">
                        <asp:Button ID="btnBuscarAlumno" runat="server" Text="Buscar" OnClick="btnBuscarAlumno_Click" CssClass="btn btn-green" />
                    </span>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdatePanel runat="server" ID="UpdExpedienteAlumno">
        <ContentTemplate>
            <div class="table-responsive">
                <asp:GridView AutoGenerateColumns="false" runat="server" ID="gvAlumnos" CssClass="table table-bordered table-striped"
                    OnRowCommand="gvAlumnos_RowCommand" PageSize="10" AllowPaging="true" AllowSorting="false" OnRowDataBound="gvAlumnos_RowDataBound">
                    <Columns>
                        <asp:BoundField HeaderText="Nombre del estudiante" DataField="NombreCompletoAlumno" />
                        <asp:BoundField HeaderText="Nivel de estudio" DataField="Grado" HeaderStyle-CssClass="hidden-xs hidden-sm" ItemStyle-CssClass="hidden-sm hidden-xs"/>
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnVerExpediente" ToolTip="Ver expediente" runat="server" ImageUrl="~/images/VOCAREER_buscar.png"
                                    Width="25px" Height="25px" CommandName="VerExpediente" CommandArgument='<%# Eval("AlumnoID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div>
                            <p>
                                No se encontraron resultados
                            </p>
                        </div>
                    </EmptyDataTemplate>
                    <HeaderStyle />
                    <RowStyle />
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <script type="text/javascript">
        function CerrarDialogo() {
            $('#MainContent_hdnDialogResultado').val('');
        }
    </script>
</asp:Content>
