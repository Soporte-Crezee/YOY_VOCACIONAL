﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using POV.Core.Universidades.Implements;
using POV.Core.Universidades.Interfaces;

namespace POV.Web.PortalUniversidad.AppCode.Page
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