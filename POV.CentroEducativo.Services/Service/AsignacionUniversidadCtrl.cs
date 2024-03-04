using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace POV.CentroEducativo.Service
{
    /// <summary>
    /// Controlador del objeto Universidad
    /// </summary>
    public class AsignacionUniversidadCtrl
    {
        /// <summary>
        /// Consulta registros de Universidad en la base de datos.
        /// </summary>
        /// <param name="dctx"> El DataContext que proveerá acceso a la base de datos </param>
        /// <param name="universidad"> Universidad que provee el criterio de selección para realizar la consulta </param>
        /// <returns> El DataSet que contiene la información de Universidad generada por la consulta </returns>
        public DataSet RetrieveUniversidad(IDataContext dctx, Universidad universidad) 
        {
            UniversidadRetHlp da = new UniversidadRetHlp();
            DataSet ds = da.Action(dctx, universidad);
            return ds;
        }

        /// <summary>
        /// Crea un objeto de Universidad a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds"> El DataSet que contiene la información de Universidad </param>
        /// <returns> Un objeto de Universidad creado a partir de los datos </returns>
        public Universidad LastDataRowToUniversidad(DataSet ds)
        {
            if (!ds.Tables.Contains("Universidad"))
                throw new Exception("LastDataRowToUniversidad: DataSet no tiene la tabla Universidad");
            int index = ds.Tables["Universidad"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToUniversidad: El DataSet no tiene filas");
            return this.DataRowToUniversidad(ds.Tables["Universidad"].Rows[index - 1]);
        }

        /// <summary>
        /// Crea un objeto de Universidad a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de Universidad</param>
        /// <returns>Un objeto de Universidad creado a partir de los datos</returns>
        public Universidad DataRowToUniversidad(DataRow row)
        {
            Universidad universidad = new Universidad();

            if (row.IsNull("UniversidadID"))
                universidad.UniversidadID = null;
            else
                universidad.UniversidadID = (int)Convert.ChangeType(row["UniversidadID"], typeof(int));
            if (row.IsNull("NombreUniversidad"))
                universidad.NombreUniversidad = null;
            else
                universidad.NombreUniversidad = (string)Convert.ChangeType(row["NombreUniversidad"], typeof(string));
            if (row.IsNull("Descripcion"))
                universidad.Descripcion = null;
            else
                universidad.Descripcion = (string)Convert.ChangeType(row["Descripcion"], typeof(string));
            if (row.IsNull("Direccion"))
                universidad.Direccion = null;
            else
                universidad.Direccion = (string)Convert.ChangeType(row["Direccion"], typeof(string));
            if (row.IsNull("PaginaWEB"))
                universidad.PaginaWEB = null;
            else
                universidad.PaginaWEB = (string)Convert.ChangeType(row["PaginaWEB"], typeof(string));
            if (row.IsNull("CoordinadorCarrera"))
                universidad.CoordinadorCarrera = null;
            else
                universidad.CoordinadorCarrera = (string)Convert.ChangeType(row["CoordinadorCarrera"], typeof(string));
            if (row.IsNull("Activo"))
                universidad.Activo = null;
            else
                universidad.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
            if (row.IsNull("ClaveEscolar"))
                universidad.ClaveEscolar = null;
            else
                universidad.ClaveEscolar = (string)Convert.ChangeType(row["ClaveEscolar"], typeof(string));
            if (row.IsNull("Siglas"))
                universidad.Siglas = null;
            else
                universidad.Siglas = (string)Convert.ChangeType(row["Siglas"], typeof(string));
            if (row.IsNull("NivelEscolar"))
                universidad.NivelEscolar = null;
            else
                universidad.NivelEscolar = (ENivelEscolar)Convert.ChangeType(row["NivelEscolar"], typeof(byte));
            return universidad;
        }

        /// <summary>
        /// Consulta un registro UniversidadAlumno en la base de datos.
        /// </summary>
        /// <param name="dctx"> El DataContext que proveerá acceso a la base de datos </param>
        /// <param name="universidadAlumno"> UniversidadAlumno que proveerá el criterio de selección para realizar la consulta </param>
        /// <returns> El DataSet que contiene la informacion de UniversidadAlumno generada por la consulta </returns>
        public DataSet RetrieveUniversidadAlumno(IDataContext dctx, UniversidadAlumno universidadAlumno)
        {
            UniversidadAlumnoRetHlp da = new UniversidadAlumnoRetHlp();
            DataSet ds = da.Action(dctx, universidadAlumno);
            return ds;
        }

        /// <summary>
        /// Crea un objeto UniversidadAlumno a partir de los datos del último DataRow del DataSet
        /// </summary>
        /// <param name="ds"> El DataSet que contiene la informacion de UniversidadAlumno </param>
        /// <returns> Un objeto de UniversidadAlumno creado a partir de los datos </returns>
        public UniversidadAlumno LastDataRowToUniversidadAlumno(DataSet ds)
        {
            if (!ds.Tables.Contains("UniversidadAlumno"))
                throw new Exception("LastDataRowToUniversidadAlumno: DataSer no tiene la tabla UniversidadAlumno");

            int index = ds.Tables["UniversidadAlumno"].Rows.Count;
            if (index < 1)
            {
                UniversidadAlumno universidadAlumno = null;
                return universidadAlumno;
            }
            return this.DataRowToUniversidadAlumno(ds.Tables["UniversidadAlumno"].Rows[index - 1]);
        }

        /// <summary>
        /// Crea un objeto UniversidadAlumno a partir de los datos de un DataRow
        /// </summary>
        /// <param name="row"> El DataRow que contiene la informacion de UniversidadAlumno</param>
        /// <returns> Un objeto UniversidadAlumno creado a partir de lo datos</returns>
        public UniversidadAlumno DataRowToUniversidadAlumno(DataRow row)
        {
            UniversidadAlumno universidadAlumno = new UniversidadAlumno();

            if (row.IsNull("UniversidadID"))
                universidadAlumno.UniversidadID = null;
            else
                universidadAlumno.UniversidadID = (int)Convert.ChangeType(row["UniversidadID"], typeof(int));

            if (row.IsNull("AlumnoId"))
                universidadAlumno.AlumnoID = null;
            else
                universidadAlumno.AlumnoID = (int)Convert.ChangeType(row["AlumnoID"], typeof(int));

            return universidadAlumno;
        }

        /// <summary>
        /// Crea un registro de UniversidadAlumno en la base de datos
        /// </summary>
        /// <param name="dctx"> El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="universidadAlumno"> UniversidadAlumnoInsHlp que desea crear</param>
        public void InsertUniversidadAlumno(IDataContext dctx, UniversidadAlumno universidadAlumno)
        {
            UniversidadAlumnoInsHlp da = new UniversidadAlumnoInsHlp();
            da.Action(dctx, universidadAlumno);
        }
    }
}
