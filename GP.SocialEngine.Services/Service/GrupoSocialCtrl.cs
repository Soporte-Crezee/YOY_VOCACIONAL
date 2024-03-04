using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.DAO;
using GP.SocialEngine.BO;
using POV.CentroEducativo.BO;

namespace GP.SocialEngine.Service
{
    /// <summary>
    /// GrupoSocialCtrl
    /// </summary>
    public class GrupoSocialCtrl
    {
        /// <summary>
        /// Consulta registros de GrupoSocialRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="grupoSocialRetHlp">GrupoSocialRetHlp que provee el criterio de selecciÃ³n para realizar la consulta</param>
        /// <returns>El DataSet que contiene la informaciÃ³n de GrupoSocialRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, GrupoSocial grupoSocial)
        {
            GrupoSocialRetHlp da = new GrupoSocialRetHlp();
            DataSet ds = da.Action(dctx, grupoSocial);
            return ds;
        }
        /// <summary>
        /// Devuelve un GrupoSocial Completo
        /// </summary>
        public GrupoSocial RetrieveComplete(IDataContext dctx, GrupoSocial grupoSocial, List<AreaConocimiento> areasConocimiento, long? docenteID, long universidadId, bool tienepublicacion = false)
        {
            UsuarioGrupoCtrl usuarioGrupoCtrl = new UsuarioGrupoCtrl();
            grupoSocial = LastDataRowToGrupoSocial(Retrieve(dctx, grupoSocial));
            UsuarioGrupo usrGrp = new UsuarioGrupo() { Estatus = true };
            if (docenteID != null) usrGrp.DocenteID = docenteID;
            DataSet dsUsuarioGrupo = usuarioGrupoCtrl.Retrieve(dctx, usrGrp, new GrupoSocial { GrupoSocialID = grupoSocial.GrupoSocialID }, areasConocimiento, universidadId, tienepublicacion);
            grupoSocial.ListaUsuarioGrupo = new List<UsuarioGrupo>();
            Int32 usuarioGrupoID;
            foreach (DataRow row in dsUsuarioGrupo.Tables["UsuarioGrupo"].Rows)
            {
                usuarioGrupoID = Convert.ToInt32(row["UsuarioGrupoID"].ToString());
                UsuarioGrupo usuarioGrupo = usuarioGrupoCtrl.DataRowToUsuarioGrupo(row);
                usuarioGrupo.FechaAsignacion = null;
                // Adecuacion Crezee   
                long? orientadorID = usuarioGrupo.DocenteID;
                usuarioGrupo = usuarioGrupoCtrl.RetrieveComplete(dctx, usuarioGrupo);
                usuarioGrupo.DocenteID = orientadorID;
                grupoSocial.ListaUsuarioGrupo.Add(usuarioGrupo);
            }
            return grupoSocial;
        }

