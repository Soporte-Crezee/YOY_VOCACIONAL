using System;
using POV.Core.HerramientasDocente.Interfaces;

namespace POV.Core.HerramientasDocente.Implement
{
    public class Redirector:IRedirector
    {
        #region Miembros de IRedirector

        /// <summary>
        /// Redirecciona a la pagina de inicio de session
        /// </summary>
        /// <param name="bandera"> El valor booleano indica si se debe detener la ejecución de la página actual</param>
        public void GoToLoginPage ( System.Boolean bandera )
        {
            Redirect(UrlHelper.GetLoginURL(), bandera);
        }

        /// <summary>
        /// Redirecciona a la pagina por Default
        /// </summary>
        /// <param name="bandera"> El valor booleano indica si se debe detener la ejecución de la página actual </param>
        public void GoToHomePage ( System.Boolean bandera )
        {
            Redirect(UrlHelper.GetDefaultURL(), bandera);
        }

        public void GoToAccesDenied ( System.Boolean bandera )
        {
            throw new NotImplementedException( );
        }

        public void GoToNotFound(System.Boolean bandera)
        {
            Redirect(UrlHelper.Get404URL(), bandera);
        }
        ///<summary>
        ///Redirecciona a la prueba Diagnostica
        ///</summary>
       
        #endregion

        /// <summary>
        /// Metodo de clase para redireccionar a la pagina en cuestion
        /// </summary>
        /// <param name="path"> String que representa la direccion a la cual se redireccionará </param>
        /// <param name="bandera"> El valor booleano indica si se debe detener la ejecución de la página actual </param>
        private void Redirect ( string path , Boolean bandera)
        {            
            System.Web.HttpContext.Current.Response.Redirect( path, bandera );
        }

        public void GoToSocial(bool accion)
        {
            throw new NotImplementedException();
        }
    }
}
