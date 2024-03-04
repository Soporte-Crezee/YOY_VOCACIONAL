using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;
using POV.Comun.BO;
using POV.Localizacion.BO;
using POV.Modelo.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using Framework.Base.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace POV.CentroEducativo.Service
{
    public class InfoAlumnoUsuarioCtrl
    {
        /// <summary>
        /// Consulta registros de AlumnoUsuarioRetHlp en la base de datos.
        /// </summary>
        public List<InfoAlumnoUsuario> Retrieve(IDataContext dctx, InfoAlumnoUsuario infoAlumnoUsuario) 
        {
            List<InfoAlumnoUsuario> alumnoUsuario = new List<InfoAlumnoUsuario>();
            InfoAlumnoUsuarioRetHlp da = new InfoAlumnoUsuarioRetHlp();
            var ds = da.Action(dctx, infoAlumnoUsuario);
           
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                alumnoUsuario.Add(DataRowToAlumnoUsuario(row));
            }

            return alumnoUsuario;
        }

        public InfoAlumnoUsuario DataRowToAlumnoUsuario(DataRow row)
        {
            InfoAlumnoUsuario obj = new InfoAlumnoUsuario();
            obj.clasificador = new Clasificador();
            obj.estado = new Estado();

            if (row.IsNull("AlumnoID"))
                obj.AlumnoID = null;
            else
                obj.AlumnoID = (long)Convert.ChangeType(row["AlumnoID"], typeof(long));

            if (row.IsNull("UsuarioID"))
                obj.UsuarioID = null;
            else
                obj.UsuarioID = (Int32)Convert.ChangeType(row["UsuarioID"], typeof(Int32));

            if (row.IsNull("Nombre"))
                obj.Nombre = null;
            else
                obj.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));

            if (row.IsNull("PrimerApellido"))
                obj.PrimerApellido = null;
            else
                obj.PrimerApellido = (string)Convert.ChangeType(row["PrimerApellido"], typeof(string));

            if (row.IsNull("SegundoApellido"))
                obj.SegundoApellido = null;
            else
                obj.SegundoApellido = (string)Convert.ChangeType(row["SegundoApellido"], typeof(string));

            if (row.IsNull("AreaConocimientoID"))
                obj.clasificador.ClasificadorID = null;
            else
                obj.clasificador.ClasificadorID = (Int32)Convert.ChangeType(row["AreaConocimientoID"], typeof(Int32));

            if (row.IsNull("Escuela"))
                obj.Escuela = null;
            else
                obj.Escuela = (string)Convert.ChangeType(row["Escuela"], typeof(string));

            if (row.IsNull("Grado"))
                obj.Grado = null;
            else
                obj.Grado = (EGrado)Convert.ChangeType(row["Grado"], typeof(byte));

            if (row.IsNull("EstadoID"))
                obj.estado.EstadoID = null;
            else
                obj.estado.EstadoID = (Int32)Convert.ChangeType(row["EstadoID"], typeof(Int32));

            if (row.IsNull("Email"))
                obj.Email = null;
            else
                obj.Email = (string)Convert.ChangeType(row["Email"], typeof(string));

            if (row.IsNull("Premium"))
                obj.Premium = null;
            else
                obj.Premium = (bool)Convert.ChangeType(row["Premium"], typeof(bool));

            if (row.IsNull("RecibirInformacion"))
                obj.RecibirInformacion = null;
            else
                obj.RecibirInformacion = (bool)Convert.ChangeType(row["RecibirInformacion"], typeof(bool));

            if (row.IsNull("DatosCompletos"))
                obj.DatosCompletos = null;
            else
                obj.DatosCompletos = (bool)Convert.ChangeType(row["DatosCompletos"], typeof(bool));

            return obj;
        }
    }
}
