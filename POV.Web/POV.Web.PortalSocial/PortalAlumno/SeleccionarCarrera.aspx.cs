using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;
using POV.CentroEducativo.Service;
using POV.CentroEducativo.Services;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.DetalleExpediente.Services;
using POV.Expediente.BO;
using POV.Expediente.Service;
using POV.Expediente.Services;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Logger.Service;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Web.Administracion.Helper;
using POV.Web.PortalSocial.AppCode;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalSocial.PortalAlumno
{
    public partial class SeleccionarCarrera : System.Web.UI.Page
    {
        private List<string> ListaSeleccionados
        {
            get { return Session["ProdSelection"] as List<string>; }
            set { Session["ProdSelection"] = value; }
        }

        public DataTable DtCarreraUniversidad
        {
            set { Session["dtCarreraUniversidad"] = value; }
            get { return Session["dtCarreraUniversidad"] != null ? Session["dtCarreraUniversidad"] as DataTable : null; }
        }

        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "DESC"; }
            set { ViewState["SortDirection"] = value; }
        }

        private IUserSession userSession;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;
        private IRedirector redirector;
        private Alumno alumno;
        private AlumnoCtrl alumnoCtrl;
        private UniversidadCarrera universidadCarreraSelecciones;

        public SeleccionarCarrera() 
        {
            userSession = new UserSession();
            redirector = new Redirector();
            alumno = new Alumno();
            alumnoCtrl = new AlumnoCtrl();
            universidadCarreraSelecciones = new UniversidadCarrera();
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try 
            {
                if (!IsPostBack) 
                {
                    if (userSession.IsLogin())
                    {
                        int[] areas = GetAreasConocimientoAlumno();
                        List<Carrera> carreras = new CarreraCtrl(null).RetrieveWithRelationship(new Carrera(), false);
                        var carrerabyAreaConocimiento = (from c in carreras
                                                         where areas.Any(x => x == c.ClasificadorID)
                                                         select c).ToList();

                        // Sacar la lista de universidades de acuerdo a las carreras
                        var universidades = carrerabyAreaConocimiento.SelectMany(c => c.Universidades).Distinct().ToList();

                        // Crear la lista de universidades unicas
                        var uniqueUniversidades = (from c in universidades
                                                   select c.UniversidadID).Distinct().ToList();

                        // Mostrar carreras y universidades que lo imparten
                        var carreraUniversidad = (from u in universidades
                                                  select new
                                                  {
                                                      UniversidadID = u.UniversidadID,
                                                      CarreraID = u.Carreras[0].CarreraID,
                                                      Universidad = u.NombreUniversidad,
                                                      Carrera = u.Carreras[0].NombreCarrera,
                                                      Direccion = u.Direccion,
                                                      isSelected = u.UniversidadID.ToString() + ',' + u.Carreras[0].CarreraID.ToString(),
                                                  });

                        // Se convierte el objeto anonimo a un DataTable
                        // esto se hace para que tengo un tipo y se puede aplicar el Sorting
                        DataTable dt = new DataTable();
                        dt.Columns.Add("UniversidadID");
                        dt.Columns.Add("CarreraID");
                        dt.Columns.Add("Universidad");
                        dt.Columns.Add("Carrera"); ;
                        dt.Columns.Add("Direccion");
                        dt.Columns.Add("isSelected");
                        foreach (var item in carreraUniversidad)
                        {
                            DataRow row = dt.NewRow();
                            row["UniversidadID"] = item.UniversidadID;
                            row["CarreraID"] = item.CarreraID;
                            row["Universidad"] = item.Universidad;
                            row["Carrera"] = item.Carrera;
                            row["Direccion"] = item.Direccion;
                            row["isSelected"] = item.isSelected;
                            dt.Rows.Add(row);
                        }
                        DtCarreraUniversidad = dt;
                        LoadCarreraUniversidad(DtCarreraUniversidad);
                    }
                    else
                    {
                        redirector.GoToLoginPage(false);
                    }
                }
            }
            catch (Exception ex) 
            {
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        private void LoadCarreraUniversidad(DataTable dtCarreraUniversidad) 
        {
            gvCarreras.DataSource = dtCarreraUniversidad;
            gvCarreras.DataBind();
        }

        private int[] GetAreasConocimientoAlumno() 
        {
            
                ArrayList arrAreaConocimiento = new ArrayList();
                int[] areasConocimiento;
                Alumno alumno = userSession.CurrentAlumno;
                Escuela escuela = userSession.CurrentEscuela;
                CicloEscolar cicloEscolar = userSession.CurrentCicloEscolar;
                GrupoCicloEscolar grupoCicloEscolar = userSession.CurrentGrupoCicloEscolar;
                Contrato contrato = userSession.Contrato;

                ContratoCtrl contratoCtrl = new ContratoCtrl();
                List<PruebaContrato> pruebas = contratoCtrl.RetrievePruebasAsignadoContrato(dctx, contrato, new CicloContrato { CicloEscolar = grupoCicloEscolar.CicloEscolar });
                PruebaContrato pruebaPivoteContrato = pruebas.FirstOrDefault(item => item.TipoPruebaContrato == ETipoPruebaContrato.Pivote);

                var pruebaDimanica = (PruebaDinamica)pruebaPivoteContrato.Prueba;

                ExpedienteEscolarCtrl expCtrl = new ExpedienteEscolarCtrl();

                List<InteresAspirante> interesAspirante = expCtrl.RetrieveInteresesAspirante(dctx, alumno, pruebaDimanica).Distinct().ToList();

                int arreglo = 0;
                foreach (InteresAspirante clas in interesAspirante)
                {
                    if (arrAreaConocimiento.IndexOf(clas.clasificador.ClasificadorID) == -1)
                    {
                        arrAreaConocimiento.Add(clas.clasificador.ClasificadorID);
                        arreglo += 1;
                    }
                }

                int[] ret = (int[])arrAreaConocimiento.ToArray(typeof(int));

                return ret;
            
        }

        protected void gvCarreras_Sorting(object sender, GridViewSortEventArgs e)
        {
            var dtSortTable = DtCarreraUniversidad;
            if (dtSortTable != null) 
            {
                DataView dvSortedView = new DataView(dtSortTable);
                dvSortedView.Sort = e.SortExpression + " " + getSortDirectionString();
                gvCarreras.DataSource = dvSortedView;
                gvCarreras.DataBind();
            }
        }

        private string getSortDirectionString() 
        {
            switch (GridViewSortDirection)
            {
                case "ASC":
                    GridViewSortDirection = "DESC";
                    break;

                case "DESC":
                    GridViewSortDirection = "ASC";
                    break;
            }

            return GridViewSortDirection;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                alumno = userSession.CurrentAlumno;
                var student = new Alumno();
                student = (Alumno)alumno.Clone();
                student.CarreraSeleccionada = true;
                student.CorreoConfirmado = false;
                alumnoCtrl.Update(dctx, student, alumno);

                // UniversidadCarreraAspirante
                DetalleExpedienteCtrl dExpCtrl = new DetalleExpedienteCtrl();
                ExpedienteEscolar expEsc = dExpCtrl.RetrieveSimple(dctx, new ExpedienteEscolar() { Alumno = alumno });
                UniversidadCarreraAspiranteCtrl unCarAsp = new UniversidadCarreraAspiranteCtrl(null);
                // de los check seleccionado cuando sean iguales al de la carrera recuperar id

                string idCarrera = string.Empty, idUniversidad = string.Empty;
                string[] separator;
                
                // Recorre el grid para que la ListaSeleccionados obtenga los id's
                recorreGridView();

                if (ListaSeleccionados.Count() == 0)
                {
                    redirector.GoToHomePage(false);
                }
                else
                {
                    foreach (var item in ListaSeleccionados)
                    {

                        separator = item.Split(',');
                        idUniversidad = separator[0].ToString();
                        idCarrera = separator[1].ToString();

                        UniversidadCarrera uni = new UniversidadCarrera();
                        uni.UniversidadID = Convert.ToInt64(idUniversidad);
                        uni.CarreraID = Convert.ToInt64(idCarrera);

                        EscuelaCtrl ctrl = new EscuelaCtrl();

                        DataSet ds = ctrl.RetrieveUniversidadCarrera(dctx, uni);
                        if (ds.Tables[0].Rows.Count > 0)
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                var ccc = ctrl.DataRowToUniversidadCarrera(ds.Tables[0].Rows[i]);
                                unCarAsp.Insert(new UniversidadCarreraAspirante() { ExpedienteEscolarID = expEsc.ExpedienteEscolarID, UniversidadCarreraID = ccc.UniversidadCarreraID });
                            }
                    }
                    redirector.GoToHomePage(false);
                }
            }
            catch (Exception ex) 
            {
                LoggerHlp.Default.Error(this, ex);
            }
            
        }

        private void recorreGridView() 
        {
            List<string> checkedProd = (from item in gvCarreras.Rows.Cast<GridViewRow>()
                                        let check = (CheckBox)item.FindControl("chkSeleccionado")
                                        where check.Checked
                                        select Convert.ToString(gvCarreras.DataKeys[item.RowIndex].Value)).ToList();
            
            // se recupera de session la lista de seleccionados previamente
            List<string> productsIdSel = HttpContext.Current.Session["ProdSelection"] as List<string>;

            if (productsIdSel == null)
                productsIdSel = new List<string>();

            
            // se cruzan todos los registros de la pagina actual del gridview con la lista de seleccionados,
            // si algun item de esa pagina fue marcado previamente no se devuelve
            productsIdSel = (from item in productsIdSel
                             join item2 in gvCarreras.Rows.Cast<GridViewRow>()
                                on item equals Convert.ToString(gvCarreras.DataKeys[item2.RowIndex].Value) into g
                             where !g.Any()
                             select item).ToList();
            
            // se agregan los seleccionados
            productsIdSel.AddRange(checkedProd);

            HttpContext.Current.Session["ProdSelection"] = productsIdSel;
            ListaSeleccionados = productsIdSel;
        }

        protected void gvCarreras_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            KeppSelection((GridView)sender);
            gvCarreras.PageIndex = e.NewPageIndex;
            LoadCarreraUniversidad(DtCarreraUniversidad);
        }

        protected void gvCarreras_PageIndexChanged(object sender, EventArgs e)
        {
            RestoreSelection((GridView)sender);
        }

        public  void KeppSelection(GridView gv) 
        {
            List<string> checkedProd = (from item in gv.Rows.Cast<GridViewRow>()
                                     let check = (CheckBox)item.FindControl("chkSeleccionado")
                                     where check.Checked
                                      select Convert.ToString(gv.DataKeys[item.RowIndex].Value)).ToList();
            
            // se recupera de session la lista de seleccionados previamente
            List<string> productsIdSel = HttpContext.Current.Session["ProdSelection"] as List<string>;

            if (productsIdSel == null)
                productsIdSel = new List<string>();

            
            // se cruzan todos los registros de la pagina actual del gridview con la lista de seleccionados,
            // si algun item de esa pagina fue marcado previamente no se devuelve
            productsIdSel = (from item in productsIdSel
                             join item2 in gv.Rows.Cast<GridViewRow>()
                                on item equals Convert.ToString(gv.DataKeys[item2.RowIndex].Value) into g
                             where !g.Any()
                             select item).ToList();
            
            // se agregan los seleccionados
            productsIdSel.AddRange(checkedProd);
            
            HttpContext.Current.Session["ProdSelection"] = productsIdSel;
            ListaSeleccionados = productsIdSel;
        }
        public  void RestoreSelection(GridView grid)
        {
            List<string> productsIdSel = HttpContext.Current.Session["ProdSelection"] as List<string>;
            if (productsIdSel == null)
                return;
            
            // se comparan los registros de la pagina del grid con los recuperados de la Session
            // los coincidentes se devuelven para ser seleccionados
            List<GridViewRow> result = (from item in grid.Rows.Cast<GridViewRow>()
                                        join item2 in productsIdSel
                                        on Convert.ToString(grid.DataKeys[item.RowIndex].Value) equals item2 into g
                                        where g.Any()
                                        select item).ToList();
            
            // se recorre cada item para marcarlo
            result.ForEach(x => ((CheckBox)x.FindControl("chkSeleccionado")).Checked = true);
        }
    }
}