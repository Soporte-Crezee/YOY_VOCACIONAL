using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Framework.Base.DataAccess;

namespace POV.Web.Portal.Pruebas.Helper
{
    public class ConnectionHlp
    {
        private ConnectionHlp connectionHlp;
        private static ConnectionHlp singleton;
        private IDataContext dataContext;
        private  ConnectionHlp()
        {

        }
        static ConnectionHlp()
        {
            singleton = new ConnectionHlp();
        }
        public static ConnectionHlp Default
        {
            get 
            {
                return singleton; 
            }
        }

        public IDataContext Connection {
            get { return new DataContext((new DataProviderFactory()).GetDataProvider("POV")); }
        }
    }
}