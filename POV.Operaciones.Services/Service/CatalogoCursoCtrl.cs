using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using POV.Profesionalizacion.BO;
using POV.Comun.BO;
using POV.Profesionalizacion.Service;
using POV.Comun.Service;
using POV.Logger.Service;
using System.Data;
using System.IO;

namespace POV.Operaciones.Service
{
    public class CatalogoCursoCtrl
    {
        /// <summary>
        /// Inserta un registro completo de curso y graba en el servidor el archivo de Información adicional en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="curso">Curso que se desea registrar</param>
        /// <param name="agrupadorPadreID">Identificador del Agrupador Padre</param>
        /// <param name="rutaDoctos">Opcional. Ruta donde se guardarán los archivos (Server.MapPath)</param>
        /// <param name="documentoInformacion">Opcional. Documento</param>
        public void InsertComplete(IDataContext dctx, AAgrupadorContenidoDigital curso, Int64? agrupadorPadreID,
            string rutaDoctos = null, FileWrapper documentoInformacion = null)
        {
            if (curso == null) throw new Exception("El Curso no debe ser nulo");
            if (documentoInformacion != null && rutaDoctos == null) throw new Exception("La ruta para el archivo de información es requerida");

            object myFirm = new object();

            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                CursoCtrl cursoCtrl = new CursoCtrl();

                cursoCtrl.InsertComplete(dctx, curso, agrupadorPadreID);

                //si hay un archivo se registra
                if (documentoInformacion != null && !string.IsNullOrEmpty(rutaDoctos))
                {
                    FileManagerCtrl fileManager = new FileManagerCtrl();

                    fileManager.CreateFolder(rutaDoctos);

                    byte[] data = documentoInformacion.Data;

                    fileManager.WriteFile(string.Format("{0}{1}", rutaDoctos, documentoInformacion.Name), ref data);
                }
                dctx.CommitTransaction(myFirm);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }

        }

        /// <summary>
        /// Actualiza de manera optimista un registro de Curso en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="curso">Curso que se desea actualizar</param>
        /// <param name="previousCurso">Curso con los datos originales</param>
        /// <param name="eliminarDocumentoAnterior">Opcional. Indica si se debe eliminar el documento anterior</param>
        /// <param name="rutaDoctos">Opcional. Ruta donde se guardara los archivos</param>
        /// <param name="documentoInformacion">Opcional. Documento</param>
        /// <param name="rutaDocAnterior">Opcional. Ruta del Documento anterior</param>
        public void Update(IDataContext dctx, AAgrupadorContenidoDigital curso, AAgrupadorContenidoDigital previousCurso,
            bool eliminarDocumentoAnterior = false, string rutaDoctos = null, FileWrapper documentoInformacion = null, string rutaDocAnterior = null)
        {
            if (documentoInformacion != null && rutaDoctos == null) throw new Exception("La ruta para los archivos es requerido");
            if (eliminarDocumentoAnterior && rutaDocAnterior == null) throw new Exception("La ruta para los archivos es requerido");
            if (curso == null) throw new Exception("El Curso no debe ser nulo");
            if (previousCurso == null) throw new Exception("El Curso original no debe ser nulo");

            object myFirm = new object();

            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                CursoCtrl cursoCtrl = new CursoCtrl();

                cursoCtrl.Update(dctx, curso, previousCurso);

                FileManagerCtrl fileManager = new FileManagerCtrl();
                //si hay un archivo se registra
                if (documentoInformacion != null && !string.IsNullOrEmpty(rutaDoctos))
                {
                    fileManager.CreateFolder(rutaDoctos);

                    byte[] data = documentoInformacion.Data;

                    fileManager.WriteFile(string.Format("{0}{1}", rutaDoctos, documentoInformacion.Name), ref data);
                }

                if (eliminarDocumentoAnterior)
                {
                    string fileDelete = Path.GetFileName(((Curso)previousCurso).Informacion);
                    if (!(string.IsNullOrEmpty(fileDelete)))
                        fileManager.DeleteFile(string.Format("{0}{1}", rutaDocAnterior, fileDelete));
                }


                dctx.CommitTransaction(myFirm);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
        }

        /// <summary>
        /// Elimina un registro completo de juego de la base de datos (de manera logica)
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="curso">Curso a eliminar</param>
        /// <param name="rutaDoc">Opcional. Ruta del documento</param>
        public void DeleteComplete(IDataContext dctx, Curso curso, string rutaDoc = null)
        {
            if (curso == null) throw new Exception("El Curso no puede ser nulo");
            if (curso.AgrupadorContenidoDigitalID == null) throw new Exception("El Identificador del Curso no puede ser nulo");
            if (!string.IsNullOrEmpty(curso.Informacion) && string.IsNullOrEmpty(rutaDoc)) throw new Exception("La ruta del documento es requerida");

            object myFirm = new object();

            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                CursoCtrl cursoCtrl = new CursoCtrl();
                FileManagerCtrl fileManager = new FileManagerCtrl();

                string fileDelete = Path.GetFileName(curso.Informacion);
                if (!(string.IsNullOrEmpty(fileDelete)))
                    fileManager.DeleteFile(string.Format("{0}{1}", rutaDoc, fileDelete));

                cursoCtrl.DeleteComplete(dctx, curso.AgrupadorContenidoDigitalID.Value);

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
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
        }
    }
}
