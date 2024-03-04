<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CheckoutComplete.aspx.cs" Inherits="POV.Web.PortalUniversidad.Checkout.CheckoutComplete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p></p>
    <div class="col-xs-12">
        <div class="col-xs-12 col-md-2"></div>
        <div class="col-xs-12 col-md-8">
            <div class="panel panel-default box-shadow compra_completa">
                <div class="panel-body card">
                    <div class="col-xs-12">
                        <div class="col-xs-12 col-sm-12 tabla_titulo_marco_general">
                            <h1>Compra completa</h1>
                        </div>
                        <div class="col-xs-12 ui-widget-content">
                            <br />
                            <p></p>
                            <div role="form" class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-md-4">
                                        <h3>
                                            <label class="control-label">Id. de transacci&oacute;n:</label></h3>
                                    </div>
                                    <div class="col-md-8">
                                        <h3>
                                            <asp:Label ID="TransactionId" CssClass="control-label boldText" runat="server"></asp:Label>
                                        </h3>
                                    </div>
                                </div>
                            </div>
                            <p></p>
                            <div id="divCompraExpedientes" runat="server">
                                <h3>Gracias por completar la compra, el siguiente paso ser&aacute; asignar los expedientes a sus orientadores.</h3>
                                <br />
                            </div>
                        </div>
                        <div class="col-xs-12" style="padding-top:2em;"></div>
                        <div class="col-sm-offset-4">                            
                            <p></p>
                            <hr />
                            <asp:Button ID="Continue" runat="server" Text="Aceptar" CssClass="btn-green btn-md" OnClick="Continue_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-2"></div>
    </div>
    <p></p>
</asp:Content>
