<%@ Page Title="YOY - ESTUDIANTE" Language="C#" MasterPageFile="~/PortalAlumno/PortalAlumno.master"
    AutoEventWireup="true" CodeBehind="Noticias.aspx.cs" Inherits="POV.Web.PortalSocial.PortalAlumno.Noticias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head_selected" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>autoresize.jquery.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.textareaCounter.plugin.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>json.js" type="text/javascript"></script>

    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.shared.js" type="text/javascript"></script>

    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.publicaciones.js" type="text/javascript"></script>    
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>core.publicaciones.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {

            $("#btnConfirmarCorreo").on("click", function () {
                __doPostBack('btnConfirmarCorreo', '');
            });
        });

    </script>
    <style>
        .marginderecho {
            margin-left: 15px;
        }

        @media screen and (max-width:768px) {
            .marginderecho {
                margin: 0px 15px;
            }

            .text_content {
                margin: 8px 0px;
            }

            .container {
                padding: 0;
            }

            .container-fluid {
                padding: 0 !important;
            }

            .floatRight {
                float: none;
                position: relative;
                top: -78px;
                left: 40%;
            }
        }

        .dateformat, .boton_link > input[type="button"],
        .item_pub_content, .user_pub.link_blue, a.link_blue {
            font-size: 18px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content_selected" runat="server">
    <!-- Modal -->
    <div class="bodyadaptable container-fluid linea-divisor">
        <div style="visibility: hidden">
            <asp:TextBox ID="txtCorreoConfirmado" Style="margin-top: -35px;" MaxLength="30" runat="server" CssClass="form-control" Width="250px" Enabled="true"></asp:TextBox>
            <asp:TextBox ID="txtDatosCompletos" Style="margin-top: -35px;" MaxLength="30" runat="server" CssClass="form-control" Width="250px" Enabled="true"></asp:TextBox>
            <asp:TextBox ID="txtEstatusIdentificacion" Style="margin-top: -35px;" MaxLength="30" runat="server" CssClass="form-control" Width="250px" Enabled="true"></asp:TextBox>
        </div>
        <div class="modal fade" id="welcomModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" id="myModalLabel">Mensaje de bienvenida</h4>
                    </div>
                    <div class="modal-body">
                        Este es el portal del estudiante donde podrás interactuar con otros usuarios y orientadores						
					<br />
                        A continuación te presentamos una introducción acerca del funcionamiento de cada apartado.
					
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Iniciar</button>
                    </div>
                </div>
            </div>
        </div>
        <div id="info_perfil" class="row">
            <h2 class="tBienvenida">Muro</h2>
        </div>
        <div class="">
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
                var pubs_current = 1;

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
    </div>

</asp:Content>
