<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="POV.Web.Administracion.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet"
        type="text/css" />
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
    <div class="bodyadaptable menu-default" style="padding: 10px;">
        <h1 class="title_6"></h1>
        <div>
            <div runat="server" id="DivMenuDirector">              
                <div class="col-xs-12">
                    <div class="col-xs-12 col-md-6" id="LeftColumn" runat="server" >
                        <a href="~/Docentes/BuscarDocentes.aspx" id="menu_DocentesCatalogo" runat="server" visible="false">
                            <div class="panel panel-default box-shadow" id="opcion_Docentes" runat="server">
                                <div class="panel-body card">
                                    <div class="col-xs-12">
                                        <div class="col-xs-4 col-sm-4">
                                            <img alt="Orientadores" class="img-responsive" src="images/VOCAREER_orientadores.png" width="100%" style="max-width: 100px" />
                                        </div>
                                        <div class="col-xs-8 col-sm-8">
                                            <h1 class="opcion_menu_default">ORIENTADORES
                                            </h1>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>
                    <div class="col-xs-12 col-md-6" id="RightColumn" runat="server">
                        <a href="~/Universidades/BuscarUniversidad.aspx" id="menu_UniversidadCatalogo" runat="server" visible="false">
                            <div class="panel panel-default box-shadow" id="opcion_Universidades" runat="server">
                                <div class="panel-body card">
                                    <div class="col-xs-12">
                                        <div class="col-xs-4 col-sm-4">
                                            <img alt="Universidades" class="img-responsive" src="images/VOCAREER_universidades.png" width="100%" style="max-width: 100px" />
                                        </div>
                                        <div class="col-xs-8 col-sm-8">
                                            <h1 class="opcion_menu_default">ESCUELAS</h1>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </div>
    </div>
</asp:Content>
