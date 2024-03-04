<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DetalleExpedienteAlumnoUniversidad.aspx.cs" Inherits="POV.Web.PortalUniversidad.Pages.DetalleExpedienteAlumnoUniversidad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .selector_view {
            display: block;
            text-decoration: none;
        }

        tr.tab_selector {
            font-size: 14px;
            font-weight: bold;
        }

            tr.tab_selector td {
                padding: 5px 0 5px 8px;
                margin: 0;
                border: 1px solid #ccc;
                color: #aaa;
                width: 16%;
            }

                tr.tab_selector td:hover {
                    font-size: 14px;
                    font-weight: bold;
                    background: #f1f1f1;
                }

                tr.tab_selector td.seleccionado {
                    background-color: #0067ac;
                    color: #fff !important;
                }

                    tr.tab_selector td.seleccionado a {
                        color: #fff !important;
                    }

        .divContenido {
            background-color: #F3F3F5;
            width: 97%;
            margin: 0 auto;
        }

        tr.tab {
            height: auto;
            width: 200px;
            font-size: 14px;
            font-weight: bold;
            border: 1px solid #ccc;
            color: #aaa;
            margin: 0;
            padding: 5px 0 5px 8px;
        }

        td.tabtr {
            padding: 5px 0 5px 8px;
            margin: 0;
            border: 1px solid #ccc;
            color: #aaa;
            width: 16%;
            height: auto;
            width: 200px;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.tab-item').css('cursor', 'pointer');
        });
        $(function () {
            $('#mytabs a').click(function (e) {
                e.preventDefault()
                $(this).tab('show')
            })
        });
        $(function () {


            $.blockUI.defaults.overlayCSS.backgroundColor = "white";
            $.blockUI.defaults.message = '<h1 style="font-size:20px;">Registrando, por favor espere...</h1>';

            $('.tab-item').click(function () {
                window.location = $(this).find("a").first().attr("href");
            });
        });

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ol class="breadcrumb">
      <li>
          <asp:HyperLink runat="server" ID="HplVolver" ToolTip="Volver" NavigateUrl="~/Pages/ExpedienteAlumnoUniversidad.aspx" style="font-size:30px !important;" CssClass="">Volver</asp:HyperLink>
      </li>
      <li style="font-size:30px !important;">Ver expediente</li>
    </ol>
    <div class="">
        <asp:Label ID="lblError" runat="server" CssClass="error_label"></asp:Label>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Informaci&oacute;n del estudiante
        </div>
        <div class="panel-body">
            <div>
                <ul class="nav nav-tabs" role="tablist" id="mytabs">
                    <li role="presentation" class="active">
                        <a href="#InfoPersonal" aria-controls="InfoPersonal" role="tab" data-toggle="tab">Informaci&oacute;n personal</a>
                    </li>
                    <li role="presentation">
                        <a href="#HistorialServicios" aria-controls="HistorialServicios" role="tab" data-toggle="tab">Historial de servicios</a>
                    </li>
                </ul>
            </div>
            <div class="tab-content">
                <div role="tabpanel" class="tab-pane active" id="InfoPersonal">      
                    <div class="form-horizontal" style="padding-top:20px;">
                        <!-- B Procedencia -->
                        <div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label">Estado</label>
                                <div class="col-sm-4">
                                    <asp:DropDownList ID="ddlEstado" CssClass="form-control" runat="server" Enabled="false" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                </div>
                                <label class="col-sm-2 control-label">Municipio</label>
                                <div class="col-sm-4">
                                    <asp:DropDownList ID="ddlMunicipio" CssClass="form-control" runat="server" Enabled="false" AutoPostBack="true"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-2 control-label">Pa&iacute;s</label>
                                <div class="col-sm-4">
                                    <asp:DropDownList ID="ddlPais" CssClass="form-control" runat="server" Enabled="false" OnSelectedIndexChanged="ddlPais_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div>
                            <div class="form-group">
                                <!-- B1 Nombre -->
                                <label class="col-sm-2 control-label">Nombre</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtNombre" MaxLength="30" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <!-- B2 Fecha de Nacimiento -->
                                <label class="col-sm-2 control-label">Fecha de nacimiento</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtFechNacimiento" MaxLength="30" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>  
                            </div>           
                            <div class="form-group">
                                <!-- B3 Nivel de estudio -->
                                <label class="col-sm-2 control-label">Nivel de estudio</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtNivEstudio" MaxLength="30" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <!-- B4 Escuela de Procedencia -->
                                <label class="col-sm-2 control-label">Escuela de procedencia</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtEscuela" MaxLength="30" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>               
                            <div class="form-group">
                                <!-- B5 Telefono de Casa -->
                                <label class="col-sm-2 control-label">Tel&eacute;fono de casa</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtTelCasa" MaxLength="30" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <!-- B5 Telefono de Referencia -->
                                <label class="col-sm-2 control-label">Tel&eacute;fono de referencia</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtTelReferencia" MaxLength="30" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>         
                            <div class="form-group">
                                <!-- B6 Email -->
                                <label class="col-sm-2 control-label">Correo electr&oacute;nico</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtEmail" MaxLength="30" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>                 
                            <!-- B7 Areas d concomiento -->
                            <div class="form-group">
                                <div class="text-center">
                                    <label class="label-header">&Aacute;reas de conocimiento</label>
                                </div>
                                <div class="table-responsive col-md-offset-2 col-md-8 col-lg-offset-2 col-lg-8">
                                    <asp:GridView AutoGenerateColumns="false" runat="server" ID="gvAreasConocimiento" CssClass="table table-bordered table-striped"
                                        RowStyle-CssClass="td" HeaderStyle-CssClass="th">
                                        <Columns>
                                            <asp:BoundField HeaderText="&Aacute;reas de conocimiento del estudiante" DataField="Nombre">
                                                <HeaderStyle Width="540px" HorizontalAlign="Left" />
                                                <ItemStyle Width="540px" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div role="tabpanel" class="tab-pane" id="HistorialServicios">
                    <div class="form-horizontal" style="padding-top: 20px;">
                        <!-- A1 Intereses -->
                        <div class="form-group">
                            <div class="text-center">
                                <label class="label-header">Intereses</label>
                            </div>
                            <div class="table-responsive col-md-8 col-md-offset-2 col-lg-8 col-lg-offset-2">
                                <asp:GridView AutoGenerateColumns="false" runat="server" 
                                    ID="gvIntereses" CssClass="table table-bordered table-striped"
                                    RowStyle-CssClass="td" HeaderStyle-CssClass="th">
                                    <Columns>
                                        <asp:BoundField HeaderText="Intereses del estudiante" DataField="NombreInteres">
                                            <HeaderStyle Width="540px" HorizontalAlign="Left" />
                                            <ItemStyle Width="540px" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <!-- A2 Pruebas realizadas -->
                        <div class="form-group">
                            <div class="text-center">
                                    <label class="label-header">Pruebas realizadas</label>
                                    <label class="label-header" runat="server" id="sinPruebas">Sin pruebas realizadas</label>
                            </div>
                            <div class="table-responsive col-md-8 col-md-offset-2 col-lg-8 col-lg-offset-2">
                                <!-- PRUEBAS GRATIS REALIZADAS-->
                                <asp:GridView AutoGenerateColumns="false" runat="server" 
                                    ID="gvPruebasGratis" CssClass="table table-bordered table-striped"
                                    RowStyle-CssClass="td" HeaderStyle-CssClass="th">
                                    <Columns>
                                        <asp:BoundField HeaderText="Pruebas" DataField="Nombre">
                                            <HeaderStyle Width="540px" HorizontalAlign="Left" />
                                            <ItemStyle Width="540px" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <!-- A4 Orientador -->
                        <div class="form-group">
                            <div class="">
                                <div class="col-md-8 col-md-offset-2 col-lg-8 col-lg-offset-2">
                                    <label class="col-sm-2 control-label">Orientador</label>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="txtOrientador" MaxLength="30" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:HiddenField runat="server" ID="hdnDialogResultado" />
        </div>
    </div>
    <div class="">
        <!-- Pestañas -->
        <div class="" runat="server" visible="false" id="divContenido">
        </div>
    </div>
    <script type="text/javascript">
        function CerrarDialogo() {
            $('#MainContent_hdnDialogResultado').val('');
        }
    </script>
</asp:Content>

