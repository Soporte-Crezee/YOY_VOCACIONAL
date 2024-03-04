<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportesAbuso.aspx.cs" Inherits="POV.Web.PortalSocial.Social.ReportesAbuso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/json.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api.dialogs.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api.shared.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api.reporteabuso.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/core.reporteabuso.js") %>" type="text/javascript"></script>


 <script id="reporteabusoTmpl" type="text/x-jquery-tmpl">
     
     <div class="reporteAbusoSocial" id="${reporteabusoid}">
          <ul>
            <li class="item_comment">
                
                <div class="floatRight" style="text-align: center;margin-right:5px;">
                  <button class="button_clip_39215E" title="confirmar el reporte de abuso" id="btn-acep-${reporteabusoid}" style="width:80px;height: 25px;" type="button" onclick="javascript:coreReporteAbuso.confirmReporteAbuso('${reporteabusoid}');">Confirmar</button>
                  <button class="btn-cancel" title="cancelar el reporte de abuso" id="btn-del-${reporteabusoid}" style="width: 80px;height:25px" type="button" onclick="javascript:coreReporteAbuso.deleteReporteAbuso('${reporteabusoid}');">Cancelar</button> 
                </div>
                <div class="img_comment_div">
                   <div class="img_comment" style="margin-left: 3px;">
                   
                      <img src="../Files/ImagenUsuario/ImagenPerfil.aspx?img=thumb&usr=${reportadoid}"  alt="imagen"/>
                   </div>
                </div>
                <div class="item_comment_content">
                   <div class="break_text">
                    {{if rendereportadolink}}
                      <a href="../Social/ViewMuro.aspx?u=${reportadoid}">${reportadonombre}</a>

                      {{else}}
                         <label>${reportadonombre}</label>   
                  {{/if}}
                   </div>
                   <div class="break_text" style="width: 90%">
                       <strong>
                       <a class="link_blue" href="${url}?n=${reporteabusoid}">${textonotificacion}</a>
                       </strong>
                       ${contenido}
                    </div>
                    
                    <p style="margin-top:10px;">
                          <span class="dateformat">
                             ${fechainicioformated}
                          </span>
                       
                       </p>
                </div>
            </li>
            </ul>
     </div>

</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="page_content" runat="server">
  <div style="margin: 5px 10px">
    <h1 class="tBienvenida">Reportes de abuso</h1>
    <div class="subline_title"></div>
      <ul>
    
          <li><div id="reporteabuso_container">
                  <ul id="reporteabuso_stream">
                  </ul>
                  <div id="more">
                  </div>
              </div>
          </li>
      </ul>
  </div>
  
    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(loadControls);
        prm.add_endRequest(endRequests);

        function loadControls(sender, args) {
            coreReporteAbuso.init({
                more:$('#more'),
                containerStream:$('#reporteabuso_stream'),
                containerTmpl: $('#reporteabusoTmpl')
            });

        }

        function endRequests(sender, args) {

        }
  </script>
</asp:Content>
