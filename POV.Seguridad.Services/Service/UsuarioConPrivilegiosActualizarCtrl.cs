using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Seguridad.BO;
using POV.Seguridad.DAO;

namespace POV.Seguridad.Service
{
    public class UsuarioConPrivilegiosActualizarCtrl
    {
        public void Action(IDataContext dctx, UsuarioPrivilegios usuarioPrivilegios, UsuarioPrivilegios anterior)
        {
            object firma;

            dctx.OpenConnection(firma = new object());
            dctx.BeginTransaction(firma);

            UsuarioCtrl us = new UsuarioCtrl();
            UsuarioPrivilegiosCtrl ups = new UsuarioPrivilegiosCtrl();

            try
            {
                ups.UpdateComplete(dctx, usuarioPrivilegios, anterior);
                us.Update(dctx, usuarioPrivilegios.Usuario, anterior.Usuario);
            }
            catch (Exception e)
            {
                dctx.RollbackTransaction(firma);
                dctx.CloseConnection(firma);
                throw e;
            }
            dctx.CommitTransaction(firma);
            dctx.CloseConnection(firma);
        }
    }
}
