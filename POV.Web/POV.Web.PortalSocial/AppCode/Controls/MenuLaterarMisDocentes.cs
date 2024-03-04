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
    [ToolboxData("<{0}:MenuLaterarMisDocentes runat=server></{0}:MenuLaterarMisDocentes>")]
    public class MenuLaterarMisDocentes : Menu
    {
        protected override void RenderContents(HtmlTextWriter output)
        {
            var root = this.MenuItems;
            output.Write(@"<div id=""menu-mis-orientadores"" class=""menu_container menu_docentes_azul"">");
            output.Write(@"<ul>");
            RecursiveRender(output, root, 0);
            output.Write(@"</ul>");
            output.Write(@"</div>");
        }

        private void RecursiveRender(HtmlTextWriter output, MenuItem item, int depth)
        {
            if (depth > 0) // Skip root node 
            {
                if (depth == 1)
                    output.Write("<li class='item-contact'>");  // main menu
                else
                    output.Write(@"<li class=""indented"">");  // sub menu
                
                output.Write(@"<img class=""user_image_top"" src=""" + item.Icon + @""" alt=""img""/>");
                output.Write(@"<a class=""link_blue"" href=""" + item.Link + @""">");
                output.Write(item.Item); 

                output.Write("</a>");
                output.Write("</li>");
            }

            // Recursively iterate over its children.
            for (int i = 0; i < item.Children.Count; i++)
            {
                RecursiveRender(output, item.Children[i], depth + 1);
            }
        }
    }
}