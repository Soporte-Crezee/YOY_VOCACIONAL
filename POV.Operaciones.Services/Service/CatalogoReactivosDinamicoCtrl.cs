using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Reactivos.BO;
using POV.Reactivos.Service;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.Modelo.BO;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.AmazonAWS.Services;
using POV.Logger.Service;
using POV.Reactivos.Validaciones.BO;

namespace POV.Operaciones.Service
{
    public class CatalogoReactivosDinamicoCtrl
    {
        /// <summary>
        /// Inserta un registro completo en la base de datos
        /// </summary>
        /// <param name="dctx">DataContext que proveera el acceso a la base de datos</param>
        /// <param name="reactivo">Reactivo</param>
        /// <param name="files">Lista de archivos</param>
        /// <param name="servidorContenidos">servidor de contenidos</param>
        /// <param name="username">usuario</param>
        /// <param name="password">contraseña</param>
        /// <param name="metodoServidor">metodo</param>
        public void InsertComplete(IDataContext dctx, Reactivo reactivo,
            List<FileWrapper> files, string servidorContenidos, string username, string password, string metodoServidor)
        {
            if (reactivo.Caracteristicas == null) throw new ArgumentNullException("CatalogoReactivosDinamicoCtrl: las caracteristicas no pueden ser vacias.");
            if (!(reactivo.Caracteristicas is CaracteristicasModeloGenerico)) throw new ArgumentException("CatalogoReactivosDinamicoCtrl: la caracteristicas deben ser de modelo generico.");
            if ((reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo == null) throw new ArgumentNullException("CatalogoReactivosDinamicoCtrl: el modelo es requerido");
            if ((reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo.MetodoCalificacion == null)  throw new ArgumentNullException("CatalogoReactivosDinamicoCtrl: el método de calificación es requerido");



            object myFirm = new object();

            IValidadorReactivo validador = new ValidadorReactivoFactory().Create(reactivo);

           RespuestaValidacionReactivo respuestaValidacion =  validador.Validar(reactivo);

           if (!respuestaValidacion.EsValido)
               throw new Exception(respuestaValidacion.Error);


            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
            
                ReactivoCtrl reactivoCtrl = new ReactivoCtrl();

                reactivoCtrl.InsertComplete(dctx, reactivo);

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


                dctx.CommitTransaction(myFirm);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }
        }

        /// <summary>
        /// Actualiza un registro completo de reactivo
        /// </summary>
        /// <param name="dctx">DataContext que proveera el acceso a la base de datos</param>
        /// <param name="reactivo">Reactivo</param>
        /// <param name="previous"></param>
        /// <param name="files">Lista de archivos</param>
        /// <param name="opcionesEliminadas">Opciones eliminadas</param>
        /// <param name="archivosEliminados">lista de archivos eliminados</param>
        /// <param name="servidorContenidos">servidor de contenidos</param>
        /// <param name="username">usuario</param>
        /// <param name="password">contraseña</param>
        /// <param name="metodoServidor">metodo</param>
        public void UpdateComplete(IDataContext dctx, Reactivo reactivo, Reactivo previous, 
            List<FileWrapper> files, List<OpcionRespuestaPlantilla> opcionesEliminadas, List<string> archivosEliminados,
            string servidorContenidos, string username, string password, string metodoServidor)
        {

            if (reactivo.Caracteristicas == null) throw new ArgumentNullException("CatalogoReactivosDinamicoCtrl: las caracteristicas no pueden ser vacias.");
            if (!(reactivo.Caracteristicas is CaracteristicasModeloGenerico)) throw new ArgumentException("CatalogoReactivosDinamicoCtrl: la caracteristicas deben ser de modelo generico.");
            if ((reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo == null) throw new ArgumentNullException("CatalogoReactivosDinamicoCtrl: el modelo es requerido");
            if ((reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo.MetodoCalificacion == null) throw new ArgumentNullException("CatalogoReactivosDinamicoCtrl: el método de calificación es requerido");




            IValidadorReactivo validador = new ValidadorReactivoFactory().Create(reactivo);


            RespuestaValidacionReactivo respuestaValidacion = validador.Validar(reactivo);

            if (!respuestaValidacion.EsValido)
                throw new Exception(respuestaValidacion.Error);

            object myFirm = new object();

            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {

                ReactivoCtrl reactivoCtrl = new ReactivoCtrl();
                OpcionRespuestaPlantillaCtrl opcionRespuestaCtrl = new OpcionRespuestaPlantillaCtrl();
                reactivoCtrl.UpdateComplete(dctx, reactivo, previous);


                foreach (OpcionRespuestaPlantilla opcion in opcionesEliminadas)
                {
                    //baja logica las opciones
                    opcionRespuestaCtrl.Delete(dctx, opcion, reactivo.TipoReactivo.Value);
                }

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

                foreach (string archivo in archivosEliminados)
                {
                    switch (metodoServidor)
                    {
                        case "FTP":
                            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException("CatalogoReactivosDinamicoCtrl: el username del servidor es requerido");
                            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("CatalogoReactivosDinamicoCtrl: el password del servidor es requerido");

                            FtpFileUploadCtrl ftpUploadCtrl = new FtpFileUploadCtrl(servidorContenidos, username, password);
                            ftpUploadCtrl.DeleteFile(archivo);
                            break;
                        case "LOCAL":
                            LocalFileUploadCtrl localUploadCtrl = new LocalFileUploadCtrl(servidorContenidos, username, password);
                            localUploadCtrl.DeleteFile(archivo);
                            break;
                        case "AMAZONS3":
                            AmazonS3ClientCtrl clientS3 = new AmazonS3ClientCtrl(username, password);
                            clientS3.DeleteFile(servidorContenidos, archivo);
                            break;
                        default:
                            throw new NotSupportedException("CatalogoReactivosDinamicoCtrl: Metodo no soportado, use : AMAZONS3, FTP o LOCAL como metodo");
                    }
                    
                }

                dctx.CommitTransaction(myFirm);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="reactivo">reactivo</param>
        /// <param name="servidorContenidos">servidor de contenidos</param>
        /// <param name="username">usuario</param>
        /// <param name="password">contraseña</param>
        /// <param name="metodoServidor">metodo</param>
        public void DeleteComplete(IDataContext dctx, Reactivo reactivo, string servidorContenidos, 
            string username, string password, string metodoServidor)
        {
            object myFirm = new object();

            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                ReactivoCtrl reactivoCtrl = new ReactivoCtrl();

                Reactivo reactivoComplete = reactivoCtrl.RetrieveComplete(dctx, reactivo);
                reactivoCtrl.Delete(dctx, reactivoComplete);


                List<string> archivosEliminados = new List<string>();

                Pregunta pregunta = reactivoComplete.Preguntas.FirstOrDefault();

                if (!string.IsNullOrEmpty(pregunta.PlantillaPregunta))
                    archivosEliminados.Add(pregunta.PlantillaPregunta);


                foreach (OpcionRespuestaPlantilla opcion in (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla)
                {
                    if (!string.IsNullOrEmpty(opcion.ImagenUrl))
                        archivosEliminados.Add(opcion.ImagenUrl);
                }

                foreach (string archivo in archivosEliminados)
                {
                    switch (metodoServidor)
                    {
                        case "FTP":
                            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException("CatalogoReactivosDinamicoCtrl: el username del servidor es requerido");
                            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("CatalogoReactivosDinamicoCtrl: el password del servidor es requerido");

                            FtpFileUploadCtrl ftpUploadCtrl = new FtpFileUploadCtrl(servidorContenidos, username, password);
                            ftpUploadCtrl.DeleteFile(archivo);
                            break;
                        case "LOCAL":
                            LocalFileUploadCtrl localUploadCtrl = new LocalFileUploadCtrl(servidorContenidos, username, password);
                            localUploadCtrl.DeleteFile(archivo);
                            break;
                        case "AMAZONS3":
                            AmazonS3ClientCtrl clientS3 = new AmazonS3ClientCtrl(username, password);
                            clientS3.DeleteFile(servidorContenidos, archivo);
                            break;
                        default:
                            throw new NotSupportedException("CatalogoReactivosDinamicoCtrl: Metodo no soportado, use : AMAZONS3, FTP o LOCAL como metodo");
                    }

                }

                dctx.CommitTransaction(myFirm);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }
        }
    }
}
