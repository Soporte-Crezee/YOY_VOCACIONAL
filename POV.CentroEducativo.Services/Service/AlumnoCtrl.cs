using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;
using POV.Localizacion.BO;
using POV.Localizacion.Service;

namespace POV.CentroEducativo.Service
{
    /// <summary>
    /// Controlador del objeto Alumno
    /// </summary>
    public class AlumnoCtrl
    {
        /// <summary>
        /// Consulta registros de AlumnoRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="alumnoRetHlp">AlumnoRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de AlumnoRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, Alumno alumno)
        {
            AlumnoRetHlp da = new AlumnoRetHlp();
            DataSet ds = da.Action(dctx, alumno);
            return ds;
        }

        public Alumno RetrieveUbicacion(IDataContext dctx, Alumno alumno)
        {
            DataSet dsAlumno = new AlumnoRetHlp().Action(dctx, alumno);
            Alumno resultado = this.LastDataRowToAlumno(dsAlumno);

            UbicacionCtrl ubicacionCtrl = new UbicacionCtrl();

            resultado.Ubicacion = ubicacionCtrl.LastDataRowToUbicacion(ubicacionCtrl.Retrieve(dctx, resultado.Ubicacion));

            return resultado;
        }

        public Alumno RetrieveAreasConocimiento(IDataContext dctx, Alumno alumno)
        {
            DataSet dsAlumno = new AlumnoRetHlp().Action(dctx, alumno);
            Alumno resultado = this.LastDataRowToAlumno(dsAlumno);

            UbicacionCtrl ubicacionCtrl = new UbicacionCtrl();

            resultado.Ubicacion = ubicacionCtrl.LastDataRowToUbicacion(ubicacionCtrl.Retrieve(dctx, resultado.Ubicacion));

            return resultado;
        }

        /// <summary>
        /// Crea un registro de AlumnoInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="alumnoInsHlp">AlumnoInsHlp que desea crear</param>
        public void Insert(IDataContext dctx, Alumno alumno)
        {
            AlumnoInsHlp da = new AlumnoInsHlp();
            da.Action(dctx, alumno);
        }

