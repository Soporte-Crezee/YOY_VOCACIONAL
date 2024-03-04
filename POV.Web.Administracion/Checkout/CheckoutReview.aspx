<%@ Page Title="" Language="C#" MasterPageFile="~/ComprasPortal/Compra.Master" AutoEventWireup="true" CodeBehind="CheckoutReview.aspx.cs" Inherits="POV.Checkout.CheckoutReview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p></p>
    <div class="col-xs-12">
        <div class="col-xs-12 col-md-2"></div>
        <div class="col-xs-12 col-md-8">
            <div class="panel panel-default box-shadow verificar_compra">
                <div class="panel-body card">
                    <div class="col-xs-12">
                        <div class="col-xs-12 col-sm-12 tabla_titulo_marco_general">
                            <h1>Detalles de compra de crédito</h1>
                            <hr />
                        </div>
                        <div class="col-xs-12 col-md-12 ui-widget-content">
                            <div class="col-xs-12 container_busqueda_general ">
                                <div class="col-md-12" style="padding: 20px 0px 0px 0px">
                                    <!-- details -->
                                    <div class="col-xs-12 form-group center-block">
                                        <div class="col-sm-12 table-responsive">

                                            <div class="col-md-12" style="padding: 20px 0px 0px 0px" runat="server" id="divCompraCreditos" visible="false">
                                                <div class="col-xs-12 titulo_marco_general">Verificar la informaci&oacute;n</div>
                                                <div class="col-xs-12 panel panel-default">
                                                    <div class="panel-body card">
                                                        <asp:UpdatePanel runat="server" ID="UpdExpedientesAspirantes">
                                                            <ContentTemplate>
                                                                <div class="col-xs-12 form-group center-block">
                                                                    <div class="table-responsive" id="tExpedientes" runat="server">


                                                                        <asp:Table ID="Table1" runat="server" CssClass="table table-bordered">
                                                                            
                                                                            <asp:TableRow>
                                                                                <asp:TableCell CssClass="ui-widget-content td col-xs-6">
                                                                                    <asp:Label ID="lblCantidadExpedientes" CssClass="label-control" Text="Cantidad de crédito: " runat="server"></asp:Label>
                                                                                </asp:TableCell>
                                                                                <asp:TableCell CssClass="ui-widget-content td col-xs-6">
                                                                                    <asp:Label ID="lblCantidad" CssClass="label-control" runat="server"></asp:Label>
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
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-md-12">
                        <br />
                        <div class="col-sm-offset-3">                            
                            
                            <asp:Button ID="CheckoutConfirm" runat="server" Text="Completar compra" CssClass="btn btn-entrar btn-md" OnClick="CheckoutConfirm_Click" />
                            <asp:Button ID="CheckoutCancel" runat="server" Text="Cancelar compra" CssClass="btn btn-cancel btn-md" OnClick="CheckoutCancel_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="col-xs-12 col-md-2"></div>
    </div>
    <p></p>
</asp:Content>
