using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.Prueba.Diagnostico.DAO.Bullying;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace POV.Prueba.Diagnostico.Service
{
    public class BateriaBullyingCtrl
    {
        #region AUTOCONCEPTO
        public DataSet RetrieveResultadoBullyingAutoconcepto(IDataContext dctx, Alumno alumno)
        {
            ViewResultadoPruebaAutoconceptoRetHlp da = new ViewResultadoPruebaAutoconceptoRetHlp();
            DataSet ds = da.Action(dctx, alumno);
            return ds;
        }
        #endregion
        #region ACTITUDES
        public DataSet RetrieveResultadoBullyingActitudes(IDataContext dctx, Alumno alumno)
        {
            ViewResultadoPruebaActitudesRetHlp da = new ViewResultadoPruebaActitudesRetHlp();
            DataSet ds = da.Action(dctx, alumno);
            return ds;
        }
        #endregion
        #region EMPATIA
        public DataSet RetrieveResultadoBullyingEmpatia(IDataContext dctx, Alumno alumno)
        {
            ViewResultadoPruebaEmpatiaRetHlp da = new ViewResultadoPruebaEmpatiaRetHlp();
            DataSet ds = da.Action(dctx, alumno);
            return ds;
        }
        #endregion
        #region HUMOR
        public DataSet RetrieveResultadoBullyingHumor(IDataContext dctx, Alumno alumno)
        {
            ViewResultadoPruebaHumorRetHlp da = new ViewResultadoPruebaHumorRetHlp();
            DataSet ds = da.Action(dctx, alumno);
            return ds;
        }
        #endregion
        #region VICTIMIZACION
        public DataSet RetrieveResultadoBullyingVictimizacion(IDataContext dctx, Alumno alumno)
        {
            ViewResultadoPruebaVictimizacionRetHlp da = new ViewResultadoPruebaVictimizacionRetHlp();
            DataSet ds = da.Action(dctx, alumno);
            return ds;
        }
        #endregion
        #region CIBERBULLYING
        public DataSet RetrieveResultadoBullyingCiberbullying(IDataContext dctx, Alumno alumno)
        {
            ViewResultadoPruebaCiberbullyingRetHlp da = new ViewResultadoPruebaCiberbullyingRetHlp();
            DataSet ds = da.Action(dctx, alumno);
            return ds;
        }
        #endregion
        #region BULLYING
        public DataSet RetrieveResultadoBullyingBullying(IDataContext dctx, Alumno alumno)
        {
            ViewResultadoPruebaBullyingRetHlp da = new ViewResultadoPruebaBullyingRetHlp();
            DataSet ds = da.Action(dctx, alumno);
            return ds;
        }
        #endregion
        #region VIOLENCIA
        public DataSet RetrieveResultadoBullyingViolencia(IDataContext dctx, Alumno alumno)
        {
            ViewResultadoPruebaViolenciaRetHlp da = new ViewResultadoPruebaViolenciaRetHlp();
            DataSet ds = da.Action(dctx, alumno);
            return ds;
        }
        #endregion
        #region COMUNICACION
        public DataSet RetrieveResultadoBullyingComunicacion(IDataContext dctx, Alumno alumno)
        {
            ViewResultadoPruebaComunicacionRetHlp da = new ViewResultadoPruebaComunicacionRetHlp();
            DataSet ds = da.Action(dctx, alumno);
            return ds;
        }
        #endregion
        #region IMAGEN
        public DataSet RetrieveResultadoBullyingImagen(IDataContext dctx, Alumno alumno)
        {
            ViewResultadoPruebaImagenRetHlp da = new ViewResultadoPruebaImagenRetHlp();
            DataSet ds = da.Action(dctx, alumno);
            return ds;
        }
        #endregion
        #region ANSIEDAD
        public DataSet RetrieveResultadoBullyingAnsiedad(IDataContext dctx, Alumno alumno)
        {
            ViewResultadoPruebaAnsiedadRetHlp da = new ViewResultadoPruebaAnsiedadRetHlp();
            DataSet ds = da.Action(dctx, alumno);
            return ds;
        }
        #endregion
        #region DEPRESION
        public DataSet RetrieveResultadoBullyingDepresion(IDataContext dctx, Alumno alumno)
        {
            ViewResultadoPruebaDepresionRetHlp da = new ViewResultadoPruebaDepresionRetHlp();
            DataSet ds = da.Action(dctx, alumno);
            return ds;
        }
        #endregion
       
    }
}
