using POV.ContenidosDigital.BO;
using POV.Core.RedSocial.Implement;
using POV.Profesionalizacion.BO;

namespace POV.ConfiguracionActividades.BO
{
    public class TareaEjeTematico : Tarea
    {
        private EjeTematico ejeTematico;
        private ContenidoDigital contenidoDigital;

        virtual public EjeTematico EjeTematico
        {
            get { return ejeTematico; }
            set { ejeTematico = value; }
        }
        public long? EjeTematicoId { get; set; }

        virtual public ContenidoDigital ContenidoDigital
        {
            get { return contenidoDigital; }
            set { contenidoDigital = value; }
        }

        public long? ContenidoDigitalId { get; set; }


        public override string GetIdentificador()
        {
            return ContenidoDigital != null && ContenidoDigital.ContenidoDigitalID != null ? ContenidoDigital.ContenidoDigitalID.ToString() : null;
        }

        public string GetIdentificadorEjeTematico()
        {
            return EjeTematico != null && EjeTematico.EjeTematicoID != null ? EjeTematico.EjeTematicoID.ToString() : null;
        }
        public override string GetUrl()
        {
            return UrlHelper.GetTareaEjeTematicoURL(GetIdentificadorEjeTematico(), GetIdentificador());
        }

        public override string GetTypeDescription()
        {
            return "Recurso didáctico";
        }
    }
}
