using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

namespace POV.Web.PortalSocial.AppCode.Controls
{
    [DefaultProperty("Selected")]
    [ToolboxData("<{0}:MenuLateralDocente runat=server></{0}:MenuLateralDocente>")]
    public class MenuLateralDocente : Menu
    {
        protected override void RenderContents(HtmlTextWriter output)
        {
            var root = this.MenuItems;
            output.Write(@"<input id=""selected-panel-menu"" type=""hidden"" value=""" +  this.SelectedItem + @"""/>");
            output.Write(@"<div id=""menu-accordion-docente"" class=""menu"">");

            RecursiveRender(output, root, 0);

            output.Write(@"</div>");
        }

        private void RecursiveRender(HtmlTextWriter output, MenuItem item, int depth)
        {
            if (depth > 0) // Skip root node 
            {
                if (depth == 1)
                {
                    output.Write("<h3>");  // main menu
                    output.Write(@"<a href=""" + item.Link + @""">");
                    output.Write(item.Item);
                    output.Write("</a>");
                    output.Write("</h3>"); 
                    output.Write("<div>");
                    output.Write("<ul>");

                    // Recursively iterate over its children.
                    for (int i = 0; i < item.Children.Count; i++)
                    {
                        RecursiveRender(output, item.Children[i], depth + 1);
                    }
                    output.Write("</ul>");
                    output.Write("</div>");
                    

                    return;
                }
                else
                {
                    output.Write(@"<li class='item-option' style='background:#FF9E19 !important;'>");  // sub menu
                    output.Write(@"<a href=""" + item.Link + @""" style='color:#FFF;'>");
                    output.Write(@"<i class=""icon " + item.Icon + @""" style='margin-right: 5px;'></i>");
                    if (item.Item == SelectedItem)  // selected item
                        output.Write(@"<span style=""font-weight:bold;"">" + item.Item + "</span>");
                    else
                        output.Write(item.Item);  // unselected item.

                    output.Write("</a>");
                }
                
            }
            else
                for (int i = 0; i < item.Children.Count; i++)
                {
                    RecursiveRender(output, item.Children[i], depth + 1);
                }
            
        }
    }
}