﻿<%@ Page Title="YOY - ORIENTADOR" MasterPageFile="~/PortalDocente/PortalDocente.master" Language="C#" AutoEventWireup="true" CodeBehind="BuscarAlumnosRotterUI.aspx.cs" Inherits="POV.Web.PortalSocial.PortalDocente.Reportes.BuscarAlumnosRotterUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head_selected" runat="server">
    <link href="<%=Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="Stylesheet" type="text/css" />
    <script src="<%=Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<%=Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
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
            Estudiantes con pruebas Rotter contestadas
        </div>
        <div class="panel-body">
            <div class="input-group">
                <span class="hidden-xs hidden-sm input-group-addon">Nombre del estudiante</span>
                <asp:TextBox ID="txtNombre" MaxLength="30" runat="server" CssClass="form-control" placeholder="Nombre del estudiante"></asp:TextBox>
                <span class="input-group-btn">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-green" OnClick="btnBuscar_Click" />
                </span>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="updAlumnosAllport" runat="server">
        <ContentTemplate>
            <div class="table-responsive">
                <asp:GridView AutoGenerateColumns="false" runat="server" ID="grdAlumnosRotter" CssClass="table table-bordered table-striped"
                    RowStyle-CssClass="td" HeaderStyle-CssClass="th" AllowPaging="true"
                    Width="100%" EnableSortingAndPagingCallbacks="true" AllowSorting="true" OnRowCommand="grdAlumnosRotter_RowCommand"
                    OnSorting="grdAlumnosRotter_Sorting" OnRowDataBound="grdAlumnosRotter_RowDataBound" OnPageIndexChanging="grdAlumnosRotter_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="NombreAlumno" HeaderText="Estudiante" />
                        <asp:BoundField DataField="FechaInicio" HeaderText="Fecha inicio" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hidden-xs hidden-sm"
                            ItemStyle-CssClass="hidden-sm hidden-xs" />
                        <asp:BoundField DataField="FechaFin" HeaderText="Fecha fin" HeaderStyle-CssClass="hidden-xs hidden-sm" ItemStyle-CssClass="hidden-xs hidden-sm" />
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="lnkCompletar" runat="server" CommandName="completar" CommandArgument='<%#Eval("AlumnoID") %>'
                                    Width="25px" Height="25px" ImageUrl="~/Images/icons/VOCAREER_buscar.png" ToolTip="Completar prueba Rotter" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="">
                            <p>
                                <span class="ui-icon ui-icon-info"></span>La búsqueda no produjo resultados
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
