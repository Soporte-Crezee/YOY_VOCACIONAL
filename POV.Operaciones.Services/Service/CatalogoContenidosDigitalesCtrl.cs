using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.Logger.Service;
using POV.ContenidosDigital.Service;
using POV.ContenidosDigital.BO;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.Service;
using POV.ContenidosDigital.Busqueda.Service;
using POV.ContenidosDigital.Busqueda.BO;
using POV.AmazonAWS.Services;

namespace POV.Operaciones.Service
{
    /// <summary>
    /// Controlador del catalogo de contenidos digitales
    /// </summary>
    public class CatalogoContenidosDigitalesCtrl
    {

        /// <summary>
        /// Inserta un registro de contenido digital junto con su archivo
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contenidoDigital">Contenido digital</param>
        /// <param name="servidorContenidos">url del servidor de contenidos</param>
        /// <param name="username">username de conexion al servidor</param>
        /// <param name="password">contraseña del servidor</param>
        /// <param name="metodoServidor">metodo</param>
        /// <param name="fileWrapper">Archivo del contenido digital</param>
        public void InsertComplete(IDataContext dctx, ContenidoDigital contenidoDigital,
            string servidorContenidos, string username, string password,
            string metodoServidor, string carpetaContenidos, FileWrapper fileWrapper = null)
        {
            if (contenidoDigital == null) throw new ArgumentNullException("CatalogoContenidosDigitalesCtrl: el contenido digital no puede ser nulo");
            if (string.IsNullOrEmpty(servidorContenidos)) throw new ArgumentNullException("CatalogoContenidosDigitalesCtrl: la direccion del servidor es requerida");
            if (string.IsNullOrEmpty(metodoServidor)) throw new ArgumentNullException("CatalogoContenidosDigitalesCtrl: el metodo del servidor es requerido");
            ContenidoDigitalCtrl contenidoDigitalCtrl = new ContenidoDigitalCtrl();
            object myFirm = new object();

            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                //el contenido digital es interno, generamos la ubicacion del archivo
                if (contenidoDigital.EsInterno.Value)
                {
                    if (fileWrapper == null) throw new ArgumentNullException("CatalogoContenidosDigitalesCtrl: FileWrapper es requerido");

                    string guidDocto = Guid.NewGuid().ToString("N");
                    string extension = fileWrapper.Name.Substring(fileWrapper.Name.LastIndexOf('.') + 1).ToLower();
                    string fileName = guidDocto + "." + extension;
                    fileWrapper.Name = carpetaContenidos + fileName;

                    URLContenido contenidoExterno = new URLContenido();
                    contenidoExterno.EsPredeterminada = true;
                    contenidoExterno.FechaRegistro = DateTime.Now;
                    contenidoExterno.Nombre = "DEFAULT";
                    contenidoExterno.Activo = true;
                    contenidoExterno.URL = fileName;

                    contenidoDigital.URLContenidoAgregar(contenidoExterno);
                    contenidoDigitalCtrl.InsertComplete(dctx, contenidoDigital);
                    switch (metodoServidor)
                    {
                        case "FTP":
                            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException("CatalogoContenidosDigitalesCtrl: el username del servidor es requerido");
                            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("CatalogoContenidosDigitalesCtrl: el password del servidor es requerido");

                            FtpFileUploadCtrl ftpUploadCtrl = new FtpFileUploadCtrl(servidorContenidos, username, password);
                            ftpUploadCtrl.UploadFile(fileWrapper, fileWrapper.Name);
                            break;
                        case "LOCAL":
                            LocalFileUploadCtrl localUploadCtrl = new LocalFileUploadCtrl(servidorContenidos, username, password);
                            localUploadCtrl.UploadFile(fileWrapper, fileWrapper.Name);
                            break;
                        case "AMAZONS3":
                            AmazonS3ClientCtrl clientS3 = new AmazonS3ClientCtrl(username, password);
                            clientS3.UploadFile(servidorContenidos, fileWrapper);
                            break;
                        default:
                            throw new NotSupportedException("Metodo no soportado, use : AMAZONS3, FTP o LOCAL como metodo");
                    }

                }
                else
                {
                    contenidoDigitalCtrl.InsertComplete(dctx, contenidoDigital);
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
        /// Metodo que actualiza un registro de contenido digital en la base de datos del sistema
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="contenidoDigital"></param>
        /// <param name="previous"></param>
        /// <param name="servidorContenidos"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="metodoServidor"></param>
        /// <param name="fileWrapper"></param>
        public void UpdateComplete(IDataContext dctx, ContenidoDigital contenidoDigital, ContenidoDigital previous,
            string servidorContenidos, string username, string password, string metodoServidor, string carpetaContenidos, FileWrapper fileWrapper = null, bool actualizarTags = false)
        {
            if (contenidoDigital == null) throw new ArgumentNullException("CatalogoContenidosDigitalesCtrl: el contenido digital no puede ser nulo");
            if (previous == null) throw new ArgumentNullException("CatalogoContenidosDigitalesCtrl: el contenido digital previo no puede ser nulo");
            if (actualizarTags)
            {
                if (contenidoDigital.Tags == null) throw new ArgumentNullException("CatalogoContenidosDigitalesCtrl: los tags del contenido digital no puede ser nulo");
                if (previous.Tags == null) throw new ArgumentNullException("CatalogoContenidosDigitalesCtrl: los tags del contenido digital previo no puede ser nulo");
            }
            if (string.IsNullOrEmpty(servidorContenidos)) throw new ArgumentNullException("CatalogoContenidosDigitalesCtrl: la direccion del servidor es requerida");
            if (string.IsNullOrEmpty(metodoServidor)) throw new ArgumentNullException("CatalogoContenidosDigitalesCtrl: el metodo del servidor es requerido");

            ContenidoDigitalCtrl contenidoDigitalCtrl = new ContenidoDigitalCtrl();
            ContenidoDigitalAgrupadorCtrl contenidoDigitalAgrupadorCtrl = new ContenidoDigitalAgrupadorCtrl();

            object myFirm = new object();

            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {

                #region *** si cambian los tags y el contenido digital es parte del motor de busqueda se actualiza el motor ***
                //obtenemos el contenido digital agrupador que queremos insertar en el motor de busqueda
                DataSet dsContenidosDigitalAgrupador = contenidoDigitalAgrupadorCtrl.Retrieve(dctx, new ContenidoDigitalAgrupador { ContenidoDigital = contenidoDigital });
                //si el contenido digital esta en uso para un eje tematico/situacion administramos las palabras clave
                if (actualizarTags && dsContenidosDigitalAgrupador.Tables[0].Rows.Count > 0)
                {
                    PalabraClaveCtrl palabraClaveCtrl = new PalabraClaveCtrl();
                    PalabraClaveContenidoCtrl palabraClaveContenidoCtrl = new PalabraClaveContenidoCtrl();

                    string[] listaTagsPrevio = previous.Tags.Split(',');
                    string[] listaTagsActual = contenidoDigital.Tags.Split(',');
                    Dictionary<string, byte> palabrasEstado = new Dictionary<string, byte>();


                    #region *** analisis de las palabras nuevas y elimadas para el diccionario ***
                    //buscar las elimnadas
                    foreach (string tag in listaTagsPrevio)
                    {
                        string tagPrevio = listaTagsActual.FirstOrDefault(item => item.Trim().ToUpper() == tag.Trim().ToUpper()); // esta en la actual?
                        if (tagPrevio == null) //no esta, entonces se quitó
                        {
                            palabrasEstado.Add(tag.Trim().ToUpper(), 0);
                        }
                    }
                    //buscar las nuevas
                    foreach (string tag in listaTagsActual)
                    {
                        string tagActual = listaTagsPrevio.FirstOrDefault(item => item.Trim().ToUpper() == tag.Trim().ToUpper()); // esta en la previa?
                        if (tagActual == null) //no esta, entonces es nuevo
                        {
                            palabrasEstado.Add(tag.Trim().ToUpper(), 1);
                        }
                    }

                    #endregion


                    foreach (var palabraEstado in palabrasEstado)
                    {
                        if (palabraEstado.Value == 0) //para la palabra clave que quiero eliminar
                        {
                            DataSet dsPalabraClave = palabraClaveCtrl.Retrieve(dctx, new PalabraClave { Tag = palabraEstado.Key, TipoPalabraClave = ETipoPalabraClave.CONTENIDODIGITAL });
                            if (dsPalabraClave.Tables[0].Rows.Count > 0) //si existe la palabra clave borramos la palabra del motor
                            {
                                PalabraClave palabraClave = palabraClaveCtrl.LastDataRowToPalabraClave(dsPalabraClave);
                                PalabraClaveContenidoDigital palabraContenido = palabraClaveContenidoCtrl.RetrieveComplete(dctx, new PalabraClaveContenidoDigital { PalabraClave = palabraClave }) as PalabraClaveContenidoDigital;

                                //por cada agrupador presente para la palabra clave lo eliminamos
                                foreach (DataRow dr in dsContenidosDigitalAgrupador.Tables[0].Rows)
                                {
                                    ContenidoDigitalAgrupador contenidoDigitalAgrupador = contenidoDigitalAgrupadorCtrl.DataRowToContenidoDigitalAgrupador(dr);

                                    contenidoDigitalAgrupador = palabraContenido.ContenidoDigitalAgrupador.FirstOrDefault(item => item.ContenidoDigitalAgrupadorID == contenidoDigitalAgrupador.ContenidoDigitalAgrupadorID);
                                    if (contenidoDigitalAgrupador != null)
                                        palabraClaveContenidoCtrl.DeletePalabraClaveContenidoDigital(dctx, palabraContenido, contenidoDigitalAgrupador);
                                }

                                palabraContenido = palabraClaveContenidoCtrl.RetrieveComplete(dctx, new PalabraClaveContenidoDigital { PalabraClave = palabraClave }) as PalabraClaveContenidoDigital;

                                bool eliminarPalabraClave = palabraContenido.ContenidoDigitalAgrupador.Count() == 0;

                                if (eliminarPalabraClave)
                                {
                                    palabraClaveContenidoCtrl.Delete(dctx, palabraContenido);
                                    palabraClaveCtrl.Delete(dctx, palabraClave);
                                }
                            }
                        }
                        else if (palabraEstado.Value == 1)//palabra clave que quiero insertar
                        {
                            //obtenemos la palabra clave
                            DataSet dsPalabraClave = palabraClaveCtrl.Retrieve(dctx, new PalabraClave { Tag = palabraEstado.Key, TipoPalabraClave = ETipoPalabraClave.CONTENIDODIGITAL });

                            if (dsPalabraClave.Tables[0].Rows.Count > 0) // si la palabra existe, insertamos los contenidos digitales al motor
                            {
                                PalabraClave palabraClave = palabraClaveCtrl.LastDataRowToPalabraClave(dsPalabraClave);
                                PalabraClaveContenidoDigital palabraContenido = palabraClaveContenidoCtrl.RetrieveComplete(dctx, new PalabraClaveContenidoDigital { PalabraClave = palabraClave }) as PalabraClaveContenidoDigital;
                                //por cada contenido digital agrupador insertamos en la palabra contenido
                                foreach (DataRow dr in dsContenidosDigitalAgrupador.Tables[0].Rows)
                                {
                                    ContenidoDigitalAgrupador contenidoDigitalAgrupador = contenidoDigitalAgrupadorCtrl.DataRowToContenidoDigitalAgrupador(dr);
                                    //si no esta registrada, la insertamos
                                    if (palabraContenido.ContenidoDigitalAgrupador.FirstOrDefault(item => item.ContenidoDigitalAgrupadorID == contenidoDigitalAgrupador.ContenidoDigitalAgrupadorID) == null)
                                        palabraClaveContenidoCtrl.InsertPalabraClaveContenidoDigital(dctx, palabraContenido, contenidoDigitalAgrupador);
                                }
                            }
                            else //no existe, se crea la estructura de la palabra clave para insertar al motor
                            {
                                PalabraClave nuevaPalabraClave = new PalabraClave { Tag = palabraEstado.Key, TipoPalabraClave = ETipoPalabraClave.CONTENIDODIGITAL };
                                PalabraClaveContenidoDigital palabraContenido = new PalabraClaveContenidoDigital();
                                palabraContenido.FechaRegistro = DateTime.Now;
                                //insertamos la palabra clave
                                palabraClaveCtrl.Insert(dctx, nuevaPalabraClave);
                                palabraContenido.PalabraClave = palabraClaveCtrl.LastDataRowToPalabraClave(palabraClaveCtrl.Retrieve(dctx, nuevaPalabraClave));
                                //insertamos la palabra clave contenido digital
                                palabraClaveContenidoCtrl.Insert(dctx, palabraContenido);
                                palabraContenido = palabraClaveContenidoCtrl.LastDataRowToAPalabraClaveContenido(palabraClaveContenidoCtrl.Retrieve(dctx, palabraContenido)) as PalabraClaveContenidoDigital;
                                //por cada contenido digital agrupador insertamos en la palabra clave contenido
                                foreach (DataRow dr in dsContenidosDigitalAgrupador.Tables[0].Rows)
                                {
                                    ContenidoDigitalAgrupador contenidoDigitalAgrupador = contenidoDigitalAgrupadorCtrl.DataRowToContenidoDigitalAgrupador(dr);
                                    palabraClaveContenidoCtrl.InsertPalabraClaveContenidoDigital(dctx, palabraContenido, contenidoDigitalAgrupador);
                                }

                            }
                        }
                    }
                }

                #endregion

                //el contenido digital es interno, generamos la ubicacion del archivo
                if (contenidoDigital.EsInterno.Value && fileWrapper != null && !string.IsNullOrEmpty(fileWrapper.Name))
                {


                    string guidDocto = Guid.NewGuid().ToString("N");
                    string extension = fileWrapper.Name.Substring(fileWrapper.Name.LastIndexOf('.') + 1).ToLower();
                    string fileName = guidDocto + "." + extension;
                    fileWrapper.Name = carpetaContenidos + fileName;
                    URLContenido contenidoExterno = contenidoDigital.ListaURLContenido.FirstOrDefault(item => item.EsPredeterminada.Value);


                    string deleteFile = contenidoExterno.URL.Clone() as string;
                    contenidoExterno.URL = fileName;
                    contenidoDigitalCtrl.UpdateComplete(dctx, contenidoDigital, previous);


                    switch (metodoServidor)
                    {
                        case "FTP":
                            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException("CatalogoContenidosDigitalesCtrl: el username del servidor es requerido");
                            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("CatalogoContenidosDigitalesCtrl: el password del servidor es requerido");

                            FtpFileUploadCtrl ftpUploadCtrl = new FtpFileUploadCtrl(servidorContenidos, username, password);
                            ftpUploadCtrl.DeleteFile(carpetaContenidos + deleteFile);
                            ftpUploadCtrl.UploadFile(fileWrapper, fileWrapper.Name);

                            break;
                        case "LOCAL":
                            LocalFileUploadCtrl localUploadCtrl = new LocalFileUploadCtrl(servidorContenidos, username, password);
                            localUploadCtrl.DeleteFile(carpetaContenidos + deleteFile);
                            localUploadCtrl.UploadFile(fileWrapper, fileWrapper.Name);
                            break;
                        case "AMAZONS3":
                            AmazonS3ClientCtrl clientS3 = new AmazonS3ClientCtrl(username, password);
                            clientS3.DeleteFile(servidorContenidos, carpetaContenidos + deleteFile);
                            clientS3.UploadFile(servidorContenidos, fileWrapper);
                            break;
                        default:
                            throw new NotSupportedException("Metodo no soportado, use : FTP o LOCAL como metodo");
                    }

                }
                else
                {
                    contenidoDigitalCtrl.UpdateComplete(dctx, contenidoDigital, previous);
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
        /// Elimina un contenido digital.
        /// </summary>
        /// <param name="dctx">DataContext que proveerá el acceso a la base de datos.</param>
        /// <param name="contenidoDigital">Contenido digital que se desea eliminar.</param>
        public void DeleteComplete(IDataContext dctx, ContenidoDigital contenidoDigital,
            string servidorContenidos, string username, string password, string metodoServidor, string carpetaContenidos)
        {
            ContenidoDigitalCtrl contenidoDigitalCtrl = new ContenidoDigitalCtrl();
            CursoCtrl cursoCtrl = new CursoCtrl();
            AsistenciaCtrl asistenciaCtrl = new AsistenciaCtrl();
            AgrupadorContenidoDigitalCtrl agrupadorContenidoDigitalCtrl = new AgrupadorContenidoDigitalCtrl(); //Situaciones.
            SituacionAprendizajeCtrl situacionAprendizajeCtrl = new SituacionAprendizajeCtrl();
            EjeTematicoCtrl ejeTematicoCtrl = new EjeTematicoCtrl();
            ContenidoDigitalAgrupadorCtrl contenidoDigitalAgrupadorCtrl = new ContenidoDigitalAgrupadorCtrl();
            PalabraClaveContenidoCtrl palabraClaveContenidoCtrl = new PalabraClaveContenidoCtrl();
            PalabraClaveCtrl palabraClaveCtrl = new PalabraClaveCtrl();
            object myFirm = new object();
            if (contenidoDigital == null) throw new ArgumentNullException("El contenido digital no puede ser nulo");

            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                //Para obtener los cursos asociados.
                DataSet dsCurso = cursoCtrl.RetrieveCurso(dctx, contenidoDigital);
                //Para obtener las asistencias asociadas.
                DataSet dsAsistencia = asistenciaCtrl.RetrieveAsistencia(dctx, contenidoDigital);
                //Para obtener las situaciones asociadas.
                DataSet dsSituacion = agrupadorContenidoDigitalCtrl.RetrieveAgrupadorContenidoDigital(dctx,
                                                                                                      contenidoDigital);
                //Para obtener los agrupadores.
                DataSet dscontenidodigitalagrupador = contenidoDigitalAgrupadorCtrl.Retrieve(dctx, contenidoDigital);

                if (dsCurso.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsCurso.Tables[0].Rows)
                    {
                        Curso curso = new Curso();
                        curso = (Curso)cursoCtrl.RetrieveSimple(dctx,
                                                                       new Curso()
                                                                           {
                                                                               AgrupadorContenidoDigitalID =
                                                                                   Convert.ToInt64(
                                                                                       row["AgrupadorPadreID"
                                                                                           ])
                                                                           });
                        if (curso != null)
                        {
                            if (curso.Estatus == EEstatusProfesionalizacion.ACTIVO ||
                                                curso.Estatus == EEstatusProfesionalizacion.MANTENIMIENTO)
                                throw new Exception(
                                    @"Existen cursos activos asociados, 
                                                            por lo cual no se puede eliminar el Contenido Digital");
                        }
                    }

                }
                else if (dsAsistencia.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsAsistencia.Tables[0].Rows)
                    {
                        Asistencia asistencia = new Asistencia();
                        asistencia = (Asistencia)asistenciaCtrl.RetrieveSimple(dctx, new Asistencia()
                            {
                                AgrupadorContenidoDigitalID = Convert.ToInt64(row["AgrupadorPadreID"])
                            });

                        if (asistencia != null)
                        {
                            if (asistencia.Estatus == EEstatusProfesionalizacion.ACTIVO ||
                                                asistencia.Estatus == EEstatusProfesionalizacion.MANTENIMIENTO)
                                throw new Exception(
                                    @"Existen asistencias activas asociados, 
                                                            por lo cual no se puede eliminar el Contenido Digital");
                        }
                    }
                }
                else if (dsSituacion.Tables[0].Rows.Count > 0) //Situación de Aprendizaje.
                {
                    foreach (DataRow row in dsSituacion.Tables[0].Rows)
                    {
                        AAgrupadorContenidoDigital agrupador = agrupadorContenidoDigitalCtrl.RetrieveSimple(dctx,
                                                                                                            new AgrupadorCompuesto
                                                                                                                ()
                                                                                                                {
                                                                                                                    AgrupadorContenidoDigitalID
                                                                                                                        =
                                                                                                                        Convert
                                                                                                                .ToInt64
                                                                                                                (row[
                                                                                                                    "AgrupadorPadreID"
                                                                                                                     ])
                                                                                                                });
                        SituacionAprendizaje situacionAprendizaje = situacionAprendizajeCtrl.RetrieveSimple(dctx,
                                                                                                            new EjeTematico(),
                                                                                                            new SituacionAprendizaje
                                                                                                                {
                                                                                                                    AgrupadorContenidoDigital
                                                                                                                        =
                                                                                                                        agrupador
                                                                                                                });
                        if (situacionAprendizaje != null)
                        {
                            if (situacionAprendizaje.EstatusProfesionalizacion == EEstatusProfesionalizacion.ACTIVO ||
                                situacionAprendizaje.EstatusProfesionalizacion ==
                                EEstatusProfesionalizacion.MANTENIMIENTO)
                            {
                                List<SituacionAprendizaje> situacionAprendizajes = new List<SituacionAprendizaje>();
                                situacionAprendizajes.Add(situacionAprendizaje);

                                DataSet dsSituacionesEje = situacionAprendizajeCtrl.Retrieve(dctx, new EjeTematico(), new SituacionAprendizaje { SituacionAprendizajeID = situacionAprendizaje.SituacionAprendizajeID });
                                EjeTematico ejeTematico = null;

                                if (dsSituacionesEje.Tables[0].Rows.Count > 0)
                                {
                                    int indexSit = dsSituacionesEje.Tables[0].Rows.Count;
                                    DataRow drMiSituacio = dsSituacionesEje.Tables[0].Rows[indexSit - 1];
                                    DataSet dsEjes = ejeTematicoCtrl.Retrieve(dctx,
                                        new EjeTematico { EjeTematicoID = Convert.ToInt64(drMiSituacio["EjeTematicoID"]) });
                                    if (dsEjes.Tables[0].Rows.Count > 0)
                                        ejeTematico = ejeTematicoCtrl.LastDataRowToEjeTematico(dsEjes);

                                    
                                }

                                if (ejeTematico != null && ejeTematico.EstatusProfesionalizacion != EEstatusProfesionalizacion.INACTIVO && situacionAprendizaje.EstatusProfesionalizacion != EEstatusProfesionalizacion.INACTIVO && situacionAprendizaje.AgrupadorContenidoDigital != null && situacionAprendizaje.EstatusProfesionalizacion != EEstatusProfesionalizacion.INACTIVO)
                                {
                                    throw new Exception(
                                        @"Existen situaciones de aprendizaje activas asociadas, 
                                        por lo cual no se puede eliminar el Contenido Digital");
                                    
                                }

                                if (ejeTematico == null)
                                {
                                    if (dscontenidodigitalagrupador.Tables[0].Rows.Count > 0)
                                    {
                                        foreach (DataRow rowc in dscontenidodigitalagrupador.Tables[0].Rows)
                                        {
                                            DataSet dspalabraclave = palabraClaveContenidoCtrl.RetrievePalabraClaveContenidoDigital(
                                                dctx,
                                                new ContenidoDigitalAgrupador()
                                                {
                                                    ContenidoDigitalAgrupadorID =
                                                        Convert.ToInt64(rowc["ContenidoDigitalAgrupadorID"])
                                                });
                                            if (dspalabraclave.Tables[0].Rows.Count > 0)
                                            {
                                                foreach (DataRow palabraClaveContenidoDigital in dspalabraclave.Tables[0].Rows)
                                                {
                                                    PalabraClaveContenidoDigital pClaveContenidoDigital =
                                                        new PalabraClaveContenidoDigital();
                                                    pClaveContenidoDigital.PalabraClaveContenidoID =
                                                        Convert.ToInt64(palabraClaveContenidoDigital["PalabraClaveContenidoID"]);
                                                    DataSet dspalabraclavecontenido = palabraClaveContenidoCtrl.Retrieve(dctx,
                                                                                                                         pClaveContenidoDigital);
                                                    if (dspalabraclavecontenido.Tables[0].Rows.Count > 0)
                                                    {
                                                        foreach (DataRow pclavecontenido in dspalabraclavecontenido.Tables[0].Rows)
                                                        {
                                                            palabraClaveContenidoCtrl.DeletePalabraClaveContenidoDigital(dctx,
                                                                                             new PalabraClaveContenidoDigital()
                                                                                             {
                                                                                                 PalabraClaveContenidoID =
                                                                                                     Convert.ToInt64(
                                                                                                         palabraClaveContenidoDigital[
                                                                                                             "PalabraClaveContenidoID"])
                                                                                             }, new ContenidoDigitalAgrupador()
                                                                                             {
                                                                                                 ContenidoDigitalAgrupadorID =
                                                                                                     Convert.ToInt64(rowc["ContenidoDigitalAgrupadorID"])
                                                                                             });
                                                            palabraClaveContenidoCtrl.Delete(dctx, new PalabraClaveContenidoDigital()
                                                            {
                                                                PalabraClaveContenidoID =
                                                                    Convert.ToInt64(
                                                                        palabraClaveContenidoDigital[
                                                                            "PalabraClaveContenidoID"])
                                                            });
                                                            palabraClaveCtrl.Delete(dctx,
                                                                                    new PalabraClave()
                                                                                    {
                                                                                        PalabraClaveID =
                                                                                            Convert.ToInt64(
                                                                                                pclavecontenido["PalabraClaveID"])
                                                                                    });
                                                        }
                                                    }
                                                }

                                                //insert here
                                            }

                                            ContenidoDigital anterior = contenidoDigitalCtrl.RetrieveComplete(dctx, contenidoDigital);
                                            contenidoDigital.EstatusContenido = EEstatusContenido.INACTIVO;
                                            //Baja lógica del ContenidoDigital.
                                            contenidoDigitalCtrl.DeleteComplete(dctx, contenidoDigital, anterior);
                                            //Baja física del ContenidoDigitalAgrupador.
                                            EjeTematico eje = new EjeTematico();
                                            SituacionAprendizaje sit = new SituacionAprendizaje();
                                            AAgrupadorContenidoDigital agrup = new AgrupadorCompuesto();
                                            eje.EjeTematicoID = Convert.ToInt64(rowc["EjeTematicoID"]);
                                            sit.SituacionAprendizajeID =
                                                Convert.ToInt64(rowc["SituacionAprendizajeID"]);
                                            agrup.AgrupadorContenidoDigitalID = Convert.ToInt64(rowc["AgrupadorContenidoDigitalID"]);
                                            contenidoDigitalAgrupadorCtrl.Delete(dctx, new ContenidoDigitalAgrupador()
                                            {
                                                ContenidoDigitalAgrupadorID = Convert.ToInt64(rowc["ContenidoDigitalAgrupadorID"]),
                                                EjeTematico = eje,
                                                SituacionAprendizaje = sit,
                                                AgrupadorContenidoDigital = agrup,
                                                ContenidoDigital = contenidoDigital
                                            });
                                        }
                                    }
                                    else
                                    {
                                        ContenidoDigital anterior = contenidoDigitalCtrl.RetrieveComplete(dctx, contenidoDigital);
                                        contenidoDigital.EstatusContenido = EEstatusContenido.INACTIVO;
                                        //Baja lógica del ContenidoDigital.
                                        contenidoDigitalCtrl.DeleteComplete(dctx, contenidoDigital, anterior);
                                    }
                                }
                                else
                                {
                                    if (ejeTematico.EstatusProfesionalizacion != EEstatusProfesionalizacion.INACTIVO)
                                    {
                                        throw new Exception(@"Existen ejes temáticos activos asociados, por lo cual no se puede eliminar el Contenido Digital");
                                    }

                                    throw new Exception(
                                        @"Existen situaciones de aprendizaje activas asociadas, 
                                        por lo cual no se puede eliminar el Contenido Digital");
                                }
                            }
                            else
                            {
                                if (dscontenidodigitalagrupador.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow rowc in dscontenidodigitalagrupador.Tables[0].Rows)
                                    {
                                        DataSet dspalabraclave = palabraClaveContenidoCtrl.RetrievePalabraClaveContenidoDigital(
                                            dctx,
                                            new ContenidoDigitalAgrupador()
                                            {
                                                ContenidoDigitalAgrupadorID =
                                                    Convert.ToInt64(rowc["ContenidoDigitalAgrupadorID"])
                                            });
                                        if (dspalabraclave.Tables[0].Rows.Count > 0)
                                        {
                                            foreach (DataRow palabraClaveContenidoDigital in dspalabraclave.Tables[0].Rows)
                                            {
                                                PalabraClaveContenidoDigital pClaveContenidoDigital =
                                                    new PalabraClaveContenidoDigital();
                                                pClaveContenidoDigital.PalabraClaveContenidoID =
                                                    Convert.ToInt64(palabraClaveContenidoDigital["PalabraClaveContenidoID"]);
                                                DataSet dspalabraclavecontenido = palabraClaveContenidoCtrl.Retrieve(dctx,
                                                                                                                     pClaveContenidoDigital);
                                                if (dspalabraclavecontenido.Tables[0].Rows.Count > 0)
                                                {
                                                    foreach (DataRow pclavecontenido in dspalabraclavecontenido.Tables[0].Rows)
                                                    {
                                                        palabraClaveContenidoCtrl.DeletePalabraClaveContenidoDigital(dctx,
                                                                                         new PalabraClaveContenidoDigital()
                                                                                         {
                                                                                             PalabraClaveContenidoID =
                                                                                                 Convert.ToInt64(
                                                                                                     palabraClaveContenidoDigital[
                                                                                                         "PalabraClaveContenidoID"])
                                                                                         }, new ContenidoDigitalAgrupador()
                                                                                         {
                                                                                             ContenidoDigitalAgrupadorID =
                                                                                                 Convert.ToInt64(rowc["ContenidoDigitalAgrupadorID"])
                                                                                         });
                                                        palabraClaveContenidoCtrl.Delete(dctx, new PalabraClaveContenidoDigital()
                                                        {
                                                            PalabraClaveContenidoID =
                                                                Convert.ToInt64(
                                                                    palabraClaveContenidoDigital[
                                                                        "PalabraClaveContenidoID"])
                                                        });
                                                        palabraClaveCtrl.Delete(dctx,
                                                                                new PalabraClave()
                                                                                {
                                                                                    PalabraClaveID =
                                                                                        Convert.ToInt64(
                                                                                            pclavecontenido["PalabraClaveID"])
                                                                                });
                                                    }
                                                    ContenidoDigital anterior = contenidoDigitalCtrl.RetrieveComplete(dctx, contenidoDigital);
                                                    contenidoDigital.EstatusContenido = EEstatusContenido.INACTIVO;
                                                    //Baja lógica del ContenidoDigital.
                                                    contenidoDigitalCtrl.DeleteComplete(dctx, contenidoDigital, anterior);
                                                    //Baja física del ContenidoDigitalAgrupador.
                                                    EjeTematico eje = new EjeTematico();
                                                    SituacionAprendizaje sit = new SituacionAprendizaje();
                                                    AAgrupadorContenidoDigital agrup = new AgrupadorCompuesto();
                                                    eje.EjeTematicoID = Convert.ToInt64(rowc["EjeTematicoID"]);
                                                    sit.SituacionAprendizajeID =
                                                        Convert.ToInt64(rowc["SituacionAprendizajeID"]);
                                                    agrup.AgrupadorContenidoDigitalID = Convert.ToInt64(rowc["AgrupadorContenidoDigitalID"]);
                                                    contenidoDigitalAgrupadorCtrl.Delete(dctx, new ContenidoDigitalAgrupador()
                                                    {
                                                        ContenidoDigitalAgrupadorID = Convert.ToInt64(palabraClaveContenidoDigital["ContenidoDigitalAgrupadorID"]),
                                                        EjeTematico = eje,
                                                        SituacionAprendizaje = sit,
                                                        AgrupadorContenidoDigital = agrup,
                                                        ContenidoDigital = contenidoDigital
                                                    });
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    ContenidoDigital anterior = contenidoDigitalCtrl.RetrieveComplete(dctx, contenidoDigital);
                                    contenidoDigital.EstatusContenido = EEstatusContenido.INACTIVO;
                                    //Baja lógica del ContenidoDigital.
                                    contenidoDigitalCtrl.DeleteComplete(dctx, contenidoDigital, anterior);
                                }
                            }
                        }
                        else
                        {
                            if (dscontenidodigitalagrupador.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow rowd in dscontenidodigitalagrupador.Tables[0].Rows)
                                {
                                    ContenidoDigitalAgrupador contenidoDelete = new ContenidoDigitalAgrupador { ContenidoDigitalAgrupadorID = Convert.ToInt64(rowd["ContenidoDigitalAgrupadorID"])};

                                    DataSet dspalabraclave = palabraClaveContenidoCtrl.RetrievePalabraClaveContenidoDigital(dctx, contenidoDelete);
                                    if (dspalabraclave.Tables[0].Rows.Count > 0)
                                    {
                                        foreach (DataRow palabraClaveContenidoDigital in dspalabraclave.Tables[0].Rows)
                                        {
                                            PalabraClaveContenidoDigital pClaveContenidoDigital =
                                                new PalabraClaveContenidoDigital();
                                            pClaveContenidoDigital.PalabraClaveContenidoID =
                                                Convert.ToInt64(palabraClaveContenidoDigital["PalabraClaveContenidoID"]);
                                            DataSet dspalabraclavecontenido = palabraClaveContenidoCtrl.Retrieve(dctx,
                                                                                                                 pClaveContenidoDigital);

                                            if (dspalabraclavecontenido.Tables[0].Rows.Count > 0)
                                            {
                                                pClaveContenidoDigital = palabraClaveContenidoCtrl.LastDataRowToAPalabraClaveContenido(dspalabraclavecontenido) as PalabraClaveContenidoDigital;

                                                //si tiene contenidos activos
                                                List<ContenidoDigitalAgrupador> contenidosActivos = palabraClaveContenidoCtrl.RetrieveListContenidoDigitalAgrupadorActivos(dctx, pClaveContenidoDigital);

                                                //verifcar que los existentes diferentes al actual no es en uso
                                                bool eliminarPalabra = true;
                                                if (contenidosActivos.Count > 0)
                                                {
                                                    int totalActivos = contenidosActivos.Count(item => item.ContenidoDigitalAgrupadorID != contenidoDelete.ContenidoDigitalAgrupadorID);
                                                    if (totalActivos > 0)
                                                    {
                                                        eliminarPalabra = false;
                                                        throw new Exception(@"Existen ejes temáticos activos asociados, por lo cual no se puede eliminar el Contenido Digital");
                                                    }
                                                }

                                                if (eliminarPalabra)
                                                {
                                                    #region eliminarPalabraClave
                                                    foreach (DataRow pclavecontenido in dspalabraclavecontenido.Tables[0].Rows)
                                                    {
                                                        palabraClaveContenidoCtrl.DeletePalabraClaveContenidoDigital(dctx,
                                                                                             new PalabraClaveContenidoDigital()
                                                                                             {
                                                                                                 PalabraClaveContenidoID =
                                                                                                     Convert.ToInt64(
                                                                                                         palabraClaveContenidoDigital[
                                                                                                             "PalabraClaveContenidoID"])
                                                                                             }, new ContenidoDigitalAgrupador()
                                                                                             {
                                                                                                 ContenidoDigitalAgrupadorID =
                                                                                                     Convert.ToInt64(rowd["ContenidoDigitalAgrupadorID"])
                                                                                             });
                                                        palabraClaveContenidoCtrl.Delete(dctx, new PalabraClaveContenidoDigital()
                                                        {
                                                            PalabraClaveContenidoID =
                                                                Convert.ToInt64(
                                                                    palabraClaveContenidoDigital[
                                                                        "PalabraClaveContenidoID"])
                                                        });
                                                        palabraClaveCtrl.Delete(dctx,
                                                                                new PalabraClave()
                                                                                {
                                                                                    PalabraClaveID =
                                                                                        Convert.ToInt64(
                                                                                            pclavecontenido["PalabraClaveID"])
                                                                                });
                                                    }
                                                    #endregion
                                                }

                                                #region eliminar logica el contenido
                                                ContenidoDigital anterior = contenidoDigitalCtrl.RetrieveComplete(dctx, contenidoDigital);
                                                contenidoDigital.EstatusContenido = EEstatusContenido.INACTIVO;
                                                //Baja lógica del ContenidoDigital.
                                                contenidoDigitalCtrl.DeleteComplete(dctx, contenidoDigital, anterior);
                                                //Baja física del ContenidoDigitalAgrupador.
                                                EjeTematico eje = new EjeTematico();
                                                SituacionAprendizaje sit = new SituacionAprendizaje();
                                                AAgrupadorContenidoDigital agrup = new AgrupadorCompuesto();
                                                eje.EjeTematicoID = Convert.ToInt64(rowd["EjeTematicoID"]);
                                                sit.SituacionAprendizajeID =
                                                    Convert.ToInt64(rowd["SituacionAprendizajeID"]);
                                                agrup.AgrupadorContenidoDigitalID = Convert.ToInt64(rowd["AgrupadorContenidoDigitalID"]);
                                                if (eliminarPalabra)
                                                {
                                                    contenidoDigitalAgrupadorCtrl.Delete(dctx, new ContenidoDigitalAgrupador()
                                                        {
                                                            ContenidoDigitalAgrupadorID = Convert.ToInt64(rowd["ContenidoDigitalAgrupadorID"]),
                                                            EjeTematico = eje,
                                                            SituacionAprendizaje = sit,
                                                            AgrupadorContenidoDigital = agrup,
                                                            ContenidoDigital = contenidoDigital
                                                        });
                                                }
                                                #endregion

                                            }
                                        }
                                    }
                                    else
                                    {
                                        ContenidoDigital anterior = contenidoDigitalCtrl.RetrieveComplete(dctx, contenidoDigital);
                                        contenidoDigital.EstatusContenido = EEstatusContenido.INACTIVO;
                                        //Baja lógica del ContenidoDigital.
                                        contenidoDigitalCtrl.DeleteComplete(dctx, contenidoDigital, anterior);
                                    }
                                }
                            }
                            else
                            {
                                ContenidoDigital anterior = contenidoDigitalCtrl.RetrieveComplete(dctx, contenidoDigital);
                                contenidoDigital.EstatusContenido = EEstatusContenido.INACTIVO;
                                //Baja lógica del ContenidoDigital.
                                contenidoDigitalCtrl.DeleteComplete(dctx, contenidoDigital, anterior);
                            }
                        }
                    }
                }
                else
                {
                    if (dscontenidodigitalagrupador.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in dscontenidodigitalagrupador.Tables[0].Rows)
                        {
                            ContenidoDigitalAgrupador contenidoDelete = new ContenidoDigitalAgrupador { ContenidoDigitalAgrupadorID = Convert.ToInt64(row["ContenidoDigitalAgrupadorID"]) };

                            DataSet dspalabraclave = palabraClaveContenidoCtrl.RetrievePalabraClaveContenidoDigital(
                                dctx,
                                contenidoDelete);
                            if (dspalabraclave.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow palabraClaveContenidoDigital in dspalabraclave.Tables[0].Rows)
                                {
                                    PalabraClaveContenidoDigital pClaveContenidoDigital =
                                        new PalabraClaveContenidoDigital();
                                    pClaveContenidoDigital.PalabraClaveContenidoID =
                                        Convert.ToInt64(palabraClaveContenidoDigital["PalabraClaveContenidoID"]);
                                    DataSet dspalabraclavecontenido = palabraClaveContenidoCtrl.Retrieve(dctx,
                                                                                                         pClaveContenidoDigital);
                                    if (dspalabraclavecontenido.Tables[0].Rows.Count > 0)
                                    {
                                        //si tiene contenidos activos
                                        List<ContenidoDigitalAgrupador> contenidosActivos = palabraClaveContenidoCtrl.RetrieveListContenidoDigitalAgrupadorActivos(dctx, pClaveContenidoDigital);

                                        //verifcar que los existentes diferentes al actual no es en uso
                                        bool eliminarPalabra = true;
                                        if (contenidosActivos.Count > 0)
                                        {
                                            int totalActivos = contenidosActivos.Count(item => item.ContenidoDigitalAgrupadorID != contenidoDelete.ContenidoDigitalAgrupadorID);
                                            if (totalActivos > 0)
                                                throw new Exception(@"Existen ejes temáticos activos asociados, por lo cual no se puede eliminar el Contenido Digital");
                                        }

                                        if (eliminarPalabra)
                                        {
                                            #region eliminar palabra clave
                                            foreach (DataRow pclavecontenido in dspalabraclavecontenido.Tables[0].Rows)
                                        {
                                            palabraClaveContenidoCtrl.DeletePalabraClaveContenidoDigital(dctx,
                                                                                         new PalabraClaveContenidoDigital()
                                                                                         {
                                                                                             PalabraClaveContenidoID =
                                                                                                 Convert.ToInt64(
                                                                                                     palabraClaveContenidoDigital[
                                                                                                         "PalabraClaveContenidoID"])
                                                                                         }, new ContenidoDigitalAgrupador()
                                                                                         {
                                                                                             ContenidoDigitalAgrupadorID =
                                                                                                 Convert.ToInt64(row["ContenidoDigitalAgrupadorID"])
                                                                                         });
                                            palabraClaveContenidoCtrl.Delete(dctx, new PalabraClaveContenidoDigital()
                                            {
                                                PalabraClaveContenidoID =
                                                    Convert.ToInt64(
                                                        palabraClaveContenidoDigital[
                                                            "PalabraClaveContenidoID"])
                                            });
                                            palabraClaveCtrl.Delete(dctx,
                                                                    new PalabraClave()
                                                                        {
                                                                            PalabraClaveID =
                                                                                Convert.ToInt64(
                                                                                    pclavecontenido["PalabraClaveID"])
                                                                        });
                                        }
                                            #endregion

                                        }
                                        ContenidoDigital anterior = contenidoDigitalCtrl.RetrieveComplete(dctx, contenidoDigital);
                                        contenidoDigital.EstatusContenido = EEstatusContenido.INACTIVO;
                                        //Baja lógica del ContenidoDigital.
                                        contenidoDigitalCtrl.DeleteComplete(dctx, contenidoDigital, anterior);
                                        //Baja física del ContenidoDigitalAgrupador.
                                        EjeTematico eje = new EjeTematico();
                                        SituacionAprendizaje sit = new SituacionAprendizaje();
                                        AAgrupadorContenidoDigital agrup = new AgrupadorCompuesto();
                                        eje.EjeTematicoID = Convert.ToInt64(row["EjeTematicoID"]);
                                        sit.SituacionAprendizajeID =
                                            Convert.ToInt64(row["SituacionAprendizajeID"]);
                                        agrup.AgrupadorContenidoDigitalID = Convert.ToInt64(row["AgrupadorContenidoDigitalID"]);
                                        if (eliminarPalabra)
                                        {
                                            contenidoDigitalAgrupadorCtrl.Delete(dctx, new ContenidoDigitalAgrupador()
                                            {
                                                ContenidoDigitalAgrupadorID = Convert.ToInt64(row["ContenidoDigitalAgrupadorID"]),
                                                EjeTematico = eje,
                                                SituacionAprendizaje = sit,
                                                AgrupadorContenidoDigital = agrup,
                                                ContenidoDigital = contenidoDigital
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ContenidoDigital anterior = contenidoDigitalCtrl.RetrieveComplete(dctx, contenidoDigital);
                        contenidoDigital.EstatusContenido = EEstatusContenido.INACTIVO;
                        //Baja lógica del ContenidoDigital.
                        contenidoDigitalCtrl.DeleteComplete(dctx, contenidoDigital, anterior);
                    }

                }
                URLContenido contenidoExterno = contenidoDigital.ListaURLContenido.FirstOrDefault(item => item.EsPredeterminada.Value);


                string deleteFile = contenidoExterno.URL;
                switch (metodoServidor)
                {
                    case "FTP":
                        if (string.IsNullOrEmpty(username)) throw new ArgumentNullException("CatalogoContenidosDigitalesCtrl: el username del servidor es requerido");
                        if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("CatalogoContenidosDigitalesCtrl: el password del servidor es requerido");

                        FtpFileUploadCtrl ftpUploadCtrl = new FtpFileUploadCtrl(servidorContenidos, username, password);
                        ftpUploadCtrl.DeleteFile(carpetaContenidos + deleteFile);

                        break;
                    case "LOCAL":
                        LocalFileUploadCtrl localUploadCtrl = new LocalFileUploadCtrl(servidorContenidos, username, password);
                        localUploadCtrl.DeleteFile(carpetaContenidos + deleteFile);
                        break;
                    case "AMAZONS3":
                        AmazonS3ClientCtrl clientS3 = new AmazonS3ClientCtrl(username, password);
                        clientS3.DeleteFile(servidorContenidos, carpetaContenidos + deleteFile);
                        break;
                    default:
                        throw new NotSupportedException("Metodo no soportado, use : FTP o LOCAL como metodo");
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
