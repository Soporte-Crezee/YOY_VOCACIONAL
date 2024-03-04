<%@ Page Title="" MasterPageFile="~/ComprasPortal/Compra.Master" Language="C#" AutoEventWireup="true" CodeBehind="AdquirirCredito.aspx.cs" Inherits="POV.Web.Administracion.ComprasPortal.AdquirirCredito" %>

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

        function showMessage(text)
        {
            var api = new MessageApi();
            api.CreateMessage(text, "ERROR");
            api.Show();
            resetHiden();
        }

        $(document).ready(function () {
            setTimeout(function () {
                if ($("#<%=txtCantidadCreditos.ClientID%>").val() != '')
                    calcularTotal();
                $("#<%=CompraCredito.ClientID%>").attr("disabled", true);
            }, 400);

            calcularTotal = function () {
                var cantidad = $("#<%=txtCantidadCreditos.ClientID%>").val();
                var lblError = $("#<%=lblError.ClientID%>");

                var total = cantidad;
                

                $("#<%=txtCantidadCreditos.ClientID%>").removeClass('error');
                lblError.text("");
                lblError.hide();

                if (cantidad != '') {
                    if ((cantidad * 1) < 1) {
                        $("#<%=CompraCredito.ClientID%>").attr("disabled", true);
                        $("#<%=lblTotal.ClientID%>").text('0');
                        lblError.text("La cantidad no puede ser menor de 1.");
                        lblError.show();
                    }
                    else {
                        $("#<%=CompraCredito.ClientID%>").removeAttr('disabled');
                        $("#<%=lblTotal.ClientID%>").text(total);
                    }
                }
                else {
                    $("#<%=CompraCredito.ClientID%>").attr("disabled", true);
                    lblError.text("Este campo es requerido");
                    lblError.show();
                    $("#<%=txtCantidadCreditos.ClientID%>").addClass('error');                    
                }
            }

            $("#<%=txtCantidadCreditos.ClientID%>").on("keypress", function (e) {
                var keynum = window.Event ? e.which : e.keyCode;
                if ((keynum == 8) || (keynum == 46))
                    return true;
                return /\d/.test(String.fromCharCode(keynum));
            });

            $("#<%=txtCantidadCreditos.ClientID%>").on("change", function (e) {
                calcularTotal();
            });

            $("#<%=txtCantidadCreditos.ClientID%>").on("keyup", function (e) {
                calcularTotal();
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="info_perfil" class="col-md-12">
        <h1 class="tBienvenida">
            <asp:Label ID="lblNombreUsuario" runat="server" Text=""></asp:Label>
            <label>
                ► Compra de cr&eacute;dito
            </label>
        </h1>
        <div class="sunline_title"></div>
    </div>

    <div class="col-md-12" style="padding: 20px 0px 0px 0px" runat="server" id="DivTabla">
        <div class="col-xs-12 col-sm-3"></div>
        <div class="col-xs-12 col-sm-6">
            <div class="col-xs-12 panel panel-default box-shadow">
                <div class="panel-body card">
                    <asp:UpdatePanel runat="server" ID="UpdPCredito">
                        <ContentTemplate>
                            <div class="col-xs-12 form-group center-block">
                                <div class="table-responsive" id="tCredito" runat="server">
                                    <asp:Table ID="Table1" runat="server" CssClass="table table-bordered">
                                        <asp:TableRow>
                                            <asp:TableCell CssClass="ui-widget-content td col-xs-6 boldText">
                                                <asp:Label ID="lblCantidadCreditos" CssClass="label-control" Text="Cantidad de créditos: " runat="server"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell CssClass="ui-widget-content td col-xs-6">
                                                <asp:TextBox ID="txtCantidadCreditos" runat="server" style="min-width:80px" TextMode="Number" min="0" step="1" CssClass="form-control" placeholder="Introduzca cantidad"></asp:TextBox>
                                                <asp:Label ID="lblError" CssClass="label-control error_label" runat="server" style="display: none"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        
                                        <asp:TableRow>
                                            <asp:TableCell CssClass="ui-widget-content td col-xs-6 bolText">
                                                <asp:Label ID="lblTotalPagar" CssClass="label-control" Text="Total a pagar: " runat="server"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell CssClass="ui-widget-content col-xs-6">
                                                $
                                                <asp:Label ID="lblTotal" CssClass="label-control" runat="server" Text="0"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="col-xs-12">
                        <hr />
                        <asp:ImageButton ID="CompraCredito" CssClass="pull-right img-responsive" runat="server" ImageUrl="~/images/btn_paypal.gif" Width="145"
                            AlternateText="Pagar con paypal" BackColor="Transparent" BorderWidth="0" OnClick="CompraCredito_Click" />
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-3"></div>
    </div>
</asp:Content>
