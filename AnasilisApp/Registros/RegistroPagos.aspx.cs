using BLL;
using Entidades;
using Extensores;
using Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AnasilisApp.Registros
{
    public partial class RegistroPagos : System.Web.UI.Page
    {
        readonly string KeyViewState = "Pagos";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FechaTextBox.Text = DateTime.Now.ToFormatDate();
                ViewState[KeyViewState] = new Pagos();
            }
        }
        public void Limpiar()
        {
            PagosIdTextBox.Text = "0";
            PacienteTextBox.Text = string.Empty;
            BalanceTextBox.Text = string.Empty;
            ViewState[KeyViewState] = new Pagos();
            AnalisisDropDownList.Items.Clear();
            this.BindGrid();
        }
        private void LlenarCombo()
        {
            RepositorioAnalisis repositorioAnalisis = new RepositorioAnalisis();
            AnalisisDropDownList.Items.Clear();
            int PacienteId = PacienteTextBox.Text.ToInt();
            List<Analisis> ListaAnalisis = repositorioAnalisis.GetList(x => x.PacienteID == PacienteId && x.Balance > 0);
            AnalisisDropDownList.DataSource = ListaAnalisis;
            AnalisisDropDownList.DataTextField = "AnalisisID";
            AnalisisDropDownList.DataValueField = "AnalisisID";
            AnalisisDropDownList.DataBind();
        }
        private void BindGrid()
        {
            Pagos Pagos = ViewState[KeyViewState].ToPago();
            DetalleGridView.DataSource = Pagos.DetallesPagos;
            DetalleGridView.DataBind();
        }
        protected void NuevoButton_Click(object sender, EventArgs e)
        {
            Limpiar();
        }
        protected void GuadarButton_Click(object sender, EventArgs e)
        {
            if (!Validar())
                return;
            bool paso = false;
            RepositorioPago repositorio = new RepositorioPago();
            Pagos Pagos = LlenaClase();
            TipoTitulo tipoTitulo = TipoTitulo.OperacionFallida;
            TiposMensajes tiposMensajes = TiposMensajes.RegistroNoGuardado;
            IconType iconType = IconType.error;

            if (Pagos.PagosID == 0)
                paso = repositorio.Guardar(Pagos);
            else
            {
                if (ExisteEnLaBaseDeDatos())
                {
                    Utils.ToastSweet(this, IconType.info, TiposMensajes.RegistroNoEncontrado);
                    return;
                }
                paso = repositorio.Modificar(Pagos);
            }
            if (paso)
            {
                Limpiar();
                tipoTitulo = TipoTitulo.OperacionExitosa;
                tiposMensajes = TiposMensajes.RegistroGuardado;
                iconType = IconType.success;
            }
            repositorio.Dispose();
            Utils.Alerta(this, tipoTitulo, tiposMensajes, iconType);
        }
        protected void BuscarButton_Click(object sender, EventArgs e)
        {
            RepositorioPago repositorio = new RepositorioPago();
            Pagos Pagos = repositorio.Buscar(PagosIdTextBox.Text.ToInt());
            if (!Pagos.EsNulo())
            {
                Limpiar();
                LlenarCampos(Pagos);
            }
            else
                Utils.ToastSweet(this, IconType.info, TiposMensajes.RegistroNoEncontrado);
            repositorio.Dispose();
        }
        protected void EliminarButton_Click(object sender, EventArgs e)
        {
            RepositorioPago repositorio = new RepositorioPago();
            int id = PagosIdTextBox.Text.ToInt();
            if (ExisteEnLaBaseDeDatos())
            {
                Utils.Alerta(this, TipoTitulo.OperacionFallida, TiposMensajes.RegistroInexistente, IconType.error);
                return;
            }
            else
            {
                if (repositorio.Eliminar(id))
                {
                    Utils.Alerta(this, TipoTitulo.OperacionExitosa, TiposMensajes.RegistroEliminado, IconType.success);
                    Limpiar();
                }
            }
            repositorio.Dispose();

        }
        private void LlenarCampos(Pagos pagos)
        {
            PagosIdTextBox.Text = pagos.PagosID.ToString();
            PacienteTextBox.Text = pagos.PacienteID.ToString();
            ViewState[KeyViewState] = pagos;
            this.BindGrid();
        }

        private Pagos LlenaClase()
        {
            Pagos Pagos = ViewState[KeyViewState].ToPago();
            Pagos.PagosID = PagosIdTextBox.Text.ToInt();
            Pagos.PacienteID = PacienteTextBox.Text.ToInt();
            Pagos.FechaRegistro = FechaTextBox.Text.ToDatetime();
            return Pagos;
        }
        protected void AgregarPagoButton_Click(object sender, EventArgs e)
        {
            if (MontoPagarTextBox.Text.ToDecimal() <= 0)
                return;
            if (!SumarTotalPagos())
                return;
            Pagos Pago = ViewState[KeyViewState].ToPago();
            Pago.AgregarDetalle(0, Pago.PagosID, AnalisisDropDownList.SelectedValue.ToInt(), MontoPagarTextBox.Text.ToDecimal(),"Prueba");
            ViewState[KeyViewState] = Pago;
            this.BindGrid();
            MontoPagarTextBox.Text = string.Empty;
        }
        protected void RemoverDetalleClick_Click(object sender, EventArgs e)
        {
            Pagos Pagos = ViewState[KeyViewState].ToPago();
            GridViewRow row = (sender as Button).NamingContainer as GridViewRow;
            Pagos.RemoverDetalle(row.RowIndex);
            ViewState[KeyViewState] = Pagos;
            this.BindGrid();
        }
        private bool SumarTotalPagos()
        {
            bool paso = false;
            Pagos Pagos = ViewState[KeyViewState].ToPago();
            RepositorioAnalisis repositorio = new RepositorioAnalisis();
            Analisis analisis = repositorio.Buscar(AnalisisDropDownList.SelectedValue.ToInt());
            decimal Total = 0;
            Pagos.DetallesPagos.ForEach(x => Total += x.Monto);
            Total += MontoPagarTextBox.Text.ToDecimal();
            paso = Total <= analisis.Monto ? true : false;
            repositorio.Dispose();
            return paso;
        }
        private bool Validar()
        {
            bool paso = true;
            if (AnalisisDropDownList.Items.Count == 0)
                paso = false;
            if (ViewState[KeyViewState].ToPago().DetallesPagos.Count() == 0)
                paso = false;
            return paso;
        }
        private bool ExisteEnLaBaseDeDatos()
        {
            RepositorioPago repositorio = new RepositorioPago();
            Pagos pagos = repositorio.Buscar(PagosIdTextBox.Text.ToInt());
            repositorio.Dispose();
            return pagos.EsNulo();
        }
        protected void DetalleGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Pagos Pago = ViewState[KeyViewState].ToPago();
            DetalleGridView.DataSource = Pago.DetallesPagos;
            DetalleGridView.PageIndex = e.NewPageIndex;
            DetalleGridView.DataBind();
        }
        protected void BuscarPaciente_Click(object sender, EventArgs e)
        {
            RepositorioBase<Pacientes> repositorioBase = new RepositorioBase<Pacientes>();
            if(!repositorioBase.Buscar(PacienteTextBox.Text.ToInt()).EsNulo())
            {
                LlenarCombo();
                AnalisisDropDownList_SelectedIndexChanged(null, null);
            }
        }
        protected void AnalisisDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            RepositorioAnalisis repositorio = new RepositorioAnalisis();
            List<Analisis> ListaAnalisis = repositorio.GetList(x=>true);
            int AnalisisId = AnalisisDropDownList.SelectedValue.ToInt();
            var Analisis = ListaAnalisis.Find(x => x.AnalisisID == AnalisisId);
            BalanceTextBox.Text = Analisis.Balance.ToString();
        }
    }
}