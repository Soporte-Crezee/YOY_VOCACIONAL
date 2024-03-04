using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Localizacion.BO;
using POV.Seguridad.BO;
using POV.Comun.BO;

namespace POV.Licencias.BO
{
    public class Contrato : ICloneable
    {
        private long? contratoID;
        private string clave;
        private DateTime? fechaContrato;
        private DateTime? inicioContrato;
        private DateTime? finContrato;
        private bool? licenciasLimitadas;
        private int? numeroLicencias;
        private Cliente cliente;
        private Ubicacion ubicacion;
        private Usuario usuarioRegistro;
        private DateTime? fechaRegistro;
        private ProfesionalizacionContrato profesionalizacionContrato;


        public Contrato() 
        { 
            this.ciclosContrato = new ObjetoListado<CicloContrato>(); 
        }
        public Contrato(IEnumerable<CicloContrato> ciclosContrato) 
        {
            this.ciclosContrato = new ObjetoListado<CicloContrato>(ciclosContrato);
        }
        
        public long? ContratoID
        {
            get { return contratoID; }
            set { contratoID = value; }
        }

        public string Clave
        {
            get { return clave; }
            set { clave = value; }
        }

        public DateTime? FechaContrato
        {
            get { return fechaContrato; }
            set { fechaContrato = value; }
        }

        public DateTime? InicioContrato
        {
            get { return inicioContrato; }
            set { inicioContrato = value; }
        }

        public DateTime? FinContrato
        {
            get { return finContrato; }
            set { finContrato = value; }
        }

        public bool? LicenciasLimitadas
        {
            get { return licenciasLimitadas; }
            set { licenciasLimitadas = value; }
        }

        private bool? estatus;

        public bool? Estatus
        {
            get { return estatus; }
            set { estatus = value; }
        }

        public int? NumeroLicencias
        {
            get { return numeroLicencias; }
            set { numeroLicencias = value; }
        }

        public Cliente Cliente
        {
            get { return cliente; }
            set { cliente = value; }
        }

        public Ubicacion Ubicacion
        {
            get { return ubicacion; }
            set { ubicacion = value; }
        }

        public Usuario UsuarioRegistro
        {
            get { return usuarioRegistro; }
            set { usuarioRegistro = value; }
        }

        /// <summary>
        /// objeto ProfesionalizacionContrato
        /// </summary>
        public ProfesionalizacionContrato ProfesionalizacionContrato
        {
            get { return profesionalizacionContrato; }
            set { profesionalizacionContrato = value; }
        }

        public DateTime? FechaRegistro
        {
            get { return fechaRegistro; }
            set { fechaRegistro = value; }
        }

        private ObjetoListado<CicloContrato> ciclosContrato;
        /// <summary>
        /// lista de PruebaContrato
        /// </summary>
        public IEnumerable<CicloContrato> CiclosContrato
        {
            get { return this.ciclosContrato.Objetos; }
        }

        #region *** metodos de lista de pruebaContrato ***
        /// <summary>
        /// Agrega CicloContrato
        /// </summary>
        public CicloContrato CicloContratoAgregar(CicloContrato cicloContrato)
        {
            return this.ciclosContrato.Agregar(CicloContratoIdentificar(cicloContrato), cicloContrato);
        }
        /// <summary>
        /// Obtener CicloContrato basado en el indice
        /// </summary>
        public CicloContrato CicloContratoObtener(int indice)
        {
            return this.ciclosContrato.Obtener(indice);
        }
        /// <summary>
        /// Elimina CicloContrato
        /// </summary>
        public CicloContrato CicloContratoEliminar(CicloContrato cicloContrato)
        {
            return this.ciclosContrato.Eliminar(CicloContratoIdentificar(cicloContrato), cicloContrato);
        }
        /// <summary>
        /// Elimina PruebaContrato basado en el indice
        /// </summary>
        public CicloContrato CicloContratoEliminar(int indice)
        {
            return this.ciclosContrato.Eliminar(indice);
        }
        /// <summary>
        /// Obtiene el numero de elementos de la lista de CicloContrato
        /// </summary>
        public int CicloContratoElementos()
        {
            return this.ciclosContrato.Elementos();
        }
        /// <summary>
        /// Obtiene el numero completo de elementos de la lista de CicloContrato
        /// </summary>
        public int CicloContratoElementosTodos()
        {
            return this.ciclosContrato.ElementosTodos();
        }
        /// <summary>
        /// Elimina todos los elementos del lista de CicloContrato
        /// </summary>
        public void CicloContratoLimpiar()
        {
            this.ciclosContrato.Limpiar();
        }
        /// <summary>
        /// Obtener el estado de la CicloContrato
        /// </summary>
        public EObjetoEstado CicloContratoEstado(CicloContrato cicloContrato)
        {
            return this.ciclosContrato.Estado(cicloContrato);
        }
        /// <summary>
        /// Obtener el  objeto original del CicloContrato
        /// </summary>
        public CicloContrato CicloContratoOriginal(CicloContrato cicloContrato)
        {
            return this.ciclosContrato.Original(cicloContrato);
        }
        /// <summary>
        /// Definir la expresión que identificara al CicloContrato
        /// </summary>
        private static Func<CicloContrato, bool> CicloContratoIdentificar(CicloContrato cicloContrato)
        {
            return (uo => cicloContrato.CicloContratoID != null && uo.CicloContratoID == cicloContrato.CicloContratoID);
        }

        #endregion

        public object Clone()
        {
            Contrato copy = (Contrato)this.MemberwiseClone();

            if (cliente != null)
                copy.Cliente = (Cliente)this.cliente.Clone();
            copy.Ubicacion = this.Ubicacion;
            copy.UsuarioRegistro = this.UsuarioRegistro;

            return copy;
        }
    }
}
