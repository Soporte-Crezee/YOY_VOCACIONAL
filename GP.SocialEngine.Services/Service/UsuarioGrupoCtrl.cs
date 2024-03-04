using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;
using GP.SocialEngine.DAO;
using POV.CentroEducativo.BO;

namespace GP.SocialEngine.Service
{

    /// <summary>
    /// UsuarioGrupoCtrl
    /// </summary>
    public class UsuarioGrupoCtrl
    {

        /// <summary>
        /// Consulta registros de UsuarioGrupoRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="usuarioGrupoRetHlp">UsuarioGrupoRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de UsuarioGrupoRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, UsuarioGrupo usuarioGrupo, GrupoSocial grupoSocial, List<AreaConocimiento> areasConocimiento, long universidadId, bool tienepublicacion = false)
        {

            //UsuarioGrupoRetHlp da = new UsuarioGrupoRetHlp();
            UsuarioByAreaConocimientoRetHlp da = new UsuarioByAreaConocimientoRetHlp();

            string strAreasConocimiento = String.Empty;
            if (areasConocimiento != null && areasConocimiento.Count > 0)
            {
                foreach (AreaConocimiento areas in areasConocimiento)
                {
                    strAreasConocimiento += "," + areas.AreaConocimentoID;
                }
                if (strAreasConocimiento.StartsWith(","))
                    strAreasConocimiento = strAreasConocimiento.Substring(1);
            }
            //usuarioGrupo.DocenteID = 10;
            DataSet ds = new DataSet();
            ds = da.Action(dctx, usuarioGrupo, grupoSocial, strAreasConocimiento, universidadId, tienepublicacion);
            return ds;

        }

        public DataSet Retrieve(IDataContext dctx, UsuarioGrupo usuarioGrupo, GrupoSocial grupoSocial, List<AreaConocimiento> areasConocimiento, long universidadId, int pagesiez, int currentpage, bool tienepublicacion = false)
        {

            //UsuarioGrupoRetHlp da = new UsuarioGrupoRetHlp();
            UsuarioByAreaConocimientoRetHlp da = new UsuarioByAreaConocimientoRetHlp();

            string strAreasConocimiento = String.Empty;
            if (areasConocimiento != null && areasConocimiento.Count > 0)
            {
                foreach (AreaConocimiento areas in areasConocimiento)
                {
                    strAreasConocimiento += "," + areas.AreaConocimentoID;
                }
                if (strAreasConocimiento.StartsWith(","))
                    strAreasConocimiento = strAreasConocimiento.Substring(1);
            }
            //usuarioGrupo.DocenteID = 10;
            DataSet ds = new DataSet();
            ds = da.Action(dctx, usuarioGrupo, grupoSocial, strAreasConocimiento, universidadId, pagesiez, currentpage, tienepublicacion);
            return ds;

        }

        #region Adecuacion Crezee
        /// <summary>
        /// Consulta registros de UsuarioGrupoRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="usuarioGrupoRetHlp">UsuarioGrupoRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de UsuarioGrupoRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, UsuarioGrupo usuarioGrupo, GrupoSocial grupoSocial)
        {

            UsuarioGrupoRetHlp da = new UsuarioGrupoRetHlp();

            DataSet ds = da.Action(dctx, usuarioGrupo, grupoSocial);

            return ds;

        }