        public GrupoSocial RetrieveComplete(IDataContext dctx, GrupoSocial grupoSocial, List<AreaConocimiento> areasConocimiento, long? docenteID, long universidadId, int pagesize, int currentpage, bool tienepublicacion = false)
        {
            UsuarioGrupoCtrl usuarioGrupoCtrl = new UsuarioGrupoCtrl();
            grupoSocial = LastDataRowToGrupoSocial(Retrieve(dctx, grupoSocial));
            UsuarioGrupo usrGrp = new UsuarioGrupo() { Estatus = true };
            if (docenteID != null) usrGrp.DocenteID = docenteID;
            DataSet dsUsuarioGrupo = usuarioGrupoCtrl.Retrieve(dctx, usrGrp, new GrupoSocial { GrupoSocialID = grupoSocial.GrupoSocialID }, areasConocimiento, universidadId, pagesize, currentpage, tienepublicacion);
            grupoSocial.ListaUsuarioGrupo = new List<UsuarioGrupo>();
            Int32 usuarioGrupoID;
            foreach (DataRow row in dsUsuarioGrupo.Tables["UsuarioGrupo"].Rows)
            {
                usuarioGrupoID = Convert.ToInt32(row["UsuarioGrupoID"].ToString());
                UsuarioGrupo usuarioGrupo = usuarioGrupoCtrl.DataRowToUsuarioGrupo(row);
                usuarioGrupo.FechaAsignacion = null;
                // Adecuacion Crezee                
                long? orientadorID = usuarioGrupo.DocenteID;
                usuarioGrupo = usuarioGrupoCtrl.RetrieveComplete(dctx, usuarioGrupo);
                usuarioGrupo.DocenteID = orientadorID;
                grupoSocial.ListaUsuarioGrupo.Add(usuarioGrupo);
            }
            return grupoSocial;
        }
        /// <summary>
        /// Consulta si un usuario social es amigo dentro del grupo social, devuelve nulo si no lo encuentra
        /// </summary>
        public UsuarioGrupo RetrieveFriend(IDataContext dctx, GrupoSocial grupoSocial, UsuarioSocial potentialFriend, List<AreaConocimiento> areasConocimiento, long? docenteId, long universidadId, bool esAlumno = true)
        {
            UsuarioGrupo friend = null;
            //consultamos el grupo social
            GrupoSocial gs = new GrupoSocial();
            if (esAlumno)
                gs = RetrieveComplete(dctx, new GrupoSocial { GrupoSocialID = grupoSocial.GrupoSocialID }, areasConocimiento, docenteId, universidadId);
            else 
            {
                UsuarioGrupoCtrl usuarioGrupoCtrl = new UsuarioGrupoCtrl();
                UsuarioSocialCtrl usuarioSocialCtrl = new UsuarioSocialCtrl();

                var dsUsuarioGrupo = usuarioGrupoCtrl.Retrieve(dctx, new UsuarioGrupo { UsuarioSocial = potentialFriend }, new GrupoSocial());
                gs.ListaUsuarioGrupo = new List<UsuarioGrupo>();
                if (dsUsuarioGrupo.Tables[0].Rows.Count > 0)
                {

                    var usuarioGrupo = usuarioGrupoCtrl.LastDataRowToUsuarioGrupo(dsUsuarioGrupo);

                    usuarioGrupo.UsuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, usuarioGrupo.UsuarioSocial));
                    gs.ListaUsuarioGrupo.Add(usuarioGrupo);
                }
            }

            friend = gs.ListaUsuarioGrupo.Find(item => item.UsuarioSocial.UsuarioSocialID == potentialFriend.UsuarioSocialID);

