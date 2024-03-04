<%@ Page Title="" Language="C#" MasterPageFile="~/ComprasPortal/Compra.Master" AutoEventWireup="true" CodeBehind="CheckoutError.aspx.cs" Inherits="POV.Checkout.CheckoutError" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p></p>
    <div class="col-xs-12">
        <div class="col-xs-12 col-md-2"></div>
        <div class="col-xs-12 col-md-8">
            <div class="panel panel-default box-shadow error error_compra">
                <div class="panel-body card">
                    <div class="col-xs-12">
                        <div class="col-xs-12 col-sm-12 tabla_titulo_marco_general">
                            <h1>Error en la compra del paquete premium</h1>
                        </div>
                        <div class="col-xs-12 ui-widget-content">
                            <div class="col-xs-12">
                                <div class="col-xs-12 form-group">
                                    <label class="col-sm-10 control-label error"><%=Request.QueryString.Get("ErrorCode")%></label>
                                    <div class="col-sm-2">
                                    </div>
                                </div>
                                <div class="col-xs-12 form-group">
                                    <label class="col-sm-10 control-label error"><%=Request.QueryString.Get("Desc")%></label>
                                    <div class="col-sm-2">
                                    </div>
                                </div>
                                <div class="col-xs-12 form-group">
                                    <label class="col-sm-10 control-label error"><%=Request.QueryString.Get("Desc2")%></label>
                                    <div class="col-sm-2">
                                    </div>
                                </div>
                                <div class="col-xs-12 form-group">
                                    <label class="col-sm-10 control-label error"><%=Request.QueryString.Get("ErrorEx")%></label>
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
