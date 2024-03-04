using POV.CentroEducativo.BO;
using POV.Seguridad.BO;
namespace POV.Licencias.BO
{
    public class LicenciaEspecialistaPruebas : ALicencia
    {

        private EspecialistaPruebas especialistaPruebas;

        public EspecialistaPruebas EspecialistaPruebas
        {
            get { return especialistaPruebas;  }
            set { especialistaPruebas = value; }
        }
        
        public override bool Descontar
        {
            get { return false; }
        }
        
        public override ETipoLicencia Tipo
        {
            get { return ETipoLicencia.ESPECIALISTA; }
        }

        public override object Clone()
        {
            LicenciaEspecialistaPruebas licencia = (LicenciaEspecialistaPruebas) MemberwiseClone();
            licencia.Usuario = (Usuario) Usuario.Clone();
            return licencia;
        }
    }
}
