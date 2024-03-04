using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using System.Data;
using GP.SocialEngine.Service;
using POV.CentroEducativo.BO;
using POV.Licencias.Service;
using POV.ReactivosUsuario.Service;
using POV.ReactivosUsuario.BO;
using POV.Reactivos.BO;
using POV.Reactivos.Service;
using POV.Modelo.Estandarizado.BO;
using POV.Modelo.Estandarizado.Service;

namespace POV.Social.Service
{
    public class ReporteResultadoReactivoEstandarizadoCtrl
    {
        public DataSet retrieveResultadosReactivosEstandar(IDataContext dctx, Alumno alumno)
        {
            try
            {
                LicenciaEscuelaCtrl licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
                UsuarioSocial usuarioSocial = licenciaEscuelaCtrl.RetrieveUsuarioSocial(dctx, alumno);
                RespuestaReactivoUsuarioCtrl respuestaReactivoCtrl = new RespuestaReactivoUsuarioCtrl();
                RespuestaReactivoUsuario respuesta = new RespuestaReactivoUsuario();
                respuesta.UsuarioSocial = usuarioSocial;
                DataSet ds = respuestaReactivoCtrl.Retrieve(dctx, respuesta);

                ds.Tables["RespuestaReactivoUsuario"].Columns.Add("NombreReactivo");
                ds.Tables["RespuestaReactivoUsuario"].Columns.Add("AreaAplicacion");
                ds.Tables["RespuestaReactivoUsuario"].Columns.Add("Complejidad");
                foreach (DataRow row in ds.Tables["RespuestaReactivoUsuario"].Rows)
                {
                    Guid ReactivoID = Guid.Parse(row["ReactivoID"].ToString());
                    Reactivo reactivo = new Reactivo();
                    reactivo.TipoReactivo = ETipoReactivo.Estandarizado;
                    reactivo.ReactivoID = ReactivoID;
                    ReactivoCtrl reactivoCtrl = new ReactivoCtrl();
                    reactivo = reactivoCtrl.RetrieveComplete(dctx, reactivo);
                    row["NombreReactivo"] = reactivo.NombreReactivo;
                    row["AreaAplicacion"] = caracteristicasReactivo.AreaAplicacion.Descripcion;
                    row["Complejidad"] = caracteristicasReactivo.TipoComplejidad.Titulo;
                }
                return ds;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
         }

    }
}
