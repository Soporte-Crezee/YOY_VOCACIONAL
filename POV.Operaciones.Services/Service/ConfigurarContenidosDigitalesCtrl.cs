using System;
using System.Data;
using Framework.Base.DataAccess;
using POV.ContenidosDigital.Busqueda.Service;
using POV.Logger.Service;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.Service;
using POV.ContenidosDigital.Busqueda.BO;

namespace POV.Operaciones.Service
{
    /// <summary>
    /// Controlador de la configuracion de contenidos digitales
    /// </summary>
    public class ConfigurarContenidosDigitalesCtrl
    {
        public void InsertComplete(IDataContext dctx, ContenidoDigitalAgrupador contenidoDigitalAgrupador)
        {
            #region validaciones
            if (contenidoDigitalAgrupador == null)
                throw new ArgumentNullException("contenidoDigitalAgrupador", "ConfigurarContenidosDigitalesCtrl: el ContenidoDigitalAgrupador no puede ser nulo");
            if (contenidoDigitalAgrupador.AgrupadorContenidoDigital == null)
                throw new ArgumentNullException("contenidoDigitalAgrupador", "ConfigurarContenidosDigitalesCtrl: el AgrupadorContenidoDigital no puede ser nulo");
            if (contenidoDigitalAgrupador.ContenidoDigital == null)
                throw new ArgumentNullException("contenidoDigitalAgrupador", "ConfigurarContenidosDigitalesCtrl: el ContenidoDigital no puede ser nulo");
            if (contenidoDigitalAgrupador.EjeTematico == null)
                throw new ArgumentNullException("contenidoDigitalAgrupador", "ConfigurarContenidosDigitalesCtrl: el EjeTematico no puede ser nulo");
            if (contenidoDigitalAgrupador.SituacionAprendizaje == null)
                throw new ArgumentNullException("contenidoDigitalAgrupador", "ConfigurarContenidosDigitalesCtrl: la SituacionAprendizaje no puede ser nulo");
            #endregion

            var agrupadorContenidoDigitalCtrl = new AgrupadorContenidoDigitalCtrl();
            var contenidoDigitalAgrupadorCtrl = new ContenidoDigitalAgrupadorCtrl();
            var palabraClaveContenidoCtrl = new PalabraClaveContenidoCtrl();

            #region Comienza transaccion
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            #endregion

            try
            {
                #region Insertar AAgrupadorContenidoDigital con su ContenidoDigital
                agrupadorContenidoDigitalCtrl.InsertAgrupadorContenidoDigitalDetalle(dctx, 
                    contenidoDigitalAgrupador.AgrupadorContenidoDigital, contenidoDigitalAgrupador.ContenidoDigital);
                #endregion

                #region Insertar ContenidoDigitalAgrupador y Recuperarlo
                contenidoDigitalAgrupadorCtrl.Insert(dctx, contenidoDigitalAgrupador);

                DataSet dscontenidoDigitalAgrupador = contenidoDigitalAgrupadorCtrl.Retrieve(dctx, contenidoDigitalAgrupador);

                if (dscontenidoDigitalAgrupador.Tables[0].Rows.Count <= 0)
                    throw new Exception("ConfigurarContenidosDigitalesCtrl: InsertComplete, No se encontró ningún registro");
                if (dscontenidoDigitalAgrupador.Tables[0].Rows.Count > 1)
                    throw new Exception("ConfigurarContenidosDigitalesCtrl: InsertComplete, La consulta devolvió más de un registro");

                contenidoDigitalAgrupador =
                    contenidoDigitalAgrupadorCtrl.LastDataRowToContenidoDigitalAgrupador(dscontenidoDigitalAgrupador);
                #endregion

                #region Procesar PalabraClaveContenidoDigital
                palabraClaveContenidoCtrl.InsertContenidoDigitalAgrupador(dctx, contenidoDigitalAgrupador);
                #endregion

                #region Commit transaccion
                dctx.CommitTransaction(myFirm);
                #endregion
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                if (dctx.ConnectionState == ConnectionState.Open)
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

        public void DeleteComplete(IDataContext dctx, ContenidoDigitalAgrupador contenidoDigitalAgrupador)
        {
            #region validaciones
            if (contenidoDigitalAgrupador == null)
                throw new ArgumentNullException("contenidoDigitalAgrupador", "ConfigurarContenidosDigitalesCtrl: el ContenidoDigitalAgrupador no puede ser nulo");

            if (contenidoDigitalAgrupador.AgrupadorContenidoDigital == null)
                throw new ArgumentNullException("contenidoDigitalAgrupador", "ConfigurarContenidosDigitalesCtrl: el AgrupadorContenidoDigital no puede ser nulo");

            if (contenidoDigitalAgrupador.ContenidoDigital == null)
                throw new ArgumentNullException("contenidoDigitalAgrupador", "ConfigurarContenidosDigitalesCtrl: el ContenidoDigital no puede ser nulo");

            if (contenidoDigitalAgrupador.EjeTematico == null)
                throw new ArgumentNullException("contenidoDigitalAgrupador", "ConfigurarContenidosDigitalesCtrl: el EjeTematico no puede ser nulo");

            if (contenidoDigitalAgrupador.SituacionAprendizaje == null)
                throw new ArgumentNullException("contenidoDigitalAgrupador", "ConfigurarContenidosDigitalesCtrl: la SituacionAprendizaje no puede ser nulo");
            #endregion

            var contenidoDigitalAgrupadorCtrl = new ContenidoDigitalAgrupadorCtrl();
            var palabraClaveContenidoCtrl = new PalabraClaveContenidoCtrl();
            var palabraClaveCtrl = new PalabraClaveCtrl();
                        
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            
            try 
            {                
                DataSet dsContenido = contenidoDigitalAgrupadorCtrl.Retrieve(dctx, contenidoDigitalAgrupador);
                ContenidoDigitalAgrupador miContenidoDigitalAgrupador = new ContenidoDigitalAgrupador();
                miContenidoDigitalAgrupador = contenidoDigitalAgrupadorCtrl.LastDataRowToContenidoDigitalAgrupador(dsContenido);

                DataSet dsPalabraClaveCD = palabraClaveContenidoCtrl.RetrievePalabraClaveContenidoDigital(dctx, new PalabraClaveContenidoDigital(), miContenidoDigitalAgrupador);
                if (dsPalabraClaveCD.Tables["PalabraClaveContenidoDigital"].Rows.Count > 0)
                {                    
                    foreach (DataRow drPalabraClaveContenido in dsPalabraClaveCD.Tables["PalabraClaveContenidoDigital"].Rows)
                    {
                        PalabraClaveContenidoDigital palabraClaveCD = new PalabraClaveContenidoDigital();
                        palabraClaveCD.PalabraClaveContenidoID = int.Parse(drPalabraClaveContenido["PalabraClaveContenidoID"].ToString());
                        
                        DataSet ds = palabraClaveContenidoCtrl.RetrievePalabraClaveContenidoDigital(dctx, palabraClaveCD, new ContenidoDigitalAgrupador());
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            DataSet dsPalabraClave = palabraClaveContenidoCtrl.Retrieve(dctx, palabraClaveCD);

                            APalabraClaveContenido palabraClaveContenido = new PalabraClaveContenidoDigital();
                            palabraClaveContenido = palabraClaveContenidoCtrl.LastDataRowToAPalabraClaveContenido(dsPalabraClave);

                            var palabraClave = new PalabraClave();
                            palabraClave.PalabraClaveID = palabraClaveContenido.PalabraClave.PalabraClaveID;

                            //Eliminar PalabraClaveContenidoDigital
                            palabraClaveContenidoCtrl.DeletePalabraClaveContenidoDigital(dctx, palabraClaveCD, miContenidoDigitalAgrupador);
                            //Eliminar PalabraClaveContenido
                            palabraClaveContenidoCtrl.Delete(dctx, palabraClaveCD);
                            //Eliminar PalabraClave
                            palabraClaveCtrl.Delete(dctx, palabraClave);
                        }
                        else
                        {
                            //Eliminar PalabraClaveContenidoDigital
                            palabraClaveContenidoCtrl.DeletePalabraClaveContenidoDigital(dctx, palabraClaveCD, miContenidoDigitalAgrupador);
                        }
                    }                    
                }
                                
                //Eliminar ContenidoDigitalAgrupador y AgrupadorContenidoDigitalDetalle
                contenidoDigitalAgrupadorCtrl.DeleteComplete(dctx, contenidoDigitalAgrupador);

                dctx.CommitTransaction(myFirm);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                if (dctx.ConnectionState == ConnectionState.Open)
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
