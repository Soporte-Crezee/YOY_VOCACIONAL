using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;
using GP.SocialEngine.DAO;
using POV.Reactivos.BO;
using GP.SocialEngine.Interfaces;
using POV.Reactivos.Service;
using GP.SocialEngine.DA;


namespace GP.SocialEngine.Service
{
    /// <summary>
    /// Controlador del objeto AppSuscripcion
    /// </summary>
    public class AppSuscripcionCtrl
    {
        /// <summary>
        /// Consulta registros de AppSuscripcionRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="appSuscripcionRetHlp">AppSuscripcionRetHlp que provee el criterio de selecciÃ³n para realizar la consulta</param>
        /// <returns>El DataSet que contiene la informaciÃ³n de AppSuscripcionRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, SocialHub socialHub, AppSuscripcion appSuscripcion)
        {
            AppSuscripcionRetHlp da = new AppSuscripcionRetHlp();
            DataSet ds = da.Action(dctx, socialHub, appSuscripcion);
            return ds;
        }

        public bool IsSuscrito(IDataContext dctx, SocialHub socialHub, AppSuscripcion appSuscripcion)
        {
            DataSet dsAppSuscripcion = Retrieve(dctx, socialHub, appSuscripcion);
            bool suscrito = false;
            if (dsAppSuscripcion.Tables["AppSuscripcion"].Rows.Count > 0)
            {
                suscrito = true;
            }

            return suscrito;
        }

        /// <summary>
        /// Crea un registro de AppSuscripcionInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="appSuscripcionInsHlp">AppSuscripcionInsHlp que desea crear</param>
        public void Insert(IDataContext dctx, SocialHub socialHub, AppSuscripcion appSuscripcion)
        {
            AppSuscripcionInsHlp da = new AppSuscripcionInsHlp();
            da.Action(dctx, socialHub, appSuscripcion);
        }
        /// <summary>
        /// Actualiza de manera optimista un registro de AppSuscripcionUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="appSuscripcionUpdHlp">AppSuscripcionUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">AppSuscripcionUpdHlp que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, AppSuscripcion appSuscripcion, AppSuscripcion previous)
        {
            AppSuscripcionUpdHlp da = new AppSuscripcionUpdHlp();
            da.Action(dctx, appSuscripcion, previous);
        }
        /// <summary>
        /// Devuelve un registro completo de appSuscripcion
        /// </summary>
        public AppSuscripcion RetrieveComplete(IDataContext dctx, SocialHub socialHub, AppSuscripcion appSuscripcion)
        {
            DataSet ds = this.Retrieve(dctx, socialHub, appSuscripcion);
            int index = ds.Tables["AppSuscripcion"].Rows.Count;
            if (index < 1)
            {
                return null;
            }
            else
            {
                ReactivoCtrl reactivoCtrl = new ReactivoCtrl();
                appSuscripcion = LastDataRowToAppSuscripcion(ds);

                if (appSuscripcion.AppSocial is Reactivo) 
                {
                    Reactivo reactivo = reactivoCtrl.RetrieveComplete(dctx, new Reactivo { ReactivoID = ((Reactivo)appSuscripcion.AppSocial).ReactivoID });
                    appSuscripcion.AppSocial = reactivo;
                }

               return appSuscripcion;
            }
        }

