<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgregarContenidoDigitalUC.ascx.cs" Inherits="POV.Web.HerramientasDocente.Pages.Actividades.AgregarContenidoDigitalUC" %>
<script type="text/javascript">
    $(function () {

        $("input[name='<%=RdBtnTipoReferencia.UniqueID %>']").change(function () {
            displayPanelReferencias();
        });

        $.blockUI.defaults.overlayCSS.backgroundColor = "white";
        $.blockUI.defaults.message = '<h1 style="font-size:20px;">Registrando, por favor espere...</h1>';

        displayPanelReferencias();
    });

    function displayPanelReferencias() {
        var rdSelected = $("input[name='<%=RdBtnTipoReferencia.UniqueID %>']:checked").val();
        if (rdSelected == "true") {
            $("#<%=fupArchivoInterno.ClientID %>").replaceWith($("#<%=fupArchivoInterno.ClientID %>").val('').clone(true));
            $(".fileupload-preview").val('');
            $("#panelArchivoInterno").show();
            $("#panelArchivoExterno").hide();
            $("#<%=fupArchivoInterno.ClientID %>").addClass("required");
            $("#<%=TxtReferenciaExterna.ClientID %>").removeClass("required");
        } else if (rdSelected == "false") {
            $("#<%=TxtReferenciaExterna.ClientID %>").val('');
            $("#panelArchivoExterno").show();
            $("#panelArchivoInterno").hide();
            $("#<%=TxtReferenciaExterna.ClientID %>").addClass("required");
            $("#<%=fupArchivoInterno.ClientID %>").removeClass("required");
        }
}
</script>

<div class="form-group">
    <div class="col-sm-2 col-md-2 col-lg-2">
        <label>Clave</label>
    </div>
    <div class="col-sm-6 col-md-4 col-lg-4">
        <asp:TextBox ID="TxtClave" runat="server" CssClass="form-control required" MaxLength="30"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <div class="col-sm-2 col-md-2 col-lg-2">
        <label>Nombre</label>
    </div>
    <div class="col-sm-6 col-md-4 col-lg-4">
        <asp:TextBox ID="TxtNombre" runat="server" CssClass="form-control required" MaxLength="200"></asp:TextBox>
    </div>
</div>
<div class="form-group">
    <div class="col-sm-2 col-md-2 col-lg-2">
        <label>Es descargable</label>
    </div>
    <div class="col-sm-6 col-md-4 col-lg-4">
        <asp:RadioButtonList CssClass="form-control" ID="RdBtnEsDescargable" runat="server" RepeatDirection="Horizontal">
            <asp:ListItem Text="Si" Value="true" Selected="True"></asp:ListItem>
            <asp:ListItem Text="No" Value="false"></asp:ListItem>
        </asp:RadioButtonList>
    </div>
</div>
<div class="container">
    <div class="form-group">
        <asp:Panel ID="Panel1" runat="server" GroupingText="Referencia de contenido digital">
            <div class="container">
                <div class="form-group">
                    <div class="col-lg-1"></div>
                    <div class="col-sm-2 col-md-2 col-lg-2">
                        <label class="control-label">Referencia</label>
                    </div>
                    <div class="col-sm-6 col-md-4 col-lg-4">
                        <asp:RadioButtonList CssClass="form-control" ID="RdBtnTipoReferencia" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="Interna" Value="true" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Externa" Value="false"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div class="form-group" id="panelArchivoInterno">
                    <div class="col-lg-1"></div>
                    <div class="col-sm-2 col-md-2 col-lg-2">
                        <label>Archivo interno</label>
                    </div>

                    <div class="fileupload fileupload-new" data-provides="fileupload">
                        <span class="btn btn-primary btn-file"><span class="fileupload-new">Seleccionar</span>
                            <span class="fileupload-exists">Cambiar</span>
                            <asp:FileUpload runat="server" ID="fupArchivoInterno" /></span>
                        <span class="fileupload-preview"></span>
                        <a href="#" class="close fileupload-exists" data-dismiss="fileupload" style="float: none">×</a>
                    </div>
                </div>
                <div class="form-group" id="panelArchivoExterno">
                    <div class="col-lg-1"></div>
                    <div class="col-lg-2">
                        <label>Referencia externa</label>
                    </div>
                    <div class="col-lg-8">
                        <asp:TextBox CssClass="form-control" ID="TxtReferenciaExterna" runat="server" MaxLength="500" TextMode="MultiLine"
                            Width="100%" Columns="80" Rows="5"></asp:TextBox>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</div>
<div class="form-group">
    <div class="col-lg-2">
        <asp:Label ID="lblInstruccionesContenido" runat="server" Text="Instrucciones"></asp:Label>
    </div>
    <div class="col-lg-8">
        <asp:TextBox CssClass="form-control" ID="txtInstruccionesContenido" runat="server" MaxLength="200"
            TextMode="MultiLine" Width="100%" Rows="3">Bla Bla Bla...</asp:TextBox>
    </div>
</div>
<div class="form-group">
    <div class="modal-footer">
        <asp:Label ID="lblErrorAgregarContenido" runat="server" CssClass="error col-xs-12 col-lg-12"></asp:Label>
        <asp:Button ID="btnAgregarContenidoIns" runat="server" Text="Agregar" OnClick="btnAgregar_OnClick"
            CssClass="button_clip_39215E" />
    </div>
</div>
