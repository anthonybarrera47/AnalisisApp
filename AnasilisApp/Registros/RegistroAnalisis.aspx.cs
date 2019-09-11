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
    public partial class RegistroAnalisis : System.Web.UI.Page
    {
        readonly string KeyViewState = "Analisis";
        protected void Page_Load(object sender, EventArgs e)
        {
            FechaTextBox.Text = DateTime.Now.ToFormatDate();
            if (!Page.IsPostBack)
            {
                ViewState[KeyViewState] = new Analisis();
                LlenarCombo();
            }
        }
        private void LlenarCombo()
        {
            TipoAnalisisDropdonwList.Items.Clear();
            PacientesDropdownList.Items.Clear();
            RepositorioBase<TipoAnalisis> repositorio = new RepositorioBase<TipoAnalisis>();
            TipoAnalisisDropdonwList.DataSource = repositorio.GetList(x => true);
            TipoAnalisisDropdonwList.DataValueField = "TipoAnalisisID";
            TipoAnalisisDropdonwList.DataTextField = "Descripcion";
            TipoAnalisisDropdonwList.DataBind();

            RepositorioBase<Pacientes> repositorioPacientes = new RepositorioBase<Pacientes>();
            PacientesDropdownList.DataSource = repositorioPacientes.GetList(x => true);
            PacientesDropdownList.DataValueField = "PacienteID";
            PacientesDropdownList.DataTextField = "Nombre";
            PacientesDropdownList.DataBind();
        }
        protected void BindGrid()
        {
            Analisis analisis = ViewState[KeyViewState].ToAnalisis();
            DetalleGridView.DataSource = analisis.DetalleAnalisis;
            DetalleGridView.DataBind();
        }
        public Analisis LLenaClase()
        {
            Analisis Analisis = ViewState[KeyViewState].ToAnalisis();
            Analisis.AnalisisID = AnalisisIdTextBox.Text.ToInt();
            Analisis.PacienteID = TipoAnalisisDropdonwList.SelectedValue.ToInt();
            Analisis.FechaRegistro = FechaTextBox.Text.ToDatetime();
            return Analisis;
        }
        public void LlenarCampos(Analisis analisis)
        {
            Limpiar();
            AnalisisIdTextBox.Text = analisis.AnalisisID.ToString();
            PacientesDropdownList.SelectedValue = analisis.PacienteID.ToString();
            FechaTextBox.Text = analisis.FechaRegistro.ToFormatDate();
            ViewState[KeyViewState] = analisis;
            this.BindGrid();
        }
        private void Limpiar()
        {
            AnalisisIdTextBox.Text = "0";
            PacientesDropdownList.ClearSelection();
            TipoAnalisisDropdonwList.ClearSelection();
            ResultadoAnalisisTextBox.Text = string.Empty;
            FechaTextBox.Text = DateTime.Now.ToFormatDate();
            ViewState[KeyViewState] = new Analisis();
            this.BindGrid();
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
            RepositorioAnalisis repositorio = new RepositorioAnalisis();
            Analisis analisis = LLenaClase();
            TipoAlerta Alerta = TipoAlerta.ErrorAlert;
            if (analisis.AnalisisID == 0)
                paso = repositorio.Guardar(analisis);
            else
                paso = repositorio.Modificar(analisis);

            if (paso)
            {
                Limpiar();
                Alerta = TipoAlerta.SuccessAlert;
            }
            Extensores.Extensores.Alerta(this, Alerta);
        }
        protected void BuscarButton_Click(object sender, EventArgs e)
        {
            RepositorioAnalisis repositorio = new RepositorioAnalisis();
            Analisis Analisis = repositorio.Buscar(AnalisisIdTextBox.Text.ToInt());
            if (Analisis.EsNulo())
            {
                Limpiar();
                LlenarCampos(Analisis);
            }
            else
            {
                Extensores.Extensores.Alerta(this, TipoAlerta.ErrorAlert);
            }
        }
        private bool Validar()
        {
            bool paso = true;
            if (TipoAnalisisDropdonwList.Items.Count == 0)
                paso = false;
            if (PacientesDropdownList.Items.Count == 0)
                paso = false;
            if (DetalleGridView.Rows.Count == 0)
                paso = false;
            return paso;
        }
        protected void AgregarDetalleButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ResultadoAnalisisTextBox.Text))
                return;
            Analisis analisis = new Analisis();
            analisis = ViewState[KeyViewState].ToAnalisis();
            analisis.AgregarDetalle(0, analisis.AnalisisID, TipoAnalisisDropdonwList.SelectedValue.ToInt(), ResultadoAnalisisTextBox.Text);
            ViewState[KeyViewState] = analisis;
            this.BindGrid();
            ResultadoAnalisisTextBox.Text = string.Empty;
        }
        protected void AgregarAnaliss_Click(object sender, EventArgs e)
        {
            RepositorioBase<TipoAnalisis> repositorio = new RepositorioBase<TipoAnalisis>();
            if (!string.IsNullOrEmpty(DescripcionAnalisisTextBox.Text))
            {
                repositorio.Guardar(new TipoAnalisis(0, DescripcionAnalisisTextBox.Text, DateTime.Now.ToDatetime()));
            }
            LlenarCombo();
        }

        protected void AgregarPacientesButton_Click(object sender, EventArgs e)
        {
            RepositorioBase<Pacientes> repositorio = new RepositorioBase<Pacientes>();
            if (!string.IsNullOrEmpty(NombrePacienteTextBox.Text))
            {
                repositorio.Guardar(new Pacientes(0, NombrePacienteTextBox.Text, DateTime.Now));
            }
            else
            {
                Extensores.Extensores.Alerta(this, TipoAlerta.ErrorAlert);
            }
            LlenarCombo();
        }
        protected void RemoverDetalleClick_Click(object sender, EventArgs e)
        {
            Analisis analisis = ViewState[KeyViewState].ToAnalisis();
            GridViewRow row = (sender as Button).NamingContainer as GridViewRow;
            analisis.RemoverDetalle(row.RowIndex);
            ViewState[KeyViewState] = analisis;
            this.BindGrid();
        }
        private bool ExisteEnLaBaseDeDatos()
        {
            return new RepositorioBase<Analisis>().Buscar(AnalisisIdTextBox.Text.ToInt()) is null;
        }
        protected void EliminarButton_Click(object sender, EventArgs e)
        {
            int id = AnalisisIdTextBox.Text.ToInt();
            if (ExisteEnLaBaseDeDatos())
            {
                Extensores.Extensores.Alerta(this, TipoAlerta.ErrorAlert);
                return;
            }
            else
            {
                if (new RepositorioBase<Analisis>().Eliminar(id))
                {
                    Extensores.Extensores.Alerta(this, TipoAlerta.SuccessAlert);
                    Limpiar();
                }
            }
        }
    }
}