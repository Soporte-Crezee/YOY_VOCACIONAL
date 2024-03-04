using System;
using System.Web;
using POV.Core.PadreTutor.Interfaces;

namespace POV.Core.PadreTutor.Implements
{
    public class WebContext : IWebContext
    {

        public void RemoveFromSession(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }

        public bool ContainsInSession(string key)
        {
            return HttpContext.Current.Session != null && HttpContext.Current.Session[key] != null;
        }

        public void ClearSession()
        {
            HttpContext.Current.Session.Clear();
        }

        public string ApplicationPath
        {
            get { return HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"]; }
        }
        
        public void SetInSession(string key, object value)
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
            {
                return;
            }
            HttpContext.Current.Session[key] = value;
        }

        public object GetFromSession(string key)
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
            {
                return null;
            }
            return HttpContext.Current.Session[key];
        }

        private void UpdateInSession(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }

    }
}
