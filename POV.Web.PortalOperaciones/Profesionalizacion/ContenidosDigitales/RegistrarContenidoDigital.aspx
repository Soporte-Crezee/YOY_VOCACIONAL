<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="RegistrarContenidoDigital.aspx.cs" Inherits="POV.Web.PortalOperaciones.Profesionalizacion.ContenidosDigitales.RegistrarContenidoDigital" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet"
        type="text/css" />
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>jquery.tagsinput.css" rel="stylesheet"
        type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.tagsinput.js" type="text/javascript"></script>
    <style type="text/css">
        .oculto
        {
            visibility: hidden;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            $(".boton").button();

             $("#txtEspacioEtiquetas").val($("#<%=TxtEtiquetas.ClientID %>").val().trim().toUpperCase());

            $("#txtEspacioEtiquetas").tagsInput({
                'defaultText': '+ Etiqueta',
                 'width': '500px',
                 'onAddTag':function(da) {
                    da = da.toUpperCase();
                    var texto = $("#<%=TxtEtiquetas.ClientID %>").val().trim().toUpperCase();


                    if (texto != '') {
                        texto +=  "," + da.trim();
                    } else {
                        texto +=  da.trim();
                    }
                 
                    
                    $("#<%=TxtEtiquetas.ClientID %>").val(texto.trim());
                 },
                 'onRemoveTag':function(da) {
                    da = da.toUpperCase();
                    var texto = $("#<%=TxtEtiquetas.ClientID %>").val().toUpperCase();
                    
                    var tags = texto.split(",");
                    var nuevoTexto = "";
                    for (var i = 0; i < tags.length; i++){
                        
                        if (tags[i].trim() != da.trim()) {
                            if (nuevoTexto == "") {
                                nuevoTexto += tags[i].trim()
                            } else{
                                nuevoTexto += "," + tags[i].trim()
                            }
                        }
                        
                    }


                    $("#<%=TxtEtiquetas.ClientID %>").val(nuevoTexto);
                 },
             });
            
            $("input[name='<%=RdBtnTipoReferencia.UniqueID %>']").change(function(){
                displayPanelReferencias();
            });
            
            $.blockUI.defaults.overlayCSS.backgroundColor = "white";
            $.blockUI.defaults.message = '<h1 style="font-size:20px;">Registrando, por favor espere...</h1>';
            
            displayPanelReferencias();
        }

        function displayPanelReferencias(){
            var rdSelected = $("input[name='<%=RdBtnTipoReferencia.UniqueID %>']:checked").val();
            if (rdSelected == "true"){
                $("#panelArchivoInterno").show();
                $("#panelArchivoExterno").hide();
                $("#<%=fupArchivoInterno.ClientID %>").addClass("required");
                $("#<%=TxtReferenciaExterna.ClientID %>").removeClass("required");
            } else if (rdSelected == "false"){
                $("#panelArchivoExterno").show();
                $("#panelArchivoInterno").hide();
                $("#<%=TxtReferenciaExterna.ClientID %>").addClass("required");
                $("#<%=fupArchivoInterno.ClientID %>").removeClass("required");
            }
        }
        function ValidateFields() {
             $("#frmMain").validate({submitHandler: function(form) {
                    $(form).block();
                     form.submit();
            }});
        }

        function CancelValidate() {
            var form = $('#frmMain').get(0);
            $.removeData(form, 'validator');
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        <asp:LinkButton ID="lnkBack" runat="server" OnClick="BtnCancelar_OnClick" OnClientClick="CancelValidate()"
            NavigateUrl="BuscarTemaAsistencia.aspx">Volver</asp:LinkButton>/Registrar recurso didáctico</h3>
    <div class="main_div ui-widget-content" style="padding: 15px;">
        <table cellspacing="8">
            <tr>
                <td class="td-label">
                    <label>
                        Clave</label>
                </td>
                <td>
                    <asp:TextBox ID="TxtClave" runat="server" CssClass="required" MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        Nombre</label>
                </td>
                <td>
                    <asp:TextBox ID="TxtNombre" runat="server" CssClass="required" MaxLength="200"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        Instituci&oacute;n de origen</label>
                </td>
                <td>
                    <asp:TextBox ID="TxtIntitucion" runat="server" MaxLength="200"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        Tipo de documento</label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="DDLTipoDocumento" CssClass="required" />
                </td>
            </tr>
            <tr>
                <td class="td-label" style="vertical-align: top">
                    <label>
                        Etiquetas</label>
                </td>
                <td style="vertical-align: top">
                    <input id="txtEspacioEtiquetas" name="txtEspacioEtiquetas" type="text" />
                    <asp:TextBox ID="TxtEtiquetas" runat="server" MaxLength="500" Width="10" CssClass="required oculto"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        Es descargable</label>
                </td>
                <td>
                    <asp:RadioButtonList ID="RdBtnEsDescargable" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Si" Value="true" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="No" Value="false"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
        <asp:Panel ID="Panel1" runat="server" GroupingText="Referencia de recurso didáctico">
            <table>
                <tr>
                    <td class="td-label">
                        <label>
                            Referencia</label>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="RdBtnTipoReferencia" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="Interna" Value="true" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Externa" Value="false"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="panelArchivoInterno">
                    <td class="td-label">
                        <label>
                            Archivo interno</label>
                    </td>
                    <td>
                        <asp:FileUpload runat="server" ID="fupArchivoInterno" CssClass="fileup" Height="24px"
                            Width="400px" />
                    </td>
                </tr>
                <tr id="panelArchivoExterno">
                    <td class="td-label" style="vertical-align: top">
                        <label>
                            Referencia externa</label>
                    </td>
                    <td style="vertical-align: top">
                        <asp:TextBox ID="TxtReferenciaExterna" runat="server" MaxLength="500" TextMode="MultiLine"
                            Width="500" Columns="80" Rows="5"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <div class="line">
        </div>
        <asp:Button ID="BtnGuardar" Text="Guardar" runat="server" OnClick="BtnGuardar_OnClick"
            CssClass="boton" OnClientClick="ValidateFields()" />
        <span style="">
            <asp:Button ID="BtnCancelar" Text="Cancelar" runat="server" OnClick="BtnCancelar_OnClick"
                CssClass="boton" OnClientClick="CancelValidate()" />
        </span>
        <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
            style="display: none;" />
    </div>
</asp:Content>
