<%@ Page Language="C#" Title="YOY - ORIENTADOR" MasterPageFile="~/PortalDocente/PortalDocente.master" AutoEventWireup="true" CodeBehind="ExpedienteAlumnoOrientador.aspx.cs" Inherits="POV.Web.PortalSocial.PortalDocente.ExpedienteAlumnoOrientador" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head_selected" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet" type="text/css" />
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>styleAcordion.css" rel="stylesheet" type="text/css" />
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>jquery-ui-v1.12.0.css" rel="stylesheet" type="text/css" />
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>muro.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
        });
        $(function () {

            $.blockUI.defaults.overlayCSS.backgroundColor = "white";
            $.blockUI.defaults.message = '<h1 style="font-size:20px;">Registrando, por favor espere...</h1>';

        }
        );
    </script>
    <style type="text/css">
        .DDFloatLeft label, .DDFloatRight label {
            font-weight: normal !important;
           
        }

    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content_selected" runat="server">
    <div id="info_alumno" style="display: none;">
        <asp:Label ID="LblNombreUsuario" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Label ID="LblNombreGrupo" runat="server" Text="" Visible="false"></asp:Label>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            B&uacute;squeda por estudiante
        </div>
        <div class="panel-body">
            <div class="input-group">
                <span class="hidden-xs hidden-sm input-group-addon">Nombre del estudiante</span>                   
                <asp:TextBox CssClass="form-control" ID="txtAlumno" runat="server" placeholder="Nombre del estudiante"></asp:TextBox>                                             
                <div class="input-group-btn">     
                    <asp:Button ID="btnBuscarAlumno" runat="server" Text="Buscar" OnClick="btnBuscarAlumno_Click" CssClass="btn btn-green" />
                </div>
            </div>                          
        </div>
    </div>
                <asp:UpdatePanel runat="server" ID="UpdExpedienteAlumno">
                    <ContentTemplate>
                        <!-- Grid Alumnos -->
                        <div class="table-responsive">
                            <asp:GridView AutoGenerateColumns="false" runat="server" ID="gvAlumnos"
                                CssClass="table table-bordered table-striped"
                                RowStyle-CssClass="td" HeaderStyle-CssClass="th"  OnRowCommand="gvAlumnos_RowCommand"
                                PageSize="10" AllowPaging="true" AllowSorting="false" OnRowDataBound="gvAlumnos_RowDataBound" OnSelectedIndexChanged="gvAlumnos_SelectedIndexChanged">
                                <Columns>
                                    <asp:BoundField HeaderText="Nombre del estudiante" DataField="NombreCompletoAlumno"></asp:BoundField>
                                    <asp:BoundField HeaderText="Nivel de estudio" DataField="Grado"></asp:BoundField>
                                    <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnVerExpediente" ToolTip="Ver expediente" runat="server"
                                                ImageUrl="~/Images/btn_search.png" Width="25px" Height="25px" CommandName="VerExpediente"
                                                CommandArgument='<%# Eval("AlumnoID")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="ui-state-highlight ui-corner-all">
                                        <p>
                                            <span class="ui-icon ui-icon-info" style="display: inline-block; vertical-align: middle; margin-top: 0px"></span>
                                            No se encontraron resultados
                                        </p>

                                    </div>
                                </EmptyDataTemplate>
                                <HeaderStyle CssClass="th" />
                                <RowStyle CssClass="td" />
                                <PagerTemplate>
                                    <asp:GridViewPager ID="grdViewPager" runat="server" DataSourceType="List<Alumno>" SessionName="ListAlumno" />
                                </PagerTemplate>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:HiddenField runat="server" ID="hdnDialogResultado" />

        <script type="text/javascript">
            function CerrarDialogo() {
                $('#MainContent_hdnDialogResultado').val('');
            }
        </script>
        <asp:HiddenField ID="hdnSocialHubID" runat="server" />
        <asp:HiddenField ID="hdnUsuarioSocialID" runat="server" />
        <asp:HiddenField ID="hdnSessionSocialHubID" runat="server" />
        <asp:HiddenField ID="hdnSessionUsuarioSocialID" runat="server" />
        <asp:HiddenField ID="hdnTipoPublicacionTexto" runat="server" />
        <asp:HiddenField ID="hdnTipoPublicacionSuscripcionReactivo" runat="server" />
        <asp:HiddenField ID="hdnFuente" runat="server" Value="D" />
</asp:Content>
