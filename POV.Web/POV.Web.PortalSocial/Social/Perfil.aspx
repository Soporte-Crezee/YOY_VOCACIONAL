<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Perfil.aspx.cs" Inherits="POV.Web.PortalSocial.Social.Perfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script 
        type="text/javascript">window.location = "../PortalAlumno/Noticias.aspx";
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="page_content" runat="server">
    <div id="panel-container" class="panel_edicion_perfil">
    <div id="left_panel">
        <div id="user_img" style="margin-top: 10px;">
            <asp:Image runat="server" ID="ImgUser" CssClass="profile_img" />
        </div>
    </div>
    <div id="center_panel">
        <div class="perfil_div">
            <div class="button_edit_perfil boton_1">
                <asp:HyperLink ID="HplEditarPerfil" runat="server"
                    CssClass="boton_1" Visible="false">Editar mi yo</asp:HyperLink>
            </div>
            <br /><br />
            <ul>
                <li>
                    <div class="titulo_marco_general">Informaci&oacute;n b&aacute;sica</div>
                </li>
                <li>
                    <label>Nombre completo</label><strong><asp:Label ID="LblNombre" runat="server"></asp:Label></strong>
                </li>
                <li style="display:none">
                    <label>Fecha de nacimiento</label><strong><asp:Label ID="LblFechaNacimiento" runat="server"></asp:Label></strong>
                </li>
                <li style="display:none">
                    <label>Edad</label><strong><asp:Label ID="LblEdad" runat="server"></asp:Label></strong>
                </li>
                <li style="display:none">
                    <label>Escuela</label><strong><asp:Label ID="LblEscuela" runat="server"></asp:Label></strong>
                </li>
                <li style="display:none">
                    <label>Grupo</label><strong><asp:Label ID="LblGrado" runat="server"></asp:Label></strong>
                </li>
                <li style="display:none">
                    <label><asp:Label ID="LblAsignaturaName" runat="server" Visible="false">Asignatura(s)</asp:Label></label>
                    <strong><asp:Label ID="LblAsignatura" runat="server" Visible="false"></asp:Label></strong>
                </li>
                <li>
                    <div class="titulo_marco_general">Estado</div>
                        <div class="subline_title"></div>
                </li>
                <li>
                    <asp:Label ID="LblFirma" runat="server" CssClass="text_format_1"></asp:Label></li>
            </ul>
        </div>
        <div style="margin-top: 10px; margin-bottom: 10px; text-align: center; display:none">
            <asp:Button ID="btnReporteFelder" runat="server" Text="Ver mis resultados de Felder" CssClass="button_clip_39215E" OnClick="btnReporteFelder_Click" />
            <asp:Button ID="btnReporteAptitudes" runat="server" Text="Ver mis resultados de aptitudes sobresalientes" CssClass="button_clip_39215E" OnClick="btnReporteAptitudes_Click" />
        </div>
    </div>
        </div>
</asp:Content>
