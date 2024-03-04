using POV.CentroEducativo.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalUniversidad.Controls
{
    public partial class GridViewPager : System.Web.UI.UserControl
    {
        private GridView _gridView;
        private string sessionName;
        public string SessionName
        {
            get { throw new NotImplementedException(); }
            set { sessionName = value; }
        }
        public string DataSourceType { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            Control c = Parent;
            while (c != null)
            {
                if (c is GridView)
                {
                    _gridView = (GridView)c;
                    break;
                }
                c = c.Parent;
            }
        }
        protected void DropDownListPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_gridView == null)
            {
                return;
            }
            DropDownList dropdownlistpagersize = (DropDownList)sender;
            _gridView.PageSize = Convert.ToInt32(dropdownlistpagersize.SelectedValue, CultureInfo.CurrentCulture);
            int pageindex = _gridView.PageIndex;
            _gridView.DataSource = GetSession();
            _gridView.DataBind();
            _gridView.BottomPagerRow.Visible = true;

            if (_gridView.PageIndex != pageindex)
            {
                //Si el índice de la página cambió, significa que la página anterior no era válida y se ha ajustado. Vuelva a enlazar al control de relleno con la página ajustada
                _gridView.DataSource = GetSession();
                _gridView.DataBind();
                _gridView.BottomPagerRow.Visible = true;
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (_gridView != null)
            {
                LabelNumberOfPages.Text = _gridView.PageCount.ToString(CultureInfo.CurrentCulture);
                LabelPage.Text += (_gridView.PageIndex + 1).ToString(CultureInfo.CurrentCulture);
                DropDownListPageSize.SelectedValue = _gridView.PageSize.ToString(CultureInfo.CurrentCulture);
                ImageButtonFirst.Enabled = ImageButtonPrev.Enabled = (_gridView.PageIndex != 0);
                ImageButtonNext.Enabled = ImageButtonLast.Enabled = (_gridView.PageIndex < (_gridView.PageCount - 1));
            }
        }
        protected void Next(object sender, EventArgs e)
        {
            if (_gridView == null)
            {
                return;
            }
            int pageIndex = _gridView.PageIndex;
            int nextPage = pageIndex + 1;
            int pageCount = _gridView.PageCount;
            if (nextPage > pageCount)
                nextPage = pageIndex;
            _gridView.PageIndex = nextPage;
            _gridView.DataSource = GetSession();
            _gridView.DataBind();
            _gridView.BottomPagerRow.Visible = true;
        }
        protected void Previous(object sender, EventArgs e)
        {
            if (_gridView == null)
            {
                return;
            }
            int pageIndex = _gridView.PageIndex;
            int previousPage = pageIndex - 1;
            int pageCount = _gridView.PageCount;

            if (previousPage < 0)
                previousPage = 0;

            _gridView.PageIndex = previousPage;
            _gridView.DataSource = GetSession();
            _gridView.DataBind();
            _gridView.BottomPagerRow.Visible = true;
        }
        protected void First(object sender, EventArgs e)
        {
            if (_gridView == null)
            {
                return;
            }

            _gridView.PageIndex = 0;
            _gridView.DataSource = GetSession();
            _gridView.DataBind();
            _gridView.BottomPagerRow.Visible = true;
        }
        protected void Last(object sender, EventArgs e)
        {
            if (_gridView == null)
            {
                return;
            }

            _gridView.PageIndex = _gridView.PageCount - 1;
            _gridView.DataSource = GetSession();
            _gridView.DataBind();
            _gridView.BottomPagerRow.Visible = true;
        }
        protected object  GetSession()
        {
            if (DataSourceType.CompareTo("DataTable")==0)
                return Session[sessionName] as DataTable;
            if (DataSourceType.CompareTo("DataSet") == 0)
                return Session[sessionName] as DataSet;
            if (DataSourceType.CompareTo("List<Carrera>") == 0)
                return Session[sessionName] as List<Carrera>;
            if (DataSourceType.CompareTo("List<InfoAlumnoUsuario>") == 0)
                return Session[sessionName] as List<InfoAlumnoUsuario>;
            
            throw new NotSupportedException("No se encontró el origen de datos");
        }
    }

}