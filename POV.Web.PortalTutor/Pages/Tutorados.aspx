<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Tutorados.aspx.cs" Inherits="POV.Web.PortalTutor.Pages.Tutorados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="https://conektaapi.s3.amazonaws.com/v0.3.2/js/conekta.js"></script>

    <script type="text/javascript">
        Conekta.setPublishableKey('key_VXUQtCKLAXyZ3S8y6eWmjyA');

        $(document).ready(function () {

            $("#cardNumberCC").keydown(function (e) {
                // Allow: backspace, delete, tab, escape, enter and .
                if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                    // Allow: Ctrl/cmd+A
                    (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                    // Allow: Ctrl/cmd+C
                    (e.keyCode == 67 && (e.ctrlKey === true || e.metaKey === true)) ||
                    // Allow: Ctrl/cmd+X
                    (e.keyCode == 88 && (e.ctrlKey === true || e.metaKey === true)) ||
                    // Allow: home, end, left, right
                    (e.keyCode >= 35 && e.keyCode <= 39)) {
                    // let it happen, don't do anything

                    return;
                }
                // Ensure that it is a number and stop the keypress
                if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                    e.preventDefault();
                }
            });

            $("#cardCVCCC").keydown(function (e) {
                // Allow: backspace, delete, tab, escape, enter and .
                if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                    // Allow: Ctrl/cmd+A
                    (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                    // Allow: Ctrl/cmd+C
                    (e.keyCode == 67 && (e.ctrlKey === true || e.metaKey === true)) ||
                    // Allow: Ctrl/cmd+X
                    (e.keyCode == 88 && (e.ctrlKey === true || e.metaKey === true)) ||
                    // Allow: home, end, left, right
                    (e.keyCode >= 35 && e.keyCode <= 39)) {
                    // let it happen, don't do anything

                    return;
                }
                // Ensure that it is a number and stop the keypress
                if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                    e.preventDefault();
                }
            });

            $("#cardNumberMSS").keydown(function (e) {
                // Allow: backspace, delete, tab, escape, enter and .
                if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                    // Allow: Ctrl/cmd+A
                    (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                    // Allow: Ctrl/cmd+C
                    (e.keyCode == 67 && (e.ctrlKey === true || e.metaKey === true)) ||
                    // Allow: Ctrl/cmd+X
                    (e.keyCode == 88 && (e.ctrlKey === true || e.metaKey === true)) ||
                    // Allow: home, end, left, right
                    (e.keyCode >= 35 && e.keyCode <= 39)) {
                    // let it happen, don't do anything

                    return;
                }
                // Ensure that it is a number and stop the keypress
                if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                    e.preventDefault();
                }
            });

            $("#cardCVCMSS").keydown(function (e) {
                // Allow: backspace, delete, tab, escape, enter and .
                if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                    // Allow: Ctrl/cmd+A
                    (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                    // Allow: Ctrl/cmd+C
                    (e.keyCode == 67 && (e.ctrlKey === true || e.metaKey === true)) ||
                    // Allow: Ctrl/cmd+X
                    (e.keyCode == 88 && (e.ctrlKey === true || e.metaKey === true)) ||
                    // Allow: home, end, left, right
                    (e.keyCode >= 35 && e.keyCode <= 39)) {
                    // let it happen, don't do anything

                    return;
                }
                // Ensure that it is a number and stop the keypress
                if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                    e.preventDefault();
                }
            });
        });

        var whoCall = '';

        var conektaSuccessResponseHandler = function (token) {
            if (whoCall != '') {
                if (whoCall == 'CreditCard') {
                    var $form = $("#card-form-cc");
                }
                else {
                    var $form = $("#card-form-mss");
                }
            }
            //Inserta el token_id en la forma para que se envíe al servidor
            $("#txtToken").val(token.id);
            $("#<%=hdnToken.ClientID %>").val(token.id);
            if ($("#<%=hdnToken.ClientID %>").val() != '' && whoCall != '') {
                if (whoCall == 'CreditCard') {
                    $("#<%=btnPagarCC.ClientID %>").click();
                }
                else {
                    $("#<%=btnPagarMSS.ClientID %>").click();
                }
            }
        };
        var conektaErrorResponseHandler = function (response) {
            if (whoCall != '') {
                if (whoCall == 'CreditCard') {
                    var $form = $("#card-form-cc");
                }
                else {
                    var $form = $("#card-form-mss");
                }
            }
            $form.find(".card-errors").text(response.message_to_purchaser);
            $form.find(".card-errors").show();
        };

        //jQuery para que genere el token después de dar click en submit
        $(function () {
            $("#btnCreaTokenCC").on("click", function () {
                var $form = $("#card-form-cc");
                whoCall = 'CreditCard';
                Conekta.token.create($form, conektaSuccessResponseHandler, conektaErrorResponseHandler);
            });
            $("#btnCreaTokenMSS").on("click", function () {
                var $form = $("#card-form-mss");
                whoCall = 'MesesSinIntereses';
                Conekta.token.create($form, conektaSuccessResponseHandler, conektaErrorResponseHandler);
            });
        });

        function showMessage(text) {
            var api = new MessageApi();
            api.CreateMessage(text, "ERROR");
            resetHiden();
            api.Show();
        }

        function CheckAllEmp(Checkbox) {
            var GridVwHeaderChckbox = document.getElementById("<%=gvTutorados.ClientID %>");
            for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                var chechBoxChecked = GridVwHeaderChckbox.rows[i].cells[3].getElementsByTagName("INPUT")[0];
                if (chechBoxChecked.disabled == false) {
                    chechBoxChecked.checked = Checkbox.checked;
                }
                else {
                    chechBoxChecked.style.display = 'none';
                }
            }
        }
        
        $(function () {
            $("#btnClose").on("click", function () {
                $('#<%=TxtEmailAspirante.ClientID %>').val('');
                $('#<%=TxtEmailAspirante.ClientID %>').removeAttr('required');
            });

            $("#btnModal").button().on("click", function () {
                $('#<%=TxtEmailAspirante.ClientID %>').val('');
                $('#<%=TxtEmailAspirante.ClientID %>').attr('required', '');
            });
            $("#btnAsignarPaquete").on("click", function () {

            });

            
        });

        function viewModal(ModalType) {
            setTimeout(function () {
                switch (ModalType) {
                    case 1:
                        var fecha = new Date();
                        var anio = fecha.getFullYear();
                        $("#cardNameCC").val('');
                        $("#cardNumberCC").val('');
                        $("#cardCVCCC").val('');
                        $('#<%=ddlVenceMesCC.ClientID %>').val('1');
                        $("#btnPayCredit").click();
                        break;
                    case 2:
                        $("#btnPayOxxo").click();
                        break;
                    case 3:
                        $("#cardNameMSS").val('');
                        $("#cardNumberMSS").val('');
                        $("#cardCVCMSS").val('');
                        $('#<%=ddlVenceMesMSS.ClientID %>').val('1');
                        $("#btnPayMonths").click();
                        break;
                }
            }, 500);
        }
    </script>

    <style>
        .odd:nth-of-type(odd) {
            background: #e0e0e0;
        }

        .hidden_col {
            display: none;
            width: 0px !important;
            margin: 0px;
            padding: 0px !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="page_content" runat="server">
    <!-- Cabecera -->
    <div class="panel" style="margin-top: 40px">
        <div class="panel-heading">
            Paso 1 - Seleccionar a tutorados para pago de derecho
                    <button type="button" id="btnModal" class="btn pull-right btn-cancel" data-toggle="modal" data-target=".dialog-md">Invitar hijo</button>
        </div>
        <div class="panel-body">
            <div class="col-md-12 col-md-6">
                <div class="form-group">
                    <label class="col-md-4 control-label">Nombre del hijo</label>
                    <div class="col-md-7">
                        <asp:TextBox ID="TxtNombreTutorado" MaxLength="180" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-1">
                        <asp:Button runat="server" ID="btnBuscarAlumno" CssClass="btn-green" Text="Buscar" OnClick="btnBuscarAlumno_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- tutorados -->
    <div class="table-responsive">
        <asp:GridView AutoGenerateColumns="False" runat="server" ID="gvTutorados"
            BorderWidth="0px" BorderStyle="Solid" HeaderStyle-Height="30px" RowStyle-Height="25px"
            RowStyle-CssClass="ui-widget-content td" HeaderStyle-CssClass="table table-bordered table-striped"
            CssClass="table table-bordered table-striped"
            RowStyle-BackColor="Transparent" AlternatingRowStyle-BackColor="White" OnRowCommand="gvTutorados_RowCommand" OnRowDataBound="gvTutorados_RowDataBound">
            <AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
            <HeaderStyle CssClass="th tabla_titulo_marco_general" Height="30px"></HeaderStyle>
            <RowStyle BackColor="Transparent" CssClass="ui-widget-content td" Height="25px"></RowStyle>
            <Columns>
                <asp:BoundField DataField="Alumno.AlumnoID" HeaderText="">
                    <HeaderStyle CssClass="hidden_col"></HeaderStyle>
                    <ItemStyle CssClass="break_works hidden_col"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Alumno.EstatusPago" HeaderText="">
                    <HeaderStyle CssClass="hidden_col"></HeaderStyle>
                    <ItemStyle CssClass="break_works hidden_col"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="Nombre" DataField="Alumno.NombreCompletoAlumno">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkHeader" runat="server" onclick="CheckAllEmp(this);" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSeleccionado" runat="server" class="CheckUncheck" />
                        <asp:Label ID="Label4" runat="server" Text="ESTADO" Style="display: none"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="20px" />
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                        <div class="ui-state-highlight ui-corner-all">
                            <p>
                                <span class="ui-icon ui-icon-info" style="float: left"></span>La búsqueda no produjo resultados
                            </p>
                        </div>
                    </EmptyDataTemplate>
        </asp:GridView>
    </div>


    <div class="panel" style="padding: 20px 0px 0px 0px">
        <div class="panel-heading">
            Paso 2 - Selecionar forma de pago
        </div>
        <div class="panel-body">

            <div style="padding: 35px 0 0 0" class="col-md-12">
                <h2>
                    <span style="font-size: 1.0em">Formas de pago con cargo directo</span>
                </h2>
                <div class="table table-striped">
                    <div class="col-md-12 odd" style="border-top: .5px dotted; min-height: 80px!important">
                        <div class="col-md-3 odd" style="padding-top: 13px">
                            <img src="../images/methods_visa_mc.png" style="cursor: pointer; width: 220px; height: 54px" />
                        </div>
                        <div class="col-md-7" style="padding-top: 28px;">
                            <span>Aplica de manera inmediata la suscripción</span>
                        </div>
                        <div class="col-md-2" style="padding-top: 18px; text-align: center">
                            <asp:Button runat="server" ID="btnPagoTarjetas" CssClass="btn-green" Text="Pagar" OnClick="btnPagoTarjetas_Click" />
                        </div>
                    </div>
                    <div class="col-md-12" style="border-top: .5px dotted; border-bottom: .5px dotted; min-height: 80px!important">
                        <div class="col-md-3" style="padding-top: 13px">
                            <img src="../images/methods_oxxopay.png" style="cursor: pointer; width: 220px; height: 54px" />
                        </div>
                        <div class="col-md-7" style="padding-top: 14px;">
                            <span>Generar referencia para pago.</span>
                            <span>La activación del usuario se realizará entre 48 y 72 horas después de realizado el pago.</span>
                        </div>
                        <div class="col-md-2" style="padding-top: 18px; text-align: center">
                            <asp:Button runat="server" ID="btnPagoOxxo" CssClass="btn-green" Text="Generar" OnClick="btnPagoOxxo_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <div style="padding: 35px 0 0 0" class="col-md-12">
                <h2>
                    <span style="font-size: 1.0em">Formas de pago a meses sin intereses</span>
                </h2>
                <div class="table table-striped">
                    <div class="col-md-12 odd" style="border-top: .5px dotted; border-bottom: .5px dotted; min-height: 80px!important">
                        <div class="col-md-3 odd" style="padding-top: 13px">
                            <img src="../images/methods_visa_mc.png" style="cursor: pointer; width: 220px; height: 54px" />
                        </div>
                        <div class="col-md-7" style="padding-top: 28px;">
                            <span>Aplica de manera inmediata la suscripción</span>
                        </div>
                        <div class="col-md-2" style="padding-top: 18px; text-align: center">
                            <asp:Button runat="server" ID="btnPagoMeses" CssClass="btn-green" Text="Pagar" OnClick="btnPagoMeses_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Ventana modal para el envío de correo-->
    <!-- Elementos para evitar el cierre de la ventana modal al hacer clic fuera de ella  (data-keyboard="false" data-backdrop="static") -->
    <div class="modal fade dialog-md" tabindex="-1"
        data-keyboard="false" data-backdrop="static"
        role="dialog" aria-labelledby="ventanaModalLabel" id="ventanaModal1">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="col-md-12 modal_titulo_marco_general panel-heading center-block">Informaci&oacute;n de mi hijo</div>
                </div>
                <div class="modal-body container_busqueda_general ui-widget-content">
                    <div class="">
                        <div class="row bs-wizard" style="border-bottom: 0;">
                            <div class="col-md-12" style="padding: 20px 0px 0px 0px">
                                <div class="col-md-12">
                                    <div class="col-md-12 form-group">
                                        <label class="col-md-4 control-label">Correo electr&oacute;nico</label>
                                        <div class="col-md-8">
                                            <asp:TextBox ID="TxtEmailAspirante" runat="server" placeholder="email@email.com" MaxLength="100" CssClass="form-control" TextMode="Email"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="form-group">
                        <div class="row pull-right">
                            <asp:Button runat="server" ID="BtnEnviar" CssClass="btn btn-green btn-md" Text="Enviar invitación" OnClick="BtnEnviar_Click" />
                            <button type="button" class="btn btn-cancel btn-md" id="btnClose" data-dismiss="modal">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Pago con tarjeta de credito/debito) -->
    <button type="button" id="btnPayCredit" class="button_clip_39215E" data-toggle="modal" style="display: none" data-target=".dialog-md-2">ooooooooooo</button>
    <div class="modal fade dialog-md-2" tabindex="-1"
        data-keyboard="false" data-backdrop="static"
        role="dialog" aria-labelledby="ventanaModalLabel" id="ventanaModal2">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="col-md-12 modal_titulo_marco_general panel-heading center-block">Pago de derecho (Tarjetas de crédito/débito)</div>
                </div>
                <div class="modal-body container_busqueda_general ui-widget-content" id="card-form-cc">
                    <div class="">
                        <div class="row bs-wizard" style="border-bottom: 0;">
                            <div class="col-md-12" style="padding: 20px 0px 0px 0px">
                                <div class="col-md-12">
                                    <div class="col-md-12 form-group">
                                        <label class="col-md-5 control-label">Tutorado(s) seleccionado(s):</label>
                                        <div class="col-md-7">
                                            <asp:TextBox ID="txtAlumnosSeleccionados" runat="server" MaxLength="180" Style="min-width: 80px; text-align: right" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-12 form-group">
                                        <label class="col-md-5 control-label">Costo unitario: </label>
                                        <div class="col-md-7 center-block">
                                            <asp:TextBox ID="txtCostoUnitario" runat="server" Style="min-width: 80px; text-align: right" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>

                                    </div>
                                    <div class="col-md-12 form-group">
                                        <asp:Label ID="lblTotalPagar" CssClass="control-label col-md-5" Text="Total a pagar: " Style="font-weight: bold" runat="server"></asp:Label>
                                        <div class="col-md-7 center-block">
                                            <asp:TextBox ID="txtTotalPagar" runat="server" Style="min-width: 80px; text-align: right" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-md-12 form-group">
                                        <div class="row">
                                            <div class="col-md-12" style="margin-bottom:5px">
                                                <div class="card-errors" style="display:none;color: darkred; border: 1px solid darkred; padding: 3px; background: rgba(255,0,0,0.1);"></div>
                                            </div>
                                        </div>
                                        <label class="col-md-5 control-label">
                                            <span>Nombre del titular</span>
                                        </label>
                                        <div class="col-md-7">
                                            <input id="cardNameCC" class="form-control" placeholder="Nombre del titular" type="text" size="20" maxlength="20" data-conekta="card[name]" />
                                        </div>
                                    </div>
                                    <div class="col-md-12 form-group">
                                        <label class="col-md-5 control-label">
                                            <span>Número de tarjeta de crédito</span>
                                        </label>
                                        <div class="col-md-7">
                                            <input id="cardNumberCC" class="form-control" placeholder="ej: 5105105105105100" type="text" size="20" maxlength="20" data-conekta="card[number]" />
                                        </div>
                                    </div>
                                    <div class="col-md-12 form-group">
                                        <label class="col-md-5 control-label">
                                            <span>Código de seguridad</span>
                                        </label>
                                        <div class="col-md-7">
                                            <input id="cardCVCCC" class="form-control" placeholder="ej: 123" type="text" size="4" maxlength="4" data-conekta="card[cvc]" />
                                        </div>
                                    </div>
                                    <div class="col-md-12 form-group">
                                        <label class="col-md-5 control-label">
                                            <span>Fecha de expiración</span>
                                        </label>
                                        <div class="col-md-7">
                                            <asp:DropDownList runat="server" ID="ddlVenceMesCC" Enabled="True" Style="border-radius: 4px" data-conekta="card[exp_month]">
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
                                            <input id="txtTokenCreditCard" type="text" size="4" style="border-radius: 4px; display: none" />
                                            <asp:DropDownList runat="server" ID="ddlVenceAnioCC" Style="border-radius: 4px" data-conekta="card[exp_year]">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <input type="text" id="txtToken" class="form-control" style="display: none" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="form-group">
                        <div class="col-md-6 pull-right">
                            <button id="btnCreaTokenCC" type="button" class="btn btn-green btn-md" style="width: 89px">Pagar</button>
                            <asp:Button type="submit" class="btn btn-green btn-md" ID="btnPagarCC" runat="server" Text="Pagar" OnClick="btnPagarCC_Click" Style="display: none" />
                            <button type="button" class="btn btn-cancel btn-md" id="Button1" data-dismiss="modal" style="width: 89px">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Pago con OXXO -->
    <button type="button" id="btnPayOxxo" class="button_clip_39215E" data-toggle="modal" style="display: none" data-target=".dialog-md-3">ooooooooooo</button>
    <div class="modal fade dialog-md-3" tabindex="-1"
        data-keyboard="false" data-backdrop="static"
        role="dialog" aria-labelledby="ventanaModalLabel" id="ventanaModal3">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="col-md-12 modal_titulo_marco_general center-block panel-heading">Pago de derecho (OXXO)</div>
                </div>
                <div class="modal-body container_busqueda_general ui-widget-content">
                    <div class="">
                        <div class="row bs-wizard" style="border-bottom: 0;">
                            <div class="col-md-12" style="padding: 20px 0px 0px 0px">
                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <div class="col-md-12 form-group">
                                            <label class="col-md-4 control-label">Cantidad:</label>
                                            <div class="col-md-8">
                                                <asp:TextBox ID="txtCantidadOXXO" runat="server" MaxLength="50" Style="min-width: 80px; text-align: right" CssClass="form-control" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-12 form-group">
                                            <label class="col-md-4 control-label">Costo: </label>
                                            <div class="col-md-8 center-block">
                                                <asp:TextBox ID="txtCostoUnitarioOXXO" runat="server" Style="min-width: 80px; text-align: right" CssClass="form-control" Enabled="false"></asp:TextBox>
                                            </div>

                                        </div>
                                        <div class="col-md-12 form-group">
                                            <asp:Label ID="Label1" CssClass="control-label col-md-4" Text="Total a pagar: " Style="font-weight: bold" runat="server"></asp:Label>
                                            <div class="col-md-8 center-block">
                                                <asp:TextBox ID="txtTotalPagaOXXO" runat="server" Style="min-width: 80px; text-align: right" CssClass="form-control" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-12 form-group">
                                            <label class="col-md-4 control-label">
                                                Número de referencia&nbsp;
                                            </label>
                                            <div class="col-md-8">
                                                <asp:TextBox ID="txtReferenciaOXXO" runat="server" MaxLength="50" CssClass="form-control" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="col-md-12 form-group">                                            
                                            <div class="text-center">
                                                <span><strong>La ficha con la referencia ya fue enviada a su correo</strong></span>
                                            </div>
                                            <div style="margin-bottom:15px"></div>
                                            <asp:Label ID="lblInstrucciones" runat="server">
                                                        INSTRUCCIONES: <br />
                                                        <br />
                                                        1.- Acude a la tienda OXXO más cercana.<br />
                                                        2.- Indica en la caja que quieres realizar un pago de OXXO pay.<br />
                                                        3.- Indica al cajero el número de referencia.<br />
                                                        4.- Realiza el pago correspondiente en efectivo.<br />
                                                        5.- Posteriormente, el cajero te entregará un comprobante impreso; en él podrás verificar que se haya realizado tu pago.<br />
                                                            Conserva el comprobante.<br />
                                                        <br />
                                                        <strong>NOTA: El uso de este servicio tiene costo adicional propio de OXXO.</strong><br />
                                            </asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="form-group">
                        <div class="col-md-6 pull-right">
                            <button type="button" class="btn btn-cancel btn-md" id="Button4" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Pago a meses sin intereses -->
    <button type="button" id="btnPayMonths" class="button_clip_39215E" data-toggle="modal" style="display: none" data-target=".dialog-md-4">ooooooooooo</button>
    <div class="modal fade dialog-md-4" tabindex="-1"
        data-keyboard="false" data-backdrop="static"
        role="dialog" aria-labelledby="ventanaModalLabel" id="ventanModal4">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="col-md-12 modal_titulo_marco_general center-block panel-heading">Pago de derecho (Meses sin intereses)</div>
                </div>
                <div class="modal-body container_busqueda_general ui-widget-content" id="card-form-mss">
                    <div class="">
                        <div class="row bs-wizard" style="border-bottom: 0;">
                            <div class="col-md-12" style="padding: 20px 0px 0px 0px">
                                <div class="col-md-12">
                                    <div class="col-md-12 form-group">
                                        <label class="col-md-5 control-label">Tutorado(s) seleccionado(s):</label>
                                        <div class="col-md-7">
                                            <asp:TextBox ID="txtCantidaMSS" runat="server" MaxLength="50" Style="min-width: 80px; text-align: right" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-12 form-group">
                                        <label class="col-md-5 control-label">Costo unitario: </label>
                                        <div class="col-md-7 center-block">
                                            <asp:TextBox ID="txtCostoUnitarioMSS" runat="server" Style="min-width: 80px; text-align: right" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>

                                    </div>
                                    <div class="col-md-12 form-group">
                                        <asp:Label ID="Label3" CssClass="control-label col-md-5" Text="Total a pagar: " Style="font-weight: bold" runat="server"></asp:Label>
                                        <div class="col-md-7 center-block">
                                            <asp:TextBox ID="txtTotalPagoMSS" runat="server" Style="min-width: 80px; text-align: right" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12" style="margin-bottom:5px">
                                            <div class="card-errors" style="display:none;color: darkred; border: 1px solid darkred; padding: 3px; background: rgba(255,0,0,0.1);"></div>
                                        </div>
                                    </div>
                                    <div class="col-md-12 form-group">

                                        <label class="col-md-5 control-label">
                                            <span>Nombre del titular</span>
                                        </label>
                                        <div class="col-md-7">
                                            <input class="form-control" type="text" size="20" placeholder="Nombre del titular" maxlength="20" data-conekta="card[name]" />
                                        </div>
                                    </div>
                                    <div class="col-md-12 form-group">
                                        <label class="col-md-5 control-label">
                                            <span>Número de tarjeta de crédito</span>
                                        </label>
                                        <div class="col-md-7">
                                            <input id="cardNumberMSS" class="form-control" type="text" placeholder="ej: 5105105105105100" size="20" maxlength="20" data-conekta="card[number]" />

                                        </div>
                                    </div>
                                    <div class="col-md-12 form-group">
                                        <label class="col-md-5 control-label">
                                            <span>Código de seguridad</span>
                                        </label>
                                        <div class="col-md-7">
                                            <input id="cardCVCMSS" class="form-control" type="text" placeholder="ej: 123" size="4" maxlength="4" data-conekta="card[cvc]" />
                                        </div>
                                    </div>
                                    <div class="col-md-12 form-group">
                                        <label class="col-md-5 control-label">
                                            <span>Fecha de expiración</span>
                                        </label>
                                        <div class="col-md-7">
                                            <asp:DropDownList runat="server" ID="ddlVenceMesMSS" Enabled="True" Style="border-radius: 4px" data-conekta="card[exp_month]">
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
                                            <asp:DropDownList runat="server" ID="ddlVenceAnioMSS" Style="border-radius: 4px" data-conekta="card[exp_year]">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="col-md-5">
                                            <label class="control-label">Plazo</label>
                                        </div>
                                        <div class="col-md-7">
                                            <asp:DropDownList runat="server" ID="ddlPlazo" Enabled="True" Style="border-radius: 4px">
                                                <asp:ListItem Value="3">3 MESES</asp:ListItem>
                                                <asp:ListItem Value="6">6 MESES</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="form-group">
                        <div class="col-md-6 pull-right">
                            <button id="btnCreaTokenMSS" type="button" class="btn btn-green btn-md">Pagar</button>
                            <asp:Button type="button" class="btn btn-green btn-md" ID="btnPagarMSS" runat="server" Text="Asignar" OnClick="btnPagarMSS_Click" Style="display: none" />
                            <button type="button" class="btn btn-cancel btn-md" id="Button7" data-dismiss="modal">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnIDReferenciaOXXO" runat="server" />
    <asp:HiddenField ID="hdnToken" runat="server" />
</asp:Content>
