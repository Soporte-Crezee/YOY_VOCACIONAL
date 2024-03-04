using System;

namespace POV.Core.PadreTutor.Interfaces
{
    interface IWebContext
    {
        void RemoveFromSession(string key);
        bool ContainsInSession(string key);
        void SetInSession(string key, object value);
        object GetFromSession(string key);
        void ClearSession();
        string ApplicationPath { get;}

    }
}
