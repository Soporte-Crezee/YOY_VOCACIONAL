﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Web.PortalOperaciones.Helper;
using POV.Web.PortalOperaciones.AppCode.Page;

namespace POV.Web.PortalOperaciones.Perfiles
{
    public partial class EditarPermisos : PageBase
    {
        PerfilPermisoCtrl perfilPermisoCtrl;
        PerfilCtrl perfilCtrl;
        private IDataContext dctx = ConnectionHlp.Default.Connection;

        #region *** propiedades de clase ***
        private Perfil LastObject
        {
            set { Session["lastPerfilCatalogo"] = value; }
            get { return Session["lastPerfilCatalogo"] != null ? Session["lastPerfilCatalogo"] as Perfil : null; }
        }
        #endregion

        public EditarPermisos()
        {
            perfilPermisoCtrl = new PerfilPermisoCtrl();
            perfilCtrl = new PerfilCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (LastObject != null)
                {
                    DataSet ds = perfilCtrl.Retrieve(dctx, LastObject);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        LastObject = perfilCtrl.LastDataRowToPerfil(ds);

                        LastObject = perfilCtrl.RetrieveComplete(dctx, LastObject.PerfilID.Value);

                        DoOpen();

                        FillBack();
                    }
                    else
                    {
                        txtRedirect.Value = "BuscarUsuarios.aspx";
                        ShowMessage("No se ha seleccionado un elemento", MessageType.Error);
                    }
                }
                else
                {
                    txtRedirect.Value = "BuscarUsuarios.aspx";
                    ShowMessage("No se ha seleccionado un elemento", MessageType.Error);
                }
            }

        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            DoUpdate();
        }

        #region Asignar permisos
        protected void btnAgregarPermisos_Click(object sender, EventArgs e)
        {
            if (tvPermisosDisponibles.Nodes.Count > 0)
            {
                tvPermisosDisponibles.Nodes[0].NavigateUrl = "javascript:void(0)";
                tvPermisosAsignados.Nodes.Add(tvPermisosDisponibles.Nodes[0]);
                btnAgregarPermisos_Click(sender, e);

            }
        }
        protected void btnAgrearPermiso_Click(object sender, EventArgs e)
        {
            if (this.tvPermisosDisponibles.Nodes.Count > 0)
                this.AgregarNodosSelecionados();
        }
        public void AgregarNodosSelecionados()
        {


            for (int i = 0; i < this.tvPermisosDisponibles.Nodes.Count; i++)
            {
                for (int k = 0; k < this.tvPermisosDisponibles.Nodes[i].ChildNodes.Count; k++)
                {
                    bool encontrado = false;
                    if (tvPermisosDisponibles.Nodes[i].ChildNodes[k].Checked == true)
                    {
                        if (this.tvPermisosAsignados.Nodes.Count > 0)
                        {
                            //recorro permisos asignados.
                            for (int j = 0; j < this.tvPermisosAsignados.Nodes.Count; j++)
                            {
                                tvPermisosAsignados.Nodes[j].NavigateUrl = "javascript:void(0)";

                                //pregunta si nodo es igual alngun node permisos asiignados
                                if (this.tvPermisosDisponibles.Nodes[i].Text.Equals(this.tvPermisosAsignados.Nodes[j].Text))
                                {
                                    //recorro los hijos de permiso y se los asigno a permisos asignados
                                    for (int n = 0; n < this.tvPermisosDisponibles.Nodes[i].ChildNodes.Count; n++)
                                    {
                                        tvPermisosAsignados.Nodes[j].ChildNodes[n].NavigateUrl = "javascript:void(0)";

                                        if (this.tvPermisosDisponibles.Nodes[i].ChildNodes[n].Checked == true)
                                        {
                                            this.tvPermisosAsignados.Nodes[j].ChildNodes.Add(this.tvPermisosDisponibles.Nodes[i].ChildNodes[n]);
                                            n--;
                                            k--;
                                            if (this.tvPermisosDisponibles.Nodes[i].ChildNodes.Count == 0)
                                            {
                                                this.tvPermisosDisponibles.Nodes.Remove(this.tvPermisosDisponibles.Nodes[i]);

                                                break;
                                            }
                                        }
                                    }
                                    encontrado = true;
                                    break;
                                }
                            }
                            if (encontrado == false)
                            {
                                TreeNode newNode = new TreeNode();
                                newNode.Text = this.tvPermisosDisponibles.Nodes[i].Text;
                                newNode.Value = this.tvPermisosDisponibles.Nodes[i].Value;
                                for (int l = 0; l < tvPermisosDisponibles.Nodes[i].ChildNodes.Count; l++)
                                {
                                    if (tvPermisosDisponibles.Nodes[i].ChildNodes[l].Checked == true)
                                    {
                                        newNode.ChildNodes.Add(tvPermisosDisponibles.Nodes[i].ChildNodes[l]);
                                        l--;
                                    }

                                }

                                if (this.tvPermisosDisponibles.Nodes[i].ChildNodes.Count == 0)
                                {
                                    this.tvPermisosDisponibles.Nodes.Remove(this.tvPermisosDisponibles.Nodes[i]);

                                }
                                newNode.NavigateUrl = "javascript:void(0)";
                                this.tvPermisosAsignados.Nodes.Add(newNode);
                                i--;

                            }
                        }
                        else
                        {
                            TreeNode newNode = new TreeNode();
                            newNode.Text = this.tvPermisosDisponibles.Nodes[i].Text;
                            newNode.Value = this.tvPermisosDisponibles.Nodes[i].Value;
                            for (int l = 0; l < tvPermisosDisponibles.Nodes[i].ChildNodes.Count; l++)
                            {
                                if (tvPermisosDisponibles.Nodes[i].ChildNodes[l].Checked == true)
                                {
                                    newNode.ChildNodes.Add(tvPermisosDisponibles.Nodes[i].ChildNodes[l]);
                                    l--;
                                }

                            }
                            if (this.tvPermisosDisponibles.Nodes[i].ChildNodes.Count == 0)
                            {
                                this.tvPermisosDisponibles.Nodes.Remove(this.tvPermisosDisponibles.Nodes[i]);

                            }

                            newNode.NavigateUrl = "javascript:void(0)";
                            this.tvPermisosAsignados.Nodes.Add(newNode);

                            i--;
                        }
                    }
                    else
                    {
                        if (this.AgregarNodosHijosSelecionados(this.tvPermisosDisponibles.Nodes[i]) == true)
                            i--;
                    }

                    if (i < 0 || i == this.tvPermisosDisponibles.Nodes.Count)
                        break;

                }

            }
        }
        public bool AgregarNodosHijosSelecionados(System.Web.UI.WebControls.TreeNode node)
        {
            bool encontrado = false;
            bool regreso = false;
            //recorre el nodo para encontrar hijos selecionados
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                node.ChildNodes[i].NavigateUrl = "javascript:void(0)";

                //pregunta si hay hijos selecionados 
                if (node.ChildNodes[i].Checked == true)
                {
                    //Preguntar si el otro arbol tiene nodos
                    if (this.tvPermisosAsignados.Nodes.Count > 0)
                    {
                        //Recorreo el arbol permisos asignados para buscar Nodos y agregar hijos
                        for (int j = 0; j < this.tvPermisosAsignados.Nodes.Count; j++)
                        {
                            //pregunta si nodo es igual alngun node permisos asiignados
                            if (node.Text.Equals(this.tvPermisosAsignados.Nodes[j].Text))
                            {
                                if (node.ChildNodes[i].Text.Equals("TODOS"))
                                {

                                    for (int n = 0; n < this.tvPermisosAsignados.Nodes[j].ChildNodes.Count; n++)
                                    {
                                        if (this.tvPermisosAsignados.Nodes[j].ChildNodes[n].Text.Equals(node.ChildNodes[i].Text))
                                        {
                                            this.tvPermisosAsignados.Nodes[j].ChildNodes.Remove(this.tvPermisosAsignados.Nodes[j].ChildNodes[n]);
                                            break;
                                        }
                                    }
                                    this.tvPermisosAsignados.Nodes[j].ChildNodes.Add(node.ChildNodes[i]);
                                }
                                else
                                {
                                    this.tvPermisosAsignados.Nodes[j].ChildNodes.Add(node.ChildNodes[i]);
                                    encontrado = true;

                                }
                                i--;
                                if (node.ChildNodes.Count == 0)
                                {
                                    this.tvPermisosDisponibles.Nodes.Remove(node);
                                    regreso = true;
                                }
                                break;
                            }
                        }
                        if (encontrado == false)
                        {
                            System.Web.UI.WebControls.TreeNode copia = new System.Web.UI.WebControls.TreeNode();
                            copia.Value = node.Value;
                            copia.Text = node.Text;
                            copia.ChildNodes.Add(node.ChildNodes[i]);
                            this.tvPermisosAsignados.Nodes.Add(copia);
                            i--;
                            if (node.ChildNodes.Count == 0)
                            {
                                this.tvPermisosDisponibles.Nodes.Remove(node);
                                regreso = true;
                            }

                        }
                    }
                    else
                    {
                        System.Web.UI.WebControls.TreeNode copia = new System.Web.UI.WebControls.TreeNode();
                        copia.Value = node.Value;
                        copia.Text = node.Text;
                        copia.ChildNodes.Add(node.ChildNodes[i]);
                        this.tvPermisosAsignados.Nodes.Add(copia);
                        if (node.ChildNodes.Count == 0)
                        {
                            this.tvPermisosDisponibles.Nodes.Remove(node);
                            regreso = true;
                        }
                        i--;
                    }
                }

            }
            return regreso;

        }
        #endregion

        #region Retirar permisos
        protected void btnQuitarPermisos_Click(object sender, EventArgs e)
        {
            if (this.tvPermisosAsignados.Nodes.Count > 0)
            {
                this.tvPermisosAsignados.Nodes[0].NavigateUrl = "javascript:void(0)";
                tvPermisosDisponibles.Nodes.Add(this.tvPermisosAsignados.Nodes[0]);
                this.btnQuitarPermisos_Click(sender, e);
            }
        }
        protected void btnQuitarPermiso_Click(object sender, EventArgs e)
        {
            if (this.tvPermisosAsignados.Nodes.Count > 0)
                this.QuitarNodosSelecionados();
        }
        private void QuitarNodosSelecionados()
        {

            for (int i = 0; i < this.tvPermisosAsignados.Nodes.Count; i++)
            {
                for (int k = 0; k < this.tvPermisosAsignados.Nodes[i].ChildNodes.Count; k++)
                {
                    bool encontrado = false;
                    if (this.tvPermisosAsignados.Nodes[i].ChildNodes[k].Checked == true)
                    {
                        if (this.tvPermisosDisponibles.Nodes.Count > 0)
                        {
                            //recorro permisos asignados.
                            for (int j = 0; j < this.tvPermisosDisponibles.Nodes.Count; j++)
                            {

                                tvPermisosDisponibles.Nodes[j].NavigateUrl = "javascript:void(0)";

                                //pregunta si nodo es igual alngun node permisos asiignados
                                if (this.tvPermisosAsignados.Nodes[i].Text.Equals(this.tvPermisosDisponibles.Nodes[j].Text))
                                {
                                    //recorro los hijos de permiso y se los asigno a permisos asignados
                                    for (int n = 0; n < this.tvPermisosAsignados.Nodes[i].ChildNodes.Count; n++)
                                    {

                                        tvPermisosDisponibles.Nodes[j].ChildNodes[n].NavigateUrl = "javascript:void(0)";

                                        if (this.tvPermisosAsignados.Nodes[i].ChildNodes[n].Checked == true)
                                        {
                                            this.tvPermisosDisponibles.Nodes[j].ChildNodes.Add(this.tvPermisosAsignados.Nodes[i].ChildNodes[n]);
                                            n--;
                                            k--;
                                            if (this.tvPermisosAsignados.Nodes[i].ChildNodes.Count == 0)
                                            {
                                                this.tvPermisosAsignados.Nodes.Remove(this.tvPermisosAsignados.Nodes[i]);
                                                i--;
                                                break;
                                            }
                                        }
                                    }
                                    encontrado = true;
                                    break;
                                }
                            }
                            if (encontrado == false)
                            {
                                TreeNode newNode = new TreeNode();
                                newNode.Text = this.tvPermisosAsignados.Nodes[i].Text;
                                newNode.Value = this.tvPermisosAsignados.Nodes[i].Value;
                                for (int l = 0; l < tvPermisosAsignados.Nodes[i].ChildNodes.Count; l++)
                                {
                                    if (tvPermisosAsignados.Nodes[i].ChildNodes[l].Checked == true)
                                    {
                                        newNode.ChildNodes.Add(tvPermisosAsignados.Nodes[i].ChildNodes[l]);
                                        l--;
                                    }

                                }
                                if (this.tvPermisosAsignados.Nodes[i].ChildNodes.Count == 0)
                                {
                                    this.tvPermisosAsignados.Nodes.Remove(this.tvPermisosAsignados.Nodes[i]);

                                }
                                newNode.NavigateUrl = "javascript:void(0)";
                                this.tvPermisosDisponibles.Nodes.Add(newNode);
                                i--;

                            }
                        }
                        else
                        {
                            TreeNode newNode = new TreeNode();
                            newNode.Text = this.tvPermisosAsignados.Nodes[i].Text;
                            newNode.Value = this.tvPermisosAsignados.Nodes[i].Value;
                            for (int l = 0; l < tvPermisosAsignados.Nodes[i].ChildNodes.Count; l++)
                            {
                                if (tvPermisosAsignados.Nodes[i].ChildNodes[l].Checked == true)
                                {
                                    newNode.ChildNodes.Add(tvPermisosAsignados.Nodes[i].ChildNodes[l]);
                                    l--;
                                }

                            }
                            if (this.tvPermisosAsignados.Nodes[i].ChildNodes.Count == 0)
                            {
                                this.tvPermisosAsignados.Nodes.Remove(this.tvPermisosAsignados.Nodes[i]);

                            }
                            newNode.NavigateUrl = "javascript:void(0)";
                            this.tvPermisosDisponibles.Nodes.Add(newNode);
                            i--;
                        }
                    }
                    else
                    {
                        if (QuitarNodosHijosSelecionados(this.tvPermisosAsignados.Nodes[i]) == true)
                            i--;
                    }

                    if (i < 0 || i == this.tvPermisosAsignados.Nodes.Count)
                        break;
                }

            }

        }
        private bool QuitarNodosHijosSelecionados(System.Web.UI.WebControls.TreeNode node)
        {
            bool encontrado = false;
            bool regreso = false;
            //recorre el nodo para encontrar hijos selecionados
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                node.ChildNodes[i].NavigateUrl = "javascript:void(0)";

                //pregunta si hay hijos selecionados 
                if (node.ChildNodes[i].Checked == true)
                {
                    //Preguntar si el otro arbol tiene nodos
                    if (this.tvPermisosDisponibles.Nodes.Count > 0)
                    {
                        //Recorreo el arbol permisos asignados para buscar Nodos y agregar hijos
                        for (int j = 0; j < this.tvPermisosDisponibles.Nodes.Count; j++)
                        {
                            //pregunta si nodo es igual alngun node permisos asiignados
                            if (node.Text.Equals(this.tvPermisosDisponibles.Nodes[j].Text))
                            {
                                if (node.ChildNodes[i].Text.Equals("TODOS"))
                                {

                                    for (int n = 0; n < this.tvPermisosDisponibles.Nodes[j].ChildNodes.Count; n++)
                                    {
                                        if (this.tvPermisosDisponibles.Nodes[j].ChildNodes[n].Text.Equals(node.ChildNodes[i].Text))
                                        {
                                            this.tvPermisosDisponibles.Nodes[j].ChildNodes.Remove(this.tvPermisosDisponibles.Nodes[j].ChildNodes[n]);
                                            break;
                                        }
                                    }
                                }
                                this.tvPermisosDisponibles.Nodes[j].ChildNodes.Add(node.ChildNodes[i]);
                                encontrado = true;
                                i--;
                                if (node.ChildNodes.Count == 0)
                                {
                                    this.tvPermisosAsignados.Nodes.Remove(node);
                                    regreso = true;
                                }
                                break;
                            }
                        }
                        if (encontrado == false)
                        {
                            System.Web.UI.WebControls.TreeNode copia = new System.Web.UI.WebControls.TreeNode();
                            copia.Value = node.Value;
                            copia.Text = node.Text;
                            copia.ChildNodes.Add(node.ChildNodes[i]);
                            this.tvPermisosDisponibles.Nodes.Add(copia);
                            i--;
                            if (node.ChildNodes.Count == 0)
                            {
                                this.tvPermisosAsignados.Nodes.Remove(node);
                                regreso = true;
                            }

                        }
                    }
                    else
                    {
                        System.Web.UI.WebControls.TreeNode copia = new System.Web.UI.WebControls.TreeNode();
                        copia.Value = node.Value;
                        copia.Text = node.Text;
                        copia.ChildNodes.Add(node.ChildNodes[i]);
                        this.tvPermisosDisponibles.Nodes.Add(copia);
                        if (node.ChildNodes.Count == 0)
                        {
                            this.tvPermisosAsignados.Nodes.Remove(node);
                            regreso = true;
                        }
                        i--;
                    }
                }

            }
            return regreso;
        }
        #endregion

        #endregion

        #region *** validaciones ***

        #endregion

        #region *** Data to UserInterface ***
        private void CargarPermisos()
        {
            LimpiarPermisos();
            PermisoCtrl pes = new PermisoCtrl();
            Permiso permiso = new Permiso();
            AccionCtrl accionCtrl = new AccionCtrl();
            permiso.Aplicacion = new Aplicacion();
            permiso.Accion = new Accion();
            DataSet consulta = pes.Retrieve(dctx, permiso);
            DataTable tabla = consulta.Tables[0];
            DataRowCollection rows = tabla.Rows;
            Object[][] valor = new Object[rows.Count][];
            for (int i = 0; i < rows.Count; i++)
            {
                Object[] obj = rows[i].ItemArray;
                valor[i] = rows[i].ItemArray;
            }

            for (int i = 0; i < valor.Length; i++)
            {
                for (int j = 0; j < valor[i].Length; j++)
                {
                    if (j == 1)
                    {
                        System.Web.UI.WebControls.TreeNode aplicacion = new System.Web.UI.WebControls.TreeNode(valor[i][j + 1].ToString());
                        aplicacion.Value = valor[i][j].ToString();
                        System.Web.UI.WebControls.TreeNode accion = new System.Web.UI.WebControls.TreeNode(valor[i][j + 3].ToString());
                        accion.Value = valor[i][j - 1].ToString() + "." + valor[i][j + 2].ToString() + "." + valor[i][j + 4].ToString();
                        DataSet dsAccionTemp = accionCtrl.Retrieve(dctx, new Accion { AccionID = Convert.ToInt32(valor[i][j + 4].ToString()) });
                        Accion accionTemp = accionCtrl.LastDataRowToAccion(dsAccionTemp);
                        aplicacion.Text = accion.Text;
                        accion.Text = accionTemp.Nombre;
                        aplicacion.NavigateUrl = "javascript:void(0)";
                        accion.NavigateUrl = "javascript:void(0)";

                        if (tvPermisosDisponibles.Nodes.Count > 0)
                        {
                            bool bandera = false;

                            for (int x = 0; x < tvPermisosDisponibles.Nodes.Count; x++)
                            {

                                //if (tvPermisosDisponibles.Nodes[x].Text == valor[i][j].ToString())
                                if (tvPermisosDisponibles.Nodes[x].Text == aplicacion.Text)
                                {
                                    tvPermisosDisponibles.Nodes[x].ChildNodes.Add(accion);
                                    bandera = true;

                                }
                            }
                            if (bandera == false)
                            {
                                aplicacion.ChildNodes.Add(accion);
                                tvPermisosDisponibles.Nodes.Add(aplicacion);
                            }
                        }
                        else
                        {
                            aplicacion.ChildNodes.Add(accion);
                            tvPermisosDisponibles.Nodes.Add(aplicacion);
                        }
                    }
                }
            }
        }

        private void FillBack()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
                lnkBack.NavigateUrl = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
            else
                lnkBack.NavigateUrl = "~/Perfiles/BuscarPerfiles.aspx";
        }
        #endregion

        #region *** UserInterface to Data ***
        private List<IPrivilegio> UserInterfaceToData()
        {
            // Generar los datos en el objeto
            List<IPrivilegio> privilegios = new List<IPrivilegio>();
            privilegios = CrearPermisos(privilegios);
            return privilegios;
        }
        public List<IPrivilegio> CrearPermisos(List<IPrivilegio> privilegios)
        {
            for (int i = 0; i < tvPermisosAsignados.Nodes.Count; i++)
            {
                for (int j = 0; j < tvPermisosAsignados.Nodes[i].ChildNodes.Count; j++)
                {
                    if (!this.tvPermisosAsignados.Nodes[i].ChildNodes[j].Text.Equals("TODOS"))
                    {
                        String[] cadena;
                        cadena = tvPermisosAsignados.Nodes[i].ChildNodes[j].Value.Split('.');
                        int permisoID = int.Parse(cadena[0]);
                        int aplicacionID = int.Parse(cadena[1]);
                        int accionID = int.Parse(cadena[2]);

                        String accionDesc = tvPermisosAsignados.Nodes[i].ChildNodes[j].Text.ToString();
                        String aplicacionDesc = tvPermisosAsignados.Nodes[i].Text.ToString();

                        Permiso permiso = new Permiso()
                        {
                            PermisoID = permisoID,
                            Accion = new Accion()
                            {
                                AccionID = accionID,
                                Nombre = accionDesc
                            },
                            Aplicacion = new Aplicacion()
                            {
                                AplicacionID = aplicacionID,
                                Nombre = aplicacionDesc
                            }
                        };
                        privilegios.Add(permiso);
                    }
                }
            }
            return privilegios;
        }
        #endregion

        #region *** metodos auxiliares ***
        private void DoOpen()
        {

            this.CargarPermisos();
            this.AsignarPerfilesPermisos();
        }
        public void AsignarPerfilesPermisos()
        {
            foreach (Permiso permiso in LastObject.Privilegios)
            {
                if (permiso.TipoPrivilegio.ToString().Equals("PERMISO"))
                    BuscarPermisos((int)permiso.PrivilegioID);
            }
            AgregarNodosSelecionados();

            foreach (TreeNode node in this.tvPermisosAsignados.Nodes)
            {
                node.ChildNodes.Cast<TreeNode>().ToList<TreeNode>().ForEach(leaf => leaf.Checked = false);
            }
        }
        public void BuscarPermisos(int idpermiso)
        {
            for (int i = 0; i < this.tvPermisosDisponibles.Nodes.Count; i++)
            {
                for (int j = 0; j < this.tvPermisosDisponibles.Nodes[i].ChildNodes.Count; j++)
                {
                    try
                    {
                        String[] cadena;
                        cadena = this.tvPermisosDisponibles.Nodes[i].ChildNodes[j].Value.Split('.');
                        int id = int.Parse(cadena[0]);

                        if (id == idpermiso)
                        {
                            this.tvPermisosDisponibles.Nodes[i].ChildNodes[j].Checked = true;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error UI3400.- GestionUsuario:Falló la conversion  " + ex.Message);
                    }
                }
                if (this.tvPermisosDisponibles.Nodes[i].ChildNodes[0].Text.Equals("TODOS"))
                {
                    // Debug.WriteLine("queda todos");
                    this.tvPermisosDisponibles.Nodes.Remove(this.tvPermisosDisponibles.Nodes[i].ChildNodes[0]);

                }

            }
        }

        public void LimpiarPermisos()
        {
            this.tvPermisosDisponibles.Nodes.Clear();
            this.tvPermisosAsignados.Nodes.Clear();
        }

        private void DoUpdate()
        {
            IDataContext dctx = ConnectionHlp.Default.Connection;
            object myFirm = new object();
            try
            {
                dctx.OpenConnection(myFirm);
                dctx.BeginTransaction(myFirm);

                Perfil aux = (Perfil)LastObject.Clone();

                List<IPrivilegio> listPrivilegios = UserInterfaceToData();
                LastObject.Privilegios.Clear();
                foreach (IPrivilegio priv in listPrivilegios)
                {
                    LastObject.Privilegios.Add(priv);
                }

                perfilCtrl.UpdateComplete(dctx, LastObject, aux);

                dctx.CommitTransaction(myFirm);
                txtRedirect.Value = "BuscarPerfiles.aspx";
                ShowMessage("Los privilegios se actualizaron con exito", MessageType.Information);
                LastObject = null;
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                txtRedirect.Value = string.Empty;
                ShowMessage(ex.Message, MessageType.Error);
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }
        }
        #endregion

        #region AUTORIZACION DE LA PAGINA

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOPERFILES) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARPERFILES) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARPERFILES) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!lectura)
                redirector.GoToHomePage(true);
            if (!edit)
                redirector.GoToHomePage(true);
        }
        #endregion

        #region *****Message  Showing*****
        /// <summary>
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="message">Mensaje a desplegar</param>
        /// <param name="messageType">Tipo de mensaje</param>
        private void ShowMessage(string message, MessageType messageType)
        {
            string type = string.Empty;

            switch (messageType)
            {
                case MessageType.Error:
                    type = "1";
                    break;
                case MessageType.Information:
                    type = "3";
                    break;
                case MessageType.Warning:
                    type = "2";
                    break;
            }

            ShowMessage(message, type);
        }
        /// <summary>
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="message">Mensaje a desplegar</param>
        /// <param name="typeNotification">1: Error, 2: Advertencia, 3: Información</param>
        private void ShowMessage(string message, string typeNotification)
        {
            //Se ubican los controles que manejan el desplegado de error/advertencia/información
            if (Page.Master == null) return;
            Control m = Page.Master.FindControl("hdnLastMessage");
            Control t = Page.Master.FindControl("hdnShowMessage");

            if (m == null)
                throw new Exception("No se pudo desplegar correctamente el error.\nNo se encontró el control 'hdnLastMessage' en la MasterPage.\nEl error original es:\n" + message);
            if (t == null)
                throw new Exception("No se pudo desplegar correctamente el error.\nNo se encontró el control 'hdnShowMessage' en la MasterPage.\nEl error original es:\n" + message);

            if (m.GetType() != typeof(HiddenField) || t.GetType() != typeof(HiddenField))
                throw new Exception("No se pudo desplegar correctamente el error.\nAlguno de los controles de la MasterPage para el manejo de errores no es HiddenField.\nEl error original es:\n" + message);

            //Si el HiddenField del mensaje de error ya tiene un mensaje guardado, se da un 'enter' y se concatena el nuevo mensaje (errores acumulados)
            //En caso contrario, se pone el encabezado y se concatena el nuevo mensaje
            if (((HiddenField)m).Value != null && ((HiddenField)m).Value.Trim().CompareTo("") != 0)
                ((HiddenField)m).Value += "<br />";


            ((HiddenField)m).Value += message.Replace("\n", "<br />");
            ((HiddenField)t).Value = typeNotification;
        }
        #endregion        
    }
}