        /// <summary>
        /// Consulta una lista de suscripciones del alumno
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="socialHub">Provee los criterios de consulta</param>
        /// <param name="appsuscripcion">Provee los criterios de consulta</param>
        /// <returns>una lista de suscripciones</returns>
        public List<AppSuscripcion> RetrieveListSuscripcion(IDataContext dctx, SocialHub socialHub, AppSuscripcion appsuscripcion) 
        {
            if (appsuscripcion == null) throw new Exception("Appsuscripcion no puede ser nulo");

            List<AppSuscripcion> suscripciones = new List<AppSuscripcion>();

            if (socialHub != null && socialHub.SocialHubID != null) 
            {
                DataSet dsSuscripciones = Retrieve(dctx, socialHub, appsuscripcion);

                if (dsSuscripciones.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsSuscripciones.Tables[0].Rows)
                    {
                        AppSuscripcion suscripcion = DataRowToAppSuscripcion(dr);
                        suscripciones.Add(suscripcion);
                    }
                }
            }
            return suscripciones;
        }

       
        /// <summary>
        /// Consulta las suscripciones del alumno con paginador
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="pageSize">Provee los criterios del paginador</param>
        /// <param name="currentPage">Provee los criterios del paginador</param>
        /// <param name="sortColumn">Provee los criterios del paginador</param>
        /// <param name="sortorde">Provee los criterios del paginador</param>
        /// <param name="socialHub">Provee los criterios de consulta</param>
        /// <param name="appSuscripcion">Provee los criterios de consulta</param>
        /// <returns>El DataSet que contiene la informaciÃ³n de AppSuscripcionDARetHlp generada por la consulta</returns>
        public DataSet RetrieveSuscripciones(IDataContext dctx, int pageSize, int currentPage, string sortColumn, string sortorde, SocialHub socialHub, AppSuscripcion appSuscripcion)
        {
            AppSuscripcionDARetHlp da = new AppSuscripcionDARetHlp();
            DataSet ds = da.Action(dctx, pageSize, currentPage, sortColumn, sortorde, socialHub, appSuscripcion);
            return ds;
        }


        /// <summary>
        /// Elimina un registro de AppSuscripcionDelHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="appSuscripcionDelHlp">AppSuscripcionDelHlp que desea eliminar</param>
        public void Delete(IDataContext dctx, AppSuscripcion appSuscripcion, SocialHub socialHub)
        {
            AppSuscripcionDelHlp da = new AppSuscripcionDelHlp();
            da.Action(dctx, appSuscripcion, socialHub);
        }

        public void DeleteByAppSocial(IDataContext dctx, IAppSocial appSocial)
        {
            DeleteAppSuscripcionByAppSocialHlp da = new DeleteAppSuscripcionByAppSocialHlp();

            da.Action(dctx, appSocial);
        }
        /// <summary>
        /// Crea un objeto de AppSuscripcion a partir de los datos del Ãºltimo DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la informaciÃ³n de AppSuscripcion</param>
        /// <returns>Un objeto de AppSuscripcion creado a partir de los datos</returns>
        public AppSuscripcion LastDataRowToAppSuscripcion(DataSet ds)
        {
            if (!ds.Tables.Contains("AppSuscripcion"))
                throw new Exception("LastDataRowToAppSuscripcion: DataSet no tiene la tabla AppSuscripcion");
            int index = ds.Tables["AppSuscripcion"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToAppSuscripcion: El DataSet no tiene filas");
            return this.DataRowToAppSuscripcion(ds.Tables["AppSuscripcion"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de AppSuscripcion a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la informaciÃ³n de AppSuscripcion</param>
        /// <returns>Un objeto de AppSuscripcion creado a partir de los datos</returns>
        public AppSuscripcion DataRowToAppSuscripcion(DataRow row)
        {
            AppSuscripcion appSuscripcion = new AppSuscripcion();
            if (row.IsNull("AppSuscripcionID"))
                appSuscripcion.AppSuscripcionID = null;
            else
                appSuscripcion.AppSuscripcionID = (long)Convert.ChangeType(row["AppSuscripcionID"], typeof(long));
            if (row.IsNull("FechaRegistro"))
                appSuscripcion.FechaRegistro = null;
            else
                appSuscripcion.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("Estatus"))
                appSuscripcion.Estatus = null;
            else
                appSuscripcion.Estatus = (bool)Convert.ChangeType(row["Estatus"], typeof(bool));
            if (row.IsNull("AppType"))
                appSuscripcion.AppType = null;
            else
            {
                appSuscripcion.ToShortAppType = (short)Convert.ChangeType(row["AppType"], typeof(short));
                switch (appSuscripcion.AppType)
                {
                    case EAppType.REACTIVO:
                        if (row.IsNull("AppSocialID"))
                            appSuscripcion.AppSocial = null;
                        else
                        {
                            Reactivo reactivo = new Reactivo();
                            reactivo.ReactivoID = (Guid)Convert.ChangeType(row["AppSocialID"], typeof(Guid));
                            appSuscripcion.AppSocial = reactivo;
                        }
                        break;
                }

            }
            return appSuscripcion;
        }
    }
}