            return friend;
        }
        /// <summary>
        /// Devuelve un usuario grupo relacionado al usuario social
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="usuarioSocial"></param>
        /// <param name="potentialFriend"></param>
        /// <returns>Usuario grupo relacionado, null en caso que no se encuentre</returns>
        public UsuarioGrupo RetrieveFriend(IDataContext dctx, UsuarioSocial usuarioSocial, UsuarioSocial potentialFriend, List<AreaConocimiento> areasConocimiento, long universidadId)
        {
            UsuarioGrupo friend = null;

            UsuarioGrupoCtrl usuarioGrupoCtrl = new UsuarioGrupoCtrl();
            DataSet ds = usuarioGrupoCtrl.RetrieveFriends(dctx, usuarioSocial, potentialFriend);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //friend = usuarioGrupoCtrl.LastDataRowToUsuarioGrupo(ds);
                //if (IsPublicacion == true)
                friend = usuarioGrupoCtrl.RetrieveComplete(dctx, friend, areasConocimiento, universidadId);
                //else
                //    friend = usuarioGrupoCtrl.RetrieveComplete(dctx, friend, areasConocimiento);
            }

            return friend;
        }
        /// <summary>
        /// Asocia un usuario social al grupo social
        /// </summary>
        public void LinkUsuarioToGrupoSocial(IDataContext dctx, GrupoSocial grupoSocialRemitente, UsuarioSocial invitado)
        {
            UsuarioGrupoCtrl usuarioGrupoCtrl = new UsuarioGrupoCtrl();
            //creamos el usuario grupo
            UsuarioGrupo usuarioGrupoInvitado = new UsuarioGrupo();
            usuarioGrupoInvitado.Estatus = true;
            usuarioGrupoInvitado.FechaAsignacion = DateTime.Now;
            usuarioGrupoInvitado.UsuarioSocial = invitado;
            usuarioGrupoCtrl.Insert(dctx, usuarioGrupoInvitado, grupoSocialRemitente);
        }
        /// <summary>
        /// Consulta el listado de grupos sociales de un social hub
        /// </summary>
        public List<GrupoSocial> RetrieveGruposSocialSocialHub(IDataContext dctx, SocialHub socialHub)
        {
            List<GrupoSocial>
                grupos = new List<GrupoSocial>();
            SocialHubGrupoSocialRetHlp da = new SocialHubGrupoSocialRetHlp();
            DataSet dsGrupoSocial = da.Action(dctx, socialHub);
            if (dsGrupoSocial.Tables["GrupoSocial"].Rows.Count > 0)
            {
                foreach (DataRow dr in dsGrupoSocial.Tables["GrupoSocial"].Rows)
                {
                    GrupoSocial grupoSocial = LastDataRowToGrupoSocial(Retrieve(dctx, new GrupoSocial { GrupoSocialID = (int)Convert.ChangeType(dr["GrupoSocialID"], typeof(int)) }));
                    grupos.Add(grupoSocial);
                }
            }
            return grupos;
        }
        /// <summary>
        /// Crea un registro de GrupoSocialInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="grupoSocialInsHlp">GrupoSocialInsHlp que desea crear</param>
        public void Insert(IDataContext dctx, GrupoSocial grupoSocial)
        {
            GrupoSocialInsHlp da = new GrupoSocialInsHlp();
            da.Action(dctx, grupoSocial);
        }
        /// <summary>
        /// Actualiza de manera optimista un registro de GrupoSocialUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="grupoSocialUpdHlp">GrupoSocialUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">GrupoSocialUpdHlp que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, GrupoSocial grupoSocial, GrupoSocial previous)
        {
            GrupoSocialUpdHlp da = new GrupoSocialUpdHlp();
            da.Action(dctx, grupoSocial, previous);
        }
        /// <summary>
        /// Elimina un registro de GrupoSocialDelHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="grupoSocialDelHlp">GrupoSocialDelHlp que desea eliminar</param>
        public void Delete(IDataContext dctx, GrupoSocial grupoSocial)
        {
            GrupoSocialDelHlp da = new GrupoSocialDelHlp();
            da.Action(dctx, grupoSocial);
        }
        /// <summary>
        /// Elimina un registro completo de grupo social
        /// </summary>
        public void DeleteComplete(IDataContext dctx, GrupoSocial grupoSocial, List<AreaConocimiento> areasConocimiento, long universidadId)
        {
            grupoSocial = RetrieveComplete(dctx, grupoSocial, areasConocimiento, null, universidadId);
            UsuarioGrupoCtrl usuarioGrupoCtrl = new UsuarioGrupoCtrl();
            foreach (UsuarioGrupo usuarioGrupo in grupoSocial.ListaUsuarioGrupo)
            {
                usuarioGrupoCtrl.Delete(dctx, usuarioGrupo);
            }
            this.Delete(dctx, grupoSocial);
        }

        public void DeleteUsuarioGrupoGrupoSocial(IDataContext dctx, GrupoSocial grupoSocial, UsuarioGrupo usuarioGrupo, List<AreaConocimiento> areasConocimiento, long universidadID)
        {
            string sError = string.Empty;
            if (usuarioGrupo == null)
                sError += ", UsuarioSocial";
            if (grupoSocial == null)
                sError += ", GrupoSocial";

            if (sError.Length > 0)
                throw new Exception("DeleteUsuarioGrupoGrupoSocial: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (grupoSocial.GrupoSocialID == null)
                sError += " ,GrupoSocialID";
            if (sError.Length > 0)
                throw new Exception("DeleteUsuarioGrupoGrupoSocial: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));

            UsuarioGrupoCtrl usuarioGrupoCtrl = new UsuarioGrupoCtrl();
            DataSet ds = usuarioGrupoCtrl.Retrieve(dctx, usuarioGrupo, grupoSocial, areasConocimiento, universidadID);
            if (ds.Tables["UsuarioGrupo"].Rows.Count == 1)
            {
                usuarioGrupo = usuarioGrupoCtrl.LastDataRowToUsuarioGrupo(ds);
                UsuarioGrupo usuarioGrupoAnterior = (UsuarioGrupo)usuarioGrupo.Clone();
                usuarioGrupo.Estatus = false;
                usuarioGrupoCtrl.Update(dctx, usuarioGrupo, usuarioGrupoAnterior);
            }
            else
                throw new Exception("DeleteUsuarioGrupoGrupoSocial: UsuarioGrupo no encontrado");
        }

        /// <summary>
        /// Crea un objeto de GrupoSocial a partir de los datos del Ãºltimo DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la informaciÃ³n de GrupoSocial</param>
        /// <returns>Un objeto de GrupoSocial creado a partir de los datos</returns>
        public GrupoSocial LastDataRowToGrupoSocial(DataSet ds)
        {
            if (!ds.Tables.Contains("GrupoSocial"))
                throw new Exception("LastDataRowToGrupoSocial: DataSet no tiene la tabla GrupoSocial");
            int index = ds.Tables["GrupoSocial"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToGrupoSocial: El DataSet no tiene filas");
            return this.DataRowToGrupoSocial(ds.Tables["GrupoSocial"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de GrupoSocial a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la informaciÃ³n de GrupoSocial</param>
        /// <returns>Un objeto de GrupoSocial creado a partir de los datos</returns>
        public GrupoSocial DataRowToGrupoSocial(DataRow row)
        {
            GrupoSocial grupoSocial = new GrupoSocial();
            grupoSocial.ListaUsuarioGrupo = new List<UsuarioGrupo>();
            if (row.IsNull("GrupoSocialID"))
                grupoSocial.GrupoSocialID = null;
            else
                grupoSocial.GrupoSocialID = (int)Convert.ChangeType(row["GrupoSocialID"], typeof(int));
            if (row.IsNull("GrupoSocialGuid"))
                grupoSocial.GrupoSocialGuid = null;
            else
                grupoSocial.GrupoSocialGuid = (Guid)Convert.ChangeType(row["GrupoSocialGuid"], typeof(Guid));
            if (row.IsNull("Nombre"))
                grupoSocial.Nombre = null;
            else
                grupoSocial.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
            if (row.IsNull("Descripcion"))
                grupoSocial.Descripcion = null;
            else
                grupoSocial.Descripcion = (string)Convert.ChangeType(row["Descripcion"], typeof(string));
            if (row.IsNull("FechaCreacion"))
                grupoSocial.FechaCreacion = null;
            else
                grupoSocial.FechaCreacion = (DateTime)Convert.ChangeType(row["FechaCreacion"], typeof(DateTime));
            if (row.IsNull("NumeroMiembros"))
                grupoSocial.NumeroMiembros = null;
            else
                grupoSocial.NumeroMiembros = (int)Convert.ChangeType(row["NumeroMiembros"], typeof(int));
            if (row.IsNull("TipoGrupoSocial"))
                grupoSocial.ToShortTipoGrupoSocial = null;
            else
                grupoSocial.ToShortTipoGrupoSocial = (short)Convert.ChangeType(row["TipoGrupoSocial"], typeof(short));
            return grupoSocial;
        }
    }
}
