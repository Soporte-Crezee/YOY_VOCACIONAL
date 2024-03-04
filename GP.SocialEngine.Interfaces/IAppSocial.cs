using System;
using System.Collections.Generic;
using System.Text;

namespace GP.SocialEngine.Interfaces
{
    /// <summary>
    /// IAppSocial
    /// </summary>
    public interface IAppSocial
    {

        string GetNombreAplicacion();
        string GetImagen();
        string GetInformacionActual();
        string GetUrlApp();
        string GetAppKey();

    }
}
