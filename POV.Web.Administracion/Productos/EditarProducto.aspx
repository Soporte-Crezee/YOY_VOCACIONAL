<%@ Page Title="" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="EditarProducto.aspx.cs" Inherits="POV.Web.Administracion.Productos.EditarProducto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/messages_es.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/json.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api/api.dialogs.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api/api.shared.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>validator.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.textareaCounter.plugin.js" type="text/javascript"></script>

    <style type="text/css">
        .textarea_no_resize {
            resize: none;
            width: 550px;
            height: 200px;
            font-family:helvetica;
            font-size:12pt;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(initPage);

        function initPage()
        {
            $(".boton").button();
           
            validateCostoProducto();

            var options = {
                'maxCharacterSize': 500,
                'originalStyle': 'display_info_textarea',
                'warningStyle': 'display_warning_textarea',
                'warningNumber': 480,
                'displayFormat': '#left caracteres restantes de #max max.'
            };
            $('#<%=txtDescripcion.ClientID%>').textareaCount(options);
        }

        function validateCostoProducto()
        {
            var rules =
                {
                    <%=txtNombre.UniqueID %>:{required:true, maxlength:250},
                    <%=txtDescripcion.UniqueID %>:{required:true, maxlength:500},
                    <%=txtPrecio.UniqueID %>:{required:true, maxlength:15},
                    <%=DDLTipoProducto.UniqueID %>:{required:true, maxlength:1}
                };

            jQuery.extend(jQuery.validator.messages,
                {
                    required: jQuery.validator.format("Este campo es obligatorio")
                });

            $('#frmMain').validate(
                {
                    rules: rules,
                    submitHandler: function(form)
                    {
                        DocumentBlockUI();
                        form.submit();
                    }
                }
                );

            $("#<%=txtPrecio.ClientID%>").on("keypress", function (e) {
                var keynum = window.event ? window.event.keyCode : e.which;
                if ((keynum == 8) || (keynum == 46))
                    return true;
                return /\d/.test(String.fromCharCode(keynum));
            });
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        <asp:HyperLink runat="server" ID="lnkBack" NavigateUrl="~/Productos/BuscarProductos.aspx">
            Volver
        </asp:HyperLink>
        /
        <asp:Label ID="lblAccion" runat="server" Text="Editar"></asp:Label>
        producto
    </h3>
    <div class="ui-widget-content" style="padding: 5px">
        <br />
        <h2>
            <asp:Label runat="server" ID="lblSubtitulo" Text="Informaci&oacute;n general"></asp:Label>
        </h2>
        <hr />
        <asp:UpdatePanel ID="UpdCreate" runat="server">
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td class="td-label">
                            <asp:Label runat="server" ID="lblNombre" Text="Nombre" ToolTip="Nombre producto"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtNombre" TabIndex="1" MaxLength="250"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-label">
                            <asp:Label runat="server" ID="lblTipoProducto" Text="Tipo producto" ToolTip="Tipo de producto"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDLTipoProducto" runat="server" TabIndex="2">
                                <asp:ListItem Text="SELECCIONE..." Value=""></asp:ListItem>
                                <asp:ListItem Text="Expediente" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Prueba" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Horas orientador" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>

                    <tr>
                        <td class="td-label">
                            <asp:Label runat="server" ID="lblPruebas" Text="Prueba" ToolTip="Prueba"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPruebas" runat="server" TabIndex="3">
                            </asp:DropDownList>
                        </td>
                    </tr>

                    <tr>
                        <td class="td-label">
                            <asp:Label runat="server" ID="lblPrecio" Text="Precio" ToolTip="Precio producto"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPrecio" TabIndex="4" MaxLength="15"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-label">
                            <asp:Label runat="server" ID="lblDescripcion" Text="Descripci&oacute;n" ToolTip="Descripci&oacute;n producto"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtDescripcion" TabIndex="5" MaxLength="500" TextMode="MultiLine" CssClass="textarea_no_resize"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="line"></div>

        <asp:HiddenField ID="hdnFechaInicio" runat="server" />
        <table>
            <tr>
                <td class="td-label"></td><td class="td-label"></td><td class="td-label"></td>
                <td class="td-label" style="text-align:right">
                    <asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="boton" OnClick="btnGuardar_Click" />                    
                </td>
                <td class="td-label" style="text-align:left">
                    <asp:HyperLink ID="HpLnkCancelar" Text="Cancelar" runat="server" NavigateUrl="~/Productos/BuscarProductos.aspx" CssClass="btn-cancel"></asp:HyperLink>
                </td>
                <td class="td-label"></td><td class="td-label"></td><td class="td-label"></td>
            </tr>
        </table>

        <script type="text/javascript">
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_pageLoaded(loadControls);
            prm.add_endRequest(endRequests);

            function loadControls(sender, args)
            {
                var ddate = new Date();
                var maxddate = new Date(ddate.getYear() -17, -1, 1);
                $(".boton").button();
                
                validateCostoProducto();
            }

            function endRequests(sender, args){
            }
        </script>
        <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server" style="display: none;" />
    </div>
</asp:Content>
