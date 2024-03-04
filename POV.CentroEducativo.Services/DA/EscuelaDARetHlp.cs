using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Framework.Base.DataAccess;
using System.Data.Common;
using POV.CentroEducativo.BO;
using POV.Comun.BO;
using POV.Localizacion.BO;

namespace POV.CentroEducativo.DA
{
    public class EscuelaDARetHlp
    {
        public DataSet Escuelas ( IDataContext dctx, POV.CentroEducativo.BO.Docente docente )
        {
    
            DataSet dsEscuelas=new DataSet();
          
            //variables
            string swhere = string.Empty;
            string sError = string.Empty;
            StringBuilder query = new StringBuilder( );
            StringBuilder queryControl = new StringBuilder( );
            StringBuilder queryCols = new StringBuilder( );
            DbParameter dbParameter = null;
            DbCommand dbCommand = null;
            DbCommand dbCommandControl = null;
            object myFirm = new object( );

            try
            {
                dctx.OpenConnection( myFirm );
                dbCommand = dctx.CreateCommand( );
                dbCommandControl = dctx.CreateCommand( );
            }
            catch ( Exception ex )
            {
                throw new Exception( string.Format( "DocenteDARetHlp: No se pudo conectar con la base de datos {0}" , ex.Message ) );
            }
            #region ControlData
            queryControl.Append( " SELECT" );
            queryControl.Append( " DocenteEscuela.DocenteEscuelaID, DocenteEscuela.DocenteID, Escuela.* " );            
            queryControl.Append( " FROM DocenteEscuela INNER JOIN Escuela ON DocenteEscuela.EscuelaID = Escuela.EscuelaID " );            

            if ( docente.DocenteID!=null )
            {
                if ( swhere.Length > 0 ) swhere += " AND ";
                swhere += "DocenteID LIKE  @DocenteID";
                dbParameter = dbCommand.CreateParameter( );
                dbParameter.ParameterName = "@DocenteID";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = docente.DocenteID;
                dbCommandControl.Parameters.Add( dbParameter );
            }

            

            string notinwhere = string.Empty;

            notinwhere += "DocenteEscuela.Estatus =  @Estatus";
            dbParameter = dbCommand.CreateParameter( );
            dbParameter.ParameterName = "@Estatus";
            dbParameter.DbType = DbType.Boolean;
            dbParameter.Value = true;
            dbCommandControl.Parameters.Add( dbParameter );

            if ( swhere.Length > 0 ) query.Append( " WHERE " + swhere );
            

            try
            {
                DbDataAdapter adapter = dctx.CreateDataAdapter( );
                dbCommandControl.CommandText = queryControl.ToString( );
                adapter.SelectCommand = dbCommandControl;
                adapter.Fill( dsEscuelas , "DocenteEscuela" );                
            }
            catch ( Exception ex )
            {
                throw new Exception( string.Format( "DocenteDARetHlp: Ocurrió un problema al recuperar los datos de control: {0}" , ex.Message ) );
            }


            #endregion

            return dsEscuelas;
        }

