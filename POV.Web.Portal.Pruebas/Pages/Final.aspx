<%@ Page Title="" Language="C#" MasterPageFile="~/Content/MasterPages/PruebaMaster.Master" AutoEventWireup="true" CodeBehind="Final.aspx.cs" Inherits="POV.Web.Portal.Pruebas.Pages.Final" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/Layout.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #content {
            background: url('../Content/images/YOY_finalprueba.jpg') !important;
        }
        #form1 {
            background: #0e9ec1 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <div class="bodyadaptable">
        <div runat="server" id="divPivote">
            <div class="row">
                <div class="col-md-12" style="margin-top:75px">
                    <div style="height: 454px; margin: 0 auto" class="border_div_contenedor background_div_contenedor alinear_contenido_centro">

                        <div id="HechoBien1">
                            <img src="../Content/Images/part-73.png" alt="Lo haz hecho muy bien" />
                        </div>
                        <br />
                        <div id="Participacion1" class="texto_normal_titulo">
                            <p>
                                Has concluido la encuesta de manera exitosa,
                            </p>
                        </div>
                        <div id="Participacion_2" class="texto_normal_titulo">
                            <p>
                                recuerda que para continuar debes confirmar y completar tu perfil.
                            </p>
                        </div>
                        <br />
                        <br />
                        <br />
                        <asp:LinkButton CssClass="boton_Finalizar btn-cancel" Text="Siguiente" runat="server" ID="lnkBotonTerminar" PostBackUrl="" ClientIDMode="Static" Height="58px" Width="380px"
                            OnClick="lnkBotonTerminar_Click"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>

        <div runat="server" id="divNoPivote">
            <div class="row">
                <div class="col-md-12" style="margin-top:75px">
                    <div style="height: 454px; margin: 0 auto"
                        class="border_div_contenedor background_div_contenedor alinear_contenido_centro">

                        <div id="HechoBien">
                            <img src="../Content/Images/part-73.png" alt="Lo haz hecho muy bien" />
                        </div>
                        <br />
                        <div id="Participacion" class="texto_normal_titulo">
                            <p>
                                Has concluido de manera exitosa la
                        <asp:Label ID="NombrePrueba" runat="server"></asp:Label>.
                            </p>
                            <p>
                                El reporte de resultados puede ser consultado en el apartado <b>Pruebas</b>.
                            </p>
                        </div>
                        <br />
                        <br />
                        <br />
                        <asp:LinkButton CssClass="boton_Finalizar btn-cancel" Text="Ir al portal" runat="server" ID="lnkBotonTerminarOp2"
                            PostBackUrl="" ClientIDMode="Static" Height="58px" Width="250px"
                            OnClick="lnkBotonTerminar_Click"></asp:LinkButton>
                        <br />
                        <br />
                        <asp:LinkButton CssClass="boton_Finalizar_Bullying btn-cancel" Text="Realizar otra prueba de Bullying" runat="server" ID="lnkRealizarPruebaBateriaBullying"
                            PostBackUrl="" ClientIDMode="Static" Height="90px" Width="250px" Visible="false"
                            OnClick="lnkRealizarPruebaBateriaBullying_Click"></asp:LinkButton>                    
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
