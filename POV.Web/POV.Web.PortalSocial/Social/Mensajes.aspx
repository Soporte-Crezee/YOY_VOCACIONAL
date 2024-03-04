<%@ Page Title="YOY - ESTUDIANTE" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Mensajes.aspx.cs" Inherits="POV.Web.PortalSocial.Social.Mensajes" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>muro.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="page_content" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.shared.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.contactos.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.mensajes.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>core.mensajes.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>json.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>autoresize.jquery.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.textareaCounter.plugin.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.dialogs.js" type="text/javascript"></script>

    <!-- Contenedor message Inicio -->
    <div class="bodyadaptable">

        <div class="row">
            <div class="col-md-3 col-sm-2"></div>
            <div class="col-md-6 col-sm-8">
                <div id="message" style="margin-top: 25px">


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

                                    <asp:LinkButton ID="btnnuevo" runat="server" CssClass="button_clip_39215E btn-green"
                                        PostBackUrl="NuevoMensaje.aspx" Width="150">Nuevo mensaje</asp:LinkButton>


                                    <asp:LinkButton ID="btnconsultar" runat="server" CssClass="button_clip_39215E btn-green" PostBackUrl="Mensajes.aspx" Width="150">Ver todos</asp:LinkButton>
                                </div>
                                <br />
                            </div>
                        </div>
                    </div>
                    <!-- Contenido Header Fin -->


                    <!-- Contenedor Main Inicio -->
                    <div class="msmain row">

                        <!-- Inicio acciones iniciales-->
                        <div class="actions col-xs-12" id="actions">

                            <!-- Inicio buscar contacto-->
                            <div class="searchcontact">
                                <asp:Label runat="server" ID="lblContactos" Text="Filtrar por compañer@:"></asp:Label>
                                <asp:TextBox ID="txtContactos" runat="server" CssClass="input_text_general"></asp:TextBox>

                            </div>
                            <!-- Fin buscar contacto-->
                        </div>
                        <!-- Fin acciones iniciales-->

                        <!-- Inicio Consultar mensaje-->
                        <div class="searchms">
                            <div class="title_1">Mensajes enviados por:</div>
                            <div class="selectedcontact">

                                <ul id="contactls">
                                    <li></li>
                                </ul>
                            </div>
                            <div>
                            </div>
                            <div class="clear"></div>
                        </div>
                        <!-- Fin Consultar mensaje -->


                        <!-- Inicio contenedor mensajes-->
                        <div class="contentms" id="contentms" style="margin:0px">
                        </div>


                        <div class="contentms" id="contentmsnotificacion">
                        </div>
                        <!-- Inicio contenedor mensajes-->

                    </div>
                    <!-- Contenedor Main Fin -->


                    <!-- Contenedor Pie de Contenido Inicio -->
                    <div class="msfooter row">
                        <!-- Incio Ver Mas -->
                        <div id="more" class="col-xs-12"></div>
                        <!-- Fin Ver Mas -->

                    </div>
                    <!-- Contenedor Pie de Contenido Inicio -->

                    <div id="dialogquestion" title="Sistema">
                        <p id="dialogtext"></p>
                    </div>

                    <div class="msnotificaciones">
                        <div id="notificacion">

                            <asp:HiddenField runat="server" ID="hdnNotificacionID" />
                            <asp:HiddenField runat="server" ID="hdnMensajeID" />
                            <asp:HiddenField runat="server" ID="hdnRecordcount" />
                            <asp:HiddenField runat="server" ID="hdnTipoNotificacionID" />

                        </div>
                    </div>

                    <!-- Contenedor messagearea Fin -->

                </div>
            </div>
            <div class="col-md-3 col-sm-2"></div>
        </div>
    </div>
    <!-- Contenedor message Fin -->


    <!-- Inicio template consultar mensaje -->
    <script id="addmessage_template" type="text/x-jquery-tmpl">
        <div class="mensajeoutput" id="${mensajeid}_panel">

            <!-- Inicio Ul Mensaje -->
            <ul class="mnsjs" style="padding:0px">

                <!-- Inicio Main Contenedor Mensaje -->
                <div class="${mensajeid}">
                    <!-- Inicio Lista Mensaje -->
                    <li class="mnsj">

                        <!--Inicio SubContainer Mensaje -->
                        <div class="contentmnsj" id="${mensajeid}">

                            <!--Inicio  contenido Mensaje Padre -->
                            <div class="item_pub_content">

                                <a class="user_pub link_blue" href="../Social/ViewMuro.aspx?u=${remitenteid}">${remitentenombre}
                                </a>
                                <span class="dateformat" title="${fechamensaje}">${fechamensaje}</span>
                                <!-- <p id="lbl_${remitenteid}">${remitentenombre}</p> -->

                                <!--Inicio header opciones Mensaje -->
                                <div class="floatRight" style="text-align: center;">

                                    <button id="btn-del-${mensajeid}" type="button" onclick="javascript:Search.askRemoveMessage('${guidconversacion}');" title="Eliminar mensaje" />
                                </div>
                                <!--Fin header opciones Mensaje -->

                                <!--Incio Mensaje -->
                                <div class="contentmnsj padre">
                                    <input type="hidden" name="ms_${mensajeid}" value="${mensajeid}" />
                                    <div class="contact">
                                        <div class="img_comment">
                                            <a href="../Social/ViewMuro.aspx?u=${remitenteid}">
                                                <center>
                                                    <img src="../Files/ImagenUsuario/ImagenPerfil.aspx?img=thumb&usr=${remitenteid}" alt="imagen" />
                                                </center>
                                            </a>
                                        </div>


                                    </div>
                                    <div class="text_contentms">
                                        <p>${contenido}</p>
                                    </div>

                                    <div class="clear"></div>

                                    <div class="opcdestinatariosmnsj">
                                        <label>Aspirantes que comparten el mensaje:</label>
                                        <button id="btn-mas" type="button" onclick="javascript:Search.showContacts('${guidconversacion}');" tooltip="Mostrar todos" title="Mostrar todos" style="vertical-align: middle;" />
                                    </div>

                                    <div class="destinatariosmnsj">
                                        <div id="lsdestinatarios-${guidconversacion}" class="lsdestinatarios" style="display: none;">
                                            {{each destinatarios}}
                	  						 <span class="boton_link">
                                                   <a href="../Social/ViewMuro.aspx?u=${usuariosocialid}">${screenname} . &nbsp; 
                                                   </a>
                                               </span>
                                            {{/each}}
                                        </div>
                                    </div>

                                </div>
                                <!--Fin Mensaje -->

                                <!--Incio Respuestas -->
                                <div class="comments" id="${mensajeid}_comments">
                                    {{if totalrespuestas}}
								     {{if totalrespuestas !== 0}}
                    					<div class="boton_link" id="more_comments_${guidconversacion}">
                                            <input type="button" title="Mostrar respuestas" value="" />
                                        </div>
                                    {{/if}}
									{{/if}}


                	  					<div class="clear"></div>
                                    <div class="respuestas">
                                        {{each respuestas}}
                    					<div class="img_comment">

                                            <a href="../Social/ViewMuro.aspx?u=${remitenteid}">
                                                <center>
                                                    <img src="../Files/ImagenUsuario/ImagenPerfil.aspx?img=thumb&usr=${remitenteid}" alt="imagen" />
                                                </center>
                                            </a>

                                            <!--<input type="image" src="../Files/ImagenUsuario/ImagenPerfil.aspx?img=thumb&usr=${remitenteid}" class="img_comment" alt="imagen" id="img_${remitenteid}"/> -->
                                        </div>
                                        <div class="remitentemnsj">
                                            <span class="boton_link">
                                                <a href="../Social/ViewMuro.aspx?u=${remitenteid}">${remitentenombre} . &nbsp; 
                                                </a>
                                            </span>
                                            <span class="dateformat" title="${fechamensaje}">${fechamensaje}</span>
                                        </div>
                                        <p>${contenido}</p>
                                        </br>
										<div class="clear"></div>
                                        {{/each}}
                                    </div>
                                </div>
                                <!--Fin Respuestas -->

                            </div>
                            <!--Fin  contenido Mensaje Padre -->

                        </div>

                        <!--Fin SubContainer Mensaje -->


                        <!-- Inicio Panel Responder -->
                        <div id="panelcomment_${mensajeid}" class="panelcomment">
                            <ul>
                                <li>
                                    <div class="clear"></div>
                                    <div class="boton_link">
                                        <input type="button" title="Responder" value="Responder" onclick="javascript: Search.commentenable('${mensajeid}');" />
                                    </div>
                                </li>
                            </ul>
                        </div>
                        <!-- Fin Panel Responder -->


                    </li>
                    <!--Fin Lista Mensaje -->

                </div>
                <!-- Inicio Main Contenedor Mensaje -->
            </ul>
            <!-- Fin Ul Mensaje -->



        </div>
    </script>
    <!-- Fin template consultar mensaje -->

    <!--Incio Template Panel Respuesta -->
    <script id="areacomTmpl" type="text/x-jquery-tmpl">
        <div id="panelcomment">
            <div id="cmt_${mensajeid}">
                <textarea class="autoresizable" id="txtComment"></textarea>
                <div class="">
                    <input type="button" value="Enviar" onclick="javascript: Search.sendcomment('${mensajeid}');" class="btn-green" />
                    <input type="button" value="Cancelar" onclick="javascript: Search.cancelComment();" class="btn-cancel" />
                </div>
            </div>
        </div>
    </script>
    <!--Fin Template Panel Respuesta -->

    <!--Incio Template Comentario -->
    <script id="panelcommt_template" type="my/template">
              <div class="clear"></div>
                 <div class="img_comment">
				  <input type="image" src="../Files/ImagenUsuario/ImagenPerfil.aspx?img=thumb&usr={{remitenteid}}" class="img_comment" alt="imagen" id="img_{{remitenteid}}"/>  		
			    </div>

			     <span class="boton_link"> 
                	 <a href="../Social/ViewMuro.aspx?u={{remitenteid}}">
                	  		{{remitentenombre}} . &nbsp; 
                	  </a>
                 </span>
                <span class="dateformat" title="{{fechamensaje}}">{{fechamensaje}}
                </span>

				
				<p>{{contenido}}</p>
				</br>
				<div class="clear"></div>
    </script>
    <!--Fin Template Panel Respuesta -->


    <!--Inicio Templete agregar contacto -->
    <script id="addcontact_template" type="text/x-jquery-tmpl">

        <div class="tempaddcontact">

            <li class="selectedcontact">
                <div class="id">
                    <input type="hidden" name="dest_${val}" value="${val}">

                    <div class="img_comment">

                        <center>
                            <input type="image" title="click para eliminar filtro" src="../Files/ImagenUsuario/ImagenPerfil.aspx?img=thumb&usr=${val}" class="img_pub" alt="imagen" id="img_${val}" />
                        </center>

                    </div>

                    <p>${label}</p>
                </div>

            </li>


        </div>
    </script>
    <!--Fin template agregar contacto -->



    <!-- Inicio javascript -->
    <script type="text/javascript">
        Search.init({
            actions: $('div.actions'),
            btnconsultar: $('<%=btnconsultar.ClientID %>'),
            txtcontactos: $('input[id$=txtContactos]'),
            messagearea: $('div.messagearea'),
            msheader: $('div.msheader'),
            msmain: $('div.msmain'),
            msfooter: $('div.msfooter'),
            searchcontactContainer: $('div.searchcontact'),
            messageContainer: $('#contentms'),
            messageContainerNotificacion: $('#contentmsnotificacion'),
            hdnRecordcount: $("#<%=hdnRecordcount.ClientID %>"),
            hdnNotificacionID: $("#<%=hdnNotificacionID.ClientID %>"),
            hdnMensajeID: $("#<%=hdnMensajeID.ClientID %>"),
            templateaddcontact: $('#addcontact_template').html(),
            templatemessages: $('#addmessage_template').html(),
            templatecomment: $('#panelcommt_template').html(),
            dialogquestion: $('#dialogquestion'),
            searchms: $('div.searchms'),
            contact: $('ul#contactls'),
            searchmsContactContainer: $('div.selectedcontact')
        });
        var pubs_current = 1;
    </script>
    <!-- Inicio javascript -->


</asp:Content>
