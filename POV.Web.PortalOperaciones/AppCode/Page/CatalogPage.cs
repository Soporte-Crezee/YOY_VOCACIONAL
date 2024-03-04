using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using POV.Core.Operaciones.Implements;
using POV.Core.Operaciones.Interfaces;

namespace POV.Web.PortalOperaciones.AppCode.Page
{
    public abstract class CatalogPage : PageBase
    {
        // Display CRUD acciones

        protected abstract void DisplayCreateAction();

        protected abstract void DisplayReadAction();

        protected abstract void DisplayUpdateAction();

        protected abstract void DisplayDeleteAction();

        
    }
}