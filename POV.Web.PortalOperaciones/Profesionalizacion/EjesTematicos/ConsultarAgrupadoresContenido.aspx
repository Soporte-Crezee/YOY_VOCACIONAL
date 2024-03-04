<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ConsultarAgrupadoresContenido.aspx.cs" Inherits="POV.Web.PortalOperaciones.Profesionalizacion.EjesTematicos.ConsultarAgrupadoresContenido" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet"
        type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <style type="text/css">
        table.DDGridView .th, table.DDGridView .td, table.DDListView .th, table.DDListView .td
        {
            white-space: pre-line;
        }
        .wrap
        {
            white-space: nowrap;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        <asp:LinkButton ID="lnkBack" runat="server" OnClick="BtnCancelar_OnClick">Volver</asp:LinkButton>/Consultar
        contenidos</h3>
    <div class="main_div ui-widget-content" style="padding: 15px; min-height: 500px;">
        <table cellspacing="5px">
            <tr>
                <td class="td-label">
                    <label>
                        <asp:Label runat="server" ID="lblNivelEducativo" Text="Nivel educativo"></asp:Label>
                    </label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtNivelEducativo" ReadOnly="True" Enabled="False"
                        TabIndex="1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        <asp:Label runat="server" ID="lblGrado" Text="Grado"></asp:Label>
                    </label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtGrado" ReadOnly="True" Enabled="False"
                        TabIndex="2"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        <asp:Label runat="server" ID="lblAsignatura" Text="Asignatura"></asp:Label>
                    </label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtAsignatura" ReadOnly="True" Enabled="False"
                        TabIndex="3"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        <asp:Label runat="server" ID="lblBloque" Text="Bloque"></asp:Label>
                    </label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtBloque" ReadOnly="True" Enabled="False"
                        TabIndex="4"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label" style="vertical-align: top">
                    Eje o &aacute;mbito
                </td>
                <td>
                    <asp:TextBox ID="txtNombreEjeTematico" runat="server" Enabled="false" ReadOnly="true"
                        TextMode="MultiLine" Rows="2" Width="400px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label" style="vertical-align: top">
                    Tema
                </td>
                <td>
                    <asp:TextBox ID="txtNombreSituacion" runat="server" Enabled="false" ReadOnly="true"
                        TextMode="MultiLine" Rows="2" Width="400px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <div style="margin: 10px auto;">
            <h2>
                Contenidos</h2>
            <div class="line">
            </div>
            <br />
            <asp:UpdatePanel ID="updAgrupadores" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="grdAgrupadores" runat="server" CssClass="DDGridView" HeaderStyle-CssClass="th"
                        AutoGenerateColumns="false" PageSize="10" AllowPaging="true" Width="900" EnableSortingAndPagingCallbacks="True"
                        OnRowCommand="grdAgrupadores_RowCommand" OnSorting="grdAgrupadores_Sorting" AllowSorting="true">
                        <Columns>
                            <%--<asp:BoundField DataField="AgrupadorContenidoDigitalID" HeaderText="ID" SortExpression="AgrupadorContenidoDigitalID" />--%>
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" HeaderStyle-Width="50%"
                                ItemStyle-CssClass="break_text" />
                            <asp:BoundField DataField="Predeterminado" HeaderText="Predeterminado" SortExpression="Predeterminado"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="Configurar" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LnkBtnConfigurar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "AgrupadorContenidoDigitalID")%>'
                                        CommandName="config" runat="server">
                                            <i class="icon icon_pagegear"></i><span>Configurar recursos didácticos</span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="ui-state-highlight ui-corner-all">
                                <p>
                                    <span class="ui-icon ui-icon-info" style="float: left"></span>La búsqueda no produjo
                                    resultados</p>
                            </div>
                        </EmptyDataTemplate>
                        <PagerTemplate>
                            <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="dtAgrupadoresSituacion"
                                DataSourceType="DataTable" />
                        </PagerTemplate>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            <%-- end resultado consulta contenidos --%>
        </div>
    </div>
</asp:Content>
