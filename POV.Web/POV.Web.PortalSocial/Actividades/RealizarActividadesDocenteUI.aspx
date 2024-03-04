<%@ Page Title="YOY - ESTUDIANTE" Language="C#" MasterPageFile="~/PortalAlumno/PortalAlumno.master" AutoEventWireup="true" CodeBehind="RealizarActividadesDocenteUI.aspx.cs" Inherits="POV.Web.PortalSocial.Actividades.RealizarActividadesDocenteUI" %>

<%@ Import Namespace="POV.ConfiguracionActividades.BO" %>
<%@ Import Namespace="POV.Core.RedSocial.Implement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head_selected" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet"
        type="text/css" />
    <style type="text/css">
        table.DDGridView .th {
            color: #000;
        }

        .DDGridView .td {
            white-space: normal !important;
        }

            .DDGridView .td:hover {
                background-color: #F5F6CE !important;
                text-decoration: none;
            }

        .titulo_marco_general {
            width: 25%;
            border: none;
            background-color: #F5F5F5;
        }

        .encabezadoActividad {
            width: 100%;
            background-color: #ccc;
            color: #000 !important;
        }

            .encabezadoActividad li {
                padding-left: 10px;
                padding-top: 3px;
            }

        .pnl-actividad {
            border: 1px solid #f1f1f1;
            margin-bottom: 15px;
        }

        .tBienvenidaLabel {
            font-family: Roboto-Bold !important;
        }
    </style>
    <script type="text/javascript">
        $(function () {

            $("#accordion").accordion();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content_selected" runat="server">
    <div class="panel panel-default">
        <div class="panel-heading">
            Actividades de 
            <asp:Label ID="LblNombreUsuario" runat="server" Text="" CssClass="tBienvenidaLabel"></asp:Label>
            <div class="subline_title"></div>
        </div>
        <div class="panel-body">
            <div id="tabs">
                <div id="actividades-1">
                    <div style="display: none">
                        <b>Mostrar por docente:</b>
                        <asp:DropDownList ID="ddlDocentes" runat="server" CssClass="input_text_general"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlDocentes_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <br />
                    <div>
                        <label><b>Actividades pendientes:</b> </label>
                        <asp:Label ID="lblTotalActividadesDocentePendientes" runat="server"></asp:Label>
                    </div>
                    <br />
                    <asp:ListView runat="server" ID="LvwActividadDocentes" OnItemDataBound="LvwActividad_ItemDataBound">
                        <ItemTemplate>
                            <div class="pnl-actividad">
                                <div class="col-xs-12 encabezadoActividad">
                                    <div class="col-xs-12 col-md-6">
                                        <div class="col-xs-12 form-group">
                                            <label class="col-sm-4 control-label">Orientador: </label>
                                            <div class="col-sm-8">
                                                <asp:Label runat="server" ID="lblNombreDocente" Enabled="false"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-xs-12 form-group">
                                            <label class="col-sm-4 control-label">Actividad: </label>
                                            <div class="col-sm-8">
                                                <asp:Label runat="server" ID="lblNombreActividad" Enabled="false"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-xs-12 form-group" style="display: none;">
                                            <label class="col-sm-4 control-label">Periodo de vigencia: </label>
                                            <div class="col-sm-8">
                                                <asp:Label runat="server" ID="lblVigenciaActividad" Enabled="false"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-md-6">
                                        <div class="col-xs-12 form-group">
                                            <label class="col-sm-4 control-label">Estado: </label>
                                            <div class="col-sm-8">
                                                <asp:Label runat="server" ID="lblEstadoActividad" Enabled="false" Height="10px"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!--Grid-->
                                <div class="">
                                    <div class="col-xs-12">
                                        <label class="col-sm-12 control-label text-center">Tarea(s) de la actividad: </label>
                                    </div>
                                    <asp:GridView runat="server" ID="grdTareas"
                                        AutoGenerateColumns="False"
                                        CssClass="table table-condensed" RowStyle-CssClass="td"
                                        HeaderStyle-CssClass="th"
                                        BorderWidth="1" HorizontalAlign="Center" HeaderStyle-BackColor="#fcfcfc"
                                        RowStyle-BackColor="transparent" AlternatingRowStyle-BackColor="#eeeeee"
                                        OnRowCommand="grdTareas_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="text-left">
                                                <HeaderTemplate>
                                                    <asp:Label runat="server" Text="Nombre" ID="lblTituloNombre"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>

                                                    <asp:Label runat="server" ID="lblNombre" Text='<%# System.Web.HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "Nombre")) %>' Font-Bold='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Negrita")) %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="text-justify">
                                                <HeaderTemplate>
                                                    <asp:Label runat="server" Text="Instrucción" ID="lblTituloInstruccion"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblInstruccion" Text='<%# System.Web.HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "Instruccion")) %>' Font-Bold='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Negrita")) %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                ItemStyle-Width="15%" ItemStyle-CssClass="text-left hidden-xs" HeaderStyle-CssClass="hidden-xs">
                                                <HeaderTemplate>
                                                    <asp:Label runat="server" Text="Estatus" ID="lblTituloEstatus"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblEstatus" Text='<%# DataBinder.Eval(Container.DataItem,"Estatus").ToString()  %>' Font-Bold='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Negrita")) %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                ItemStyle-Width="7%">
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageButton1" runat="server" CssClass="img" CommandName="RealizarTarea"
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ActividadId").ToString()+"&"+DataBinder.Eval(Container.DataItem,"TareaRealizadaId").ToString() %>'
                                                        Width="16px"
                                                        ImageUrl='<%#  Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"EsEjecutable")) ? "../Images/icons/ico.iniciar.tarea.png" : "../Images/icons/ico.iniciar.off.png" %>'
                                                        AlternateText="Iniciar Tarea" Enabled='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"EsEjecutable")) %>'
                                                        title='<%#  Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"EsEjecutable")) ? "Iniciar con la tarea" : "El período de la actividad ha terminado" %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                        <EmptyDataTemplate>
                                            <div id="more">
                                                <p>
                                                    <span>No cuentas con tareas para realizar.</span>
                                                </p>
                                            </div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </div>
                                <!--EndGrid-->
                            </div>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <div id="more">
                                <p>
                                    <span>No tienes actividades asignadas.</span>
                                </p>
                            </div>
                        </EmptyDataTemplate>

                    </asp:ListView>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
