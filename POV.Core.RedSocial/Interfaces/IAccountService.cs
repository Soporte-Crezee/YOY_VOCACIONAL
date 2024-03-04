using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;

namespace POV.Core.RedSocial.Interfaces
{
    public interface IAccountService
    {
        string Login(IDataContext dctx, string username, string password, byte[] binaryPassword = null, string tipoUsuario = "asp");
         void Logout();

    }
}
