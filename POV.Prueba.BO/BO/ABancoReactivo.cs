using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using POV.Reactivos.BO;
using POV.Prueba.BO;

namespace POV.Prueba.BO
{
    /// <summary>
    /// Representa el Banco de Reactivos
    /// </summary>
    public abstract class ABancoReactivo
    {
        private int? bancoReactivoID;
        /// <summary>
        /// Identificador del reactivo
        /// </summary>
        public int? BancoReactivoID
        {
            get { return this.bancoReactivoID; }
            set { this.bancoReactivoID = value; }
        }
        private int? numeroReactivos;
        /// <summary>
        /// Numero de Reactivos
        /// </summary>
        public int? NumeroReactivos
        {
            get { return this.numeroReactivos; }
            set { this.numeroReactivos = value; }
        }
        private DateTime? fechaRegistro;
        /// <summary>
        /// Fecha de Registro
        /// </summary>
        public DateTime? FechaRegistro
        {
            get { return this.fechaRegistro; }
            set { this.fechaRegistro = value; }
        }
        private bool? activo;
        /// <summary>
        /// Estatus del Banco
        /// </summary>
        public bool? Activo
        {
            get { return this.activo; }
            set { this.activo = value; }
        }
        private bool? esSeleccionOrdenada;
        /// <summary>
        /// EsSeleccion Aleatoria
        /// </summary>
        public bool? EsSeleccionOrdenada
        {
            get { return this.esSeleccionOrdenada; }
            set { this.esSeleccionOrdenada = value; }
        }
        private APrueba prueba;
        /// <summary>
        /// Identificador de la Prueba
        /// </summary>
        public APrueba Prueba
        {
            get { return this.prueba; }
            set { this.prueba = value; }
        }
        private ETipoSeleccionBanco? tipoSeleccionBanco;
        /// <summary>
        /// Tipo de seleccion del banco
        /// </summary>
        public ETipoSeleccionBanco? TipoSeleccionBanco
        {
            get { return this.tipoSeleccionBanco; }
            set { this.tipoSeleccionBanco = value; }
        }
        private int? reactivosPorPagina;
        /// <summary>
        /// Numero de Reactivo por pagina
        /// </summary>
        public int? ReactivosPorPagina
        {
            get { return this.reactivosPorPagina; }
            set { this.reactivosPorPagina = value; }
        }
        private List<ReactivoBanco> listaReactivosBanco;
        /// <summary>
        /// Lista de Reactivos Banco
        /// </summary>
        public List<ReactivoBanco> ListaReactivosBanco
        {
            get { return this.listaReactivosBanco; }
            set { this.listaReactivosBanco = value; }
        }

        public bool? EsPorGrupo { get; set; }

        /// <summary>
        /// Genera la lista de reactivos de una prueba concreta
        /// </summary>
        /// <returns></returns>
        public List<Reactivo> GenerarListaReactivosPruebaConcreta()
        {
            if (tipoSeleccionBanco == null) throw new ArgumentNullException("ABancoReactivo: tipo de seleccion requerido");
            if (listaReactivosBanco == null) throw new ArgumentNullException("ABancoReactivo: listaReactivos requerido");
            if (ListaReactivosBanco.Count == 0) throw new ArgumentNullException("ABancoReactivo: listaReactivos requerido");
            if (NumeroReactivos == null) throw new ArgumentNullException("ABancoReactivo: NumeroReactivos requerido");

            if (listaReactivosBanco.Count < numeroReactivos) throw new Exception("No existen suficientes Reactivos en el Banco de la Prueba");
            
            List<Reactivo> reactivos = new List<Reactivo>();
            List<ReactivoBanco> reactivosBanco = listaReactivosBanco.OrderBy(item => item.Orden).ToList();
            List<ReactivoBanco> reactivosSeleccionados = new List<ReactivoBanco>();
            switch (tipoSeleccionBanco)
            {
                case ETipoSeleccionBanco.TOTAL:
                    reactivosSeleccionados = reactivosBanco;
                    break;
                case ETipoSeleccionBanco.ALEATORIA:
                    var seleccionAleatoria = new Random(new object().GetHashCode());
                    //ordenar de forma aleatoria
                    var reactivosMezclados = reactivosBanco.OrderBy(r => seleccionAleatoria.Next()).ToList();
                    //seleccionamos de acuerdo al numero de reactivos
                    for( int index = 0; index < NumeroReactivos; index++)
                    {
                        reactivosSeleccionados.Add(reactivosMezclados[index]);
                    }
                    break;
                case ETipoSeleccionBanco.NUMERO_ESPECIFICO:
                    reactivosSeleccionados = reactivosBanco.Where(item => item.EstaSeleccionado.Value).ToList();

                    break;
            }

            if (esSeleccionOrdenada.Value)
            {
                reactivosSeleccionados = reactivosSeleccionados.OrderBy(item => item.Orden).ToList();
            }
            else
            {
                var ordenAleatorio = new Random(new object().GetHashCode());
                //ordenar de forma aleatoria
                reactivosSeleccionados = reactivosSeleccionados.OrderBy(r => ordenAleatorio.Next()).ToList();
            }

            foreach (ReactivoBanco reactivoBanco in reactivosSeleccionados)
            {
                reactivos.Add(reactivoBanco.Reactivo);
            }

            return reactivos;
        }
    }
}
