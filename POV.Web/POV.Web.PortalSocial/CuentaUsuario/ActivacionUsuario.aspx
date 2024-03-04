<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/DefaultSite.Master" CodeBehind="ActivacionUsuario.aspx.cs" Inherits="POV.Web.PortalSocial.CuentaUsuario.ActivacionUsuario" %>
<asp:Content ContentPlaceHolderID="head" ID="Content1" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="page_content" ID="Content2" runat="server">
    <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>--%>
                            <div class="col-xs-12 modal_titulo_marco_general center-block">Activcaci&oacute;n de usuario</div>
                        </div>
                        <div class="modal-body container_busqueda_general ui-widget-content">
                            <div class="">
                                <div class="row bs-wizard" style="border-bottom: 0;">
                                    <div class="col-md-12" style="padding: 20px 0px 0px 0px">
                                        <div class="col-xs-12">
                                            <div class="col-xs-12 form-group">
                                                <label class="col-sm-4 control-label">Tutorado:</label>
                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="txtNombreAlumno" runat="server" placeholder="Nombre" MaxLength="50" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-xs-12 form-group" style="display:none">
                                                <label class="col-sm-4 control-label">Cr&eacute;dito disponible: </label>
                                                <div class="col-sm-8 center-block">
                                                    <asp:TextBox ID="txtCreditoDisponible" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>

                                            </div>
                                            <div class="col-xs-12 form-group">
                                                <asp:Label ID="lblCantidadCreditos" CssClass="control-label col-sm-4" Text="Cantidad de créditos: " Style="font-weight: bold" runat="server"></asp:Label>
                                                <div class="col-sm-8 center-block">
                                                    <asp:TextBox ID="txtCantidadCreditos" runat="server" Style="min-width: 80px" TextMode="Number" min="0" step="1" CssClass="form-control" placeholder="Introduzca cantidad"></asp:TextBox>
                                                    <asp:Label ID="lblError" CssClass="label-control error_label" runat="server" Style="display: none"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-xs-12 form-group">
                                                <div class="col-xs-12 form-group center-block">
                                                    <div class="col-sm-12 table-responsive">
                                                        <asp:Label ID="lblNoDisponible" CssClass="label-control error_label" runat="server" Style="display: none"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <span class="card-errors"></span>
                                            <div class="col-xs-12 form-group">
                                                <label class="col-sm-4 control-label">
                                                    <span>Nombre del tarjetahabiente</span>
                                                </label>
                                                <div class="col-sm-8">
                                                    <input class="form-control" type="text" size="20" data-conekta="card[name]" />
                                                </div>
                                            </div>
                                            <div class="col-xs-12 form-group">
                                                <label class="col-sm-4 control-label">
                                                    <span>Número de tarjeta de crédito</span>
                                                </label>
                                                <div class="col-sm-8">
                                                    <input  class="form-control" type="text" size="20" data-conekta="card[number]" />

                                                </div>
                                            </div>
                                            <div class="col-xs-12 form-group">
                                                <label class="col-sm-4 control-label">
                                                    <span>CVC</span>
                                                </label>
                                                <div class="col-sm-8">
                                                    <input  class="form-control" type="text" size="4" data-conekta="card[cvc]" />
                                                </div>
                                            </div>
                                            <div class="col-xs-12 form-group">
                                                <label class="col-sm-4 control-label">
                                                    <span>Fecha de expiración (MM/AAAA)</span>
                                                </label>
                                                <div class="col-sm-8">
                                                    <input style="border-radius: 4px"  type="text" size="2" data-conekta="card[exp_month]" />
                                                    <asp:DropDownList runat="server" ID="ddlMesExpiraCard" Enabled="True" style="border-radius:4px">
                                                        <asp:ListItem Value="1">ENERO</asp:ListItem>
                                                        <asp:ListItem Value="2">FEBRERO</asp:ListItem>
                                                        <asp:ListItem Value="3">MARZO</asp:ListItem>
                                                        <asp:ListItem Value="4">ABRIL</asp:ListItem>
                                                        <asp:ListItem Value="5">MAYO</asp:ListItem>
                                                        <asp:ListItem Value="6">JUNIO</asp:ListItem>
                                                        <asp:ListItem Value="7">JULIO</asp:ListItem>
                                                        <asp:ListItem Value="8">AGOSTO</asp:ListItem>
                                                        <asp:ListItem Value="9">SEPTIEMBRE</asp:ListItem>
                                                        <asp:ListItem Value="10">OCTUBRE</asp:ListItem>
                                                        <asp:ListItem Value="11">NOVIEMBRE</asp:ListItem>
                                                        <asp:ListItem Value="12">DICIEMBRE</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span>/</span>
                                                    <input type="text" size="4" style="border-radius: 4px; display:none"/>
                                                    <asp:DropDownList runat="server" ID="ddlYearExpiraCard" style="border-radius: 4px" data-conekta="card[exp_year]">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <button type="submit">Crear token</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <div class="form-group">
                                <div class="col-md-6 pull-right">
                                    <asp:Button type="button" class="btn btn-green btn-md" ID="Button2" runat="server" Text="Asignar" OnClick="Button2_Click" />
                                    <button type="button" class="btn btn-cancel btn-md" id="Button1" data-dismiss="modal">Cancelar</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
</asp:Content>