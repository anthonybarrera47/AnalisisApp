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
            if (!Page.IsPostBack)
            {
                ViewState[KeyViewState] = new Analisis();
                LlenarCombo();
            }
        }
        private void LlenarCombo()
        {
            RepositorioBase<TipoAnalisis> repositorio = new RepositorioBase<TipoAnalisis>();
            TipoAnalisisDropdonwList.DataSource = repositorio.GetList(x => true);
            TipoAnalisisDropdonwList.DataValueField = "TipoAnalisisID";
            TipoAnalisisDropdonwList.DataTextField = "Descripcion";
            TipoAnalisisDropdonwList.DataBind();
            repositorio.Dispose();
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
            FechaTextBox.Text = String.Format("dd-MM-yyyy",analisis.FechaRegistro.ToString());
            ViewState[KeyViewState] = analisis;
            this.BindGrid();
        }
        private void Limpiar()
        {
            AnalisisIdTextBox.Text = "0";
            PacientesDropdownList.ClearSelection();
            TipoAnalisisDropdonwList.ClearSelection();
            ResultadoAnalisisTextBox.Text = string.Empty;
            FechaTextBox.Text = DateTime.Now.ToString();
            ViewState[KeyViewState] = new Analisis();
            this.BindGrid();
        }
        protected void NuevoButton_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        protected void GuadarButton_Click(object sender, EventArgs e)
        {
            bool paso = false;
            RepositorioAnalisis repositorio = new RepositorioAnalisis();
            Analisis analisis = LLenaClase();

            if (analisis.AnalisisID == 0)
                paso = repositorio.Guardar(analisis);
            else
                paso = repositorio.Modificar(analisis);

            if (paso)
                Limpiar();

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

            }

        }

        protected void AgregarDetalleButton_Click(object sender, EventArgs e)
        {
            Analisis analisis = new Analisis();
            analisis = ViewState[KeyViewState].ToAnalisis();
            analisis.AgregarDetalle(0, analisis.AnalisisID, TipoAnalisisDropdonwList.SelectedValue.ToInt(), ResultadoAnalisisTextBox.Text);
            ViewState[KeyViewState] = analisis;
            this.BindGrid();
            ResultadoAnalisisTextBox.Text = string.Empty;
        }

    }
}