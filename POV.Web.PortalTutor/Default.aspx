<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="POV.Web.PortalTutor.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .box-shadow {
            box-shadow: 0px 2px 4px 0px rgba(0, 0, 0, 0.35);
        }

            .box-shadow:hover {
                background-color: #f1f1f1;
            }
    </style>
    <script type="text/javascript">
        $(function () {
            $("#btnConfirmarCorreo").on("click", function () {
                __doPostBack('btnConfirmarCorreo', '');
            });           
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="page_content" runat="server">
        <!-- Modal -->
        <div style="visibility: hidden">
            <asp:TextBox ID="txtCorreoConfirmado" Style="margin-top: -35px;" MaxLength="30" runat="server" CssClass="form-control" Width="250px" Enabled="true"></asp:TextBox>
            <asp:TextBox ID="txtEstatusIdentificacion" Style="margin-top: -35px;" MaxLength="30" runat="server" CssClass="form-control" Width="250px" Enabled="true"></asp:TextBox>
        </div>
        <!-- ui-widget-header -->
        <%-- begin columna izq --%>
        <div class="">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <a id="A1" href="~/Pages/Tutorados.aspx" runat="server" visible="true" class="pconfirmacorreo pcompletaperfils">
                    <div class="panel panel-default box-shadow">
                        <div class="panel-body card">
                            <div>
                                <div class="col-xs-4 col-sm-4">
                                    <img alt="Invitación" class="img-responsive" src="images/VOCAREER_tutorados.png" width="50" />
                                </div>
                                <div class="col-xs-8 col-sm-8">
                                    <h3 class="menu_default_opcion" style="font-size:20px !important;">AGREGAR HIJOS
                                    </h3>
                                </div>
                            </div>
                        </div>
                    </div>
                </a>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <a href="~/Pages/ExpedienteAlumnoTutor.aspx" id="A2" runat="server" visible="true" class="pconfirmacorreo pcompletaperfils">
                    <div class="panel panel-default box-shadow">
                        <div class="panel-body card">
                            <div>
                                <div class="col-xs-4 col-sm-4">
                                    <img alt="Expedientes" src="images/VOCAREER_expedienteTutorado.png" width="50" />
                                </div>
                                <div class="col-xs-8 col-sm-8">
                                    <h3 class="menu_default_opcion" style="font-size:20px !important;">EXPEDIENTE HIJOS</h3>
                                </div>
                            </div>
                        </div>
                    </div>
                </a>
            </div>
        </div>
    <ul></ul>
    <%-- end columna izq --%>
</asp:Content>
