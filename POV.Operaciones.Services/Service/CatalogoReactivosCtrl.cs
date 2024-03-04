using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using POV.AmazonAWS.Services;
using POV.Comun.Service;
using POV.Operaciones.BO;
using POV.Reactivos.BO;
using POV.Reactivos.Service;
using POV.Comun.BO;
using POV.Logger.Service;

namespace POV.Operaciones.Service
{
    public class CatalogoReactivosCtrl
    {
        private string _servidorContenidos;
        private string _username;
        private string _password;
        private string _metodoServidor;
        private string _carpetaContenidos;

        public CatalogoReactivosCtrl()
        {

        }
        public CatalogoReactivosCtrl(string servidorContenidos, string username, string password, string metodoServidor, string carpetaContenidos)
        {
            this._servidorContenidos = servidorContenidos;
            this._username = username;
            this._password = password;
            this._metodoServidor = metodoServidor;
            this._carpetaContenidos = carpetaContenidos;
        }
        /// <summary>
        /// Registra un reactivo simple con su imagen
        /// </summary>
        /// <param name="dctx">Parametros de conexion</param>
        /// <param name="reactivoWrapper">el reactivo que contiene las imagen y su informacion</param>
        public void InsertReactivoSimple(IDataContext dctx,ReactivoTemp reactivoWrapper) {
            ReactivoCtrl reactivoSrv = new ReactivoCtrl();
            if (reactivoWrapper == null)
                throw new ArgumentNullException("CatalogoReactivosCtrl: Reactivo es requerido");
            if(reactivoWrapper.Reactivo==null)
                throw new ArgumentNullException("CatalogoReactivosCtrl: Reactivo es requerido");
            
            if (reactivoWrapper.Reactivo.TipoReactivo == null)
                throw new ArgumentNullException("CatalogoReactivosCtrl: Tipo de Reactivo es requerido");
           
          
            object con = new object();
            try
            {
                dctx.OpenConnection(con);
            }
            catch (Exception ex){
                throw new Exception("No se pudo conectar a la base de datso");
            }
            try {
                bool tieneImagen = reactivoWrapper.ImagenReactivo != null && reactivoWrapper.ImagenReactivo.FileWrapper != null;
                if (tieneImagen)
                {
                    CopiarReactivoImagen(reactivoWrapper);
                }
                dctx.BeginTransaction(con);
                reactivoSrv.Insert(dctx, reactivoWrapper.Reactivo);
                this.UploadReactivoImagen(reactivoWrapper);
               
                dctx.CommitTransaction(con);
            }          
            catch (Exception ex)
            {
                dctx.RollbackTransaction(con);
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(con);
                throw ex;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(con);
            }
            


        }
        protected void CopiarReactivoImagen(ReactivoTemp reactivoWrapper) {
            string guidDocto = Guid.NewGuid().ToString("N");
            string extension = reactivoWrapper.ImagenReactivo.FileWrapper.Name.Substring(reactivoWrapper.ImagenReactivo.FileWrapper.Name.LastIndexOf('.') + 1).ToLower();
            reactivoWrapper.ImagenReactivo.FileWrapper.Name = _carpetaContenidos + guidDocto + "." + extension;
            reactivoWrapper.Reactivo.PlantillaReactivo = guidDocto + "." + extension; ;
        }
        protected void UploadReactivoImagen(ReactivoTemp reactivoWrapper)
        {
          
            bool tieneImagen = reactivoWrapper.ImagenReactivo != null && reactivoWrapper.ImagenReactivo.FileWrapper != null;

            if (tieneImagen)
            {
                switch (this._metodoServidor)
                {
                    case "FTP":
                        if (string.IsNullOrEmpty(this._username))
                            throw new ArgumentNullException("CatalogoReactivosCtrl: el username del servidor es requerido");
                        if (string.IsNullOrEmpty(this._password))
                            throw new ArgumentNullException("CatalogoReactivosCtrl: el password del servidor es requerido");

                        FtpFileUploadCtrl ftpUploadCtrl = new FtpFileUploadCtrl(this._servidorContenidos, this._username, this._password);
                        ftpUploadCtrl.UploadFile(reactivoWrapper.ImagenReactivo.FileWrapper, reactivoWrapper.ImagenReactivo.FileWrapper.Name);
                        break;
                    case "LOCAL":
                        LocalFileUploadCtrl localUploadCtrl = new LocalFileUploadCtrl(this._servidorContenidos, this._username, this._password);
                        localUploadCtrl.UploadFile(reactivoWrapper.ImagenReactivo.FileWrapper, reactivoWrapper.ImagenReactivo.FileWrapper.Name);
                        break;
                    case "AMAZONS3":
                        AmazonS3ClientCtrl clientS3 = new AmazonS3ClientCtrl(this._username, this._password);
                        clientS3.UploadFile(this._servidorContenidos, reactivoWrapper.ImagenReactivo.FileWrapper);
                        break;
                    default:
                        throw new NotSupportedException("Metodo no soportado, use : AMAZONS3, FTP o LOCAL como metodo");
                }
            }
        }
        /// <summary>
        /// Actualiza un reactivo y su imagen en el sistema
        /// </summary>
        /// <param name="dctx">Parámetros de conexion</param>
        /// <param name="reactivoWrapper">El reactivo que contiene la imagen y su informacion</param>
        /// <param name="previous">El reactivo que contiene la informacion de la imagen anterior</param>
        /// <param name="imageDeleted">Parametro para saber si se sube o se elimina la imagen</param>
        public void UpdateReactivoSimple(IDataContext dctx, ReactivoTemp reactivoWrapper,Reactivo previous,bool imageDeleted = false)
        {
           
            if (reactivoWrapper == null)
                throw new ArgumentNullException("CatalogoReactivosCtrl: Reactivo es requerido");
            if (reactivoWrapper.Reactivo == null)
                throw new ArgumentNullException("CatalogoReactivosCtrl: Reactivo es requerido");

            if (reactivoWrapper.Reactivo.TipoReactivo == null)
                throw new ArgumentNullException("CatalogoReactivosCtrl: Tipo de Reactivo es requerido");
            object con = new object();
            try
            {
                dctx.OpenConnection(con);
            }
            catch (Exception ex)
            {
                throw new Exception("Inconsistencias al conectarse a la base de datos");
            }
            try
            {
                ReactivoCtrl reactivoCtrl = new ReactivoCtrl();
                bool tieneImagen =reactivoWrapper.ImagenReactivo!=null && reactivoWrapper.ImagenReactivo.FileWrapper != null;
                if (tieneImagen)
                {
                    CopiarReactivoImagen(reactivoWrapper);
                }

                dctx.BeginTransaction(con);
                reactivoCtrl.Update(dctx, reactivoWrapper.Reactivo,previous);
                if (imageDeleted)
                    this.DeleteImagenReactivo(dctx, previous);
                if (tieneImagen)
                {
                    this.UploadReactivoImagen(reactivoWrapper);                   
                }

                DataSet ds = reactivoCtrl.Retrieve(dctx, reactivoWrapper.Reactivo);
                if (ds.Tables[0].Rows.Count <= 0)
                    throw new Exception("Ocurrió un error durante el registro del reactivo ");
                Reactivo reactivo = reactivoWrapper.Reactivo;
                reactivo = reactivoCtrl.LastDataRowToReactivo(ds,reactivo.TipoReactivo.Value);

                dctx.CommitTransaction(con);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(con);
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(con);
                throw ex;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(con);
            }
        }
        /// <summary>
        /// Elimina un reactivo y su imagen en el sistema
        /// </summary>
        /// <param name="dctx">Parámetros de conexion</param>
        /// <param name="reactivo">El reactivo que contiene la imagen y su informacion</param>      
        public void DeleteReactivoSimple(IDataContext dctx, Reactivo reactivo)
        {

            ReactivoCtrl reactivoCtrl = new ReactivoCtrl();
            
            
            object con = new object();
            try
            {
                dctx.OpenConnection(con);
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo conectar a la base de datso");
            }
            try
            {
                bool tieneImagen = reactivo.PlantillaReactivo != null && !string.IsNullOrEmpty(reactivo.PlantillaReactivo);
                dctx.BeginTransaction(con);
                reactivoCtrl.Delete(dctx, reactivo);
                if (tieneImagen)
                {
                    DeleteImagenReactivo(dctx, reactivo);
                }
               
                dctx.CommitTransaction(con);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(con);
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(con);
                throw ex;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(con);
            }
           

        }
        public Pregunta InsertPreguntaRespuestaCorta(IDataContext dctx, Reactivo reactivo, PreguntaTemp preguntaTemp)
        {
            if (reactivo == null || reactivo.ReactivoID == null || reactivo.TipoReactivo == null) throw new ArgumentNullException("CatalogoReactivosCtrl: Reactivo es requerido");
            if (preguntaTemp == null || preguntaTemp.Pregunta == null) throw new ArgumentNullException("CatalogoReactivosCtrl: Pregunta es requerido");
            if(preguntaTemp.Pregunta.RespuestaPlantilla==null) throw  new ArgumentNullException("CatalogoReactivosCtrl:RespuestaPlantilla es requerido");
            if(preguntaTemp.Pregunta.RespuestaPlantilla.TipoRespuestaPlantilla!=ETipoRespuestaPlantilla.ABIERTA) throw new ArgumentNullException("CatálogoReactivoCtrl:La pregunta no es abierta");


            Pregunta p = null;
            object con = new object();
            try
            {
                dctx.OpenConnection(con);
            }
            catch (Exception ex)
            {

                throw new Exception("Inconsistencias al conectarse a la base de datos");
            }

            try
            {
                p = preguntaTemp.Pregunta;
                PreguntaCtrl preguntaCtrl = new PreguntaCtrl();
                bool tieneImagen = preguntaTemp.ImagenPregunta != null && preguntaTemp.ImagenPregunta.FileWrapper != null;
                if (tieneImagen)
                {
                    string guidDocto = Guid.NewGuid().ToString("N");
                    string extension = preguntaTemp.ImagenPregunta.FileWrapper.Name.Substring(preguntaTemp.ImagenPregunta.FileWrapper.Name.LastIndexOf('.') + 1).ToLower();
                    preguntaTemp.ImagenPregunta.FileWrapper.Name = _carpetaContenidos +  guidDocto + "." + extension;
                    preguntaTemp.Pregunta.PlantillaPregunta = guidDocto + "." + extension;
                }

                dctx.BeginTransaction(con);
                preguntaCtrl.InsertComplete(dctx, preguntaTemp.Pregunta, reactivo);

                if (tieneImagen)
                {
                    switch (this._metodoServidor)
                    {
                        case "FTP":
                            if (string.IsNullOrEmpty(this._username))
                                throw new ArgumentNullException("CatalogoReactivosCtrl: el username del servidor es requerido");
                            if (string.IsNullOrEmpty(this._password))
                                throw new ArgumentNullException("CatalogoReactivosCtrl: el password del servidor es requerido");

                            FtpFileUploadCtrl ftpUploadCtrl = new FtpFileUploadCtrl(this._servidorContenidos, this._username, this._password);
                            ftpUploadCtrl.UploadFile(preguntaTemp.ImagenPregunta.FileWrapper, preguntaTemp.ImagenPregunta.FileWrapper.Name);
                            break;
                        case "LOCAL":
                            LocalFileUploadCtrl localUploadCtrl = new LocalFileUploadCtrl(this._servidorContenidos, this._username, this._password);
                            localUploadCtrl.UploadFile(preguntaTemp.ImagenPregunta.FileWrapper, preguntaTemp.ImagenPregunta.FileWrapper.Name);
                            break;
                        case "AMAZONS3":
                            AmazonS3ClientCtrl clientS3 = new AmazonS3ClientCtrl(this._username, this._password);
                            clientS3.UploadFile(this._servidorContenidos, preguntaTemp.ImagenPregunta.FileWrapper);
                            break;
                        default:
                            throw new NotSupportedException("Metodo no soportado, use : AMAZONS3, FTP o LOCAL como metodo");
                    }
                }

                DataSet ds = preguntaCtrl.Retrieve(dctx, preguntaTemp.Pregunta, reactivo);
                if (ds.Tables[0].Rows.Count <= 0)
                    throw new Exception("Ocurrió un error durante el registro de la pregunta");
                p = preguntaCtrl.LastDataRowToPregunta(ds);

                dctx.CommitTransaction(con);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(con);
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(con);

            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(con);
            }

            return p;
        }
        public Pregunta UpdatePreguntaRespuestaCorta(IDataContext dctx, Reactivo reactivo, PreguntaTemp preguntaTemp, PreguntaTemp previous)
        {
            if (preguntaTemp == null || preguntaTemp.Pregunta == null || preguntaTemp.Pregunta.PreguntaID == null) throw new ArgumentNullException("CatalogoReactivosCtrl: Pregunta es requerido");
            if (previous == null || previous.Pregunta == null || previous.Pregunta.PreguntaID == null) throw new ArgumentNullException("CatalogoReactivosCtrl: Pregunta previa es requerido");
            if (preguntaTemp.Pregunta.RespuestaPlantilla == null) throw new ArgumentNullException("CatalogoReactivosCtrl:RespuestaPlantilla es requerido");
            if (preguntaTemp.Pregunta.RespuestaPlantilla.TipoRespuestaPlantilla != ETipoRespuestaPlantilla.ABIERTA) throw new ArgumentNullException("CatálogoReactivoCtrl:La pregunta no es abierta");

            Pregunta p = null;
            object con = new object();
            try
            {
                dctx.OpenConnection(con);
            }
            catch (Exception ex)
            {
                throw new Exception("Inconsistencias al conectarse a la base de datos");
            }

            try
            {
                p = preguntaTemp.Pregunta;
                PreguntaCtrl preguntaCtrl = new PreguntaCtrl();
                bool tieneImagen = preguntaTemp.ImagenPregunta != null && preguntaTemp.ImagenPregunta.FileWrapper != null;
                bool tieneImagenPrevia = !string.IsNullOrEmpty(previous.Pregunta.PlantillaPregunta);


                if (tieneImagen)
                {
                    string guidDocto = Guid.NewGuid().ToString("N");
                    string extension = preguntaTemp.ImagenPregunta.FileWrapper.Name.Substring(preguntaTemp.ImagenPregunta.FileWrapper.Name.LastIndexOf('.') + 1).ToLower();
                    preguntaTemp.ImagenPregunta.FileWrapper.Name = _carpetaContenidos + guidDocto + "." + extension;
                    preguntaTemp.Pregunta.PlantillaPregunta = guidDocto + "." + extension;
                }

                dctx.BeginTransaction(con);
                preguntaCtrl.UpdateComplete(dctx, preguntaTemp.Pregunta, previous.Pregunta, reactivo);

                if (tieneImagen)
                {
                    switch (this._metodoServidor)
                    {
                        case "FTP":
                            if (string.IsNullOrEmpty(this._username))
                                throw new ArgumentNullException("CatalogoReactivosCtrl: el username del servidor es requerido");
                            if (string.IsNullOrEmpty(this._password))
                                throw new ArgumentNullException("CatalogoReactivosCtrl: el password del servidor es requerido");

                            FtpFileUploadCtrl ftpUploadCtrl = new FtpFileUploadCtrl(this._servidorContenidos, this._username, this._password);
                            ftpUploadCtrl.UploadFile(preguntaTemp.ImagenPregunta.FileWrapper, preguntaTemp.ImagenPregunta.FileWrapper.Name);
                            if (tieneImagenPrevia) { ftpUploadCtrl.DeleteFile(_carpetaContenidos + previous.Pregunta.PlantillaPregunta); }
                            break;
                        case "LOCAL":
                            LocalFileUploadCtrl localUploadCtrl = new LocalFileUploadCtrl(this._servidorContenidos, this._username, this._password);
                            localUploadCtrl.UploadFile(preguntaTemp.ImagenPregunta.FileWrapper, preguntaTemp.ImagenPregunta.FileWrapper.Name);
                            if (tieneImagenPrevia) { localUploadCtrl.DeleteFile(_carpetaContenidos + previous.Pregunta.PlantillaPregunta); }
                            break;
                        case "AMAZONS3":
                             if (string.IsNullOrEmpty(this._username))
                                throw new ArgumentNullException("CatalogoReactivosCtrl: el username del servidor es requerido");
                            if (string.IsNullOrEmpty(this._password))
                                throw new ArgumentNullException("CatalogoReactivosCtrl: el password del servidor es requerido");

                            AmazonS3ClientCtrl clientS3 = new AmazonS3ClientCtrl(this._username, this._password);
                            clientS3.UploadFile(this._servidorContenidos, preguntaTemp.ImagenPregunta.FileWrapper);
                            if (tieneImagenPrevia) { clientS3.DeleteFile(this._servidorContenidos, _carpetaContenidos + previous.Pregunta.PlantillaPregunta); }
                            break;
                        default:
                            throw new NotSupportedException("Método no soportado, use : AMAZONS3, FTP o LOCAL como método");
                    }
                }

                DataSet ds = preguntaCtrl.Retrieve(dctx, new Pregunta { PreguntaID = preguntaTemp.Pregunta.PreguntaID }, reactivo);
                if (ds.Tables[0].Rows.Count <= 0)
                    throw new Exception("Ocurrió un error durante la actualización de la pregunta");
                p = preguntaCtrl.LastDataRowToPregunta(ds);
                dctx.CommitTransaction(con);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(con);
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(con);

            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(con);
            }

            return p;
        }

