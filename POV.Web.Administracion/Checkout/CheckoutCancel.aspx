<%@ Page Title="" Language="C#" MasterPageFile="~/ComprasPortal/Compra.Master" AutoEventWireup="true" CodeBehind="CheckoutCancel.aspx.cs" Inherits="POV.Checkout.CheckoutCancel" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p></p>
    <div class="col-xs-12">
        <div class="col-xs-12 col-md-2"></div>
        <div class="col-xs-12 col-md-8">
            <div class="panel panel-default box-shadow verificar_compra">
                <div class="panel-body card">
                    <div class="col-xs-12">
                        <div class="col-xs-12 col-sm-12 tabla_titulo_marco_general">
                            <h1>Compra cancelada</h1>
                        </div>
                        <div class="col-xs-12 ui-widget-content">
                            <div class="col-xs-12">
                                <div class="col-xs-12 form-group">
                                    <h2><label class="col-sm-10 control-label"><br />Su compra ha sido cancelada.</label></h2>
                                    <div class="col-sm-2">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12" style="padding-top:2em;"></div>
                        <div class="col-sm-offset-4">
                            <asp:Button ID="Continue" runat="server" CssClass="btn btn-entrar btn-md" Text="Regresar al portal" OnClick="Continue_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-2"></div>
    </div>
    <p></p>
</asp:Content>
