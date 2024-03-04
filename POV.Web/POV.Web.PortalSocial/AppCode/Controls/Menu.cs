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
    public class Menu : WebControl
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string SelectedItem
        {
            get { return (string)ViewState["SelectedItem"] ?? string.Empty; }
            set { ViewState["SelectedItem"] = value; }
        }

        /// <summary>
        /// Gets and sets the entire menu tree using the ASP.NET Viewstate.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(null)]
        [Localizable(true)]
        public MenuItem MenuItems
        {
            get { return ViewState["MenuItems"] as MenuItem; }
            set { ViewState["MenuItems"] = value; }
        }
    }
}