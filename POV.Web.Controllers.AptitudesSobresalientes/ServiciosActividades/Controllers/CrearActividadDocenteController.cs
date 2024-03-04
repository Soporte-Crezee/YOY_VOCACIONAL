using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.ConfiguracionActividades.BO;
using POV.ConfiguracionActividades.Services;
using POV.Modelo.Context;
using POV.AmazonAWS.Services;
using POV.Logger.Service;
using POV.Comun.BO;
using POV.Comun.Service;

namespace POV.ServiciosActividades.Controllers
{
    /// <summary>
    /// Controlador de la interfaz de usuario Crear Actividad
    /// </summary>
    public class CrearActividadDocenteController
    {
        #region Atributos

        private readonly Contexto ctx;
        private object firma = new object();
        #endregion

        #region Constructores
		/// <summary>
		/// Constructor por defecto
		/// </summary>
        public CrearActividadDocenteController()
		{
			ctx = new Contexto(firma);
			
		}
        #endregion

        /// <summary>
        /// Metodo que inserta una actividad en el sistema
        /// </summary>
        /// <param name="actividad">Actividad que se desea registrar</param>
        /// <returns>Actividad insertada</returns>
        public ActividadDocente InsertActividadDocente(ActividadDocente actividadDocente,
            List<FileWrapper> files, string servidorContenidos, string username, string password, string metodoServidor)
        {
            using (var ctrl = new ActividadDocenteCtrl(ctx))
            {
                ctrl.Insert(actividadDocente);

                if (files.Count > 0)
                {

                    foreach (FileWrapper file in files)
                    {
                        switch (metodoServidor)
                        {
                            case "FTP":
                                if (string.IsNullOrEmpty(username)) throw new ArgumentNullException("CatalogoReactivosDinamicoCtrl: el username del servidor es requerido");
                                if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("CatalogoReactivosDinamicoCtrl: el password del servidor es requerido");

                                FtpFileUploadCtrl ftpUploadCtrl = new FtpFileUploadCtrl(servidorContenidos, username, password);
                                ftpUploadCtrl.UploadFile(file, file.Name);
                                break;
                            case "LOCAL":
                                LocalFileUploadCtrl localUploadCtrl = new LocalFileUploadCtrl(servidorContenidos, username, password);
                                localUploadCtrl.UploadFile(file, file.Name);
                                break;
                            case "AMAZONS3":
                                AmazonS3ClientCtrl clientS3 = new AmazonS3ClientCtrl(username, password);
                                clientS3.UploadFile(servidorContenidos, file);
                                break;
                            default:
                                throw new NotSupportedException("CatalogoReactivosDinamicoCtrl: Metodo no soportado, use : AMAZONS3, FTP o LOCAL como metodo");
                        }
                    }
                }
                ctx.Commit(firma);
            }
            return actividadDocente;
        }
    }
}
