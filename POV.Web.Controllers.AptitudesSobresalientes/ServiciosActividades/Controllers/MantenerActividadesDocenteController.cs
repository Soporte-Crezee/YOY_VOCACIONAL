using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Base.DataAccess;
using POV.AmazonAWS.Services;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.ConfiguracionActividades.BO;
using POV.ConfiguracionActividades.Services;
using POV.ContenidosDigital.BO;
using POV.ContenidosDigital.Service;
using POV.Modelo.Context;

namespace POV.ServiciosActividades.Controllers
{
    public class MantenerActividadesDocenteController : IDisposable
    {

        #region Atributos

        private readonly Contexto ctx;
        private object firma = new object();
        IDataContext dctx = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));
        ActividadDocenteCtrl ctrl;

        #endregion
        
        #region Constructores
        /// <summary>
		/// Constructor por defecto
		/// </summary>
        public MantenerActividadesDocenteController()
		{
            ctx = new Contexto(firma);
            ctrl = new ActividadDocenteCtrl(ctx);
			
		}
        #endregion

        /// <summary>
        /// Consulta un actividad con todas sus relaciones
        /// </summary>
        /// <param name="actividadDocente">Actividad filtro</param>
        /// <returns>Actividad encontrada, en caso contrario una actividad vacia</returns>
        public ActividadDocente RetrieveActividadWithRelationship(ActividadDocente actividadDocente)
        {
            
            List<ActividadDocente> list = ctrl.RetrieveWithRelationship(actividadDocente, false);
            ActividadDocente result = list.FirstOrDefault();
            if (result == null)
                return new ActividadDocente();

            return result;
        }

        /// <summary>
        /// Consulta actividades de acuerdo a un filtro
        /// </summary>
        /// <param name="actividadDocente">Actividad que servirá como filtro</param>
        /// <returns>Lista de actividades encontradas</returns>
        public List<ActividadDocente> Retrieve(ActividadDocente actividadDocente)
        {
            List<ActividadDocente> list = ctrl.RetrieveWithRelationship(actividadDocente, false);

            return list;
        }
        /// <summary>
        /// Consulta un actividad
        /// </summary>
        /// <param name="actividadDocente">Actividad filtro</param>
        /// <returns>Actividad encontrada, en caso contrario devuelve una actividad vacia</returns>
        public ActividadDocente RetrieveActividad(ActividadDocente actividadDocente)
        {

            List<ActividadDocente> list = ctrl.Retrieve(actividadDocente, true);
            ActividadDocente result = list.FirstOrDefault();
            if (result == null)
                return new ActividadDocente();

            return result;
        }

        /// <summary>
        /// Actualiza los datos de una actividad
        /// </summary>
        /// <param name="actividadDocente">Actividad que se quiere actualizar</param>
        /// <param name="tareasEliminadas">Lista de tareas de la actividad que se quiere eliminar de la actividad</param>
        /// <returns></returns>
        public ActividadDocente UpdateActividad(ActividadDocente actividadDocente, List<long> tareasEliminadas, List<FileWrapper> files, string servidorContenidos, string username, string password, string metodoServidor, string rutaContenidos)
        {
            using (ctrl)
            {
                tareasEliminadas.ForEach(t => DeleteFileTarea(actividadDocente.Tareas.FirstOrDefault(at => at.TareaId == t), servidorContenidos, username, password, metodoServidor, rutaContenidos));

                //actualizamos actividad

                bool resp = ctrl.Update(actividadDocente, tareasEliminadas);

                if (files.Count > 0)
                {

                    foreach (FileWrapper file in files)
                    {
                        switch (metodoServidor)
                        {
                            case "FTP":
                                if (string.IsNullOrEmpty(username)) throw new ArgumentNullException("MantenerActividadesDocenteController: el username del servidor es requerido");
                                if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("MantenerActividadesDocenteController: el password del servidor es requerido");

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
                                throw new NotSupportedException("MantenerActividadesDocenteController: Metodo no soportado, use : AMAZONS3, FTP o LOCAL como metodo");
                        }
                    }
                }
                ctx.Commit(firma);
            }
            return actividadDocente;      
        }

        protected void DeleteFileTarea(Tarea tarea, string servidorContenidos, string username, string password, string metodoServidor, string rutaContenidos)
        {

            if (tarea is TareaContenidoDigital)
            {
                TareaContenidoDigital tareaContenido = (TareaContenidoDigital)tarea;
                ContenidoDigital contenidoDigital = tareaContenido.ContenidoDigital;
                ContenidoDigitalCtrl contenidoDigitalCtrl = new ContenidoDigitalCtrl();
            
                if (contenidoDigital.EsInterno != null && contenidoDigital.EsInterno.Value)
                {
                    contenidoDigital = contenidoDigitalCtrl.RetrieveComplete(dctx, contenidoDigital);
                    string filename = rutaContenidos + contenidoDigital.ListaURLContenido.FirstOrDefault().URL;
                    
                    switch (metodoServidor)
                    {
                        case "FTP":
                            if (string.IsNullOrEmpty(username))
                                throw new ArgumentNullException("MantenerActividadesDocenteController: el username del servidor es requerido");
                            if (string.IsNullOrEmpty(password))
                                throw new ArgumentNullException("MantenerActividadesDocenteController: el password del servidor es requerido");

                            FtpFileUploadCtrl ftpUploadCtrl = new FtpFileUploadCtrl(servidorContenidos, username, password);

                            ftpUploadCtrl.DeleteFile(filename);
                            break;
                        case "LOCAL":
                            LocalFileUploadCtrl localUploadCtrl = new LocalFileUploadCtrl(servidorContenidos, username, password);
                            localUploadCtrl.DeleteFile(filename); 
                            break;
                        case "AMAZONS3":
                            if (string.IsNullOrEmpty(username))
                                throw new ArgumentNullException("MantenerActividadesDocenteController: el username del servidor es requerido");
                            if (string.IsNullOrEmpty(password))
                                throw new ArgumentNullException("MantenerActividadesDocenteController: el password del servidor es requerido");

                            AmazonS3ClientCtrl clientS3 = new AmazonS3ClientCtrl(username, password);
                            clientS3.DeleteFile(servidorContenidos, filename);
                            break;
                        default:
                            throw new NotSupportedException("Método no soportado, use : AMAZONS3, FTP o LOCAL como método");
                    }
                }
            }
        }

        /// <summary>
        /// Verifica que la actividad no se encuentre asignada en el sistema
        /// </summary>
        /// <param name="actividad">Actividad que se quiere validar</param>
        /// <returns>true si la actividad está asignada, false en caso contrario</returns>
        public bool EsActividadAsignada(ActividadDocente actividad)
        {
            bool esAsignada = false;

            AsignacionActividadCtrl ctrl = new AsignacionActividadCtrl(null);

            List<AsignacionActividad> asignaciones = ctrl.Retrieve(new AsignacionActividad { ActividadId = actividad.ActividadID }, false);

            if (asignaciones.Any())
                esAsignada = true;

            return esAsignada;
        }
        
        public void Dispose()
        {
            ctrl.Dispose();
        }
    }
}
