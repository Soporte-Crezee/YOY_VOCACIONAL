<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BuscarPlantilla.aspx.cs" Inherits="POV.Web.PortalOperaciones.CatalogoPlantillas.BuscarPlantilla" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/gridview.css" rel="stylesheet" type="text/css" />
            <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            $(".boton").button();
            $(".boton_search").button({
                icons: {
                    primary: "ui-icon-search"
                }
            });
            $("#frmMain").validate();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <h3 class="ui-widget-header">
        Cat&aacute;logo de plantillas lúdicas</h3>
    <div class="finder ui-widget-content">
        <table class="finder">
            <tr>
                <td class="label">
                    <asp:Label ID="lblNombre" runat="server" Text="Nombre"  ></asp:Label>
                </td>
                <td class="input">
                    <asp:TextBox ID="txtNombre" MaxLength="200"  runat="server" CssClass="textoEnunciado"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="label">
                    <asp:Label ID="lblPredeterminado" runat="server" Text="Es predeterminado"  ></asp:Label>
                </td>
                <td class="input">
                     <asp:DropDownList ID="ddlEsPredeterminado" runat="server">
                          <asp:ListItem Value="" Text="Seleccionar" Selected="true"></asp:ListItem>
                        <asp:ListItem Value="true" Text="SI" ></asp:ListItem>
                        <asp:ListItem Value="false" Text="NO"></asp:ListItem>
                      </asp:DropDownList>
                </td>
             </tr>
            <tr>
                  <td></td>
                  <td >
                      <asp:Button ID="btnConsultar" runat="server" Text="Buscar" CssClass="boton_search" OnClick="btnConsultar_Click" />
                  </td>
              </tr>
        </table>
     </div>
         <div id="PnlCreate" class="nuevo" runat="server" visible="false">
            <a href = "RegistrarPlantillaLudica.aspx" runat="server" id="lnkNuevaPlantillaLudica" class="boton"><span class=" ui-icon ui-icon-circle-plus"
                style="display: inline-block; vertical-align: middle; margin-top: -5px;"></span>
                <asp:Label ID="lblNuevaPlantillaLudica" runat="server" Text="Agregar nueva plantilla lúdica"></asp:Label>
            </a>
        </div>
        <asp:UpdatePanel ID="updPlantillasLudicas" runat="server">
         <ContentTemplate>
          <asp:GridView ID="grdPlantillasLudicas" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                    HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                    Width="100%" OnRowCommand="grdPlantillasLudicas_RowCommand" EnableSortingAndPagingCallbacks="True"
                    OnRowDataBound="grdPlantillasLudicas_DataBound" OnSorting="grdPlantillasLudicas_Sorting" 
                    Visible="false" DataKeyNames="PlantillaLudicaId" AllowSorting="True"  
                    SortedAscendingHeaderStyle-VerticalAlign="Top" BorderStyle="none" OnPageIndexChanging="grdPlantillasLudicas_PageIndexChanging">
                <Columns>
                    <asp:BoundField HeaderText="Id" DataField="PlantillaLudicaId" Visible="False">
                        <HeaderStyle Width="20%"/>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Nombre" DataField="Nombre">
                        <HeaderStyle   />
                    </asp:BoundField> 
                   <asp:TemplateField >
                    <HeaderTemplate >
                        ¿Es predeterminado?
                    </HeaderTemplate>
                    <ItemTemplate >
                        <%# DataBinder.Eval(Container, "DataItem.EsPredeterminado").ToString().Replace("True", "Si").Replace("False", "No") %>
                    </ItemTemplate>
                    <HeaderStyle   />
                    <ItemStyle HorizontalAlign="Center" Width="100px"/>
                </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="../images/edit-button.png" ToolTip="Editar" CommandName="editar"
                                CommandArgument='<%# Eval("PlantillaLudicaId")%>' />
                            <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="../images/minus-button.png" ToolTip="Eliminar" CommandName="eliminar"
                                CommandArgument='<%# Eval("PlantillaLudicaId")%>' />
                            <asp:ImageButton ID="btnSeleccionar" runat="server" ImageUrl="../images/tick.png" ToolTip="Seleccionar" CommandName="eliminar" Visible="false"
                                CommandArgument='<%# Eval("PlantillaLudicaId")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
               <EmptyDataTemplate>
                        <div class="ui-state-highlight ui-corner-all">
                            <p>
                                <span class="ui-icon ui-icon-info" style="float: left"></span>La búsqueda no produjo
                                resultados</p>
                        </div>
                    </EmptyDataTemplate>


                <SortedAscendingHeaderStyle VerticalAlign="Top"></SortedAscendingHeaderStyle>
            </asp:GridView>
         </ContentTemplate>
        </asp:UpdatePanel>

</asp:Content>
