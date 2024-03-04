using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Expediente.BO;
using POV.Expediente.DAO;

namespace POV.Expediente.Service
{
    /// <summary>
    /// Controlador abstracto de AsignacionRecurso que define operaciones basicas de mantenimiento
    /// </summary>
    public abstract class AAsignacionRecursoCtrl 
    {
        /// <summary>
        /// Inserta un registro de AsignacionRecurso en la BD
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="detalleCicloEscolar"></param>
        /// <param name="asignacionRecurso"></param>
        public void InsertAAsignacionRecurso(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar, AAsignacionRecurso asignacionRecurso)
        {
            AsignacionRecursoInsHlp da = new AsignacionRecursoInsHlp();
            da.Action(dctx, detalleCicloEscolar, asignacionRecurso);
        }
        /// <summary>
        /// Consulta un registro de AsignacionRecurso en la BD
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="detalleCicloEscolar">Detalle Ciclo Escolar que</param>
        /// <param name="asignacionRecurso"></param>
        /// <returns></returns>
        public DataSet RetrieveAAsignacionRecurso(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar, AAsignacionRecurso asignacionRecurso)
        {
            AsignacionRecursoRetHlp da = new AsignacionRecursoRetHlp();
            return da.Action(dctx, detalleCicloEscolar, asignacionRecurso);
        }
        /// <summary>
        /// Consulta todos los registros de asignacion de un detalle ciclo escolar
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="detalleCicloEscolar"></param>
        /// <returns></returns>
        public DataSet RetrieveAAsignacionRecurso(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar)
        {
            AsignacionRecursoRetHlp da = new AsignacionRecursoRetHlp();
            return da.Action(dctx, detalleCicloEscolar);
        }
        /// <summary>
        /// Actualiza de manera optimista un registro de asignacion recurso en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="asignacionRecurso"></param>
        /// <param name="previous"></param>
        public void UpdateAAsignacionRecurso(IDataContext dctx, AAsignacionRecurso asignacionRecurso, AAsignacionRecurso previous)
        {
            AsignacionRecursoUpdHlp da = new AsignacionRecursoUpdHlp();
            da.Action(dctx, asignacionRecurso, previous);
        }
        public abstract void InsertComplete(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar, AAsignacionRecurso asignacionRecurso);

        public abstract AAsignacionRecurso RetrieveComplete(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar, AAsignacionRecurso asignacionRecurso);

        public abstract void UpdateComplete(IDataContext dctx, AAsignacionRecurso asignacionRecurso, AAsignacionRecurso previous);

        public abstract void DeleteComplete(IDataContext dctx, AAsignacionRecurso asignacionRecurso);

        public abstract AAsignacionRecurso LastDataRowToAsignacionRecurso(DataSet ds);

        public abstract AAsignacionRecurso DataRowToAsignacionRecurso(DataRow dr);
    }
}
