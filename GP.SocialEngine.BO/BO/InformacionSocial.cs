using System;
using System.Text;

namespace GP.SocialEngine.BO
{
    /// <summary>
    /// Informacion Social
    /// </summary>
    public class InformacionSocial
    {
        private long? informacionSocialID;
        /// <summary>
        /// Identificador autonumÃ©rico de la InformaciÃ³n Social
        /// </summary>
        public long? InformacionSocialID
        {
            get { return this.informacionSocialID; }
            set { this.informacionSocialID = value; }
        }
        private string iMMSN;
        /// <summary>
        /// IMMSN
        /// </summary>
        public string IMMSN
        {
            get { return this.iMMSN; }
            set { this.iMMSN = value; }
        }
        private string iMSkype;
        /// <summary>
        /// IMSKYPE
        /// </summary>
        public string IMSkype
        {
            get { return this.iMSkype; }
            set { this.iMSkype = value; }
        }
        private string iMAOL;
        /// <summary>
        /// IMAOL
        /// </summary>
        public string IMAOL
        {
            get { return this.iMAOL; }
            set { this.iMAOL = value; }
        }
        private string iMICQ;
        /// <summary>
        /// IMICQ
        /// </summary>
        public string IMICQ
        {
            get { return this.iMICQ; }
            set { this.iMICQ = value; }
        }
        private string iMYahoo;
        /// <summary>
        /// IMYahoo
        /// </summary>
        public string IMYahoo
        {
            get { return this.iMYahoo; }
            set { this.iMYahoo = value; }
        }
        private string firma;
        /// <summary>
        /// Firma
        /// </summary>
        public string Firma
        {
            get { return this.firma; }
            set { this.firma = value; }
        }
        private DateTime? fechaRegistro;
        /// <summary>
        /// FechaRegistro
        /// </summary>
        public DateTime? FechaRegistro
        {
            get { return this.fechaRegistro; }
            set { this.fechaRegistro = value; }
        }
    }
}
