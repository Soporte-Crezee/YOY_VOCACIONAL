using System;
using System.Collections.Generic;
using POV.CentroEducativo.BO;
using POV.Seguridad.BO;
using System.Linq;

namespace POV.Licencias.BO
{
    /// <summary>
    /// Licencias de Escuela
    /// </summary>
    public class LicenciaEscuela : ICloneable
    {
        private long? licenciaEscuelaID;
        /// <summary>
        /// Identificador autonumerico de la licencia de escuela.
        /// </summary>
        public long? LicenciaEscuelaID
        {
            get { return this.licenciaEscuelaID; }
            set { this.licenciaEscuelaID = value; }
        }
        private short? numeroLicencias;
        /// <summary>
        /// Número de licencias de la escuela.
        /// </summary>
        public short? NumeroLicencias
        {
            get { return this.numeroLicencias; }
            set { this.numeroLicencias = value; }
        }
        private Contrato contrato;
        /// <summary>
        /// Contrato de la licencia escuela.
        /// </summary>
        public Contrato Contrato
        {
            get { return contrato; }
            set { contrato = value; }
        }
        private Escuela escuela;
        /// <summary>
        /// Escuela para la licencia.
        /// </summary>
        public Escuela Escuela
        {
            get { return this.escuela; }
            set { this.escuela = value; }
        }
        private CicloEscolar cicloEscolar;
        /// <summary>
        /// Ubicacion del alumno
        /// </summary>
        public CicloEscolar CicloEscolar
        {
            get { return this.cicloEscolar; }
            set { this.cicloEscolar = value; }
        }
        private bool? activo;
        /// <summary>
        /// Estado de la licencia de escuela. Valor true para activa, caso contrario false.
        /// </summary>
        public bool? Activo
        {
            get { return this.activo; }
            set { this.activo = value; }
        }

        /// <summary>
        /// Indica si el licenciamiento es temporal. Valor TEMPORAL para temporal, caso contrario false.
        /// </summary>
        public virtual ELicenciaEscuela Tipo
        {
            get { return ELicenciaEscuela.NOTEMPORAL; }
        }

        /// <summary>
        /// Implementación de IClonable
        /// </summary>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        /// <summary>
        /// Retorna el número total de licencias asignadas por la escuela.
        /// </summary>
        public short LicenciasAsignadas()
        {
            return (short)listaLicencia.Count(l => l.Descontar == true && l.Activo == true);
        }
        /// <summary>
        /// Retorna el número de total de licencias disponibles para asignación.
        /// </summary>
        public short LicenciasPorAsignar()
        {
            return (short)(this.numeroLicencias.Value - listaLicencia.Count(l => l.Descontar == true && l.Activo == true));
        }

        private readonly List<ALicencia> listaLicencia = new List<ALicencia>();
        /// <summary>
        /// Lista de licencias
        /// </summary>
        public List<ALicencia> ListaLicencia
        {
            get { return this.listaLicencia; }
        }

        /// <summary>
        /// Agrega un licencia a la Lista
        /// </summary>
        public void AddLicencia(ALicencia licencia)
        {
            if (licencia == null)
                throw new ArgumentNullException("licencia","Argumento licencia no debe ser nulo.");

            listaLicencia.Add(licencia);
        }
        /// <summary>
        /// Remueve un licencia de la Lista
        /// </summary>
        public void RemoveLicencia(ALicencia licencia)
        {
            if (licencia != null)
                listaLicencia.RemoveAll(l => l.LicenciaID == licencia.LicenciaID);
        }

        private List<ModuloFuncional> modulosFuncionales;
        /// <summary>
        /// Representa
        /// </summary>
        public List<ModuloFuncional> ModulosFuncionales
        {
            get { return modulosFuncionales; }
            set { modulosFuncionales = value; }
        }
        
    }
}