        protected void DeleteImagenReactivo(IDataContext dctx, Reactivo reactivo)
        {
            bool tieneImagen = !string.IsNullOrEmpty(reactivo.PlantillaReactivo);


            object con = new object();
            try
            {
                dctx.OpenConnection(con);
            }
            catch (Exception ex)
            {
                throw new Exception("Inconsistencias al conectarse a la base de datos");
            }


            if (tieneImagen)
            {
                string filename = _carpetaContenidos + reactivo.PlantillaReactivo;
              
                switch (this._metodoServidor)
                {
                    case "FTP":
                        if (string.IsNullOrEmpty(this._username))
                            throw new ArgumentNullException("CatalogoReactivosCtrl: el username del servidor es requerido");
                        if (string.IsNullOrEmpty(this._password))
                            throw new ArgumentNullException("CatalogoReactivosCtrl: el password del servidor es requerido");

                        FtpFileUploadCtrl ftpUploadCtrl = new FtpFileUploadCtrl(this._servidorContenidos, this._username, this._password);

                        if (tieneImagen) { ftpUploadCtrl.DeleteFile(filename); }
                        break;
                    case "LOCAL":
                        LocalFileUploadCtrl localUploadCtrl = new LocalFileUploadCtrl(this._servidorContenidos, this._username, this._password);
                        if (tieneImagen) { localUploadCtrl.DeleteFile(filename); }
                        break;
                    case "AMAZONS3":
                        if (string.IsNullOrEmpty(this._username))
                            throw new ArgumentNullException("CatalogoReactivosCtrl: el username del servidor es requerido");
                        if (string.IsNullOrEmpty(this._password))
                            throw new ArgumentNullException("CatalogoReactivosCtrl: el password del servidor es requerido");

                        AmazonS3ClientCtrl clientS3 = new AmazonS3ClientCtrl(this._username, this._password);
                        if (tieneImagen) { clientS3.DeleteFile(this._servidorContenidos, filename); }
                        break;
                    default:
                        throw new NotSupportedException("Método no soportado, use : AMAZONS3, FTP o LOCAL como método");
                }
            }
        }
        public void DeleteImagenPregunta(IDataContext dctx, Reactivo reactivo, Pregunta pregunta)
        {
            bool tieneImagen = !string.IsNullOrEmpty(pregunta.PlantillaPregunta);

            object con = new object();
            try
            {
                dctx.OpenConnection(con);
            }
            catch (Exception ex)
            {
                throw new Exception("Inconsistencias al conectarse a la base de datos");
            }
            

            if (tieneImagen)
            {
                PreguntaCtrl preguntaCtrl = new PreguntaCtrl();
                Pregunta previa = (Pregunta)pregunta.Clone();
                pregunta.PlantillaPregunta = string.Empty;
                preguntaCtrl.Update(dctx,pregunta,previa,reactivo);



                switch (this._metodoServidor)
                {
                    case "FTP":
                        if (string.IsNullOrEmpty(this._username))
                            throw new ArgumentNullException("CatalogoReactivosCtrl: el username del servidor es requerido");
                        if (string.IsNullOrEmpty(this._password))
                            throw new ArgumentNullException("CatalogoReactivosCtrl: el password del servidor es requerido");

                        FtpFileUploadCtrl ftpUploadCtrl = new FtpFileUploadCtrl(this._servidorContenidos, this._username, this._password);

                        if (tieneImagen) { ftpUploadCtrl.DeleteFile(_carpetaContenidos + previa.PlantillaPregunta); }
                        break;
                    case "LOCAL":
                        LocalFileUploadCtrl localUploadCtrl = new LocalFileUploadCtrl(this._servidorContenidos, this._username, this._password);
                        if (tieneImagen) { localUploadCtrl.DeleteFile(_carpetaContenidos + previa.PlantillaPregunta); }
                        break;
                    case "AMAZONS3":
                        if (string.IsNullOrEmpty(this._username))
                            throw new ArgumentNullException("CatalogoReactivosCtrl: el username del servidor es requerido");
                        if (string.IsNullOrEmpty(this._password))
                            throw new ArgumentNullException("CatalogoReactivosCtrl: el password del servidor es requerido");

                        AmazonS3ClientCtrl clientS3 = new AmazonS3ClientCtrl(this._username, this._password);
                        if (tieneImagen) { clientS3.DeleteFile(this._servidorContenidos, _carpetaContenidos + previa.PlantillaPregunta); }
                        break;
                    default:
                        throw new NotSupportedException("Método no soportado, use : AMAZONS3, FTP o LOCAL como método");
                }
            }
        }

