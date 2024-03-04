<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="POV.Web.PortalUniversidad.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .box-shadow {
            box-shadow: 0px 2px 4px 0px rgba(0, 0, 0, 0.35);
        }
            .box-shadow:hover {
                background-color: #f1f1f1;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="">
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-4" id="LeftColumn" runat="server" visible="false">
                <a runat="server" id="menu_OrientadoresCatalogo" href="~/Orientadores/BuscarOrientadores.aspx">
                    <div class="panel panel-default box-shadow" id="opcion_Orientadores" runat="server">
                        <div class="panel-body card">
                            <div class="">
                                <div class="col-xs-4 col-sm-4">
                                    <img alt="Orientadores" src="images/VOCAREER_orientadores.png" width="50" />
                                </div>
                                <div class="col-xs-8 col-sm-8">
                                    <h3 class="menu_default_opcion" style="font-size: 20px;">Orientadores</h3>
                                </div>
                            </div>
                        </div>
                    </div>
                </a>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4" id="CenterColumn" runat="server" visible="false">
                <a runat="server" id="menu_EventosCatalogo" href="~/Eventos/BuscarEventos.aspx">
                    <div class="panel panel-default box-shadow" id="opcion_Eventos" runat="server">
                        <div class="panel-body card">
                            <div class="">
                                <div class="col-xs-4 col-sm-4">
                                    <img alt="Eventos" src="images/VOCAREER_eventos.png" width="50" />
                                </div>
                                <div class="col-xs-8 col-sm-8">
                                    <h3 class="menu_default_opcion" style="font-size: 20px;">Eventos</h3>
                                </div>
                            </div>
                        </div>
                    </div>
                </a>
            </div>

            <div class="col-xs-12 col-sm-6 col-md-4" id="RightColumn" runat="server" visible="false">
                <a runat="server" id="menu_ExpedientesCatalogo" href="~/Pages/ExpedienteAlumnoUniversidad.aspx">
                    <div class="panel panel-default box-shadow" id="opcion_Expedientes" runat="server">
                        <div class="panel-body card">
                            <div class="">
                                <div class="">
                                    <div class="col-xs-4 col-sm-4">
                                        <img alt="Expedientes" src="images/VOCAREER_expedientes.png" width="50" />
                                    </div>
                                    <div class="col-xs-8 col-sm-8">
                                        <h3 class="menu_default_opcion" style="font-size: 20px;">Expedientes</h3>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </a>
            </div>
        </div>
    </div>
    <div class="clear"></div>
</asp:Content>
