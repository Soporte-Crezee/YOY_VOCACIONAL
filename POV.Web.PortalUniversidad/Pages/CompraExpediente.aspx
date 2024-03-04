<%@ Page Title="" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="CompraExpediente.aspx.cs" Inherits="POV.Web.PortalUniversidad.Pages.CompraExpediente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .box-shadow {
            box-shadow: 0px 2px 9px 0px rgba(0, 0, 0, 0.35);
        }

            .box-shadow:hover {
                background-color: #fff4d2;
            }
    </style>

    <script type="text/javascript">

        function showMessage(text) {
            var api = new MessageApi();
            api.CreateMessage(text, "ERROR");
            api.Show();
            resetHiden();
        }

        $(document).ready(function () {

            setTimeout(function () {
                if ($("#<%=txtCantidadExpedintes.ClientID%>").val() != '')
                    calcularTotal();
            }, 400);

            calcularTotal = function () {
                var existencia = $("#<%=lblCantidad.ClientID%>").text();
                var precio = $("#<%=lblPrecio.ClientID%>").text()
                var cantidad = $("#<%=txtCantidadExpedintes.ClientID%>").val();
                var lblError = $("#<%=lblError.ClientID%>");

                var total = precio * cantidad;

                $("#<%=txtCantidadExpedintes.ClientID%>").removeClass('error');
                lblError.text("");
                lblError.hide();

                if (cantidad != '') {
                    if ((cantidad * 1) > (existencia * 1)) {
                        $("#<%=lblTotal.ClientID%>").text('0');
                        lblError.text("La cantidad no puede ser mayor a la cantidad de expedientes disponibles.");
                        lblError.show();
                    }
                    else {
                        if ((cantidad * 1) < 1) {
                            $("#<%=lblTotal.ClientID%>").text('0');
                            lblError.text("La cantidad no puede ser menor que 1.");
                            lblError.show();
                        }
                        else
                            $("#<%=lblTotal.ClientID%>").text(total);
                    }
                }
                else {
                    lblError.text("Este campo es requerido");
                    lblError.show();

                    $("#<%=txtCantidadExpedintes.ClientID%>").addClass('error');
                }
            }

            $("#<%=txtCantidadExpedintes.ClientID%>").on("keypress", function (e) {
                var keynum = window.event ? window.event.keyCode : e.which;
                if ((keynum == 8) || (keynum == 46))
                    return true;
                return /\d/.test(String.fromCharCode(keynum));
            });

            $("#<%=txtCantidadExpedintes.ClientID%>").on("change", function (e) {
                calcularTotal();
            });

            $("#<%=txtCantidadExpedintes.ClientID%>").on("keyup", function (e) {
                calcularTotal();
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <h1 class="tBienvenida">
            <asp:Label ID="LblNombreUsuario" runat="server" Text="" CssClass="tBienvenidaLabel-usuario"></asp:Label>
        </h1>
    </div>
    <div class="tFoto" style="margin-top: -15px">
        <label class="tBienvenidaLabel-titulo">
            ► Compra de expedientes
        </label>
    </div>

    <div class="col-xs-12 col-md-12">
        <div class="row">
            <div class="col-md-12">
                <asp:Label ID="Label1" runat="server" CssClass="error_label"></asp:Label>
            </div>
            <div class="col-md-12" style="padding: 0px 0px 0px 0px">
                <!-- Cabecera -->
                <div class="col-xs-12 titulo_marco_general">B&uacute;squeda de expedientes</div>
                <div class="col-xs-12 container_busqueda_general ui-widget-content">
                    <div class="col-xs-12 col-sm-6">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblSearchArea" Text="&Aacute;rea de conocimiento" ToolTip="Area conocmiento" class="col-sm-4 control-label"></asp:Label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlSearchAreasConocimiento" runat="server" CssClass="form-control"></asp:DropDownList>
                                <div class="help-block with-errors"></div>
                            </div>
                            <asp:Label runat="server" ID="lblSearchEscuela" Text="Escuela de procedencia" ToolTip="Escuela procedencia" class="col-sm-4 control-label"></asp:Label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtEscuelaSearch" MaxLength="50" runat="server" CssClass="form-control"></asp:TextBox>
                                <div class="help-block with-errors"></div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblSearchEstado" Text="Estado" ToolTip="Estado" class="col-sm-4 control-label"></asp:Label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlSearchEstado" runat="server" CssClass="form-control"></asp:DropDownList>
                                <div class="help-block with-errors"></div>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblSearchNivel" Text="Nivel de estudio" ToolTip="Nivel estudio" class="col-sm-4 control-label"></asp:Label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlSearchNivel" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text="Seleccionar"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="1 Semestre"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="2 Semestre"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="3 Semestre"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="4 Semestre"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="5 Semestre"></asp:ListItem>
                                    <asp:ListItem Value="6" Text="6 Semestre"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-sm-8">
                                <asp:Button ID="btnBuscarExpedientes" runat="server" Text="Buscar" OnClick="btnBuscarExpedientes_Click" CssClass="btn-green btn" />
                            </div>
                        </div>
                    </div>
                </div>

            </div>
            <div class="col-md-12" style="padding: 20px 0px 0px 0px" runat="server" id="DivInfoEncontrada" visible="false">
                <div class="col-xs-12 col-sm-3"></div>
                <div class="col-xs-12 col-sm-6">
                    <div class="col-xs-12 titulo_marco_general">Informaci&oacute;n encontrada</div>
                    <div class="col-xs-12 panel panel-default box-shadow">
                        <div class="panel-body card">
                            <asp:UpdatePanel runat="server" ID="UpdExpedientesAspirantes">
                                <ContentTemplate>
                                    <div class="col-xs-12 form-group center-block">
                                        <div class="table-responsive" id="tExpedientes" runat="server">
                                            <asp:Table ID="Table1" runat="server" CssClass="table table-bordered">
                                                <asp:TableRow>
                                                    <asp:TableCell CssClass="ui-widget-content td col-xs-6 boldText">
                                                        <asp:Label ID="lblNoExpedientes" CssClass="label-control" Text="Expedientes disponibles: " runat="server"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell CssClass="ui-widget-content td col-xs-6 boldText">
                                                        <asp:Label ID="lblCantidad" CssClass="label-control" runat="server"></asp:Label>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell CssClass="ui-widget-content td col-xs-6">
                                                        <asp:Label ID="lblPrecioExpediente" CssClass="label-control" Text="Precio por expediente: " runat="server"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell CssClass="ui-widget-content td col-xs-6">
                                                        $
                                                <asp:Label ID="lblPrecio" CssClass="label-control" runat="server"></asp:Label>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell CssClass="ui-widget-content td col-xs-6">
                                                        <asp:Label ID="lblCantidadExpedientes" CssClass="label-control" Text="Cantidad de expedientes: " runat="server"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell CssClass="ui-widget-content td col-xs-6">
                                                        <asp:TextBox runat="server" ID="txtCantidadExpedintes" Style="min-width: 80px" TextMode="Number" min="0" step="1" CssClass="form-control" placeholder="Introduzca cantidad"></asp:TextBox>
                                                        <asp:Label ID="lblError" CssClass="label-control error_label" runat="server" Text="" Style="display: none"></asp:Label>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell CssClass="ui-widget-content td col-xs-6 boldText">
                                                        <asp:Label ID="lblTotalPagar" CssClass="label-control" Text="Total a pagar: " runat="server"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell CssClass="ui-widget-content td col-xs-6 boldText">
                                                        $
                                                <asp:Label ID="lblTotal" CssClass="label-control" runat="server" Text="0"></asp:Label>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                        </div>
                                    </div>
                                    <div class="ui-state-highlight ui-corner-all col-xs-12" runat="server" id="EmptyDiv" visible="false">
                                        <p>
                                            <span class="ui-icon ui-icon-info" style="display: inline-block; vertical-align: middle; margin-top: 0px"></span>
                                            La b&uacute;squeda no produjo resultados
                                        </p>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="col-xs-12">
                                <hr />
                                <asp:ImageButton ID="ComprarPaquetePremium" CssClass="pull-right img-responsive" runat="server" ImageUrl="~/images/btn_paypal.gif"
                                    Width="145" AlternateText="Pagar con paypal"
                                    BackColor="Transparent" BorderWidth="0" OnClick="ComprarPaquete_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xs-12 col-md-3"></div>
            </div>

        </div>
    </div>

</asp:Content>
