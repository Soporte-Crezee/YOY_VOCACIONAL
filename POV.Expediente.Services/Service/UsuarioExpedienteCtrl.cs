using Framework.Base.DataAccess;
using POV.Expediente.BO;
using POV.Expediente.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.Expediente.Services
{
    /// <summary>
    /// Controlador del objeto UsuarioExpediente
    /// </summary>
    public class UsuarioExpedienteCtrl
    {
        /// <summary>
        /// Consulta un registro UsuarioExpediente en la base de datos.
        /// </summary>
        /// <param name="dctx"> El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="usuarioExpedienteRetHlp"> UsuarioExpedienteRetHlp que proveerá el criterio de selección para realizar la consulta</param>
        /// <returns> El DataSet que contiene la informacion de UsuarioExpdienteRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, UsuarioExpediente usuarioExpediente)
        {
            UsuarioExpedienteRetHlp da = new UsuarioExpedienteRetHlp();
            DataSet ds = da.Action(dctx, usuarioExpediente);
            return ds;
        }

        /// <summary>
        /// Crea un registro de UsuarioExpediente en la base de datos
        /// </summary>
        /// <param name="dctx"> El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="usuarioExpedienteInsHlp"> UsuarioExpedienteInsHlp que desea crear</param>
        public void Insert(IDataContext dctx, UsuarioExpediente usuarioExpediente)
        {
            UsuarioExpedienteInsHlp da = new UsuarioExpedienteInsHlp();
            da.Action(dctx, usuarioExpediente);
        }

        /// <summary>
        /// Elimina un registro de UsuarioExpediente en la base de datos
        /// </summary>
        /// <param name="dctx"> El DataContext que porveerá acceso a la base de datos</param>
        /// <param name="usuarioExpediente"> UsuarioExpedienteRetHlp que desea eliminar</param>
        public void Delete(IDataContext dctx, UsuarioExpediente usuarioExpediente)
        {
            UsuarioExpedienteDelHlp da = new UsuarioExpedienteDelHlp();
            da.Action(dctx, usuarioExpediente);
        }

        /// <summary>
        /// Crea un objeto UsuarioExpediente a partir de los datos del último DataRow del DataSet
        /// </summary>
        /// <param name="ds"> El DataSet que contiene la informacion de UsuarioExpediente</param>
        /// <returns> Un objeto de UsuarioExpediente creado a partir de los datos</returns>
        public UsuarioExpediente LastDataRowToUsuarioExpediente(DataSet ds)
        {
            if (!ds.Tables.Contains("UsuarioExpediente"))
                throw new Exception("LastDataRowToUsuarioExpediente: DataSer no tiene la tabla UsuarioExpediente");

            int index = ds.Tables["UsuarioExpediente"].Rows.Count;
            if (index < 1)
            {
                UsuarioExpediente usuarioExpediente = null;
                return usuarioExpediente;
            }
            return this.DataRowToUsuarioExpediente(ds.Tables["UsuarioExpediente"].Rows[index - 1]);
        }

        /// <summary>
        /// Crea un objeto UsuarioExpediente a partir de los datos de un DataRow
        /// </summary>
        /// <param name="row"> El DataRow que contiene la informacion de UsuarioExpediente</param>
        /// <returns> Un objeto UsuarioExpediente creado a partir de lo datos</returns>
        public UsuarioExpediente DataRowToUsuarioExpediente(DataRow row)
        {
            UsuarioExpediente usuarioExpediente = new UsuarioExpediente();

            if (row.IsNull("UsuarioID"))
                usuarioExpediente.UsuarioID = null;
            else
                usuarioExpediente.UsuarioID = (int)Convert.ChangeType(row["UsuarioID"], typeof(int));

            if (row.IsNull("AlumnoId"))
                usuarioExpediente.AlumnoID = null;
            else
                usuarioExpediente.AlumnoID = (long)Convert.ChangeType(row["AlumnoID"], typeof(long));

            return usuarioExpediente;
        }
    }
}
