<%@ Page Title="" Language="C#" MasterPageFile="~/Content/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="POV.Web.HerramientasDocente.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #lista_menu_contenedor li {
            float: left;
            display: block;
            width: 45%;
            min-height: 100px;
        }

        .box-shadow {
            box-shadow: 0px 2px 4px 0px rgba(0, 0, 0, 0.35);
        }

            .box-shadow:hover {
                background-color: #f1f1f1;
            }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-lg-12">
        <h3 class="titulo_marco">Herramientas del orientador</h3>
    </div>
    <!-- ui-widget-header -->
    <div class="menu-default " style="min-height: 300px;">
        <h1 class="title_6"></h1>
        <div class="clearfix"></div>
        <div class="row" style="margin-top: 20px">
            <div class="col-xs-12 col-md-4 col-lg-4">
                <a href="~/Pages/Actividades/MantenerActividadesUI.aspx" id="A1" runat="server" visible="true">
                    <div class="panel panel-default">
                        <div class="panel-body card box-shadow">
                            <div class="row">
                                <div class="col-xs-5 col-md-5 col-sm-4">
                                    <img alt="Asignar Actividades" src="Content/images/VOCAREER_actividades.png" class="img-responsive" />
                                </div>
                                <div class="col-xs-7 col-md-7 col-sm-8 leftpadding" style="padding-top: 5%">
                                    <h3 style="margin-top: 15px">ACTIVIDADES</h3>
                                </div>
                            </div>
                        </div>
                    </div>
                </a>
            </div>
            <div class="col-xs-12 col-md-4 col-lg-4">
                <a href="~/Pages/Actividades/ConsultarAlumnosUI.aspx" id="A6" runat="server" visible="true">
                    <div class="panel panel-default">
                        <div class="panel-body card box-shadow">
                            <div class="row">
                                <div class="col-xs-5 col-md-5  col-sm-4">
                                    <img alt="Asignar Actividades" src="Content/images/VOCAREER_asignar.png" class="img-responsive" />
                                </div>
                                <div class="col-xs-7 col-md-7 col-sm-8 leftpadding" style="padding-top: 5%">
                                    <h3 style="margin-top: 15px">ASIGNAR</h3>

                                </div>
                            </div>
                        </div>
                    </div>
                </a>
            </div>
            <div class="col-xs-12 col-md-4 col-lg-4">
                <a id="A5" href="~/Pages/Actividades/MantenerAsignacionesUI.aspx" visible="true" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-body card box-shadow">
                            <div class="row">
                                <div class="col-xs-5 col-md-5  col-sm-4">
                                    <img alt="Desasignar Actividades" src="Content/images/VOCAREER_desasignar.png" class="img-responsive" />
                                </div>
                                <div class="col-xs-7 col-md-7 col-sm-8 leftpadding" style="padding-top: 5%">
                                    <h3 style="margin-top: 15px">DESASIGNAR</h3>
                                </div>
                            </div>
                        </div>
                    </div>
                </a>
            </div>
        </div>

        <div>
            <div class="clear"></div>
        </div>
        <ul>
        </ul>
    </div>
</asp:Content>
