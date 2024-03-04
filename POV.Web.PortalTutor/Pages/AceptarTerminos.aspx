<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultSite.Master" AutoEventWireup="true"
    CodeBehind="AceptarTerminos.aspx.cs" Inherits="POV.Web.PortalSocial.CuentaUsuario.AceptarTerminos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #ContenidoTerminos
        {            
            overflow: auto;
        }
        .title_1 {
            font-size:20px !important;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(initPage);

        function initPage() {
            //$.get($("#<%=hdnRefTerminos.ClientID%>").val(), function (contenido) {
              //  $("#ContenidoTerminos").append(contenido);   
            //});
        }

        $(function () {

            $(".boton").button();
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="page_content" runat="server">
    <h1 class="title_1">
        T&eacute;rminos y condiciones</h1>
    <br />
    <label class="title_5">
        Para continuar el acceso debe aceptar los t&eacute;rminos y condiciones de este
        portal</label><br />
    <br />
    <div>
        <div id="ContenidoTerminos" class="ui-corner-all ui-widget-content" style="padding: 10px; text-align:center">
             <iframe width="100%" style="min-height:500px" src="/pdf/terminoscondicionesyoy.pdf"></iframe>
        </div>
    </div>
    <br />
    <label>
    </label>
    <div class="">        
        <asp:Button ID="BtnAceptarTerminos" Text="He leído y estoy de acuerdo" runat="server"
            OnClick="BtnAceptarTerminos_OnClick" CssClass="btn-green" />
        <asp:Button ID="BtnRechazarTerminos" Text="No estoy de acuerdo" runat="server" OnClick="BtnRechazarTerminos_OnClick"
            CssClass="btn-cancel" />
        <asp:HiddenField ID="hdnRefTerminos" runat="server" />
    </div>
</asp:Content>
