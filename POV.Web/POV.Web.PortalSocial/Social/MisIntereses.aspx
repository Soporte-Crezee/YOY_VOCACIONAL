<%@ Page Title="YOY - ESTUDIANTE" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MisIntereses.aspx.cs" Inherits="POV.Web.PortalSocial.Social.MisIntereses" %>
<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet" type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>muro.css" rel="stylesheet" type="text/css" />
	<style type="text/css">
        
        .break_words {
            white-space: pre-line;
            word-break: break-all;
        }	
    </style>

	<script type="text/javascript">

		$(document).ready(initPage);

		function initPage() {
			$("#form1").validate({
				rules: {

				},
				messages: {
				}
			});
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="page_content" runat="server">
    <div id="panel-container" class="panel_edicion_perfil">
        <div id="info_perfil">
            <asp:Label ID="LblNombreUsuario" runat="server" Text="" CssClass="tBienvenidaLabel" Style="display:none;"></asp:Label>
		</div>
        <ol class="breadcrumb">
            <li>Mis intereses</li>
        </ol>
        <div class="col-xs-12 col-md-3">
			<div id="user_img" style="margin-top: 10px;" class="profile_img_marco_aspirante">
				<asp:Image runat="server" ID="ImgUser" CssClass="profile_img_aspirante" /><br />
			</div>
		</div>
		<div class="col-xs-12 col-md-9">
			<asp:Label ID="lblError" runat="server" CssClass="error_label"></asp:Label>
            <div class="panel panel-default">
                <div class="panel-heading">
                    Informaci&oacute;n de intereses
                </div>
                <div class="panel-body">
                    <div id="divIntereses" class="" runat="server"></div>
                    <asp:UpdatePanel ID="updIntereses" runat="server">
						<ContentTemplate>
							<div class="table-responsive">
								<asp:GridView ID="grdIntereses" runat="server" CssClass="table table-striped table-bordered table-hover" RowStyle-CssClass="td"
									HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
									    EnableSortingAndPagingCallbacks="True" OnRowCommand="grd_RowCommand"
									OnSorting="grd_Sorting" AllowSorting="true" Visible="false">
									<Columns>
										<asp:BoundField DataField="NombreInteres" HeaderText="Nombre del interés" SortExpression="NombreInteres" ItemStyle-CssClass="break_words" />
									</Columns>
									<EmptyDataTemplate>
										<div class="">
											<p>
												<span class="ui-icon ui-icon-info" style="float: left"></span>La búsqueda no produjo resultados.
											</p>
										</div>																						  
									</EmptyDataTemplate>
									<PagerTemplate>
										<asp:GridViewPager ID="grdViewPager" runat="server" SessionName="dsIntereses" DataSourceType="DataSet" />
									</PagerTemplate>
								</asp:GridView>
							</div>
						</ContentTemplate>
					</asp:UpdatePanel>
                </div>
            </div>
        </div>
	</div>
</asp:Content>
