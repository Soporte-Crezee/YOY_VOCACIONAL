using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;
using System;
using System.Data;
namespace POV.CentroEducativo.Service
{
    public class EspecialistaCtrl
    {
        public DataSet Retrieve(IDataContext dctx, EspecialistaPruebas especialista)
        {
            EspecialistaRetHlp da = new EspecialistaRetHlp();
            DataSet ds = da.Action(dctx, especialista);
            return ds;
        }

        public EspecialistaPruebas RetrieveComplete(IDataContext dctx, EspecialistaPruebas especialista)
        {
            DataSet ds = Retrieve(dctx, especialista);
            if (ds.Tables["EspecialistaPruebas"].Rows.Count == 0)
                throw new Exception("EspecialistaCtrl: No se encontró ningún Usuario con los parámetros proporcionados");
            especialista = LastDataRowToEspecialista(ds);
            return especialista;
        }

        public void Insert(IDataContext dctx, EspecialistaPruebas especialista)
        {
            EspecialistaInsHlp da = new EspecialistaInsHlp();
            da.Action(dctx, especialista);
            
        }

        public void Update(IDataContext dctx, EspecialistaPruebas especialista, EspecialistaPruebas previus)
        {
            EspecialistaUpdHlp da = new EspecialistaUpdHlp();
            if (especialista.Curp != previus.Curp)
            {
                if (NoRepetido(dctx, especialista))
                    da.Action(dctx, especialista, previus);
                else
                    throw new Exception("Update: El curp que intenta agregar ya esta asignado a otro Especialista.");
            }
            else
            {
                da.Action(dctx, especialista, previus);
            }
        }

        public void UpdateComplete(IDataContext dctx, EspecialistaPruebas especialista, EspecialistaPruebas previous)
        {
            if (especialista == null)
                throw new ArgumentNullException("EspecialistaCtrl: especialista no puede ser nulo");

            if (previous == null)
                throw new ArgumentNullException("EspecialistaCtrl: previous no puede ser nulo");
            object myfirm = new object();
            try
            {
                dctx.OpenConnection(myfirm);
                dctx.BeginTransaction(myfirm);
                Update(dctx,especialista,previous);
                dctx.CommitTransaction(myfirm);
            }
            catch (Exception ex)
            {
                
                dctx.RollbackTransaction(myfirm);
                dctx.CommitTransaction(myfirm);
                throw new Exception("EspecialistaCtrl: UpdateComplete ocurrió un error, " + ex.Message);
            }         
        }

        public EspecialistaPruebas LastDataRowToEspecialista(DataSet ds)
        {
            if (!ds.Tables.Contains("EspecialistaPruebas"))
                throw new Exception("LastDataRowToEspecialista: DataSet no tiene la tabla EspecialistaPrueba");
            int index = ds.Tables["EspecialistaPruebas"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToEspecialista: El DataSet no tiene filas");
            return this.DataRowToEspecialista(ds.Tables["EspecialistaPruebas"].Rows[index - 1]);
        }

        public EspecialistaPruebas DataRowToEspecialista(DataRow row)
        {
            EspecialistaPruebas especialista = new EspecialistaPruebas();
            if (row.IsNull("EspecialistaID"))
                especialista.EspecialistaPruebaID = null;
            else
                especialista.EspecialistaPruebaID = (int)Convert.ChangeType(row["EspecialistaID"], typeof(int));
            if (row.IsNull("Curp"))
                especialista.Curp = null;
            else
                especialista.Curp = (String)Convert.ChangeType(row["Curp"], typeof(String));
            if (row.IsNull("Nombre"))
                especialista.Nombre = null;
            else
                especialista.Nombre = (String)Convert.ChangeType(row["Nombre"], typeof(String));
            if (row.IsNull("PrimerApellido"))
                especialista.PrimerApellido = null;
            else
                especialista.PrimerApellido = (String)Convert.ChangeType(row["PrimerApellido"], typeof(String));
            if (row.IsNull("SegundoApellido"))
                especialista.SegundoApellido = null;
            else
                especialista.SegundoApellido = (String)Convert.ChangeType(row["SegundoApellido"], typeof(String));
            if (row.IsNull("Estatus"))
                especialista.Estatus = null;
            else
                especialista.Estatus = (bool)Convert.ChangeType(row["Estatus"], typeof(bool));
            if (row.IsNull("FechaRegistro"))
                especialista.FechaRegistro = null;
            else
                especialista.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("FechaNacimiento"))
                especialista.FechaNacimiento = null;
            else
                especialista.FechaNacimiento = (DateTime)Convert.ChangeType(row["FechaNacimiento"], typeof(DateTime));
            if (row.IsNull("Sexo"))
                especialista.Sexo = null;
            else
                especialista.Sexo = (bool)Convert.ChangeType(row["Sexo"], typeof(bool));
            if (row.IsNull("Correo"))
                especialista.Correo = null;
            else
                especialista.Correo = (String)Convert.ChangeType(row["Correo"], typeof(String));
            if (row.IsNull("Clave"))
                especialista.Clave = null;
            else
                especialista.Clave = (String)Convert.ChangeType(row["Clave"], typeof(String));
            if (row.IsNull("EstatusIdentificacion"))
                especialista.EstatusIdentificacion = null;
            else
                especialista.EstatusIdentificacion = (bool)Convert.ChangeType(row["EstatusIdentificacion"], typeof(bool));

            return especialista;
        }

        private bool NoRepetido(IDataContext dctx, EspecialistaPruebas especialista)
        {
            DataSet ds = Retrieve(dctx, new EspecialistaPruebas { Curp = especialista.Curp });
            return ds.Tables[0].Rows.Count <= 0;
        }

    }
}
