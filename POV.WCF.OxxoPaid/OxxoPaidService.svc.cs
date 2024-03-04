using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace POV.WCF.OxxoPaid
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class OxxoPaidService : IOxxoPaidService
    {
        public string UpdateAspirantes(string id)
        {
            string result = "";
            try
            {
                OXXOPay.Service.OxxoPayCtrl oxxoPay = new OXXOPay.Service.OxxoPayCtrl();
                result = oxxoPay.ActivateUser(id);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        public string RetrieveAspirante(string id)
        {
            string result = "";
            try
            {
                OXXOPay.Service.OxxoPayCtrl oxxoPay = new OXXOPay.Service.OxxoPayCtrl();
                result = oxxoPay.RetrieveUser(id);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
    }
}
