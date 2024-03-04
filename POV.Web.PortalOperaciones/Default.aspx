<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="POV.Web.PortalOperaciones.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #lista_menu_contenedor li {
            float: left;
            display: block;
            width: 45%;
            min-height: 100px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- ui-widget-header -->
    <div class="bodyadaptable menu-default " style="padding: 10px;">
        <h1 class="title_6"></h1>
        <div>
            <%-- begin columna izq --%>
            <div class="row">
                <div class="col-xs-6 col-md-4 item_pub car">
                    <a href="~/CatalogoReactivos/Dinamico/BuscarReactivos.aspx" id="A1" runat="server" visible="true">
                        <div class="panel panel-default">
                            <div class="panel-body card">
                                <div class="row">
                                    <div class="col-xs-8 col-sm-4" style="padding: 20px">
                                        <img alt="Reactivos" src="Images/VOCAREER_reactivos.png" />
                                    </div>
                                    <div class="col-xs-8 col-sm-8">
                                        <h3 class="opcion-default">Reactivos
                                        </h3>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="col-xs-6 col-md-4">
                    <div class="panel panel-default">
                        <div class="panel-body card">
                            <div class="row">
                                <div class="col-xs-8 col-sm-4" style="padding: 20px">
                                    <img alt="Pruebas" src="Images/VOCAREER_pruebas.png" />
                                </div>
                                <div class="col-xs-8 col-sm-8">
                                    <h3 class="opcion-default">Pruebas	</h3>
                                    <a href="~/CatalogoModeloPrueba/BuscarModeloPrueba.aspx" id="A2" runat="server" visible="true">Configurar modelo de pruebas</a><br />
                                    <a href="~/Pruebas/BuscarPruebas.aspx" id="A3" runat="server" visible="true">Pruebas</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xs-6 col-md-4">
                    <div class="panel panel-default">
                        <div class="panel-body card">
                            <div class="row">
                                <div class="col-xs-8 col-sm-4" style="padding: 20px">
                                    <img alt="Configuraci&oacute;n" src="Images/VOCAREER_configuracion.png" />
                                </div>
                                <div class="col-xs-8 col-sm-8">
                                    <h3 class="opcion-default">Configuraci&oacute;n</h3>
                                    <a id="A4" href="~/paises/buscarpaises.aspx" visible="true" runat="server">Paises</a><br />
                                    <a id="A5" href="~/estados/buscarestado.aspx" runat="server" visible="true">Estados</a><br />
                                    <a href="~/ciudades/buscarciudades.aspx" id="A6" runat="server" visible="true">Ciudades</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xs-6 col-md-4">
                    <div class="panel panel-default">
                        <div class="panel-body card">
                            <div class="row">
                                <div class="col-xs-8 col-sm-4" style="padding: 20px">
                                    <img alt="Datos" src="Images/VOCAREER_configuracion.png" />
                                </div>
                                <div class="col-xs-8 col-sm-8">
                                    <h3 class="opcion-default">Carga de datos</h3>
                                    <a id="A8" href="~/Orientadores/RegistrarOrientador.aspx" visible="true" runat="server">Carga de orientadores</a><br />
                                    <a id="A9" href="~/Cargador/CArgaTutores.aspx" visible="true" runat="server">Carga de padres</a><br />
                                    <a id="A7" href="~/Alumnos/RegistrarAlumno.aspx" visible="true" runat="server">Carga de alumnos</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-body card">
                    <div class="col-xs-12">
                        <br />
                        <div class="col-md-8 col-md-offset-2">
                            <div class="row">
                                <div class="col-xs-3 col-sm-3">
                                    <div class="text-center">
                                        <img alt="Configuraci&oacute;n" src="Images/VOCAREER_paso1.png" />
                                        <h2 class="label_paso">Paso 1</h2>
                                        <p class="label_paso_instruccion">Crea el tipo de prueba a realizar</p>
                                    </div>
                                </div>
                                <div class="col-xs-1 col-sm-1">
                                    <div class="text-center" style="margin: 60px 0px 0px 0px;">
                                        <img alt="Configuraci&oacute;n" src="Images/VOCAREER_flecha.png" width="50px;" />
                                    </div>
                                </div>
                                <div class="col-xs-3 col-sm-3">
                                    <div class="text-center">
                                        <img alt="Reactivos" src="Images/VOCAREER_paso2.png" />
                                        <h2 class="label_paso">Paso 2</h2>
                                        <p class="label_paso_instruccion">Genera los reactivos a utilizar en las pruebas</p>
                                    </div>
                                </div>
                                <div class="col-xs-1 col-sm-1">
                                    <div class="text-center" style="margin: 60px 0px 0px 0px;">
                                        <img alt="Configuraci&oacute;n" src="Images/VOCAREER_flecha.png" width="50px;" />
                                    </div>
                                </div>
                                <div class="col-xs-3 col-sm-3">
                                    <div class="text-center">
                                        <img alt="Pruebas" src="Images/VOCAREER_paso3.png" />
                                        <h2 class="label_paso">Paso 3</h2>
                                        <p class="label_paso_instruccion">Genera la prueba y selecciona los reactivos previamente diseñados</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <%-- end columna izq --%>
            <div class="clear"></div>
        </div>
        <ul>
        </ul>
    </div>
</asp:Content>
