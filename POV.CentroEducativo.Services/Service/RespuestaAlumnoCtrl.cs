using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using Framework.Base.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using POV.Prueba.BO;

namespace POV.CentroEducativo.Service
{
    public class RespuestaAlumnoCtrl
    {
        public List<RespuestaAlumno> RetrieveRespuestaAlumno(IDataContext dctx, Alumno alumno, PruebaDinamica prueba, EEstadoPrueba estadoPrueba) 
        {
            List<RespuestaAlumno> lstResAlumno = new List<RespuestaAlumno>();
            RespuestaAlumnoRetHlp da = new RespuestaAlumnoRetHlp();
            var ds = da.Action(dctx, alumno, prueba, estadoPrueba);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                lstResAlumno.Add(DataRowToRespuestaAlumno(row));
            }

            return lstResAlumno;
        }

        /// <summary>
        /// Obtiene la lista de pruebas del alumno segun el tipo de presentacion y estado de la prueba
        /// </summary>
        /// <param name="dctx"> El DataContext que proveerá acceso a la base de datos </param>
        /// <param name="alumno"> Alumno que provee el criterio de seleccion para realizar la consulta </param>
        /// <param name="pruebaPresentacion"> Lista de EtipoPruebaPresentacion que provee el criterio de seleccion para realizar la consulta </param>
        /// <param name="estadoPrueba"> EEstadoPrueba que provee el criterio de seleccion para realizar la consulta</param>
        /// <returns> Lalista que contiene la información de RespuestaAlumnoRetHlp generada por la consulta</returns>
        public List<RespuestaAlumno> RetrieveRespuestaAlumno(IDataContext dctx, Alumno alumno, List<ETipoPruebaPresentacion> pruebaPresentacion, EEstadoPrueba estadoPrueba)
        {
            List<RespuestaAlumno> lstResAlumno = new List<RespuestaAlumno>();
            RespuestaAlumnoRetHlp da = new RespuestaAlumnoRetHlp();
            string strPruebaPresentacion = string.Empty;
            if (pruebaPresentacion != null && pruebaPresentacion.Count > 0)
            {
                foreach (ETipoPruebaPresentacion item in pruebaPresentacion)
                {
                    strPruebaPresentacion += "," + ((int)item).ToString();
                }
                if (strPruebaPresentacion.StartsWith(","))
                    strPruebaPresentacion = strPruebaPresentacion.Substring(1);
            }

            var ds = da.Action(dctx, alumno, strPruebaPresentacion, estadoPrueba);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                lstResAlumno.Add(DataRowToRespuestaAlumno(row));
            }

            return lstResAlumno;
        }

        public RespuestaAlumno DataRowToRespuestaAlumno(DataRow row) 
        {
            RespuestaAlumno obj = new RespuestaAlumno();


            if (row.IsNull("AlumnoID"))
                obj.AlumnoID = null;
            else
                obj.AlumnoID = (long)Convert.ChangeType(row["AlumnoID"], typeof(long));

            if (row.IsNull("PruebaID"))
                obj.PruebaID = null;
            else
                obj.PruebaID = (Int32)Convert.ChangeType(row["PruebaID"], typeof(Int32));

            if (row.IsNull("Nombre"))
                obj.Nombre = null;
            else
                obj.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));

            if (row.IsNull("Calificacion"))
                obj.Calificacion = null;
            else
                obj.Calificacion = (Decimal)Convert.ChangeType(row["Calificacion"], typeof(Decimal));

            if (row.IsNull("EstadoPrueba"))
                obj.EstadoPrueba = null;
            else
                obj.EstadoPrueba = (Byte)Convert.ChangeType(row["EstadoPrueba"], typeof(Byte));

            if (row.IsNull("TipoPruebaPresentacion"))
                obj.TipoPruebaPresentacion = null;
            else
                obj.TipoPruebaPresentacion = (Byte)Convert.ChangeType(row["TipoPruebaPresentacion"], typeof(Byte));

            if (row.IsNull("ResultadoPruebaID"))
                obj.ResultadoPruebaID = null;
            else
                obj.ResultadoPruebaID = (Int32)Convert.ChangeType(row["ResultadoPruebaID"], typeof(Int32));

            if (row.IsNull("Espremium"))
                obj.EsPremium = EEsPremium.Gratis;
            else
            {
                var esPremium = (Boolean)Convert.ChangeType(row["Espremium"], typeof(Boolean));
                obj.EsPremium = (esPremium) ? EEsPremium.Premium : EEsPremium.Gratis;
            }
            return obj;
        }

        public List<PruebaAsignadaAlumno> RetrievePruebasAsignadaAlumno(IDataContext dctx, Alumno alumno) 
        {
            List<PruebaAsignadaAlumno> lstPruAsiAlumno = new List<PruebaAsignadaAlumno>();
            PruebaAsignadaAlumnoRetHlp da = new PruebaAsignadaAlumnoRetHlp();
            var ds = da.Action(dctx, alumno);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                lstPruAsiAlumno.Add(DataRowToPruebaAsignadaAlumno(row));
            }

            return lstPruAsiAlumno;
        }

        public PruebaAsignadaAlumno DataRowToPruebaAsignadaAlumno(DataRow row) 
        {
            PruebaAsignadaAlumno obj = new PruebaAsignadaAlumno();

            if (row.IsNull("PruebaID"))
                obj.PruebaID = null;
            else
                obj.PruebaID = (Int32)Convert.ChangeType(row["PruebaID"], typeof(Int32));

            if (row.IsNull("EstadoPrueba"))
                obj.EstadoPrueba = null;
            else
                obj.EstadoPrueba = (Byte)Convert.ChangeType(row["EstadoPrueba"], typeof(Byte));

            if (row.IsNull("TipoPruebaPresentacion"))
                obj.TipoPruebaPresentacion = null;
            else
                obj.TipoPruebaPresentacion = (Byte)Convert.ChangeType(row["TipoPruebaPresentacion"], typeof(Byte));

            return obj;
        }
    }
}
