using BLL;
using Entidades;
using Extensores;
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
            FechaTextBox.Text = DateTime.Now.ToFormatDate();
            if (!Page.IsPostBack)
            {
                ViewState[KeyViewState] = new Pagos();
                LlenarCombo();
            }
        }
        public void Limpiar()
        {
            PagosIdTextBox.Text = "0";
            AnalisisDropdownList.SelectedIndex = -1;
            PacienteNombreBox.Text = string.Empty;
            BalanceTextBox.Text = string.Empty;
            ViewState[KeyViewState] = new Pagos();
            LlenarCombo();
            this.BindGrid();
        }
        private void LlenarCombo()
        {
            AnalisisDropdownList.Items.Clear();
            RepositorioAnalisis repositorioAnalisis = new RepositorioAnalisis();
            List<Analisis> lista = repositorioAnalisis.GetList(x => x.Balance > 0);
            AnalisisDropdownList.DataSource = lista;
            AnalisisDropdownList.DataValueField = "AnalisisID";
            AnalisisDropdownList.DataTextField = "AnalisisID";
            AnalisisDropdownList.DataBind();
            AnalisisDropdownList_TextChanged(null, null);
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
            TipoAlerta Alerta = TipoAlerta.ErrorAlert;
            if (Pagos.PagosID == 0)
                paso = repositorio.Guardar(Pagos);
            else
                paso = repositorio.Modificar(Pagos);
            if (paso)
            {
                Limpiar();
                Alerta = TipoAlerta.SuccessAlert;
            }
            Extensores.Extensores.Alerta(this, Alerta);
        }
        protected void BuscarButton_Click(object sender, EventArgs e)
        {
            RepositorioPago repositorio = new RepositorioPago();
            Pagos Pagos = repositorio.Buscar(PagosIdTextBox.Text.ToInt());
            if(Pagos.EsNulo())
            {
                Limpiar();
                LlenarCampos(Pagos);
            }
            else
            {
                Extensores.Extensores.Alerta(this, TipoAlerta.ErrorAlert);
            }
        }
        protected void EliminarButton_Click(object sender, EventArgs e)
        {
            RepositorioPago repositorio = new RepositorioPago();
            int id = PagosIdTextBox.Text.ToInt();
            if (ExisteEnLaBaseDeDatos())
            {
                Extensores.Extensores.Alerta(this, TipoAlerta.ErrorAlert);
                return;
            }
            else
            {
                if (repositorio.Eliminar(id))
                {
                    Extensores.Extensores.Alerta(this, TipoAlerta.SuccessAlert);
                    Limpiar();
                }
            }
            repositorio.Dispose();
        }
        private void LlenarCampos(Pagos pagos)
        {
            Limpiar();
            PagosIdTextBox.Text = pagos.PagosID.ToString();
            AnalisisDropdownList.SelectedValue = pagos.AnalisisID.ToString();
            ViewState[KeyViewState] = pagos;
            this.BindGrid();
        }

        private Pagos LlenaClase()
        {
            Pagos Pagos = ViewState[KeyViewState].ToPago();
            Pagos.PagosID = PagosIdTextBox.Text.ToInt();
            Pagos.AnalisisID = AnalisisDropdownList.SelectedValue.ToInt();
            Pagos.FechaRegistro = FechaTextBox.Text.ToDatetime();
            return Pagos;
        }
        protected void AgregarPagoButton_Click(object sender, EventArgs e)
        {
            if (MontoPagarTextBox.Text.ToDecimal() <= 0)
                return;
            if (!SumarTotalPagos())
                return;
            Pagos Pago = new Pagos();
            Pago = ViewState[KeyViewState].ToPago();
            Pago.AgregarDetalle(0, Pago.PagosID, MontoPagarTextBox.Text.ToDecimal());
            ViewState[KeyViewState] = Pago;
            this.BindGrid();
            MontoPagarTextBox.Text = string.Empty;
        }
        protected void AnalisisDropdownList_TextChanged(object sender, EventArgs e)
        {
            if(AnalisisDropdownList.Items.Count>0)
            {
                int AnalisisID = AnalisisDropdownList.SelectedValue.ToInt();
                RepositorioAnalisis repositorio = new RepositorioAnalisis();
                Analisis analisis = repositorio.Buscar(AnalisisID);
                repositorio.Dispose();
                RepositorioBase<Pacientes> repositorioPaciente = new RepositorioBase<Pacientes>();
                Pacientes paciente = repositorioPaciente.Buscar(analisis.PacienteID);
                PacienteNombreBox.Text = paciente.Nombre;
                BalanceTextBox.Text = analisis.Balance.ToString();
            }
        }
        private bool SumarTotalPagos()
        {
            bool paso = false;
            Pagos Pagos = ViewState[KeyViewState].ToPago();
            decimal Total = 0;
            Pagos.DetallesPagos.ForEach(x => Total += x.Monto);
            Total += MontoPagarTextBox.Text.ToDecimal();
            paso = Total <= BalanceTextBox.Text.ToDecimal() ? true:false;
            return paso;
        }
        private bool Validar()
        {
            bool paso = true;
            if (AnalisisDropdownList.Items.Count == 0)
                paso = false;
            if(ViewState[KeyViewState].ToPago().DetallesPagos.Count()==0)
                paso = false;
            return paso;
        }
        private bool ExisteEnLaBaseDeDatos()
        {
            RepositorioPago repositorio = new RepositorioPago();
            Pagos pagos= repositorio.Buscar(PagosIdTextBox.Text.ToInt());
            return !pagos.EsNulo();
        }
        protected void DetalleGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Pagos Pago = ViewState[KeyViewState].ToPago();
            DetalleGridView.DataSource = Pago.DetallesPagos;
            DetalleGridView.PageIndex = e.NewPageIndex;
            DetalleGridView.DataBind();
        }
    }
}