        public Pregunta InsertPreguntaRespuestaNumerica(IDataContext dctx, Reactivo reactivo, PreguntaTemp preguntaTemp)
        {
            if (reactivo == null || reactivo.ReactivoID == null || reactivo.TipoReactivo == null) throw new ArgumentNullException("CatalogoReactivosCtrl: Reactivo es requerido");
            if (preguntaTemp == null || preguntaTemp.Pregunta == null) throw new ArgumentNullException("CatalogoReactivosCtrl: Pregunta es requerido");
            if (preguntaTemp.Pregunta.RespuestaPlantilla == null) throw new ArgumentNullException("CatalogoReactivosCtrl:RespuestaPlantilla es requerido");
            if (preguntaTemp.Pregunta.RespuestaPlantilla.TipoRespuestaPlantilla != ETipoRespuestaPlantilla.ABIERTA_NUMERICO) throw new ArgumentNullException("CatálogoReactivoCtrl:La pregunta no es abierta numerica");


            Pregunta p = null;
            object con = new object();
            try
            {
                dctx.OpenConnection(con);
            }
            catch (Exception ex)
            {

                throw new Exception("Inconsistencias al conectarse a la base de datos");
            }

            try
            {
                p = preguntaTemp.Pregunta;
                PreguntaCtrl preguntaCtrl = new PreguntaCtrl();
                bool tieneImagen = preguntaTemp.ImagenPregunta != null && preguntaTemp.ImagenPregunta.FileWrapper != null;
                if (tieneImagen)
                {
                    string guidDocto = Guid.NewGuid().ToString("N");
                    string extension = preguntaTemp.ImagenPregunta.FileWrapper.Name.Substring(preguntaTemp.ImagenPregunta.FileWrapper.Name.LastIndexOf('.') + 1).ToLower();
                    preguntaTemp.ImagenPregunta.FileWrapper.Name = _carpetaContenidos + guidDocto + "." + extension;
                    preguntaTemp.Pregunta.PlantillaPregunta = guidDocto + "." + extension;
                }

                dctx.BeginTransaction(con);
                preguntaCtrl.InsertComplete(dctx, preguntaTemp.Pregunta, reactivo);

                if (tieneImagen)
                {
                    switch (this._metodoServidor)
                    {
                        case "FTP":
                            if (string.IsNullOrEmpty(this._username))
                                throw new ArgumentNullException("CatalogoReactivosCtrl: el username del servidor es requerido");
                            if (string.IsNullOrEmpty(this._password))
                                throw new ArgumentNullException("CatalogoReactivosCtrl: el password del servidor es requerido");

                            FtpFileUploadCtrl ftpUploadCtrl = new FtpFileUploadCtrl(this._servidorContenidos, this._username, this._password);
                            ftpUploadCtrl.UploadFile(preguntaTemp.ImagenPregunta.FileWrapper, preguntaTemp.ImagenPregunta.FileWrapper.Name);
                            break;
                        case "LOCAL":
                            LocalFileUploadCtrl localUploadCtrl = new LocalFileUploadCtrl(this._servidorContenidos, this._username, this._password);
                            localUploadCtrl.UploadFile(preguntaTemp.ImagenPregunta.FileWrapper, preguntaTemp.ImagenPregunta.FileWrapper.Name);
                            break;
                        case "AMAZONS3":
                            AmazonS3ClientCtrl clientS3 = new AmazonS3ClientCtrl(this._username, this._password);
                            clientS3.UploadFile(this._servidorContenidos, preguntaTemp.ImagenPregunta.FileWrapper);
                            break;
                        default:
                            throw new NotSupportedException("Metodo no soportado, use : AMAZONS3, FTP o LOCAL como metodo");
                    }
                }

                DataSet ds = preguntaCtrl.Retrieve(dctx, preguntaTemp.Pregunta, reactivo);
                if (ds.Tables[0].Rows.Count <= 0)
                    throw new Exception("Ocurrió un error durante el registro de la pregunta");
                p = preguntaCtrl.LastDataRowToPregunta(ds);

                dctx.CommitTransaction(con);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(con);
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(con);

            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(con);
            }

            return p;
        }
        public Pregunta UpdatePreguntaRespuestaNumerica(IDataContext dctx, Reactivo reactivo, PreguntaTemp preguntaTemp, PreguntaTemp previous)
        {
            if (preguntaTemp == null || preguntaTemp.Pregunta == null || preguntaTemp.Pregunta.PreguntaID == null) throw new ArgumentNullException("CatalogoReactivosCtrl: Pregunta es requerido");
            if (previous == null || previous.Pregunta == null || previous.Pregunta.PreguntaID == null) throw new ArgumentNullException("CatalogoReactivosCtrl: Pregunta previa es requerido");
            if (preguntaTemp.Pregunta.RespuestaPlantilla == null) throw new ArgumentNullException("CatalogoReactivosCtrl:RespuestaPlantilla es requerido");
            if (preguntaTemp.Pregunta.RespuestaPlantilla.TipoRespuestaPlantilla != ETipoRespuestaPlantilla.ABIERTA_NUMERICO) throw new ArgumentNullException("CatálogoReactivoCtrl:La pregunta no es abierta numerica");

            Pregunta p = null;
            object con = new object();
            try
            {
                dctx.OpenConnection(con);
            }
            catch (Exception ex)
            {
                throw new Exception("Inconsistencias al conectarse a la base de datos");
            }

            try
            {
                p = preguntaTemp.Pregunta;
                PreguntaCtrl preguntaCtrl = new PreguntaCtrl();
                bool tieneImagen = preguntaTemp.ImagenPregunta != null && preguntaTemp.ImagenPregunta.FileWrapper != null;
                bool tieneImagenPrevia = !string.IsNullOrEmpty(previous.Pregunta.PlantillaPregunta);


                if (tieneImagen)
                {
                    string guidDocto = Guid.NewGuid().ToString("N");
                    string extension = preguntaTemp.ImagenPregunta.FileWrapper.Name.Substring(preguntaTemp.ImagenPregunta.FileWrapper.Name.LastIndexOf('.') + 1).ToLower();
                    preguntaTemp.ImagenPregunta.FileWrapper.Name = _carpetaContenidos + guidDocto + "." + extension;
                    preguntaTemp.Pregunta.PlantillaPregunta = guidDocto + "." + extension;
                }

                dctx.BeginTransaction(con);
                preguntaCtrl.UpdateComplete(dctx, preguntaTemp.Pregunta, previous.Pregunta, reactivo);

                if (tieneImagen)
                {
                    switch (this._metodoServidor)
                    {
                        case "FTP":
                            if (string.IsNullOrEmpty(this._username))
                                throw new ArgumentNullException("CatalogoReactivosCtrl: el username del servidor es requerido");
                            if (string.IsNullOrEmpty(this._password))
                                throw new ArgumentNullException("CatalogoReactivosCtrl: el password del servidor es requerido");

                            FtpFileUploadCtrl ftpUploadCtrl = new FtpFileUploadCtrl(this._servidorContenidos, this._username, this._password);
                            ftpUploadCtrl.UploadFile(preguntaTemp.ImagenPregunta.FileWrapper, preguntaTemp.ImagenPregunta.FileWrapper.Name);
                            if (tieneImagenPrevia) { ftpUploadCtrl.DeleteFile(_carpetaContenidos + previous.Pregunta.PlantillaPregunta); }
                            break;
                        case "LOCAL":
                            LocalFileUploadCtrl localUploadCtrl = new LocalFileUploadCtrl(this._servidorContenidos, this._username, this._password);
                            localUploadCtrl.UploadFile(preguntaTemp.ImagenPregunta.FileWrapper, preguntaTemp.ImagenPregunta.FileWrapper.Name);
                            if (tieneImagenPrevia) { localUploadCtrl.DeleteFile(_carpetaContenidos + previous.Pregunta.PlantillaPregunta); }
                            break;
                        case "AMAZONS3":
                            if (string.IsNullOrEmpty(this._username))
                                throw new ArgumentNullException("CatalogoReactivosCtrl: el username del servidor es requerido");
                            if (string.IsNullOrEmpty(this._password))
                                throw new ArgumentNullException("CatalogoReactivosCtrl: el password del servidor es requerido");

                            AmazonS3ClientCtrl clientS3 = new AmazonS3ClientCtrl(this._username, this._password);
                            clientS3.UploadFile(this._servidorContenidos, preguntaTemp.ImagenPregunta.FileWrapper);
                            if (tieneImagenPrevia) { clientS3.DeleteFile(this._servidorContenidos, _carpetaContenidos + previous.Pregunta.PlantillaPregunta); }
                            break;
                        default:
                            throw new NotSupportedException("Método no soportado, use : AMAZONS3, FTP o LOCAL como método");
                    }
                }

                DataSet ds = preguntaCtrl.Retrieve(dctx, new Pregunta { PreguntaID = preguntaTemp.Pregunta.PreguntaID }, reactivo);
                if (ds.Tables[0].Rows.Count <= 0)
                    throw new Exception("Ocurrió un error durante la actualización de la pregunta");
                p = preguntaCtrl.LastDataRowToPregunta(ds);
                dctx.CommitTransaction(con);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(con);
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(con);

            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(con);
            }

            return p;
        }
        /// <summary>
        /// Crea registros de pregunta de opcion multiple, sube las imágenes de las opciones, y la imagen de la pregunta.
        /// </summary>
        /// <param name="dctx">Parámetros de conexión</param>
        /// <param name="reactivo">Reactivo que tiene la pregunta</param>
        /// <param name="pregunta">Pregunta con sus datos de registro</param>
        /// <param name="files">Las imagenes de las opciones, y  de la pregunta</param>
        public void InsertPreguntaOpcionMultiple(IDataContext dctx,Reactivo reactivo ,Pregunta pregunta, List<Comun.BO.FileWrapper> files)
        {

            if (reactivo == null || reactivo.ReactivoID == null || reactivo.TipoReactivo == null) throw new ArgumentNullException("CatalogoReactivosCtrl: Reactivo es requerido");
            if (pregunta == null) throw new ArgumentNullException("CatalogoReactivosCtrl: Pregunta es requerido");
            object con = new object();
            try
            {
                dctx.OpenConnection(con);
            }
            catch (Exception ex)
            {

                throw new Exception("Inconsistencias al conectarse a la base de datos");
            }
            try {
                dctx.BeginTransaction(con);
                PreguntaCtrl preguntaCtrl = new PreguntaCtrl();
                preguntaCtrl.InsertComplete(dctx, pregunta, reactivo);
                if (files.Count > 0)
                {

                    foreach (FileWrapper file in files)
                    {
                        switch (_metodoServidor)
                        {
                            case "FTP":
                                if (string.IsNullOrEmpty(_username)) throw new ArgumentNullException("CatalogoReactivosCtrl: el username del servidor es requerido");
                                if (string.IsNullOrEmpty(_password)) throw new ArgumentNullException("CatalogoReactivosCtrl: el password del servidor es requerido");

                                FtpFileUploadCtrl ftpUploadCtrl = new FtpFileUploadCtrl(_servidorContenidos, _username, _password);
                                ftpUploadCtrl.UploadFile(file, file.Name);
                                break;
                            case "LOCAL":
                                LocalFileUploadCtrl localUploadCtrl = new LocalFileUploadCtrl(_servidorContenidos, _username, _password);
                                localUploadCtrl.UploadFile(file, file.Name);
                                break;
                            case "AMAZONS3":
                                AmazonS3ClientCtrl clientS3 = new AmazonS3ClientCtrl(_username, _password);
                                clientS3.UploadFile(_servidorContenidos, file);
                                break;
                            default:
                                throw new NotSupportedException("CatalogoReactivosCtrl: Metodo no soportado, use : AMAZONS3, FTP o LOCAL como metodo");
                        }
                    }
                }
                dctx.CommitTransaction(con);
            
            }catch(Exception e ){
                
                dctx.RollbackTransaction(con);
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(con);
                
            }
            finally{
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(con);
            }            
        }
        /// <summary>
        /// Actualiza la pregunta y sus opciones, también elimina y sube imagenes para la pregunta y sus opciones
        /// </summary>
        /// <param name="dctx">Parametros de conexión</param>
        /// <param name="reactivo">El reactivo que tiene la pregunta</param>
        /// <param name="pregunta">La pregunta del reactivo</param>
        /// <param name="previous">La pregunta anterior</param>
        /// <param name="files">Archivos que se van a subir</param>
        /// <param name="opcionesEliminadas">Las opciones respuesta que se van a eliminar de la pregunta</param>
        /// <param name="archivosEliminados"></param>
        public void UpdatePreguntaOpcionMultiple(IDataContext dctx,Reactivo reactivo ,Pregunta pregunta, Pregunta previous, List<FileWrapper> files, List<OpcionRespuestaPlantilla> opcionesEliminadas, List<string> archivosEliminados)
        {
            object myFirm = new object();

            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                PreguntaCtrl preguntaCtrl = new PreguntaCtrl();
                OpcionRespuestaPlantillaCtrl opcionRespuestaPlantillaCtrl = new OpcionRespuestaPlantillaCtrl();
                preguntaCtrl.UpdateComplete(dctx,pregunta,previous,reactivo);

                 foreach (OpcionRespuestaPlantilla opcion in opcionesEliminadas)                {
                    
                    opcionRespuestaPlantillaCtrl.Delete(dctx, opcion, reactivo.TipoReactivo.Value);
                }
                 if (files.Count > 0)
                {
                    
                    foreach (FileWrapper file in files)
                    {
                        switch (_metodoServidor)
                        {
                            case "FTP":
                                if (string.IsNullOrEmpty(_username)) throw new ArgumentNullException("CatalogoReactivosCtrl: el username del servidor es requerido");
                                if (string.IsNullOrEmpty(_password)) throw new ArgumentNullException("CatalogoReactivosCtrl: el password del servidor es requerido");

                                FtpFileUploadCtrl ftpUploadCtrl = new FtpFileUploadCtrl(_servidorContenidos,_username,_password);
                                ftpUploadCtrl.UploadFile(file, file.Name);
                                break;
                            case "LOCAL":
                                LocalFileUploadCtrl localUploadCtrl = new LocalFileUploadCtrl(_servidorContenidos,_username,_password);
                                localUploadCtrl.UploadFile(file, file.Name);
                                break;
                            case "AMAZONS3":
                                AmazonS3ClientCtrl clientS3 = new AmazonS3ClientCtrl(_username, _password);
                                clientS3.UploadFile(_servidorContenidos, file);
                                break;
                            default:
                                throw new NotSupportedException("CatalogoReactivosCtrl: Metodo no soportado, use : AMAZONS3, FTP o LOCAL como metodo");
                        }
                        
                    }

                }
                   foreach (string archivo in archivosEliminados)
                {
                    switch (_metodoServidor)
                    {
                        case "FTP":
                            if (string.IsNullOrEmpty(_username)) throw new ArgumentNullException("CatalogoReactivosCtrl: el username del servidor es requerido");
                            if (string.IsNullOrEmpty(_password)) throw new ArgumentNullException("CatalogoReactivosCtrl: el password del servidor es requerido");

                            FtpFileUploadCtrl ftpUploadCtrl = new FtpFileUploadCtrl(_servidorContenidos, _username, _password);
                            ftpUploadCtrl.DeleteFile(archivo);
                            break;
                        case "LOCAL":
                            LocalFileUploadCtrl localUploadCtrl = new LocalFileUploadCtrl(_servidorContenidos, _username, _password);
                            localUploadCtrl.DeleteFile(archivo);
                            break;
                        case "AMAZONS3":
                            AmazonS3ClientCtrl clientS3 = new AmazonS3ClientCtrl(_username, _password);
                            clientS3.DeleteFile(_servidorContenidos, archivo);
                            break;
                        default:
                            throw new NotSupportedException("CatalogoReactivosCtrl: Metodo no soportado, use : AMAZONS3, FTP o LOCAL como metodo");
                    }

                } dctx.CommitTransaction(myFirm);
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
