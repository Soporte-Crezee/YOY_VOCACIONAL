using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;

namespace POV.Core.PadreTutor.Interfaces
{
    public interface IAccountService
    {
        string Login(IDataContext dctx, string nombreUsuario, string password, byte[] binaryPassword = null);
        void Logout();
    }
}