        /// <summary>
        /// Consulta las escuelas que correspondan con los criterios de búsqueda
        /// Criterios de Búsqueda:Escuela,ZonaID,UbicacionID,PaisID,EstadoID,CiudadID,LocalidadID,TipoServicioID,NivelEducativoID,TipoNivelEducativoID
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="escuela">Escuela que provee el criterio de selección para la consulta</param>
        /// <returns>DataSet que contiene la información generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Escuela escuela)
        {
            DataSet dsEscuelas = new DataSet();
            string sError = string.Empty;

            if (escuela == null)
                sError += ", Escuela";
            if (sError.Length > 0)
                throw new Exception("EscuelaDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (escuela.Ubicacion == null)
                escuela.Ubicacion = new Ubicacion();
            if(escuela.Ubicacion.Pais == null)
                escuela.Ubicacion.Pais = new Pais();

            if(escuela.Ubicacion.Estado==null)
                escuela.Ubicacion.Estado = new Estado();
            if(escuela.Ubicacion.Ciudad == null)
                escuela.Ubicacion.Ciudad = new Ciudad();
            if(escuela.Ubicacion.Localidad == null)
                escuela.Ubicacion.Localidad = new Localidad();

            if (escuela.DirectorID == null)
                escuela.DirectorID = new Director();
            if (escuela.TipoServicio == null)
                escuela.TipoServicio = new TipoServicio();
            if(escuela.TipoServicio.NivelEducativoID==null)
                escuela.TipoServicio.NivelEducativoID = new NivelEducativo();
            if(escuela.TipoServicio.NivelEducativoID.TipoNivelEducativoID==null)
                escuela.TipoServicio.NivelEducativoID.TipoNivelEducativoID = new TipoNivelEducativo();
            if (escuela.ZonaID == null)
                escuela.ZonaID = new Zona();

            //variables
            string swhere = string.Empty;
            StringBuilder query = new StringBuilder();
            DbParameter dbParameter = null;
            DbCommand dbCommand = null;
            DbCommand dbCommandControl = null;
            object myFirm = new object();
            try
            {
                dctx.OpenConnection(myFirm);
                dbCommand = dctx.CreateCommand();
                dbCommandControl = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("EscuelaDARetHlp: No se pudo conectar con la base de datos {0}", ex.Message));
            }
            #region ControlData

            query.Append(" SELECT ESC.EscuelaID, ESC.Clave, ESC.NombreEscuela, ESC.FechaRegistro, ESC.Estatus, ESC.Turno, ESC.ZonaID,ESC.DirectorID, ESC.Control, ESC.Ambito,");
            query.Append(" ESC.UbicacionID, UBC.PaisID, UBC.EstadoID, UBC.CiudadID, UBC.LocalidadID, ESC.TipoServicioID, TIPS.NivelEducativoID, TIPONIV.TipoNivelEducativoID ");
            query.Append(" FROM Escuela AS ESC INNER JOIN ");
            query.Append(" Ubicacion AS UBC ON ESC.UbicacionID = UBC.UbicacionID INNER JOIN ");
            query.Append(" TipoServicio AS TIPS ON ESC.TipoServicioID = TIPS.TipoServicioID INNER JOIN ");
            query.Append(" NivelEducativo AS NIV ON TIPS.NivelEducativoID = NIV.NivelEducativoID INNER JOIN ");
            query.Append(" TipoNivelEducativo AS TIPONIV ON NIV.TipoNivelEducativoID = TIPONIV.TipoNivelEducativoID ");

            if (escuela.EscuelaID != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ESC.EscuelaID =  @EscuelaID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@EscuelaID";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = escuela.EscuelaID;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.Clave != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ESC.Clave LIKE  @Clave";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@Clave";
                dbParameter.DbType = DbType.String;
                dbParameter.Value = escuela.Clave;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.NombreEscuela != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ESC.NombreEscuela LIKE  @NombreEscuela";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@NombreEscuela";
                dbParameter.DbType = DbType.String;
                dbParameter.Value = escuela.NombreEscuela;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.FechaRegistro != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ESC.FechaRegistro =  @CFechaRegistro";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@FechaRegistro";
                dbParameter.DbType = DbType.DateTime;
                dbParameter.Value = escuela.FechaRegistro;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.Estatus != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ESC.Estatus =  @Estatus";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@Estatus";
                dbParameter.DbType = DbType.Boolean;
                dbParameter.Value = escuela.Estatus;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.Turno != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ESC.Turno =  @Turno";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@Turno";
                dbParameter.DbType = DbType.Byte;
                dbParameter.Value = (byte)escuela.Turno.Value;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.ZonaID.ZonaID != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ESC.ZonaID =  @ZonaID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@ZonaID";
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = escuela.ZonaID.ZonaID;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.DirectorID.DirectorID != null)
            {

                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ESC.DirectorID =  @DirectorID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@DirectorID";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = escuela.DirectorID.DirectorID;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.Control != null)
            {

                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ESC.Control =  @Control";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@Control";
                dbParameter.DbType = DbType.Byte;
                dbParameter.Value = (byte)escuela.Control.Value;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.Ambito != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ESC.Ambito =  @Ambito";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@Ambito";
                dbParameter.DbType = DbType.Byte;
                dbParameter.Value = (byte)escuela.Ambito.Value;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.Ubicacion.UbicacionID != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ESC.UbicacionID =  @UbicacionID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@UbicacionID";
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = escuela.Ubicacion.UbicacionID;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.Ubicacion.Pais.PaisID != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "UBC.PaisID =  @PaisID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@PaisID";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = escuela.Ubicacion.Pais.PaisID;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.Ubicacion.Estado.EstadoID != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "UBC.EstadoID =  @EstadoID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@EstadoID";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = escuela.Ubicacion.Estado.EstadoID;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.Ubicacion.Ciudad.CiudadID != null)
            {

                if (swhere.Length > 0) swhere += " AND ";
                swhere += "UBC.CiudadID =  @CiudadID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@CiudadID";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = escuela.Ubicacion.Ciudad.CiudadID;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.Ubicacion.Localidad.LocalidadID != null)
            {

                if (swhere.Length > 0) swhere += " AND ";
                swhere += "UBC.LocalidadID =  @LocalidadID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@LocalidadID";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = escuela.Ubicacion.Localidad.LocalidadID;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.TipoServicio.TipoServicioID != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ESC.TipoServicioID =  @TipoServicioID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@TipoServicioID";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = escuela.TipoServicio.TipoServicioID;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.TipoServicio.NivelEducativoID.NivelEducativoID != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "TIPS.NivelEducativoID =  @NivelEducativoID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@NivelEducativoID";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = escuela.TipoServicio.NivelEducativoID.NivelEducativoID;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.TipoServicio.NivelEducativoID.TipoNivelEducativoID.TipoNivelEducativoID != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "TIPONIV.TipoNivelEducativoID =  @TipoNivelEducativoID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@TipoNivelEducativoID";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = escuela.TipoServicio.NivelEducativoID.TipoNivelEducativoID.TipoNivelEducativoID;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            

            if (swhere.Length > 0) query.Append(" WHERE " + swhere);
            try
            {
                DbDataAdapter adapter = dctx.CreateDataAdapter();
                dbCommandControl.CommandText = query.ToString();
                adapter.SelectCommand = dbCommandControl;
                adapter.Fill(dsEscuelas, "Escuelas");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("EscuelaDARetHlp: Ocurrió un problema al recuperar los datos de control: {0}", ex.Message));
            }


            #endregion

            return dsEscuelas;
        }
    }
}
