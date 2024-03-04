using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Framework.Base.DataAccess;

namespace POV.Web.PortalSocial.AppCode
{
    public class ConnectionHelper
    {
        private static ConnectionHelper singleton;
        private IDataContext dataContext;
        private  ConnectionHelper()
        {
            dataContext = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));
        }
        static ConnectionHelper()
        {
            singleton = new ConnectionHelper();
        }
        public static ConnectionHelper Default
        {
            get { return singleton; }
        }

        public IDataContext Connection {
            get { return new DataContext((new DataProviderFactory()).GetDataProvider("POV")); }
        }
    }
}