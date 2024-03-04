using System;
using POV.Seguridad.BO;
using POV.CentroEducativo.BO;

namespace POV.Core.Administracion.Interfaces
{
    public interface IWebContext
    {
        void RemoveFromSession(string key);
        bool ContainsInSession(string key);
        void SetInSession(string key, object value);
        object GetFromSession(string key);
        void ClearSession();
        string ApplicationPath { get; }

    }
}
