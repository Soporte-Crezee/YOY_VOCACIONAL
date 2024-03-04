using Framework.Base.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.OXXOPay.Services.AppCode
{
    public class ConnectionHelper
    {
        private static ConnectionHelper singleton;
        private IDataContext dataContext;
        private ConnectionHelper()
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

        public IDataContext Connection
        {
            get { return new DataContext((new DataProviderFactory()).GetDataProvider("POV")); }
        }
    }
}
