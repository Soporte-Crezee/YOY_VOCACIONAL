﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EditarPlanEducativo.aspx.cs" Inherits="POV.Web.PortalOperaciones.CatalogoAsignaturas.EditarPlanEducativo" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet"
        type="text/css" />
    <link href="../Styles/Reactivos.css" rel="stylesheet" type="text/css" />
    <script src="<%=Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<%=Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            $(".boton").button({ icons: {
                primary: "ui-icon-search"
            }
            });

            $('#<%=TxtValidoDesde.ClientID %>').datepicker({ changeYear: true, changeMonth: true, dateFormat: "dd/mm/yy" });
            $('#<%=TxtValidoHasta.ClientID %>').datepicker({ changeYear: true, changeMonth: true, dateFormat: "dd/mm/yy" });
        }

        function ValidateAdd() {
            CancelValidate();
            $("#frmMain").validate({
                rules: {
                    '<%=TxtClaveAsignatura.UniqueID %>': {
                        required: true,
                        maxlength: 50
                    },
                    '<%=TxtTituloAsignatura.UniqueID %>': {
                        required: true,
                        maxlength: 50
                    },
                    '<%=cbGradoAsignatura.UniqueID %>': {
                        required: true
                    },
                    '<%=cbAreaConocimiento.UniqueID %>': {
                        required: true
                    }
                }
                , submitHandler: function (form) {
                    $(form).block();
                    Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(CheckValidationStatus);
                    form.submit();
                }
            });
        }

        function ValidateSave() {
            CancelValidate();
            $("#frmMain").validate({
                rules: {
                    '<%=TxtTitulo.UniqueID %>': {
                        required: true,
                        maxlength: 50
                    },
                    '<%=TxtDescripcion.UniqueID %>': {
                        required: true,
                        maxlength: 200
                    },
                    '<%=TxtValidoDesde.UniqueID %>': {
                        required: true
                    },
                    '<%=TxtValidoHasta.UniqueID %>': {
                        required: true
                    },
                    '<%=cbNivelEducativo.UniqueID %>': {
                        required: true
                    }
                }
                , submitHandler: function (form) {
                    $(form).block();
                    Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(CheckValidationStatus);
                    form.submit();
                }
            });
        }

        function CheckValidationStatus(sender, args) {
            //Check if the form is valid; this validation state is stored in a form hidden field. 
            if ($("#frmMain").val() == "false") {
                //if the form is not valid,  then cancel the async postback 
                args.set_cancel(true);
            }
        }

        function CancelValidate() {

            var form = $('#frmMain').get(0);
            $.removeData(form, 'validator');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        <asp:HyperLink ID="lnkBack" runat="server">Volver</asp:HyperLink>
        /Editar plan educativo</h3>
    <div class="main_div ui-widget-content">
        <h2>
            Informaci&oacute;n del plan educativo</h2>
        <hr />
        <table>
            <tr>
                <td class="td-label">
                    <label>
                        ID</label>
                </td>
                <td>
                    <asp:TextBox ID="TxtID" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        T&iacute;tulo del plan educativo</label>
                </td>
                <td>
                    <asp:TextBox ID="TxtTitulo" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label-texarea">
                    <label>
                        Descripci&oacute;n</label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="TxtDescripcion" Columns="75" Rows="4" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        V&aacute;lido desde</label>
                </td>
                <td>
                    <asp:TextBox ID="TxtValidoDesde" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        V&aacute;lido hasta
                    </label>
                </td>
                <td>
                    <asp:TextBox ID="TxtValidoHasta" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        Nivel educativo</label>
                </td>
                <td>
                    <asp:DropDownList ID="cbNivelEducativo" runat="server" AutoPostBack="true" CssClass="required"
                        OnSelectedIndexChanged="cbNivelEducativo_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        Estatus</label>
                </td>
                <td>
                    <asp:DropDownList ID="cbEstatus" runat="server">
                        <asp:ListItem Value="true" Text="Activo" Selected="true"></asp:ListItem>
                        <asp:ListItem Value="false" Text="Inactivo"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <br />
        <asp:Panel ID="pnlAsignaturas" runat="server" GroupingText="Lista de asignaturas del plan educativo">
            <table>
                <tr>
                    <td class="td-label">
                        <label>
                            Clave</label>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtClaveAsignatura" runat="server"></asp:TextBox></li>
                    </td>
                </tr>
                <tr>
                    <td class="td-label">
                        <label>
                            T&iacute;tulo</label>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtTituloAsignatura" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td-label">
                        <label>
                            Grado</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cbGradoAsignatura" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td-label">
                        <label>
                            &Aacute;rea de conocimiento</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cbAreaConocimiento" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="BtnAgregar" Text="Agregar" runat="server" CssClass="boton" OnClick="BtnAgregar_OnClick"
                            OnClientClick="ValidateAdd()" />
                    </td>
                </tr>
            </table>
            <br />
            <div>
                <asp:UpdatePanel ID="updProgramaEducativo" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdAsignaturas" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                            HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                            Width="100%" OnRowCommand="grdAsignaturas_RowCommand" OnRowDataBound="grdAsignaturas_DataBound"
                            OnSorting="grdAsignaturas_Sorting">
                            <Columns>
                                <asp:BoundField DataField="MateriaID" HeaderText="ID" SortExpression="MateriaID" />
                                <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" />
                                <asp:BoundField DataField="Titulo" HeaderText="Título" SortExpression="Titulo" />
                                <asp:BoundField DataField="Grado" HeaderText="Grado" SortExpression="Grado" />
                                <asp:BoundField DataField="AreaAplicacion" HeaderText="&Aacute;rea de conocimiento"
                                    SortExpression="AreaAplicacion" />
                                <asp:BoundField DataField="Estatus" HeaderText="Estado" SortExpression="Estatus" />
                                <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEdit" runat="server" CommandName="editar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "Clave")%>'
                                            ImageUrl="../images/edit-button.png" ToolTip="Editar" />
                                        <asp:ImageButton ID="btnDelete" runat="server" CommandName="eliminar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "Clave")%>'
                                            ImageUrl="../images/minus-button.png" ToolTip="Desactivar" Visible='<%# Boolean.Parse(Eval("Existente").ToString()) %>'
                                            OnClientClick="return confirm('¿Está seguro que desea desactivar éste elemento?');" />
                                        <asp:ImageButton ID="btnActivate" runat="server" CommandName="activar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "Clave")%>'
                                            ImageUrl="../images/plus-button.png" ToolTip="Activar" OnClientClick="return confirm('¿Está seguro que desea activar éste elemento?');"
                                            Visible='<%# !Boolean.Parse(Eval("Existente").ToString()) %>' />
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
                                <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="DtMaterias" DataSourceType="DataTable" />
                            </PagerTemplate>
                        </asp:GridView>
                        </div>
                        <div class="line">
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Button ID="BtnGuardar" Text="Guardar" runat="server" CssClass="boton" OnClick="BtnGuardar_OnClick"
                    OnClientClick="ValidateSave()" />
                <asp:Button ID="BtnCancelar" Text="Cancelar" runat="server" OnClick="BtnCancelar_OnClick"
                    CssClass="boton" OnClientClick="CancelValidate()" />
                <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
                    style="display: none;" />
            </div>
        </asp:Panel>
    </div>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(loadControls);

        function loadControls() {

            var ddate = new Date();
            var maxddate = new Date(ddate.getFullYear(), -1, 1);

            $('<%=TxtValidoDesde.ClientID %>').datepicker({ maxDate: maxddate, changeYear: true });
            $('<%=TxtValidoHasta.ClientID %>').datepicker({ maxDate: maxddate, changeYear: true });
        }
    </script>
</asp:Content>
