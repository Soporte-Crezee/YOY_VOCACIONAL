<%@ Page Title="YOY - ESTUDIANTE" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NuevoMensaje.aspx.cs" Inherits="POV.Web.PortalSocial.Social.NuevoMensaje" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .autoresizable {
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="page_content" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.ui.datepicker-es.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/tour/jquery.hemiIntro.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery-ui-1.12.1.custom.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api.dialogs.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.shared.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>json.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.notificaciones.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>core.notificaciones.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.block.ui.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.notice.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>alertify.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.easing.min.js" type="text/javascript"></script>

    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.shared.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.contactos.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.mensajes.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>core.mensajes.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>json.js" type="text/javascript"></script>

    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>autoresize.jquery.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.textareaCounter.plugin.js" type="text/javascript"></script>

    <!-- Contenedor message Inicio -->
    <br />
    <div class="col-md-3 col-sm-2"></div>
    <div class="col-md-6 col-sm-8">
        <div id="message">


            <div class="messagearea">
                <!-- Contenedor messagearea Inicio -->

                <!-- Contenido header Inicio -->
                <div class="msheader row">
                    <div class="">
                        <div class="col-xs-12">
                            <img alt="Mensajes" src="../Images/VOCAREER_mensajesPrivados.png" width="50px" height="50" />
                        </div>
                        <div class="col-xs-12">
                            <label class="tBienvenidaLabel">Mensajes privados</label>
                        </div>
                    </div>
                    <div class="submenu col-xs-12">

                        <div class="actions" style="text-align: center; line-height: 24px;">

                            <asp:LinkButton ID="btnnuevo" runat="server" CssClass="btn-green"
                                PostBackUrl="NuevoMensaje.aspx" Width="150">Nuevo mensaje</asp:LinkButton>


                            <asp:LinkButton ID="btnconsultar" runat="server" CssClass="btn-green" PostBackUrl="Mensajes.aspx" Width="150">Ver todos</asp:LinkButton>
                        </div>
                        <br />
                    </div>
                </div>
            </div>
            <!-- Contenido Header Fin -->
            <!-- Contenedor Main Inicio -->
            <div class="msmain">
                <!-- Inicio nuevo mensaje-->
                <div class="newms">

                    <div class="txtnewms">
                        <br />
                        <br />
                        <div class="msubtitle">

                            <!-- Inicio buscar contacto-->
                            <div class="mssearchcontact">
                                <asp:Label runat="server" ID="lblContactos" Text="Para.."></asp:Label>
                                <asp:TextBox ID="txtContactos" runat="server" Width="80%"></asp:TextBox>
                                <br />
                                <span id="mssearchresultado" style="font-size: 12px; font-weight: bold; color: #C0C0C0; visibility: hidden; display: none;">No se encontraron resultados</span>

                            </div>
                            <!-- Fin buscar contacto-->

                        </div>
                        <div class="selectedcontacts">

                            <div id="contactlist">
                            </div>
                        </div>
                        <div class="clear"></div>

                        <br />
                        <br />
                        <asp:TextBox runat="server" TextMode="MultiLine" ID="txtms" CssClass="autoresizable" MaxLength="400" Width="100%" Height="100px"></asp:TextBox>
                        <div class="">
                            <br />
                            <input id="btnenviar" type="button" value="Enviar Mensaje" class="btn-green" />
                        </div>
                    </div>
                    <div class="clear"></div>



                </div>
                <!-- Fin nuevo mensaje -->

                <!-- Inicio contenedor mensajes-->
                <div class="contentms" id="contentms">
                </div>


                <!-- Inicio contenedor mensajes-->

            </div>
            <!-- Contenedor Main Fin -->


            <!-- Contenedor Pie de Contenido Inicio -->
            <div class="msfooter">
            </div>
            <!-- Contenedor Pie de Contenido Inicio -->

            <div id="dialogquestion" title="Sistema">
                <p id="dialogtext"></p>
            </div>

            <!-- Contenedor messagearea Fin -->
        </div>
    </div>
    <div class="col-md-3 col-sm-2"></div>
    <!-- Contenedor message Fin -->


    <!--Inicio Templete agregar contacto -->
    <script id="addcontact_template" type="text/x-jquery-tmpl">

        <div class="tempaddcontact">

            <div class="selectedcontact col-xs-6 col-md-6" style="text-align: center">
                <div class="id">
                    <input type="hidden" name="dest_${val}" value="${val}">
                    <div class="img_comment">
                        <center>
                            <span class=""></span>
                            <!-- <input type="button" id="btn-del-${val}"/>   -->
                            <input type="image" src="../Files/ImagenUsuario/ImagenPerfil.aspx?img=thumb&usr=${val}" class="img_pub" alt="imagen" id="img_${val}" title="Eliminar destina" />
                        </center>
                    </div>

                    <p>${label}</p>
                </div>

            </div>


        </div>
    </script>
    <!--Fin template agregar contacto -->


    <!-- Inicio javascript -->
    <script type="text/javascript">
        NewMessage.init({
            actions: $('div.actions'),
            lblresultado: $('#mssearchresultado'),
            btnenviar: $('input[id$=btnenviar]'),
            btnnuevo: $('input[id$=btnnuevo]'),
            txtcontactos: $('input[id$=txtContactos]'),
            txtmensaje: $('textarea[id$=txtms]'),
            messagearea: $('div.messagearea'),
            msheader: $('div.msheader'),
            msmain: $('div.msmain'),
            msfooter: $('div.msfooter'),
            searchcontactContainer: $('div.mssearchcontact'),
            newmsContainer: $('div.newms'),
            contactsContainer: $('div.selectedcontacts'),
            contactContainer: $('div#contactlist'),
            templateaddcontact: $('#addcontact_template').html(),
            dialogquestion: $('#dialogquestion')
        });
        var pubs_current = 1;
    </script>
    <!-- Inicio javascript -->
</asp:Content>
