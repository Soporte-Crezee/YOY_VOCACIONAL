<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="EditarCicloEscolar.aspx.cs" Inherits="POV.Web.PortalOperaciones.CiclosEscolares.EditarCicloEscolar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.textareaCounter.plugin.js"
        type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(init);

        function init() {
            $('.boton').button();
            $('#<%=txtInicioCiclo.ClientID %>').datepicker({ changeMonth:true, changeYear: true, dateFormat: "dd/mm/yy" });
            $('#<%=txtFinCiclo.ClientID %>').datepicker({ changeMonth:true, changeYear: true, dateFormat: "dd/mm/yy" });
            //Contador del área de texto
            var opciones = { maxCharacterSize: 300, warningNumber: 300, displayFormat: '#left caracteres restantes de #max max.' };
            $('#<%=txtDescripcion.ClientID %>').textareaCount(opciones);
		    
		    validarCicloEscolar();
        }

        function validarCicloEscolar() {
            rules= {
                <%=CbPais.UniqueID %>:{required:true,min:1},
                <%=CbEstado.UniqueID %>:{required:true,min:1},
                <%=txtDescripcion.UniqueID %>:{required:true,maxlength:300},
                <%=txtNombre.UniqueID %>:{required:true,maxlength:100},
                <%=txtFinCiclo.UniqueID %>:{required:true,maxlength:10},
                <%=txtInicioCiclo.UniqueID %>:{required:true,maxlength:10}
            };		    
            
            jQuery.extend(jQuery.validator.messages, {
                min: jQuery.validator.format("Este campo es obligatorio")
            });
                  
            $('#frmMain').validate({
                rules: rules,  
                submitHandler: function(form) {
                    $('.main').block();
                    form.submit();
                }
            });              
        }
		
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="bodyadaptable col-xs-13 col-md-13">
        <h3 class="ui-widget-header ui-widget-header-label">
            <asp:HyperLink runat="server" ID="lnkBack" NavigateUrl="BuscarCicloEscolar.aspx">
			Volver
            </asp:HyperLink>/Editar ciclo orientaci&oacute;n vocacional
        </h3>
        <div class="ui-widget-content">
            <table>
                <tr>
                    <td class="td-label">
                        <asp:Label runat="server" ID="lblCicloEscolarID" Text="ID" CssClass="label"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtCicloEscolarID" ReadOnly="True" Enabled="False"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td-label">
                        <asp:Label runat="server" ID="lblPais" Text="País" CssClass="label"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="updPais">
                            <ContentTemplate>
                                <asp:DropDownList runat="server" ID="CbPais" ToolTip="pais del ciclo escolar" AutoPostBack="True"
                                    OnSelectedIndexChanged="CbPais_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td class="td-label">
                        <asp:Label runat="server" ID="lblEstado" Text="Estado" CssClass="label"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="updEstado">
                            <ContentTemplate>
                                <asp:DropDownList runat="server" ID="CbEstado" AutoPostBack="True">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td class="td-label">
                        <asp:Label runat="server" ID="lblNombre" Text="Nombre" CssClass="label"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtNombre" ReadOnly="False" Enabled="True" MaxLength="100"
                            TabIndex="3" CssClass="textoEnunciado"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td-label">
                        <asp:Label runat="server" ID="lblInicioCiclo" Text="Fecha Inicio" CssClass="label"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtInicioCiclo" ReadOnly="False" Enabled="True" TabIndex="4"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td-label">
                        <asp:Label runat="server" ID="lblFinCiclo" Text="Fecha Fin" CssClass="label"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtFinCiclo" ReadOnly="False" Enabled="True" TabIndex="5"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td-label-texarea">
                        <asp:Label runat="server" ID="lblDescripcion" Text="Descripción" CssClass="label"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtDescripcion" Columns="23" Rows="5" ReadOnly="False"
                            Enabled="True" TabIndex="6" TextMode="MultiLine" MaxLength="100" CssClass="textoEnunciado"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td-label">
                        <asp:Label runat="server" ID="lblLiberado" Text="Liberado" CssClass="label"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkEstaLiberado" runat="server" />
                    </td>
                </tr>
            </table>
            <div class="line">
            </div>
            <table>
                <tr>
                    <td class="td-label"></td>
                    <td class="label">
                        <asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="btn-green" OnClick="btnGuardar_Click" />
                    </td>
                    <td class="label">
                        <asp:HyperLink ID="btnCancelar" CssClass="btn-cancel" NavigateUrl="~/CiclosEscolares/BuscarCicloEscolar.aspx"
                            runat="server">Cancelar</asp:HyperLink>
                    </td>
                </tr>
            </table>
            <br />
        </div>
    </div>
    <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
        style="display: none;" />
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(loadControls);

        function loadControls() {

            var ddate = new Date();

            $('.boton').button();
            $('#<%=txtInicioCiclo.ClientID %>').datepicker({ changeMonth: true, changeYear: true, dateFormat: "dd/mm/yy" });
            $('#<%=txtFinCiclo.ClientID %>').datepicker({ changeMonth: true, changeYear: true, dateFormat: "dd/mm/yy" });

            validarCicloEscolar();
        }
    </script>
</asp:Content>
