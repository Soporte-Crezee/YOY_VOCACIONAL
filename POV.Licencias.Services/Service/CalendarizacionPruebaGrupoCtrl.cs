using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Licencias.BO;
using POV.Licencias.DAO;
using POV.CentroEducativo.BO;
using POV.Prueba.BO;
using POV.CentroEducativo.Service;

namespace POV.Licencias.Service
{
    /// <summary>
    /// Controlador del objeto CalendarizacionPruebaGrupo
    /// </summary>
    public class CalendarizacionPruebaGrupoCtrl
    {
        /// <summary>
        /// Consulta registros de CalendarizacionPruebaGrupoRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="calendarizacionPruebaGrupoRetHlp">CalendarizacionPruebaGrupoRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de CalendarizacionPruebaGrupoRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, CalendarizacionPruebaGrupo calendarizacionPruebaGrupo)
        {
            CalendarizacionPruebaGrupoRetHlp da = new CalendarizacionPruebaGrupoRetHlp();
            DataSet ds = da.Action(dctx, calendarizacionPruebaGrupo);
            return ds;
        }
        /// <summary>
        /// Consulta todos los registros de CalendarizacionPrueba en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="grupoCicloEscolar">GrupoCicloEscolar que provee el criterio de selección para realizar la consulta</param>
        /// <returns>Una lista de CalendarizacionPruebaGrupo creado a partir de los datos</returns>
        public List<CalendarizacionPruebaGrupo> RetrieveListaCalendarizacionPruebaGrupo(IDataContext dctx, GrupoCicloEscolar grupoCicloEscolar)
        {
            if (grupoCicloEscolar == null) throw new Exception("El GrupoCicloEscolar no puede ser nulo.");
            if (grupoCicloEscolar.GrupoCicloEscolarID == null) throw new Exception("El GrupoCicloEscolarID no puede ser nulo.");

            List<CalendarizacionPruebaGrupo> listaCalendarizaciones = new List<CalendarizacionPruebaGrupo>();
            CalendarizacionPruebaGrupo calendarizacionPruebaGrupo = new CalendarizacionPruebaGrupo();
            calendarizacionPruebaGrupo.GrupoCicloEscolar = grupoCicloEscolar;
            DataSet ds = null;
            ds = Retrieve(dctx, calendarizacionPruebaGrupo);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow cont in ds.Tables[0].Rows)
                {
                    CalendarizacionPruebaGrupo calendarizaciones = new CalendarizacionPruebaGrupo();
                    calendarizaciones = DataRowToCalendarizacionPruebaGrupo(cont);
                    listaCalendarizaciones.Add(calendarizaciones);
                }
            }
            return listaCalendarizaciones;
        }

        /// <summary>
        /// Consulta y obtiene la lista de calendarizaciones para una escuela en ciclo escolar de todos sus grupos para una prueba (opcional) especifica
        /// </summary>
        /// <param name="dctx">El DataContext que proveera acceso a la base de datos</param>
        /// <param name="escuela">Escuela que se usara como filtro</param>
        /// <param name="cicloEscolar">CicloEscolar que se usara como filtro</param>
        /// <param name="prueba">Prueba que se usara como filtro opcional</param>
        /// <returns>Una lista de CalendarizacionPruebaGrupo creado a partir de los datos</returns>
        public List<CalendarizacionPruebaGrupo> RetrieveCalendarizacionPruebaGrupoEscuela(IDataContext dctx, Escuela escuela, CicloEscolar cicloEscolar, APrueba prueba)
        {
            List<CalendarizacionPruebaGrupo> listaCalendarizaciones = new List<CalendarizacionPruebaGrupo>();
            CalendarizacionPruebaGrupoRetHlp da = new CalendarizacionPruebaGrupoRetHlp();
            DataSet ds = da.Action(dctx, escuela, cicloEscolar, prueba);
            GrupoCicloEscolarCtrl grupoCtrl = new GrupoCicloEscolarCtrl();
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow cont in ds.Tables[0].Rows)
                {
                    CalendarizacionPruebaGrupo calendarizaciones = new CalendarizacionPruebaGrupo();
                    calendarizaciones = DataRowToCalendarizacionPruebaGrupo(cont);
                    calendarizaciones.GrupoCicloEscolar = grupoCtrl.RetriveComplete(dctx, new GrupoCicloEscolar { GrupoCicloEscolarID = calendarizaciones.GrupoCicloEscolar.GrupoCicloEscolarID , Escuela = escuela});

                    listaCalendarizaciones.Add(calendarizaciones);
                }
            }


            return listaCalendarizaciones;
        }
        /// <summary>
        /// Crea un registro de CalendarizacionPruebaGrupoInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="calendarizacionPruebaGrupoInsHlp">CalendarizacionPruebaGrupoInsHlp que desea crear</param>
        public void Insert(IDataContext dctx, CalendarizacionPruebaGrupo calendarizacionPruebaGrupo)
        {
            CalendarizacionPruebaGrupo calendarizacionFiltro = new CalendarizacionPruebaGrupo
            {
                GrupoCicloEscolar = calendarizacionPruebaGrupo.GrupoCicloEscolar,
                PruebaContrato = calendarizacionPruebaGrupo.PruebaContrato
            };

            if (Retrieve(dctx, calendarizacionFiltro).Tables[0].Rows.Count > 0)
                throw new Exception("La prueba seleccionada ya se encuentra asignada al grupo.");
            else
            {
                CalendarizacionPruebaGrupoInsHlp da = new CalendarizacionPruebaGrupoInsHlp();
                da.Action(dctx, calendarizacionPruebaGrupo);
            }
        }
        /// <summary>
        /// Actualiza de manera optimista un registro de CalendarizacionPruebaGrupoUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="calendarizacionPruebaGrupoUpdHlp">CalendarizacionPruebaGrupoUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">CalendarizacionPruebaGrupoUpdHlp que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, CalendarizacionPruebaGrupo calendarizacionPruebaGrupo, CalendarizacionPruebaGrupo previous)
        {
            CalendarizacionPruebaGrupoUpdHlp da = new CalendarizacionPruebaGrupoUpdHlp();
            da.Action(dctx, calendarizacionPruebaGrupo, previous);
        }
        /// <summary>
        /// Elimina un registro de CalendarizacionPruebaGrupoDelHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="calendarizacionPruebaGrupoDelHlp">CalendarizacionPruebaGrupoDelHlp que desea eliminar</param>
        public void Delete(IDataContext dctx, CalendarizacionPruebaGrupo calendarizacionPruebaGrupo)
        {
            CalendarizacionPruebaGrupoDelHlp da = new CalendarizacionPruebaGrupoDelHlp();
            da.Action(dctx, calendarizacionPruebaGrupo);
        }
        /// <summary>
        /// Crea un objeto de CalendarizacionPruebaGrupo a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de CalendarizacionPruebaGrupo</param>
        /// <returns>Un objeto de CalendarizacionPruebaGrupo creado a partir de los datos</returns>
        public CalendarizacionPruebaGrupo LastDataRowToCalendarizacionPruebaGrupo(DataSet ds)
        {
            if (!ds.Tables.Contains("CalendarizacionPruebaGrupo"))
                throw new Exception("LastDataRowToCalendarizacionPruebaGrupo: DataSet no tiene la tabla CalendarizacionPruebaGrupo");
            int index = ds.Tables["CalendarizacionPruebaGrupo"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToCalendarizacionPruebaGrupo: El DataSet no tiene filas");
            return this.DataRowToCalendarizacionPruebaGrupo(ds.Tables["CalendarizacionPruebaGrupo"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de CalendarizacionPruebaGrupo a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de CalendarizacionPruebaGrupo</param>
        /// <returns>Un objeto de CalendarizacionPruebaGrupo creado a partir de los datos</returns>
        public CalendarizacionPruebaGrupo DataRowToCalendarizacionPruebaGrupo(DataRow row)
        {
            CalendarizacionPruebaGrupo calendarizacionPruebaGrupo = new CalendarizacionPruebaGrupo();
            calendarizacionPruebaGrupo.GrupoCicloEscolar = new GrupoCicloEscolar();
            calendarizacionPruebaGrupo.PruebaContrato = new PruebaContrato();
            if (row.IsNull("CalendarizacionPruebaGrupoID"))
                calendarizacionPruebaGrupo.CalendarizacionPruebaGrupoID = null;
            else
                calendarizacionPruebaGrupo.CalendarizacionPruebaGrupoID = (int)Convert.ChangeType(row["CalendarizacionPruebaGrupoID"], typeof(int));
            if (row.IsNull("GrupoCicloEscolarID"))
                calendarizacionPruebaGrupo.GrupoCicloEscolar.GrupoCicloEscolarID = null;
            else
                calendarizacionPruebaGrupo.GrupoCicloEscolar.GrupoCicloEscolarID = (Guid)Convert.ChangeType(row["GrupoCicloEscolarID"], typeof(Guid));
            if (row.IsNull("PruebaContratoID"))
                calendarizacionPruebaGrupo.PruebaContrato.PruebaContratoID = null;
            else
                calendarizacionPruebaGrupo.PruebaContrato.PruebaContratoID = (long)Convert.ChangeType(row["PruebaContratoID"], typeof(long));
            if (row.IsNull("ConVigencia"))
                calendarizacionPruebaGrupo.ConVigencia = null;
            else
                calendarizacionPruebaGrupo.ConVigencia = (bool)Convert.ChangeType(row["ConVigencia"], typeof(bool));
            if (row.IsNull("FechaInicioVigencia"))
                calendarizacionPruebaGrupo.FechaInicioVigencia = null;
            else
                calendarizacionPruebaGrupo.FechaInicioVigencia = (DateTime)Convert.ChangeType(row["FechaInicioVigencia"], typeof(DateTime));
            if (row.IsNull("FechaFinVigencia"))
                calendarizacionPruebaGrupo.FechaFinVigencia = null;
            else
                calendarizacionPruebaGrupo.FechaFinVigencia = (DateTime)Convert.ChangeType(row["FechaFinVigencia"], typeof(DateTime));
            if (row.IsNull("FechaRegistro"))
                calendarizacionPruebaGrupo.FechaRegistro = null;
            else
                calendarizacionPruebaGrupo.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("Activo"))
                calendarizacionPruebaGrupo.Activo = null;
            else
                calendarizacionPruebaGrupo.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
            return calendarizacionPruebaGrupo;
        }
    }
}
