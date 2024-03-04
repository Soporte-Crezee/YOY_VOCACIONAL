using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Comun.BO;

namespace POV.CentroEducativo.BO
{
    public class PlanEducativo
    {
        private int? planEducativoID;
        private string titulo;
        private string descripcion;
        private DateTime? validoDesde;
        private DateTime? validoHasta;
        private NivelEducativo nivelEducativo;
        private bool? estatus; 

        public PlanEducativo()
        {
            this.materias = new ObjetoListado<Materia>();
        }

        public PlanEducativo(IEnumerable<Materia> materias)
        {
            this.materias = new ObjetoListado<Materia>(materias);
        }

        public int? PlanEducativoID
        {
            get { return planEducativoID; }
            set { planEducativoID = value; }
        }

        public string Titulo
        {
            get { return titulo; }
            set { titulo = value; }
        }

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        public DateTime? ValidoDesde
        {
            get { return validoDesde; }
            set { validoDesde = value; }
        }

        public DateTime? ValidoHasta
        {
            get { return validoHasta; }
            set { validoHasta = value; }
        }

        public NivelEducativo NivelEducativo
        {
            get { return nivelEducativo; }
            set { nivelEducativo = value; }
        }

        public bool? Estatus
        {
            get { return estatus; }
            set { this.estatus = value; }
        }

        private ObjetoListado<Materia> materias;
        public IEnumerable<Materia> Materias
        {
            get { return this.materias.Objetos; }
        }
        /// <summary>
        /// Agrega Materia
        /// </summary>
        public Materia MateriaAgregar(Materia materia)
        {
            return this.materias.Agregar(MateriaIdentificar(materia), materia);
        }
        /// <summary>
        /// Obtener Materia basado en el indice
        /// </summary>
        public Materia MateriaObtener(int indice)
        {
            return this.materias.Obtener(indice);
        }
        /// <summary>
        /// Elimina Materia
        /// </summary>
        public Materia MateriaEliminar(Materia materia)
        {
            return this.materias.Eliminar(MateriaIdentificar(materia), materia);
        }
        /// <summary>
        /// Elimina Materia basado en el indice
        /// </summary>
        public Materia MateriaEliminar(int indice)
        {
            return this.materias.Eliminar(indice);
        }
        /// <summary>
        /// Obtiene el numero de elementos de la lista de Materia
        /// </summary>
        public int MateriaElementos()
        {
            return this.materias.Elementos();
        }
        /// <summary>
        /// Obtiene el numero completo de elementos de la lista de Materia
        /// </summary>
        public int MateriaElementosTodos()
        {
            return this.materias.ElementosTodos();
        }
        /// <summary>
        /// Elimina todos los elementos del lista de Materia
        /// </summary>
        public void GrupoLimpiar()
        {
            this.materias.Limpiar();
        }
        /// <summary>
        /// Obtener el estado del Materia
        /// </summary>
        public EObjetoEstado MateriaEstado(Materia materia)
        {
            return this.materias.Estado(materia);
        }
        /// <summary>
        /// Obtener el  objeto original del Materia
        /// </summary>
        public Materia MateriaOriginal(Materia materia)
        {
            return this.materias.Original(materia);
        }
        /// <summary>
        /// Definir la expresión que identificara al Materia
        /// </summary>
        private static Func<Materia, bool> MateriaIdentificar(Materia materia)
        {
            return (uo => uo.MateriaID != null && uo.MateriaID == materia.MateriaID);
        }
    }
}
