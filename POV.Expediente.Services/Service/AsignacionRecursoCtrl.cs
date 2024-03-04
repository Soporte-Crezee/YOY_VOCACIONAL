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
    public class AsignacionRecursoCtrl : AAsignacionRecursoCtrl
    {
        public override void InsertComplete(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar, AAsignacionRecurso asignacionRecurso)
        {
            throw new NotImplementedException();
        }

        public override AAsignacionRecurso RetrieveComplete(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar, AAsignacionRecurso asignacionRecurso)
        {
            throw new NotImplementedException();
        }

        public override void UpdateComplete(IDataContext dctx, AAsignacionRecurso asignacionRecurso, AAsignacionRecurso previous)
        {
            throw new NotImplementedException();
        }

        public override void DeleteComplete(IDataContext dctx, AAsignacionRecurso asignacionRecurso)
        {
            throw new NotImplementedException();
        }

        public override AAsignacionRecurso LastDataRowToAsignacionRecurso(DataSet ds)
        {
            throw new NotImplementedException();
        }

        public override AAsignacionRecurso DataRowToAsignacionRecurso(DataRow dr)
        {
            throw new NotImplementedException();
        }
    }
}