        /// <summary>
        /// Devuelve un UsuarioGrupo Completo
        /// </summary>
        public UsuarioGrupo RetrieveComplete(IDataContext dctx, UsuarioGrupo usuarioGrupo)
        {
            UsuarioSocialCtrl usuarioSocialCtrl = new UsuarioSocialCtrl();

            usuarioGrupo = LastDataRowToUsuarioGrupo(Retrieve(dctx, usuarioGrupo, new GrupoSocial()));
            usuarioGrupo.UsuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, usuarioGrupo.UsuarioSocial));
            return usuarioGrupo;

        }
        #endregion

        /// <summary>
        /// Devuelve todos los usuarios grupo activos relacionados a un usuario social en todos los grupos sociales
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="usuarioSocial"></param>
        /// <param name="potentialFriend"></param>
        /// <returns>DataSet contiene la informacion de UsuarioGrupoUsuarioSocialDARetHlp generada por la consulta</returns>
        public DataSet RetrieveFriends(IDataContext dctx, UsuarioSocial usuarioSocial, UsuarioSocial potentialFriend)
        {

            UsuarioGrupoUsuarioSocialDARetHlp da = new UsuarioGrupoUsuarioSocialDARetHlp();

            DataSet ds = da.Action(dctx, usuarioSocial, potentialFriend);

            return ds;

        }
        /// <summary>
        /// Devuelve un UsuarioGrupo Completo
        /// </summary>
        public UsuarioGrupo RetrieveComplete(IDataContext dctx, UsuarioGrupo usuarioGrupo, List<AreaConocimiento> areasConocimiento, long universidadId)
        {
            UsuarioSocialCtrl usuarioSocialCtrl = new UsuarioSocialCtrl();

            var dsUsuarioGrupo = Retrieve(dctx, usuarioGrupo, new GrupoSocial(), areasConocimiento, universidadId);

            if (dsUsuarioGrupo.Tables[0].Rows.Count > 0)
            {

                usuarioGrupo = LastDataRowToUsuarioGrupo(dsUsuarioGrupo);

                usuarioGrupo.UsuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, usuarioGrupo.UsuarioSocial));
                return usuarioGrupo;
            }
            return null;

        }

        /// <summary>
        /// Crea un registro de UsuarioGrupoInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="usuarioGrupoInsHlp">UsuarioGrupoInsHlp que desea crear</param>
        public void Insert(IDataContext dctx, UsuarioGrupo usuarioGrupo, GrupoSocial grupoSocial)
        {
            UsuarioGrupoInsHlp da = new UsuarioGrupoInsHlp();
            da.Action(dctx, usuarioGrupo, grupoSocial);
        }
        /// <summary>
        /// Actualiza de manera optimista un registro de UsuarioGrupoUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="usuarioGrupoUpdHlp">UsuarioGrupoUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">UsuarioGrupoUpdHlp que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, UsuarioGrupo usuarioGrupo, UsuarioGrupo previous)
        {
            UsuarioGrupoUpdHlp da = new UsuarioGrupoUpdHlp();
            da.Action(dctx, usuarioGrupo, previous);
        }
        /// <summary>
        /// Elimina un registro de UsuarioGrupoDelHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="usuarioGrupoDelHlp">UsuarioGrupoDelHlp que desea eliminar</param>
        public void Delete(IDataContext dctx, UsuarioGrupo usuarioGrupo)
        {
            UsuarioGrupoDelHlp da = new UsuarioGrupoDelHlp();
            da.Action(dctx, usuarioGrupo);
        }

        /// <summary>
        /// Crea un objeto de UsuarioGrupo a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de UsuarioGrupo</param>
        /// <returns>Un objeto de UsuarioGrupo creado a partir de los datos</returns>
        public UsuarioGrupo LastDataRowToUsuarioGrupo(DataSet ds)
        {
            if (!ds.Tables.Contains("UsuarioGrupo"))
                throw new Exception("LastDataRowToUsuarioGrupo: DataSet no tiene la tabla UsuarioGrupo");
            int index = ds.Tables["UsuarioGrupo"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToUsuarioGrupo: El DataSet no tiene filas");
            return this.DataRowToUsuarioGrupo(ds.Tables["UsuarioGrupo"].Rows[index - 1]);
        }

        /// <summary>
        /// Crea un objeto de UsuarioGrupo a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de UsuarioGrupo</param>
        /// <returns>Un objeto de UsuarioGrupo creado a partir de los datos</returns>
        public UsuarioGrupo DataRowToUsuarioGrupo(DataRow row)
        {
            UsuarioGrupo usuarioGrupo = new UsuarioGrupo();
            usuarioGrupo.UsuarioSocial = new UsuarioSocial();
            if (row.IsNull("UsuarioGrupoID"))
                usuarioGrupo.UsuarioGrupoID = null;
            else
                usuarioGrupo.UsuarioGrupoID = (int)Convert.ChangeType(row["UsuarioGrupoID"], typeof(int));
            if (row.IsNull("UsuarioSocialID"))
                usuarioGrupo.UsuarioSocial.UsuarioSocialID = null;
            else
                usuarioGrupo.UsuarioSocial.UsuarioSocialID = (int)Convert.ChangeType(row["UsuarioSocialID"], typeof(int));
            if (row.IsNull("FechaAsignacion"))
                usuarioGrupo.FechaAsignacion = null;
            else
                usuarioGrupo.FechaAsignacion = (DateTime)Convert.ChangeType(row["FechaAsignacion"], typeof(DateTime));
            if (row.IsNull("Estatus"))
                usuarioGrupo.Estatus = null;
            else
                usuarioGrupo.Estatus = (bool)Convert.ChangeType(row["Estatus"], typeof(bool));
            if (row.IsNull("EsModerador"))
                usuarioGrupo.EsModerador = null;
            else
                usuarioGrupo.EsModerador = (bool)Convert.ChangeType(row["EsModerador"], typeof(bool));
            if (row.IsNull("DocenteID"))
                usuarioGrupo.DocenteID = null;
            else
                usuarioGrupo.DocenteID = (long)Convert.ChangeType(row["DocenteID"], typeof(long));
            return usuarioGrupo;
        }

    }
}
