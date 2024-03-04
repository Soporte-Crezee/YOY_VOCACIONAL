<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SeleccionarCarrera.aspx.cs" Inherits="POV.Web.PortalSocial.PortalAlumno.SeleccionarCarrera" %>

<%--@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp"--%>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>YOY - ESTUDIANTE</title>
    <link href="~/Styles/Default.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/Talentos.css" rel="stylesheet" type="text/css" />
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet" type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/bootstrap.js")%>" type="text/javascript"></script>
    <style type="text/css">
        .break_words {
            white-space: pre-line;
            word-break: break-all;
        }

        .button {
            border: none;
            background: #3a7999;
            color: #f2f2f2;
            padding: 10px;
            font-size: 18px;
            border-radius: 5px;
            position: relative;
            top: 13px;
            left: 1210px;
            box-sizing: border-box;
            transition: all 500ms ease;
        }


        .hidden_col {
            display: none;
            width: 0px !important;
            margin: 0px;
            padding: 0px !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="background-color: #c8c6e2;">

        <asp:ScriptManager ID="scrip" runat="server"></asp:ScriptManager>
        <div style="background-color:#fff;">
            <img src="../Images/SOV_VOCAREER1.png" />
        </div>
        <div style="background-color: #c8c6e2;" class="col-xs-12 col-md-12">
            <div style="background-color: #c8c6e2;" class="col-xs-12 col-md-12">
                <br />
                <h1 class="ui-widget-header" style="background-color: #77aea9;"></h1>


                <div class="col-xs-12 form-group">
                    <div class="col-xs-12 col-md-3"></div>
                    <div class="col-xs-12 col-md-12">
                        <h3>
                            <label class="col-sm-8 control-label">Seleccionar carrera</label></h3>
                        <div>
                            <div class="col-sm-12">
                                <!-- PRUEBAS GRATIS REALIZADAS-->
                                <div class="col-xs-12 form-group background_div_contenedor">
                                    <asp:UpdatePanel ID="updPruebas" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvCarreras" runat="server" AutoGenerateColumns="false"
                                                EmptyDataText="No se encontraron resultados" CssClass="SCGridView" RowStyle-CssClass="td"
                                                HeaderStyle-CssClass="th" Width="100%" OnSorting="gvCarreras_Sorting" AllowSorting="true"
                                                AllowPaging="true" OnPageIndexChanging="gvCarreras_PageIndexChanging" OnPageIndexChanged="gvCarreras_PageIndexChanged"
                                                DataKeyNames="isSelected">
                                                <Columns>
                                                    <asp:BoundField DataField="UniversidadID" HeaderText="" ItemStyle-CssClass="break_works hidden_col" HeaderStyle-CssClass="hidden_col"></asp:BoundField>
                                                    <asp:BoundField DataField="CarreraID" HeaderText="" ItemStyle-CssClass="break_works hidden_col" HeaderStyle-CssClass="hidden_col"></asp:BoundField>
                                                    <asp:BoundField DataField="Carrera" HeaderText="Carrera" SortExpression="Carrera" HeaderStyle-BackColor="#b1b3b4"></asp:BoundField>
                                                    <asp:BoundField DataField="Universidad" HeaderText="Universidad" SortExpression="Universidad" HeaderStyle-BackColor="#b1b3b4"></asp:BoundField>

                                                    <asp:BoundField DataField="Direccion" HeaderText="Direccion" HeaderStyle-BackColor="#b1b3b4"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="Seleccionar" HeaderStyle-BackColor="#b1b3b4">
                                                        <ItemTemplate>
                                                            <asp:CheckBox runat="server" ID="chkSeleccionado" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="isSelected" HeaderText="" ItemStyle-CssClass="break_works hidden_col" HeaderStyle-CssClass="hidden_col"></asp:BoundField>
                                                </Columns>>
                                                <HeaderStyle CssClass="th" />
                                                <RowStyle CssClass="td" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </div>

                            </div>
                        </div>
                        <div class="col-sm-12">
                            <div class="col-xs-10"></div>
                            <div class="col-xs-2">
                                <div class="col-xs-offset-3"></div>
                                <div class="col-xs-offset-3"></div>
                                <div class="col-xs-offset-2">
                                    <asp:Button ID="btnGuardar" CssClass="button_clip_39215E" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
                                </div>

                            </div>

                        </div>

                    </div>
                    <div class="col-xs-12 col-md-3"></div>

                </div>

            </div>
        </div>

    </form>
</body>
</html>
