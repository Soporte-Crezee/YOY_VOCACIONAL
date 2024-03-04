using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace POV.Social.Service
{
    public class ConfirmarAlumnoCtrl
    {
        public void ConfirmarAlumno(Alumno alumnoNuevo, Alumno alumnoPrevio, Usuario usuarioNuevo, Usuario usuarioPrevio, IDataContext dctx)
        {
            UsuarioCtrl usuarioCtrl = new UsuarioCtrl();
            AlumnoCtrl alumnoCtrl = new AlumnoCtrl();
            object myFirm = new object();
            try
            {
                dctx.OpenConnection(myFirm);
                dctx.BeginTransaction(myFirm);
                usuarioCtrl.Update(dctx, usuarioNuevo, usuarioPrevio);
                DataSet dsalumno = alumnoCtrl.Retrieve(dctx, new Alumno { AlumnoID = alumnoPrevio.AlumnoID });
                alumnoPrevio = alumnoCtrl.LastDataRowToAlumno(dsalumno);
                alumnoCtrl.Update(dctx, alumnoNuevo, alumnoPrevio);
                dctx.CommitTransaction(myFirm);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                throw (ex);
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }

        }
    }
}
