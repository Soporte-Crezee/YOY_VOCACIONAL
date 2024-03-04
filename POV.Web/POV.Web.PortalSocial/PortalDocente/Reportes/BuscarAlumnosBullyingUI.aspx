<%@ Page Title="YOY - ORIENTADOR" Language="C#" MasterPageFile="~/PortalDocente/PortalDocente.master" AutoEventWireup="true" CodeBehind="BuscarAlumnosBullyingUI.aspx.cs" Inherits="POV.Web.PortalSocial.PortalDocente.Reportes.BuscarAlumnosBullyingUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head_selected" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet" type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            $(".boton").button();
            $("#frmMain").validate();
        }
    </script>
    <style type="text/css">
        label {
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
            width: 100%;
            display: inline-block;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content_selected" runat="server">
   <div class="panel panel-default">
            <div class="panel-heading">         
                Estudiantes con pruebas Bullying contestadas
            </div>
            <div class="panel-body">
                <div class="input-group">
                    <span class="hidden-xs hidden-sm input-group-addon">Nombre del estudiante</span>
                        <asp:TextBox ID="txtNombre" MaxLength="30" runat="server" CssClass="form-control" placeholder="Nombre del estudiante"></asp:TextBox>
                    <div class="input-group-btn">
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-green" OnClick="btnBuscar_Click" />
                    </div>
                </div>
        </div>
    </div>
            <asp:UpdatePanel ID="updAlumnosBullying" runat="server">
                <ContentTemplate>
                    <div style="padding-top: 10px" class="table-responsive">                    
                        <asp:GridView AutoGenerateColumns="false" runat="server" ID="grdAlumnosBullying" CssClass="table table-bordered table-striped"
                            RowStyle-CssClass="td" HeaderStyle-CssClass="th" AllowPaging="True"
                            Width="100%" EnableSortingAndPagingCallbacks="True" AllowSorting="True" OnRowCommand="grdAlumnosBullying_RowCommand"
                            OnSorting="grdAlumnosBullying_Sorting" OnRowDataBound="grdAlumnosBullying_DataBound" OnPageIndexChanging="grdAlumnosBullying_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="NombreAlumno" HeaderText="Estudiante" />
                                <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="lnkCompletar" runat="server" CommandName="completar" CommandArgument='<%#Eval("AlumnoID")%>'
                                            Width="25px" Height="25px" ImageUrl="~/Images/icons/VOCAREER_buscar.png" ToolTip="Completar prueba Bullying"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="">
                                    <p>
                                        <span class="ui-icon ui-icon-info"></span>La búsqueda no produjo
                                    resultados
                                    </p>
                                </div>
                            </EmptyDataTemplate>
                            <HeaderStyle CssClass="th" />
                            <RowStyle CssClass="td" />
                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
</asp:Content>
