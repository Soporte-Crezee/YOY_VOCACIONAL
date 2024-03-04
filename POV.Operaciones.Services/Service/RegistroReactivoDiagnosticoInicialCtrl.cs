using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using POV.Reactivos.BO;
using POV.Operaciones.BO;
using POV.Reactivos.Service;
using Framework.Base.DataAccess;
using POV.Comun.Service;
using POV.Logger.Service;

namespace POV.Operaciones.Service
{
    /// <summary>
    /// Servicios para registrar preguntas iniciales.
    /// </summary>
    public class RegistroReactivoDiagnosticoInicialCtrl
    {

        /// <summary>
        /// Crea un registro de pregunta inicial en la base de datos
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="preguntaInicialTemporal">Wrapper para la pregunta inicial.</param>
        /// <param name="rutaImagenes">ruta absoluta donde se guardaran las imagenes</param>
        public void InsertReactivoDiagnosticoInicial(IDataContext dctx, PreguntaInicialTemporal preguntaInicialTemporal, string rutaImagenes)
        {
            #region *** validaciones ***
            string sError = string.Empty;

            if (preguntaInicialTemporal.Reactivo == null) throw new Exception("RegistroReactivoDiagnosticoInicialCtrl: El parámetro 'reactivo' no puede ser vacio.");
            if (preguntaInicialTemporal.Reactivo.TipoReactivo == null) throw new Exception("RegistroReactivoDiagnosticoInicialCtrl: El campo 'reactivo.TipoReactivo' no puede ser vacio.");

            if (preguntaInicialTemporal.Reactivo.TipoReactivo != ETipoReactivo.InicialDiagnostico)
                throw new Exception("RegistroReactivoDiagnosticoInicialCtrl: El tipo de reactivo debe ser una pregunta inicial.");
            if (preguntaInicialTemporal.preguntaTemporal == null)
                throw new Exception("RegistroReactivoDiagnosticoInicialCtrl: El parámetro 'preguntaTemporal' no puede ser vacio.");

            if (preguntaInicialTemporal.preguntaTemporal.ImagenOpcion1 == null)
                throw new Exception("RegistroReactivoDiagnosticoInicialCtrl: El parámetro 'ImagenOpcion1' de la pregunta Temporal no puede ser vacio.");

            if (preguntaInicialTemporal.preguntaTemporal.ImagenOpcion2 == null)
                throw new Exception("RegistroReactivoDiagnosticoInicialCtrl: El parámetro 'ImagenOpcion2' de la pregunta Temporal no puede ser vacio.");

            if (preguntaInicialTemporal.preguntaTemporal.ImagenOpcion3 == null)
                throw new Exception("RegistroReactivoDiagnosticoInicialCtrl: El parámetro 'ImagenOpcion3' de la pregunta Temporal no puede ser vacio.");

            if (preguntaInicialTemporal.preguntaTemporal.ImagenOpcion4 == null)
                throw new Exception("RegistroReactivoDiagnosticoInicialCtrl: El parámetro 'ImagenOpcion4' de la pregunta Temporal no puede ser vacio.");

            if (preguntaInicialTemporal.preguntaTemporal.ImagenOpcion5 == null)
                throw new Exception("RegistroReactivoDiagnosticoInicialCtrl: El parámetro 'ImagenOpcion5' de la pregunta Temporal no puede ser vacio.");

            if (preguntaInicialTemporal.preguntaTemporal.ImagenOpcion6 == null)
                throw new Exception("RegistroReactivoDiagnosticoInicialCtrl: El parámetro 'ImagenOpcion6' de la pregunta Temporal no puede ser vacio.");

            if (string.IsNullOrEmpty(preguntaInicialTemporal.preguntaTemporal.ImagenOpcion1.ImageName))
                throw new Exception("RegistroReactivoDiagnosticoInicialCtrl: El parámetro 'ImagenOpcion1' de la pregunta Temporal no puede ser vacio.");

            if (string.IsNullOrEmpty(preguntaInicialTemporal.preguntaTemporal.ImagenOpcion2.ImageName))
                throw new Exception("RegistroReactivoDiagnosticoInicialCtrl: El parámetro 'ImagenOpcion2' de la pregunta Temporal no puede ser vacio.");

            if (string.IsNullOrEmpty(preguntaInicialTemporal.preguntaTemporal.ImagenOpcion3.ImageName))
                throw new Exception("RegistroReactivoDiagnosticoInicialCtrl: El parámetro 'ImagenOpcion3' de la pregunta Temporal no puede ser vacio.");

            if (string.IsNullOrEmpty(preguntaInicialTemporal.preguntaTemporal.ImagenOpcion4.ImageName))
                throw new Exception("RegistroReactivoDiagnosticoInicialCtrl: El parámetro 'ImagenOpcion4' de la pregunta Temporal no puede ser vacio.");

            if (string.IsNullOrEmpty(preguntaInicialTemporal.preguntaTemporal.ImagenOpcion5.ImageName))
                throw new Exception("RegistroReactivoDiagnosticoInicialCtrl: El parámetro 'ImagenOpcion5' de la pregunta Temporal no puede ser vacio.");

            if (string.IsNullOrEmpty(preguntaInicialTemporal.preguntaTemporal.ImagenOpcion6.ImageName))
                throw new Exception("RegistroReactivoDiagnosticoInicialCtrl: El parámetro 'ImagenOpcion6' de la pregunta Temporal no puede ser vacio.");
            #endregion

            object myFirm = new object();

            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                ReactivoCtrl reactivoCtrl = new ReactivoCtrl();
                FileManagerCtrl fileManagerCtrl = new FileManagerCtrl();

                //validamos que no exista el reactivo con la misma edad

                if (ExistByEdadAplicacion(dctx, preguntaInicialTemporal.Reactivo))
                    throw new Exception("Ya existe una pregunta inicial configurada con la edad seleccionada.");

                reactivoCtrl.InsertComplete(dctx, preguntaInicialTemporal.Reactivo);
                fileManagerCtrl.CreateFolder(rutaImagenes);
                
                
                byte[] imageData1 = preguntaInicialTemporal.preguntaTemporal.ImagenOpcion1.ImageData;
                fileManagerCtrl.WriteFile(string.Format("{0}{1}", rutaImagenes, preguntaInicialTemporal.preguntaTemporal.ImagenOpcion1.ImageName), ref imageData1);

                byte[] imageData2 = preguntaInicialTemporal.preguntaTemporal.ImagenOpcion2.ImageData;
                fileManagerCtrl.WriteFile(string.Format("{0}{1}", rutaImagenes, preguntaInicialTemporal.preguntaTemporal.ImagenOpcion2.ImageName), ref imageData2);

                byte[] imageData3 = preguntaInicialTemporal.preguntaTemporal.ImagenOpcion3.ImageData;
                fileManagerCtrl.WriteFile(string.Format("{0}{1}", rutaImagenes, preguntaInicialTemporal.preguntaTemporal.ImagenOpcion3.ImageName), ref imageData3);

                byte[] imageData4 = preguntaInicialTemporal.preguntaTemporal.ImagenOpcion4.ImageData;
                fileManagerCtrl.WriteFile(string.Format("{0}{1}", rutaImagenes, preguntaInicialTemporal.preguntaTemporal.ImagenOpcion4.ImageName), ref imageData4);

                byte[] imageData5 = preguntaInicialTemporal.preguntaTemporal.ImagenOpcion5.ImageData;
                fileManagerCtrl.WriteFile(string.Format("{0}{1}", rutaImagenes, preguntaInicialTemporal.preguntaTemporal.ImagenOpcion5.ImageName), ref imageData5);

                byte[] imageData6 = preguntaInicialTemporal.preguntaTemporal.ImagenOpcion6.ImageData;
                fileManagerCtrl.WriteFile(string.Format("{0}{1}", rutaImagenes, preguntaInicialTemporal.preguntaTemporal.ImagenOpcion6.ImageName), ref imageData6);

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
        /// Actualiza un registro de pregunta inicial en la base de datos.
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="preguntaInicialTemporal">Wrapper para la pregunta inicial</param>
        /// <param name="rutaImagenes">ruta absoluta donde se guardaran las imagenes</param>
        public void UpdateReactivoDiagnosticoInicial(IDataContext dctx, PreguntaInicialTemporal preguntaInicialTemporal, string rutaImagenes)
        {
            #region *** validaciones ***
            string sError = string.Empty;

            if (preguntaInicialTemporal.Reactivo == null) throw new Exception("RegistroReactivoDiagnosticoInicialCtrl: El parámetro 'reactivo' no puede ser vacio.");
            if (preguntaInicialTemporal.Reactivo.TipoReactivo == null) throw new Exception("RegistroReactivoDiagnosticoInicialCtrl: El campo 'reactivo.TipoReactivo' no puede ser vacio.");

            if (preguntaInicialTemporal.Reactivo.TipoReactivo != ETipoReactivo.InicialDiagnostico)
                throw new Exception("RegistroReactivoDiagnosticoInicialCtrl: El tipo de reactivo debe ser una pregunta inicial.");

            if (preguntaInicialTemporal.preguntaTemporal == null)
                throw new Exception("RegistroReactivoDiagnosticoInicialCtrl: El parámetro 'preguntaTemporal' no puede ser vacio.");
            #endregion

            object myFirm = new object();

            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                ReactivoCtrl reactivoCtrl = new ReactivoCtrl();
                FileManagerCtrl fileManagerCtrl = new FileManagerCtrl();

                //validamos que no exista el reactivo con la misma edad
                if (ExistByEdadAplicacion(dctx,preguntaInicialTemporal.Reactivo))
                    throw new Exception("Ya existe una pregunta inicial configurada con la edad seleccionada.");
                
                reactivoCtrl.UpdateComplete(dctx, preguntaInicialTemporal.Reactivo);
                fileManagerCtrl.CreateFolder(rutaImagenes);

                if (preguntaInicialTemporal.preguntaTemporal.ImagenOpcion1 != null && !string.IsNullOrEmpty(preguntaInicialTemporal.preguntaTemporal.ImagenOpcion1.ImageName))
                {
                    byte[] imageData1 = preguntaInicialTemporal.preguntaTemporal.ImagenOpcion1.ImageData;
                    fileManagerCtrl.WriteFile(string.Format("{0}{1}", rutaImagenes, preguntaInicialTemporal.preguntaTemporal.ImagenOpcion1.ImageName), ref imageData1);
                }

                if (preguntaInicialTemporal.preguntaTemporal.ImagenOpcion2 != null && !string.IsNullOrEmpty(preguntaInicialTemporal.preguntaTemporal.ImagenOpcion2.ImageName))
                {
                    byte[] imageData2 = preguntaInicialTemporal.preguntaTemporal.ImagenOpcion2.ImageData;
                    fileManagerCtrl.WriteFile(string.Format("{0}{1}", rutaImagenes, preguntaInicialTemporal.preguntaTemporal.ImagenOpcion2.ImageName), ref imageData2);
                }

                if (preguntaInicialTemporal.preguntaTemporal.ImagenOpcion3 != null && !string.IsNullOrEmpty(preguntaInicialTemporal.preguntaTemporal.ImagenOpcion3.ImageName))
                {
                    byte[] imageData3 = preguntaInicialTemporal.preguntaTemporal.ImagenOpcion3.ImageData;
                    fileManagerCtrl.WriteFile(string.Format("{0}{1}", rutaImagenes, preguntaInicialTemporal.preguntaTemporal.ImagenOpcion3.ImageName), ref imageData3);
                }

                if (preguntaInicialTemporal.preguntaTemporal.ImagenOpcion4 != null && !string.IsNullOrEmpty(preguntaInicialTemporal.preguntaTemporal.ImagenOpcion4.ImageName))
                {
                    byte[] imageData4 = preguntaInicialTemporal.preguntaTemporal.ImagenOpcion4.ImageData;
                    fileManagerCtrl.WriteFile(string.Format("{0}{1}", rutaImagenes, preguntaInicialTemporal.preguntaTemporal.ImagenOpcion4.ImageName), ref imageData4);
                }

                if (preguntaInicialTemporal.preguntaTemporal.ImagenOpcion5 != null && !string.IsNullOrEmpty(preguntaInicialTemporal.preguntaTemporal.ImagenOpcion5.ImageName))
                {
                    byte[] imageData5 = preguntaInicialTemporal.preguntaTemporal.ImagenOpcion5.ImageData;
                    fileManagerCtrl.WriteFile(string.Format("{0}{1}", rutaImagenes, preguntaInicialTemporal.preguntaTemporal.ImagenOpcion5.ImageName), ref imageData5);
                }

                if (preguntaInicialTemporal.preguntaTemporal.ImagenOpcion6 != null && !string.IsNullOrEmpty(preguntaInicialTemporal.preguntaTemporal.ImagenOpcion6.ImageName))
                {
                    byte[] imageData6 = preguntaInicialTemporal.preguntaTemporal.ImagenOpcion6.ImageData;
                    fileManagerCtrl.WriteFile(string.Format("{0}{1}", rutaImagenes, preguntaInicialTemporal.preguntaTemporal.ImagenOpcion6.ImageName), ref imageData6);
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
        /// Valida que una reactivo diagnostico inicial exista con determinada edad.
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="reactivo">Reactivo que se quiere validar si existe la edad</param>
        /// <returns>true en caso de existir uno con la edad, false en caso contrario</returns>
        public bool ExistByEdadAplicacion(IDataContext dctx, Reactivo reactivo)
        {
            ReactivoCtrl reactivoCtrl = new ReactivoCtrl();

            Reactivo reactivoTemp = new Reactivo
            {
                Caracteristicas = new CaracteristicasDiagnosticoInicial(),
                Activo = true,
                TipoReactivo = ETipoReactivo.InicialDiagnostico
            };
            (reactivoTemp.Caracteristicas as CaracteristicasDiagnosticoInicial).Edad = (reactivo.Caracteristicas as CaracteristicasDiagnosticoInicial).Edad;

            DataSet ds = reactivoCtrl.Retrieve(dctx, reactivoTemp);

            if (ds.Tables[0].Rows.Count > 0 && reactivo.ReactivoID != reactivoCtrl.LastDataRowToReactivo(ds, ETipoReactivo.InicialDiagnostico).ReactivoID)
                return true;

            return false;
        }
    }
}
