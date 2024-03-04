<%@ Page Language="C#" MasterPageFile="~/DefaultSite.Master" AutoEventWireup="true"
    CodeBehind="SeleccionarEscuela.aspx.cs" Inherits="POV.Web.Administracion.SeleccionarEscuela" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <style>
        body {
            list-style-type: none !Important;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(initPage);

        function initPage() {
            DoFormBlockUI();
        }
			


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="seleccion-escuela ui-widget-content" style="padding:10px; min-height:300px;">
        <h1 class="title_6">
            Selecciona una escuela</h1>
            <br />
        <h2>
            <asp:Label ID="LblErrorEscuela" runat="server" CssClass="error"></asp:Label></h2>
        <ul>
            <asp:Repeater ID="RptEscuelas" runat="server" OnItemCommand="Escuelas_ItemCommand"
                OnItemDataBound="Escuelas_DataBound">
                <ItemTemplate>
                    <li class="item_pub">
                        <asp:LinkButton ID="LnkBtnSeleccionar" runat="server" 
                            CommandArgument='<%#Eval("LicenciaEscuelaID") %>' CommandName="SELECT_LICENCIA">
                                <i class="icon_48 icon_escuela"></i>
                                <span><%#Eval("NombreEscuela")%>, Turno <%#Eval("Turno")%></span>
                        </asp:LinkButton>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:Label ID="LblSinEscuelas" runat="server" Visible="false">
                        <div id="more">
                            <span>No hay escuelas asignadas.</span>
                        </div>
                    </asp:Label>
                </FooterTemplate>
            </asp:Repeater>
        </ul>
    </div>
</asp:Content>
