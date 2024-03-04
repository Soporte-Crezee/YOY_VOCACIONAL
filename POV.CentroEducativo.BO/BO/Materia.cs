using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Comun.BO;
using POV.Modelo.Estandarizado.BO;

namespace POV.CentroEducativo.BO
{
    public class Materia : ICloneable, IObservableChange
    {
        private int? materiaID;
        private string clave;
        private string titulo;
        private byte? grado;
        private AreaAplicacion areaAplicacion;

        public int? MateriaID
        {
            get { return materiaID; }
            set
            {
                if (this.materiaID != value)
                {
                    materiaID = value;
                    this.Cambio();
                }
            }
        }

        public string Clave
        {
            get { return clave; }
            set
            {
                if (this.clave != value)
                {
                    clave = value;
                    this.Cambio();
                }
            }
        }

        public string Titulo
        {
            get { return titulo; }
            set
            {
                if (this.titulo != value)
                {
                    titulo = value; 
                    this.Cambio();
                }
            }
        }

        public byte? Grado
        {
            get { return grado; }
            set
            {
                if (this.grado != value)
                {
                    grado = value;
                    this.Cambio();
                }
            }
        }

        public AreaAplicacion AreaAplicacion
        {
            get { return areaAplicacion; }
            set
            {
                if (areaAplicacion != value)
                {
                    areaAplicacion = value;

                    if ((this.areaAplicacion == null || value != null) || (this.areaAplicacion.AreaAplicacionID != value.AreaAplicacionID))
                    {
                        this.Cambio();
                    }
                }
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public object CloneAll()
        {
            Materia clone = (Materia)this.Clone();
            clone.areaAplicacion = (AreaAplicacion)this.areaAplicacion.Clone();

            return clone;
        }

        private IObserverChange observer;
        public void Registrar(IObserverChange observer)
        {
            this.observer = observer;
        }

        private void Cambio()
        {
            if (this.observer != null)
                this.observer.Cambio();
        }
    }
}
