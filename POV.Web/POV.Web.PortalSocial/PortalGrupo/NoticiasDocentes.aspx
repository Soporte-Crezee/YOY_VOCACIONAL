<%@ Page Title="" Language="C#" MasterPageFile="~/PortalGrupo/PortalGrupo.master"
    AutoEventWireup="true" CodeBehind="NoticiasDocentes.aspx.cs" Inherits="POV.Web.PortalSocial.PortalGrupo.NoticiasDocentes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head_selected" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>autoresize.jquery.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.textareaCounter.plugin.js"
        type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>json.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.publicaciones.js" type="text/javascript"></script>
     <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.reporteabuso.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>core.publicaciones.mi.docente.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content4" runat="server" ContentPlaceHolderID="page_title_select">
<label>Mi orientador</label>
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="content_left_panel">
         <div id="user_img" style="margin-top: 10px;">
            <asp:Image runat="server" ID="ImgUser" CssClass="profile_img"  ImageUrl="../images/misDocentes.jpg" />
        </div>
        <div class="clear"></div>
        
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content_selected" runat="server">
    <div id="info_perfil">
            <div class="titulo_marco_general">
            Apuntes de Mi orientador
            </div>
    </div>
    <div>
        <!-- publicaciones muro -->
        <div>
            <div id="PublicacionStream">
            </div>
            <div id="more">
            </div>
        </div>
        <asp:HiddenField ID="hdnSocialHubID" runat="server" />
        <asp:HiddenField ID="hdnUsuarioSocialID" runat="server" />
        <asp:HiddenField ID="hdnSessionSocialHubID" runat="server" />
        <asp:HiddenField ID="hdnSessionUsuarioSocialID" runat="server" />
        <asp:HiddenField ID="hdnTipoPublicacionTexto" runat="server" />
        <asp:HiddenField ID="hdnTipoPublicacionSuscripcionReactivo" runat="server" />
        <script type="text/javascript">
            var hub = $("#<%=hdnSocialHubID.ClientID%>");
            var usr = $("#<%= hdnUsuarioSocialID.ClientID%>");
            var curpage = 1;

            var INICIO = 0;
            var MURO = 1;
            var VIEW_PUB = 2;
            var place = INICIO;

            var hubse = $("#<%=hdnSessionSocialHubID.ClientID%>");
            var usrse = $("#<%= hdnSessionUsuarioSocialID.ClientID%>");

            var TIPO_PUBLICACION_TEXTO = $("#<%=hdnTipoPublicacionTexto.ClientID%>");
            var TIPO_PUBLICACION_REACTIVO = $("#<%=hdnTipoPublicacionSuscripcionReactivo.ClientID%>");
       
        </script>
    </div>
    <div id="dialog-people-likes" title="Personas que le ha gustado">
        <div id="people-stream">
        </div>
    </div>
</asp:Content>
