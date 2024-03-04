using System;
using POV.Core.RedSocial.Interfaces;

namespace POV.Core.RedSocial.Implement
{
    public class Redirector: IRedirector
    {
        

        #region Miembros de IRedirector

        /// <summary>
        /// Redirecciona a la pagina de inicio de session
        /// </summary>
        /// <param name="bandera"> El valor booleano indica si se debe detener la ejecución de la página actual</param>
        public void GoToLoginPage(System.Boolean bandera)
        {
            Redirect(UrlHelper.GetLoginURL(), bandera);
        }

        /// <summary>
        /// Redirecciona a la pagina de inicio de session
        /// </summary>
        /// <param name="bandera"> El valor booleano indica si se debe detener la ejecución de la página actual</param>
        public void GoToLogoutPage(System.Boolean bandera)
        {
            Redirect(UrlHelper.GetLogoutURL(), bandera);
        }

        /// <summary>
        /// Redirecciona a la pagina de inicio de session de orientador
        /// </summary>
        /// <param name="bandera"> El valor booleano indica si se debe detener la ejecución de la página actual</param>
        public void GoToLoginPageOrientador(System.Boolean bandera)
        {
            Redirect(UrlHelper.GetLoginURLOrientador(), bandera);
        }

        /// <summary>
        /// Redirecciona a la pagina de inicio de session de aspirante
        /// </summary>
        /// <param name="bandera"> El valor booleano indica si se debe detener la ejecución de la página actual</param>
        public void GoToLoginPageAspirante(System.Boolean bandera)
        {
            Redirect(UrlHelper.GetLoginURLAspirante(), bandera);
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

        public void GoToHomeAlumno(bool accion)
        {
            Redirect(UrlHelper.GetAlumnoNoticiasURL(), accion);
        }

        public void GoToHomeDocente(bool accion)
        {
            Redirect(UrlHelper.GetDocenteInicioURL(), accion);
        }

        public void GoToCambiarEscuela(bool accion)
        {
            Redirect(UrlHelper.GetDocenteCambiarEscuelaURL(), accion);
        }
        public void GoToDiagnostica( bool accion)
        {
            DateTime fecha = DateTime.Now;
            Redirect(UrlHelper.GetDiagnosticaURL(),accion);
        }

        public void GoToValidarDiagnostico(bool accion)
        {
            Redirect(UrlHelper.GetValidarDiagnosticaURL(), accion);
        }
        public void GoToConfirmarAlumno(bool accion)
        {
            Redirect(UrlHelper.GetConfirmarAlumnoURL(), accion);
        }
        public void GoToConfirmarMaestro(bool accion)
        {
            Redirect(UrlHelper.GetConfirmarMaestroURL(), accion);
        }
        public void GoToAceptarTerminos(bool accion)
        {
            Redirect(UrlHelper.GetAceptarTerminosUrl(), accion);
        }

        public void GoToActividades(bool accion)
        {
            Redirect(UrlHelper.GetActividadesAlumnoURL(), accion);
        }

        public void GoToRegistrarNuevoAspirante(bool accion)
        {
            Redirect(UrlHelper.GetRegistrarNuevoAspiranteURL(), accion);
        }

        public void GoToSeleccionarCarrera(bool accion)
        {
            Redirect(UrlHelper.GetSeleccionarCarreraURL(), accion);
        }

        public void GoToActivarUsuario(bool accion)
        {
            Redirect(UrlHelper.GetActivarUsuarioURL(), accion);
        }

        public void GoToPruebas(bool accion)
        {
            Redirect(UrlHelper.GetPruebasURL(), accion);
        }

        // Para realizar otra prueba de la Bateria de Bullying
        public void GoToValidarBateriaBullying(bool accion)
        {
            Redirect(UrlHelper.GetValidarBateriaBullyingURL(), accion);
        }
    }
}