        private bool NoRepetido(Alumno alumno, IDataContext dctx)
        {
            DataSet ds = Retrieve(dctx, new Alumno { Curp = alumno.Curp });
            return ds.Tables["Alumno"].Rows.Count <= 0;

        }
        /// <summary>
        /// Actualiza de manera optimista un registro de AlumnoUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="alumnoUpdHlp">AlumnoUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">AlumnoUpdHlp que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, Alumno alumno, Alumno previous)
        {
            AlumnoUpdHlp da = new AlumnoUpdHlp();
            if (alumno.Curp != previous.Curp)
            {
                if (NoRepetido(alumno, dctx))
                    da.Action(dctx, alumno, previous);
                else
                    throw new Exception("Update: El curp que intenta agregar ya esta asignado a otro alumno.");
            }
            else
                da.Action(dctx, alumno, previous);
        }
        /// <summary>
        /// Crea un objeto de Alumno a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de Alumno</param>
        /// <returns>Un objeto de Alumno creado a partir de los datos</returns>
        public Alumno LastDataRowToAlumno(DataSet ds)
        {
            if (!ds.Tables.Contains("Alumno"))
                throw new Exception("LastDataRowToAlumno: DataSet no tiene la tabla Alumno");
            int index = ds.Tables["Alumno"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToAlumno: El DataSet no tiene filas");
            return this.DataRowToAlumno(ds.Tables["Alumno"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de Alumno a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de Alumno</param>
        /// <returns>Un objeto de Alumno creado a partir de los datos</returns>
        public Alumno DataRowToAlumno(DataRow row)
        {
            Alumno alumno = new Alumno();
            if (alumno.Ubicacion == null)
                alumno.Ubicacion = new Ubicacion();

            if (row.IsNull("AlumnoID"))
                alumno.AlumnoID = null;
            else
                alumno.AlumnoID = (int)Convert.ChangeType(row["AlumnoID"], typeof(int));
            if (row.IsNull("Curp"))
                alumno.Curp = null;
            else
                alumno.Curp = (string)Convert.ChangeType(row["Curp"], typeof(string));
            if (row.IsNull("Matricula"))
                alumno.Matricula = null;
            else
                alumno.Matricula = (string)Convert.ChangeType(row["Matricula"], typeof(string));
            if (row.IsNull("Nombre"))
                alumno.Nombre = null;
            else
                alumno.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
            if (row.IsNull("PrimerApellido"))
                alumno.PrimerApellido = null;
            else
                alumno.PrimerApellido = (string)Convert.ChangeType(row["PrimerApellido"], typeof(string));
            if (row.IsNull("SegundoApellido"))
                alumno.SegundoApellido = null;
            else
                alumno.SegundoApellido = (string)Convert.ChangeType(row["SegundoApellido"], typeof(string));
            if (row.IsNull("FechaNacimiento"))
                alumno.FechaNacimiento = null;
            else
                alumno.FechaNacimiento = (DateTime)Convert.ChangeType(row["FechaNacimiento"], typeof(DateTime));
            if (row.IsNull("Direccion"))
                alumno.Direccion = null;
            else
                alumno.Direccion = (string)Convert.ChangeType(row["Direccion"], typeof(string));
            if (row.IsNull("NombreCompletoTutor"))
                alumno.NombreCompletoTutor = null;
            else
                alumno.NombreCompletoTutor = (string)Convert.ChangeType(row["NombreCompletoTutor"], typeof(string));
            if (row.IsNull("NombreCompletoTutorDos"))
                alumno.NombreCompletoTutorDos = null;
            else
                alumno.NombreCompletoTutorDos = (string)Convert.ChangeType(row["NombreCompletoTutorDos"], typeof(string));
            if (row.IsNull("Estatus"))
                alumno.Estatus = null;
            else
                alumno.Estatus = (bool)Convert.ChangeType(row["Estatus"], typeof(bool));
            if (row.IsNull("FechaRegistro"))
                alumno.FechaRegistro = null;
            else
                alumno.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("Sexo"))
                alumno.Sexo = null;
            else
                alumno.Sexo = (bool)Convert.ChangeType(row["Sexo"], typeof(bool));
            if (row.IsNull("EstatusIdentificacion"))
                alumno.EstatusIdentificacion = null;
            else
                alumno.EstatusIdentificacion = (bool)Convert.ChangeType(row["EstatusIdentificacion"], typeof(bool));
            if (row.IsNull("CorreoConfirmado"))
                alumno.CorreoConfirmado = null;
            else
                alumno.CorreoConfirmado = (bool)Convert.ChangeType(row["CorreoConfirmado"], typeof(bool));
            if (row.IsNull("UbicacionID"))
                alumno.Ubicacion.UbicacionID = null;
            else
                alumno.Ubicacion.UbicacionID = (long)Convert.ChangeType(row["UbicacionID"], typeof(long));
            if (row.IsNull("Escuela"))
                alumno.Escuela = null;
            else
                alumno.Escuela = (string)Convert.ChangeType(row["Escuela"], typeof(string));
            if (row.IsNull("Grado"))
                alumno.Grado = null;
            else
                alumno.Grado = (EGrado)Convert.ChangeType(row["Grado"], typeof(byte));
            if (row.IsNull("RecibirInformacion"))
                alumno.RecibirInformacion = null;
            else
                alumno.RecibirInformacion = (bool)Convert.ChangeType(row["RecibirInformacion"], typeof(bool));
            if (row.IsNull("DatosCompletos"))
                alumno.DatosCompletos = null;
            else
                alumno.DatosCompletos = (bool)Convert.ChangeType(row["DatosCompletos"], typeof(bool));
            if (row.IsNull("CarreraSeleccionada"))
                alumno.CarreraSeleccionada = null;
            else
                alumno.CarreraSeleccionada = (bool)Convert.ChangeType(row["CarreraSeleccionada"], typeof(bool));
            if (row.IsNull("Premium"))
                alumno.Premium = null;
            else
                alumno.Premium = (bool)Convert.ChangeType(row["Premium"], typeof(bool));
            //if (row.IsNull("DocenteID"))
            //    alumno.DocenteId = null;
            //else
            //    alumno.DocenteId = (int)Convert.ChangeType(row["DocenteID"], typeof(int));
            if (row.IsNull("Credito"))
                alumno.Credito = null;
            else
                alumno.Credito = (double)Convert.ChangeType(row["Credito"], typeof(double));
            if (row.IsNull("CreditoUsado"))
                alumno.CreditoUsado = null;
            else
                alumno.CreditoUsado = (double)Convert.ChangeType(row["CreditoUsado"], typeof(double));
            if (row.IsNull("Saldo"))
                alumno.Saldo = null;
            else
                alumno.Saldo = (double)Convert.ChangeType(row["Saldo"], typeof(double));
            if (row.IsNull("NivelEscolar"))
                alumno.NivelEscolar = null;
            else
                alumno.NivelEscolar = (ENivelEscolar)Convert.ChangeType(row["NivelEscolar"], typeof(byte));
            if (row.IsNull("EstatusPago"))
                alumno.EstatusPago = null;
            else
                alumno.EstatusPago = (EEstadoPago)Convert.ChangeType(row["EstatusPago"], typeof(byte));
            if (row.IsNull("ReferenciaOXXO"))
                alumno.ReferenciaOXXO = null;
            else
                alumno.ReferenciaOXXO = (string)Convert.ChangeType(row["ReferenciaOXXO"], typeof(string));
            if (row.IsNull("IDReferenciaOXXO"))
                alumno.IDReferenciaOXXO = null;
            else
                alumno.IDReferenciaOXXO = (string)Convert.ChangeType(row["IDReferenciaOXXO"], typeof(string));
            if (row.IsNull("NotificacionPago"))
                alumno.NotificacionPago = null;
            else
                alumno.NotificacionPago = (bool)Convert.ChangeType(row["NotificacionPago"], typeof(bool));
            return alumno;
        }
    }
}
