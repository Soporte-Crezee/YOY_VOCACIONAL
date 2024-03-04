using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace POV.Web.Administracion.Logic
{
    public class PaypalSettings
    {
        #region--PayPal Settings

        public static bool IsSandbox
        {
            get { return bool.Parse((ConfigurationManager.AppSettings["IsSandbox"] != null ? ConfigurationManager.AppSettings["IsSandbox"].ToString() : "false")); }
        }

        public static string PaypalEndPointURL
        {
            get { return (ConfigurationManager.AppSettings["PaypalEndPointURL"] != null ? ConfigurationManager.AppSettings["PaypalEndPointURL"].ToString() : ""); }
        }

        public static string Paypalhost
        {
            get { return (ConfigurationManager.AppSettings["Paypalhost"] != null ? ConfigurationManager.AppSettings["Paypalhost"].ToString() : ""); }
        }

        public static string PaypalEndPointURL_SB
        {
            get { return (ConfigurationManager.AppSettings["PaypalEndPointURL_SB"] != null ? ConfigurationManager.AppSettings["PaypalEndPointURL_SB"].ToString() : ""); }
        }

        public static string Paypalhost_SB
        {
            get { return (ConfigurationManager.AppSettings["Paypalhost_SB"] != null ? ConfigurationManager.AppSettings["Paypalhost_SB"].ToString() : ""); }
        }

        public static string PayPalAPIUsername
        {
            get { return (ConfigurationManager.AppSettings["PayPalAPIUsername"] != null ? ConfigurationManager.AppSettings["PayPalAPIUsername"].ToString() : ""); }
        }

        public static string PayPalAPIPassword
        {
            get { return (ConfigurationManager.AppSettings["PayPalAPIPassword"] != null ? ConfigurationManager.AppSettings["PayPalAPIPassword"].ToString() : ""); }
        }

        public static string PayPalAPISignature
        {
            get { return (ConfigurationManager.AppSettings["PayPalAPISignature"] != null ? ConfigurationManager.AppSettings["PayPalAPISignature"].ToString() : ""); }
        }

        public static string PayPalAPISubject
        {
            get { return (ConfigurationManager.AppSettings["PayPalAPISubject"] != null ? ConfigurationManager.AppSettings["PayPalAPISubject"].ToString() : ""); }
        }

        public static string PayPalBNCode
        {
            get { return (ConfigurationManager.AppSettings["PayPalBNCode"] != null ? ConfigurationManager.AppSettings["PayPalBNCode"].ToString() : ""); }
        }

        public static int PayPalTimeout
        {
            get { return int.Parse((ConfigurationManager.AppSettings["PayPalTimeout"] != null ? ConfigurationManager.AppSettings["PayPalTimeout"].ToString() : "-1")); }
        }

        public static string PayPalreturnURL
        {
            get { return (ConfigurationManager.AppSettings["PayPalreturnURL"] != null ? ConfigurationManager.AppSettings["PayPalreturnURL"].ToString() : ""); }
        }

        public static string PayPalcancelURL
        {
            get { return (ConfigurationManager.AppSettings["PayPalcancelURL"] != null ? ConfigurationManager.AppSettings["PayPalcancelURL"].ToString() : ""); }
        }

        public static string PayPalPAYMENTACTION
        {
            get { return (ConfigurationManager.AppSettings["PayPalPAYMENTACTION"] != null ? ConfigurationManager.AppSettings["PayPalPAYMENTACTION"].ToString() : "Sale"); }
        }

        public static string PayPalCURRENCYCODE
        {
            get { return (ConfigurationManager.AppSettings["PayPalCURRENCYCODE"] != null ? ConfigurationManager.AppSettings["PayPalCURRENCYCODE"].ToString() : "MXN"); }
        }

        public static string PayPalBRANDNAME
        {
            get { return (ConfigurationManager.AppSettings["PayPalBRANDNAME"] != null ? ConfigurationManager.AppSettings["PayPalBRANDNAME"].ToString() : ""); }
        }

        public static string PayPalPaymentRequestName()
        {
            return HttpContext.Current.Session["PayPalPaymentRequestName"].ToString();
        }        
        
        #endregion
    }
}