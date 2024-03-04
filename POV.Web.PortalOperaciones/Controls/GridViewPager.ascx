<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridViewPager.ascx.cs" Inherits="POV.Web.PortalOperaciones.Controls.GridViewPager" %>
<div class="DDPager">
    <span class="DDFloatLeft">
        <asp:ImageButton AlternateText="Primera página" ToolTip="Primera página" ID="ImageButtonFirst" runat="server" ImageUrl="Images/PgFirst.gif"  CommandName="Page" CommandArgument="First" OnClick="First" />
        
        <asp:ImageButton AlternateText="Página anterior" ToolTip="Página anterior" ID="ImageButtonPrev" runat="server" ImageUrl="Images/PgPrev.gif"  CommandName="Page" CommandArgument="Prev" OnClick="Previous" />
        
        <asp:Label ID="LabelPage" runat="server" Text="Página" AssociatedControlID="TextBoxPage" />
        
        <asp:TextBox ID="TextBoxPage" runat="server" Columns="5" AutoPostBack="true" ontextchanged="TextBoxPage_TextChanged" Width="20" CssClass="DDControl" />
        <asp:Label ID="lblDe" runat="server" Text="de" AssociatedControlID="LabelNumberOfPages" />
        <asp:Label ID="LabelNumberOfPages" runat="server" />
        
        <asp:ImageButton AlternateText="Página siguiente" ToolTip="Página siguiente" ID="ImageButtonNext" runat="server" ImageUrl="Images/PgNext.gif"  CommandName="Page" CommandArgument="Next" OnClick="Next" />
        
        <asp:ImageButton AlternateText="Última página" ToolTip="Última página" ID="ImageButtonLast" runat="server" ImageUrl="Images/PgLast.gif" CommandName="Page" CommandArgument="Last" OnClick="Last" BorderStyle="NotSet" />
    </span>
    <span class="DDFloatRight">
        <asp:Label ID="LabelRows" runat="server" Text="Resultados por página:" AssociatedControlID="DropDownListPageSize" />
        <asp:DropDownList ID="DropDownListPageSize" runat="server" AutoPostBack="true" CssClass="DDControl" onselectedindexchanged="DropDownListPageSize_SelectedIndexChanged">
            <asp:ListItem Value="5" />
            <asp:ListItem Value="10" />
            <asp:ListItem Value="15" />
            <asp:ListItem Value="20" />
        </asp:DropDownList>
    </span>
</div>