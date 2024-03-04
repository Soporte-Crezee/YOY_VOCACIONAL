<%@ Page Language="C#" MasterPageFile="~/PaquetesPremium/Paquete.Master" AutoEventWireup="true" CodeBehind="AdquirirPaquete.aspx.cs" Inherits="POV.Web.Administracion.PaquetesPremium.AdquirirPaquete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .box-shadow {
            box-shadow: 0px 2px 9px 0px rgba(0, 0, 0, 0.35);
        }

            .box-shadow:hover {
                background-color: #fff4d2;
            }
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="container" class="page_container_login">
        <!-- Content -->
        <div class="col-xs-12 col-md-3"></div>
        <%-- begin columna izq --%>
        <div class="col-xs-12">
            <div class="col-xs-12 col-md-2"></div>
            <div class="col-xs-12 col-md-8">
                <div class="panel panel-default box-shadow">
                    <div class="panel-body card">
                        <div class="col-xs-12">
                            <div class="col-xs-12 col-sm-12">
                                <img alt="Adquirir" src="../images/Premium1.png" class="img-responsive" width="100%" />
                            </div>
                            <div class="col-xs-12 col-md-12"></div>
                            <div class="col-xs-12">
                                <br />
                                <hr />
                                <asp:ImageButton ID="ComprarPaquetePremium" CssClass="pull-right img-responsive" runat="server" ImageUrl="~/images/btn_paypal.gif"
                                    Width="145" AlternateText="Pagar con paypal"
                                    BackColor="Transparent" BorderWidth="0" OnClick="ComprarPaquete_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xs-12 col-md-2"></div>
        </div>
        <div class="col-xs-12 col-md-3"></div>
        <%-- end columna izq --%>
        <div class="clear"></div>
    </div>
</asp:Content>
