<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditarPrivilegios.aspx.cs" Inherits="POV.Web.PortalOperaciones.Usuarios.EditarPrivilegios" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    $(document).ready(initPage);
    function initPage() {
        $(".boton").button();
    }

    </script>
</asp:Content>
        
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
       <h3 class="ui-widget-header">
        <asp:HyperLink ID="lnkBack" runat="server">Volver</asp:HyperLink>
        /Editar privilegios</h3>
        
        <div class=" ui-widget-content" style="padding:5px">
        <div class="GroupContent" style="padding-top:2px; padding-bottom:2px">
            <asp:UpdatePanel ID="pnlContent" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h2>Perfiles</h2>
                    <hr />
                    <br />
                    <table align="center" cellspacing="0" cellpadding="0">
                        <tr>
                            <td>
                            <h3>Perfiles disponibles</h3>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td >
                                <h3>Perfiles asignados</h3>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 210px; width: 220px;">
                                <div id='lbperf' style="border: 1px solid #000000; z-index: 102; overflow: auto; width: inherit; height:inherit;">
                                    <asp:ListBox ID="lbPerfiles" runat="server" Width="220px" Height="210px" SelectionMode="Multiple" />
                                </div>
                            </td>
                            <td style="height: 210px; width: 25px;">
                               <table>
                                    <tr>
                                        <td>
                                        <asp:Button ID="btnAgregarPerfil" CssClass="boton" runat="server" OnClick="btnAgregarPerfil_Click" Text=">" ToolTip="Agregar"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        <asp:Button ID="btnQuitarPerfil" CssClass="boton" runat="server" OnClick="btnQuitarPerfil_Click"  Text="<" ToolTip="Quitar"/>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td  style="height: 210px; width: 220px;">
                                <div id='Div2' style="border: 1px solid #000000; z-index: 102;overflow: auto;  width: inherit; height:inherit;">
                                    <asp:ListBox ID="lbPerfilesAsignados" runat="server" Width="220px" Height="210px" SelectionMode="Multiple"/>
                                </div>
                            </td>
                         </tr>
                     </table>
                     <br /><h2>Permisos</h2><hr />
                    <br />
                    <table align="center" cellspacing="0" cellpadding="0">
                        <tr>
                            
                            <td>&nbsp;</td>
                            <td >
                                <h3>Permisos disponibles</h3>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <h3>Permisos asignados</h3>
                            </td>
                        </tr>
                        
                            <td>&nbsp;</td>
                            <td style="background-color:#FFF;">
                                <div style="border: 1px solid #000000; left: 13px; overflow: auto; width: 430px; height: 310px;" align="left">
                                    <asp:TreeView ID="tvPermisosDisponibles" runat="server" ShowCheckBoxes="Leaf" ShowLines="True" ToolTip="Permisos Actuales"
                                    Target="_self">
                                        <SelectedNodeStyle ForeColor="Black" />
                                        <NodeStyle Font-Bold="False" ForeColor="Black" />
                                    </asp:TreeView>
                                </div>
                            </td>
                            <td style="height: 210px; width: 25px;">
                                <table>
                                    <tr>
                                        <td>
                                        <asp:Button ID="ibtnAgregarPermisos" runat="server" OnClick="btnAgregarPermisos_Click" CssClass="boton" Text=">>" ToolTip="Agregar todos"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        <asp:Button ID="ibtnAgregarPermiso" runat="server"  OnClick="btnAgrearPermiso_Click" CssClass="boton" Text=" > " ToolTip="Agregar"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        <asp:Button ID="ibtnQuitarPermio"  runat="server" OnClick="btnQuitarPermiso_Click" CssClass="boton" Text=" < "  ToolTip="Quitar"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        <asp:Button ID="ibtnQuitarPermisos" runat="server" OnClick="btnQuitarPermisos_Click" CssClass="boton" Text="<<"  ToolTip="Quitar todos"/>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="background-color:#FFF;">
                                <div style="border: 1px solid #000000; left: 13px; overflow: auto; width: 430px; height: 310px" align="left">
                                    <asp:TreeView ID="tvPermisosAsignados" runat="server" ShowCheckBoxes="Leaf" ShowLines="True" ToolTip="Permisos Asignados" Target="_self">
                                        <SelectedNodeStyle ForeColor="Black" />
                                        <NodeStyle Font-Bold="False" ForeColor="Black" />
                                    </asp:TreeView>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div class="line"></div>
                    <div >
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="boton" OnClick="btnGuardar_Click" />
                        <asp:HyperLink ID="btnCancelar" CssClass="boton" NavigateUrl="~/Usuarios/BuscarUsuarios.aspx" runat="server">Cancelar</asp:HyperLink>

                    </div>
                    <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
            style="display: none;" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        </div>
        
</asp:Content>
