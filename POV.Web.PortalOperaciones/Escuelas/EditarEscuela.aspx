<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="EditarEscuela.aspx.cs" Inherits="POV.Web.PortalOperaciones.Escuelas.EditarEscuela" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagPrefix="asp" TagName="GridViewPager" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/gridview.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery.validate.min.js" type="text/javascript"></script>
    <script src="../Scripts/messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
		$(document).ready(initPage);

		function initPage() {
		    $('.boton').button();
		      $('#acordion').accordion({active: -1, collapsible:true, autoHeight: false});

		};

		function validateEscuela() {
		       var rules = {
				   
	        <%=CbPais.UniqueID %>:{required: true,min:1},  
	        <%=CbEstado.UniqueID %>:{required: true,min:1},  
	        <%=CbMunicipio.UniqueID %>:{required: true,min:1},
	        <%=CbLocalidad.UniqueID %>:{required: true,min:1},
	        <%=CbZona.UniqueID %>:{required:true,min:1},
	        <%=CbTurno.UniqueID %>:{required:true, min:0},
	        <%=CbAmbito.UniqueID %>:{required:true,min:0},
	        <%=CbControl.UniqueID %>:{required:true,min:0},
	        <%=CbNivel.UniqueID %>:{required:true,min:1},
	        <%=CbTipoServicio.UniqueID %>:{required:true,min:1},
	        <%=txtClaveEscuela.UniqueID %>:{required: true, maxlength:50 },        
	        <%=txtNombreEscuela.UniqueID %> :{required:true, maxlength:50},
	        <%=txtNombreDirector.UniqueID %>:{required:true},
	        <%=CbTipoNivelEducativo.UniqueID %>:{required:true,min:1}

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
    <h3 class="ui-widget-header">
        <asp:HyperLink runat="server" ID="lnkBack" NavigateUrl="BuscarEscuelas.aspx">
		Volver
        </asp:HyperLink>
        /Editar escuela
    </h3>
    <div class="ui-widget-content" style="padding: 5px">
        <div>
            <div>
                <h2>
                    Informaci&oacute;n de la ubicaci&oacute;n
                </h2>
                <hr />
                <br />
                <table>
                    <tr>
                        <td class="td-label">
                            <asp:Label runat="server" ID="lblPais" Text="País"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel runat="server" ID="updPais">
                                <ContentTemplate>
                                    <asp:DropDownList runat="server" ID="CbPais" TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="CbPais_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td class="td-label">
                            <asp:Label runat="server" ID="lblEstado" Text="Estado"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updEstado" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList runat="server" ID="CbEstado" TabIndex="2" AutoPostBack="True" OnSelectedIndexChanged="CbEstado_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-label">
                            <asp:Label runat="server" ID="lblMunicipio" Text="Municipio"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updCiudad" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList runat="server" ID="CbMunicipio" TabIndex="3" AutoPostBack="True"
                                        OnSelectedIndexChanged="CbMunicipio_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td class="td-label">
                            <asp:Label runat="server" ID="lblLocalidad" Text="Localidad"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updLocalidad" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList runat="server" ID="CbLocalidad" TabIndex="4" AutoPostBack="True"
                                        OnSelectedIndexChanged="CbLocalidad_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-label">
                            <asp:Label runat="server" ID="lblZona" Text="Zona"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel runat="server" ID="updZona">
                                <ContentTemplate>
                                    <asp:DropDownList runat="server" ID="CbZona">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <h2>
                    Informaci&oacute;n de la escuela
                </h2>
                <hr />
                <br />
                <table>
                    <tr>
                        <td class="td-label">
                            <asp:Label runat="server" ID="lblClaveEscuela" Text="Clave escuela"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtClaveEscuela" ReadOnly="False" Enabled="True"
                                MaxLength="50" CssClass="textoEnunciado"></asp:TextBox>
                        </td>
                        <td class="td-label">
                            <asp:Label runat="server" ID="lblTurno" Text="Turno"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel runat="server" ID="updTurno">
                                <ContentTemplate>
                                    <asp:DropDownList runat="server" ID="CbTurno" TabIndex="5">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-label">
                            <asp:Label runat="server" ID="lblNombreEscuela" Text="Nombre"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtNombreEscuela" ReadOnly="False" Enabled="True"
                                MaxLength="50" CssClass="textoEnunciado"></asp:TextBox>
                        </td>
                        <td class="td-label">
                            <asp:Label runat="server" ID="lblAmbito" Text="Ámbito"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel runat="server" ID="updAmbito">
                                <ContentTemplate>
                                    <asp:DropDownList runat="server" ID="CbAmbito" TabIndex="6">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-label">
                            <asp:Label runat="server" ID="lblControl" Text="Control"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel runat="server" ID="updControl">
                                <ContentTemplate>
                                    <asp:DropDownList runat="server" ID="CbControl" TabIndex="7">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td class="td-label">
                            <asp:Label runat="server" ID="lblTipoNivelEducativo" Text="Tipo nivel educativo"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel runat="server" ID="updTipoNivelEducativo">
                                <ContentTemplate>
                                    <asp:DropDownList runat="server" ID="CbTipoNivelEducativo" AutoPostBack="True" OnSelectedIndexChanged="CbTipoNivelEducativo_SelectedIndexChanged" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-label">
                            <asp:Label runat="server" ID="lblNivel" Text="Nivel educativo"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel runat="server" ID="updNivel">
                                <ContentTemplate>
                                    <asp:DropDownList runat="server" ID="CbNivel" AutoPostBack="True" OnSelectedIndexChanged="CbNivel_SelectedIndexChanged" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td class="td-label">
                            <asp:Label runat="server" ID="lblTipoServicio" Text="Tipo servicio"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel runat="server" ID="updServicio">
                                <ContentTemplate>
                                    <asp:DropDownList runat="server" ID="CbTipoServicio" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <h2>
                    Informaci&oacute;n del Director
                </h2>
                <hr/>
                <br />
                <table style="float: left;">
                    <tr>
                        <td class="td-label">
                            <asp:Label runat="server" ID="lblCurp" Text="CURP"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:UpdatePanel runat="server" ID="updDirectorInfo" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox runat="server" ID="txtCurp" ReadOnly="True" Enabled="False"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="grdDirectores" EventName="RowCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td class="td-label">
                            <asp:Label runat="server" ID="lblNombreDirector" Text="Director"></asp:Label>
                        </td>
                        <td >
                            <asp:UpdatePanel runat="server" ID="updDirectorInfo2" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox runat="server" ID="txtNombreDirector" ReadOnly="True" Enabled="False"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="grdDirectores" EventName="RowCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel runat="server" ID="updConsultar">
                                <ContentTemplate>
                                    <asp:Button runat="server" ID="btnConsultar" Text="Agregar Director" CssClass="boton ui-widget ui-button-text-icon-primary"
                                        OnClick="btnConsultar_Click" Style="visibility: hidden; display: none" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <br />
                <div id="acordion" style="float: left; width: 600px;">
                    <h3>
                        <a href="#dialogdocentes">Consultar director </a>
                    </h3>
                    <div id="colapsableCont" style="min-height: 550px;">
                        <table >
                            <tr>
                                <td class="td-label">
                                    <asp:Label runat="server" Text="CURP" ID="lblCurpConsultar"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtCurpConsultar" MaxLength="18"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td-label">
                                    <asp:Label runat="server" Text="Nombre" ID="lblNombreDirectorConsultar" ></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtNombreDirectorConsultar"  MaxLength="100"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td-label">
                                    <asp:Label runat="server" Text="Primer apellido" ID="lblPrimerApellido" ></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtPrimerApellidoConsultar" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td-label">
                                    <asp:Label runat="server" Text="Segundo apellido" ID="lblSegundoApellido" ></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtSegundoApellidoConsultar" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                            <td class="td-label">
                            </td>
                                <td colspan="2" style="text-align: left">
                                    <asp:UpdatePanel runat="server" ID="updBuscarDirector">
                                        <ContentTemplate>
                                            <asp:Button runat="server" ID="btnConsultarDirector" Text="Buscar" CssClass="boton"
                                                OnClick="btnConsultarDirector_Click" OnClientClick="seleccionDirector('false');" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <asp:UpdatePanel runat="server" ID="updDocentes" ChildrenAsTriggers="true" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div id="dialogdocentes" runat="server">
                                    <div class="clear">
                                    </div>
                                    <div class="clear">
                                    </div>
                                    <asp:GridView runat="server" ID="grdDirectores" Width="100%" CssClass="DDGridView"
                                        RowStyle-CssClass="td" HeaderStyle-CssClass="th" AutoGenerateColumns="False"
                                        AllowPaging="True" PageSize="10" OnRowCommand="grdDirectores_RowCommand">
                                        <Columns>
                                            <asp:BoundField DataField="DirectorID" FooterText="ID" SortExpression="DirectorID"
                                                HeaderText="ID" />
                                            <asp:BoundField DataField="Curp" SortExpression="Curp" HeaderText="CURP" />
                                            <asp:BoundField DataField="NombreCompleto" SortExpression="NombreCompleto" HeaderText="Nombre completo" />
                                            <asp:TemplateField HeaderText="Acción" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" ID="btnadd" CommandName="agregar" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"DirectorID") %>'
                                                        ImageUrl="../Images/add-icon.gif" OnClientClick="seleccionDirector('true');" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <div class="ui-state-highlight ui-corner-all">
                                                <p>
                                                    <span class="ui-icon ui-icon-info" style="float: left"></span>La b&uacute;squeda
                                                    no produjo resultados</p>
                                            </div>
                                        </EmptyDataTemplate>
                                        <PagerTemplate>
                                            <asp:GridViewPager runat="server" ID="grdViewPager" SessionName="directores" DataSourceType="DataSet" />
                                        </PagerTemplate>
                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnConsultarDirector" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="line" style="clear: both;">
                </div>
                <asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="boton" OnClick="btnGuardar_Click" />
                <asp:HyperLink ID="HpLnkCancelar" Text="Cancelar" runat="server" NavigateUrl="BuscarEscuelas.aspx"
                    CssClass="boton"></asp:HyperLink>
            </div>
        </div>
    </div>
    <input id="hdnVistaConsultar" type="hidden" />
    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(loadControls);
        prm.add_endRequest(endRequests);

        function loadControls(sender, args) {
            $('.boton').button();
            $('#acordion').accordion({ collapsible: true });
            //validateEscuela();
        }
        function seleccionDirector(valorW) {
            $("#hdnVistaConsultar").val(valorW);

        }
        function endRequests(sender, args) {
            //validateEscuela();
            var vHdn = $("#hdnVistaConsultar").val();
            if (vHdn != '') {
                if (vHdn == 'true')
                    $('#acordion').accordion({ active: -1, collapsible: true, autoHeight: false });
                else
                    $('#acordion').accordion();
            } else {
                $('#acordion').accordion({ active: -1, collapsible: true, autoHeight: false });
            }
        }
    
    </script>
</asp:Content>
