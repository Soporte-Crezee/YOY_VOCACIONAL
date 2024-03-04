<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VerReporteAbuso.aspx.cs" Inherits="POV.Web.PortalSocial.Social.VerReporteAbuso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	 <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.tmpl.min.js" type="text/javascript"></script>
	<script src="<% =Page.ResolveClientUrl("~/Scripts/json.js")%>" type="text/javascript"></script>
	<script src="<% =Page.ResolveClientUrl("~/Scripts/api.dialogs.js")%>" type="text/javascript"></script>
	<script src="<% =Page.ResolveClientUrl("~/Scripts/api.shared.js")%>" type="text/javascript"></script>
	<script src="<% =Page.ResolveClientUrl("~/Scripts/api.reporteabuso.js")%>" type="text/javascript"></script>
	<script src="<% =Page.ResolveClientUrl("~/Scripts/core.reporteabuso.js") %>" type="text/javascript"></script>

	<script id="pubTmpl" type="text/x-jquery-tmpl">

	<li id="${publicacionid}" class="item_pub">
			<div class="floatRight" style="text-align:center;margin-right:5px;">
			  
			  {{if renderreporteabuso}}
			  
			  <button class="button_clip_39215E" title="confirmar el reporte de abuso" id="btn-acep-${reporteabusoid}" style="width:80px;height: 25px;" type="button" onclick="javascript:coreElementoReporteAbuso.confirmElementoReportable('${publicacionid}');">Confirmar</button>
			  <button class="button_clip_39215E" title="cancelar el reporte de abuso" id="btn-del-${reporteabusoid}" style="width: 80px;height:25px" type="button" onclick="javascript:coreElementoReporteAbuso.deleteElementoReportable('${publicacionid}');">Cancelar</button> 
			  
			  {{else}}
			  
			  {{/if}}

			</div>
		<div class="border_img">
		<div class="img_pub">
			{{if renderlink}}
			<a href="../Social/ViewMuro.aspx?u=${usuariosocialid}">
				<center>
				<img src="../Files/ImagenUsuario/ImagenPerfil.aspx?img=thumb&usr=${usuariosocialidcom}" alt="imagen" />
				</center>
			</a>
			{{else}}
				<center>
				<img src="../Files/ImagenUsuario/ImagenPerfil.aspx?img=thumb&usr=${usuariosocialidcom}" alt="imagen" />
				</center>
			{{/if}}
		</div>
		</div>
		<div class="item_pub_content">
		
			<span><strong>
			{{if renderlink}}
				<a class="user_pub link_blue" href="../Social/ViewMuro.aspx?u=${usuariosocialid}">${usuariosocialnombre}</a> 
				
			{{else}}
				<label class="user_pub">${usuariosocialnombre}</label>
			{{/if}}
			</strong>

			{{if renderfor}}
			para 
				{{if renderforlink}}
				<a class="user_pub link_blue" href="../Social/ViewMuro.aspx?u=${destinatarioid}">${destinatarionombre}</a>
				{{else}}
					<label class="user_pub">${destinatarionombre}</label>
				 {{/if}}
			{{/if}}
			</span>- <span class="dateformat" title="${fechapublicacion}">${fechaformated}</span>
			
			
			
			<div class="lista_horizontal content_clear" style="margin-top:10px;">
			
			</div>
            <p class="text_content">
			 ${contenido}
			</p>
		</div>
			
			<!-- inicio comentarios -->
			<div class="comments">
				

				{{if complete}}
				<div id="request_complete_${publicacionid}"></div>
				{{/if}}
				<ul id="${publicacionid}_comments">
					{{each comentarios}}
					<li id="${comentarioid}" class="item_comment">
					
					<div class="floatRight" style="text-align:center;">
			  
						{{if renderreporteabuso}}
			  
							<button class="button_clip_39215E" title="confirmar el reporte de abuso" id="btn-acep-${reporteabusoid}" style="width:80px;height: 25px;" type="button" onclick="javascript:coreElementoReporteAbuso.confirmElementoReportable('${comentarioid}');">Confirmar</button>
							<button class="button_clip_39215E" title="cancelar el reporte de abuso" id="btn-del-${reporteabusoid}" style="width: 80px;height:25px" type="button" onclick="javascript:coreElementoReporteAbuso.deleteElementoReportable('${comentarioid}');">Cancelar</button>              
						{{/if}}

					</div>

						<div class="img_comment_div">
							<div class="img_comment">
								{{if renderlink}}
								<a href="../Social/ViewMuro.aspx?u=${usuariosocialidcom}">
									<center><img src="../Files/ImagenUsuario/ImagenPerfil.aspx?img=thumb&usr=${usuariosocialidcom}" alt="imagen" /></center>
								 </a>
								 {{else}}
									<center><img src="../Files/ImagenUsuario/ImagenPerfil.aspx?img=thumb&usr=${usuariosocialidcom}" alt="imagen" /></center>
								{{/if}}
							</div>
						</div>
						<div class="item_comment_content">
							
							<div class="break_text" style="width:90%;">
							
								<strong>
								{{if renderlink}}
								   <a  class="user_comment link_blue" href="../Social/ViewMuro.aspx?u=${usuariosocialidcom}"> ${usuariosocialnombrecom}</a>
								{{else}}
									<label  class="user_comment">${usuariosocialnombrecom}</label>
								{{/if}}
								</strong>
							 - ${contenidocom}
							</div>
							
							<p style="margin-top:10px;"><span class="dateformat" title="${fechacom}">${fechacomformated}</span>
							   
								
							</p>
							
						</div>
					</li>
					{{/each}}
				</ul>
			</div>

			<!--fin comentarios-->
	</li>
</script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="page_content" runat="server">
	<div>
		<div class="header">
			<div class="container">
            <h1>
				<asp:Label ID="lblTipo" runat="server"></asp:Label>
				<asp:Label ID="lblNombreMuroPublicacion" runat="server"></asp:Label>
				<br/>
				<asp:Label ID="lblNombreGrupo" runat="server"></asp:Label>
            </h1>
			</div>
		</div>

		<div id="PublicacionStream">
		</div>
	</div>
		<asp:HiddenField ID="hdnReporteAbusoID" runat="server" />
		<asp:HiddenField ID="hdnReportableID" runat="server"/>
		<asp:HiddenField ID="hdnTipoReporteAbusoID" runat="server"/>
		<script type="text/javascript">
			var reporteAbusoID = $("#<%=hdnReporteAbusoID.ClientID%>");
			var reporableID = $("#<%=hdnReportableID.ClientID %>");
			var tiporeporteabusoID = $("#<%=hdnTipoReporteAbusoID.ClientID %>");
		
		</script>
		
		<script type="text/javascript">
			var prm = Sys.WebForms.PageRequestManager.getInstance();
			prm.add_pageLoaded(loadControls);
			prm.add_endRequest(endRequests);

			function loadControls(sender, args) {
			    coreElementoReporteAbuso.init({
					containerStream: $('#PublicacionStream'),
					containerTmpl: $('#pubTmpl'),
					reporteabuso:$(reporteAbusoID).val(),
					tiporeporteabuso:$(tiporeporteabusoID).val()
				});

			}

			function endRequests(sender, args) {

			}
			
		</script>
</asp:Content>
