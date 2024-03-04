using POV.Expediente.BO;
using POV.Modelo.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;


namespace POV.Expediente.Services
{
    public class UniversidadCarreraAspiranteCtrl : IDisposable
    {
        private readonly Contexto model;
        private readonly object sign;

        public UniversidadCarreraAspiranteCtrl(Contexto contexto)
        {
            sign = new object();
            model = contexto ?? new Contexto(sign);
        }

        public int Insert(UniversidadCarreraAspirante universidadCarreraAspirante)
        {
            model.UniversidadCarreraAspirante.Add(universidadCarreraAspirante);

            int affectedRows = model.Commit(sign);

            return affectedRows;
        }
        
        public void Dispose()
        {
            model.Disposing(this.sign);
        }
    }
}
