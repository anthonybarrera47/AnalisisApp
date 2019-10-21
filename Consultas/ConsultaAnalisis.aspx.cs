using BLL;
using Entidades;
using Extensores;
using Herramientas;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AnasilisApp.Consultas
{
    public partial class ConsultaAnalisis : System.Web.UI.Page
    {

        static List<Analisis> lista = new List<Analisis>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FechaDesdeTextBox.Text = DateTime.Now.ToFormatDate();
                FechaHastaTextBox.Text = DateTime.Now.ToFormatDate();
            }
        }
        protected void BuscarButton_Click(object sender, EventArgs e)
        {
            Expression<Func<Analisis, bool>> filtro = x => true;
            RepositorioBase<Analisis> repositorio = new RepositorioBase<Analisis>();
            int id;
            switch (BuscarPorDropDownList.SelectedIndex)
            {
                case 0:
                    filtro = x => true;
                    break;
                case 1://ID
                    id = (FiltroTextBox.Text).ToInt();
                    filtro = x => x.AnalisisID == id;
                    break;
                case 2:// nombre
                    id = (FiltroTextBox.Text).ToInt();
                    filtro = x => x.PacienteID == id;
                    break;
            }
            DateTime fechaDesde = FechaDesdeTextBox.Text.ToDatetime();
            DateTime FechaHasta = FechaHastaTextBox.Text.ToDatetime();
            if (FechaCheckBox.Checked)
                lista = repositorio.GetList(filtro).Where(x => x.FechaRegistro >= fechaDesde && x.FechaRegistro <= FechaHasta).ToList();
            else
                lista = repositorio.GetList(filtro);
            repositorio.Dispose();
            this.BindGrid(lista);
        }
        private void BindGrid(List<Analisis> lista)
        {
            DatosGridView.DataSource = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("AnalisisID", typeof(int));
            dt.Columns.Add("PacienteID", typeof(int));
            dt.Columns.Add("Paciente", typeof(string));
            dt.Columns.Add("Monto", typeof(decimal));
            dt.Columns.Add("Balance", typeof(decimal));
            dt.Columns.Add("FechaRegistro", typeof(string));
            foreach (var item in lista)
            {
                RepositorioBase<Pacientes> repositorio = new RepositorioBase<Pacientes>();
                dt.Rows.Add(item.AnalisisID, item.PacienteID, repositorio.Buscar(item.PacienteID).Nombre,
                         item.Monto, item.Balance, item.FechaRegistro.ToFormatDate());
                repositorio.Dispose();
            }
            DatosGridView.DataSource = dt;
            DatosGridView.Columns[3].Visible = false;
            CAntidadTextBox.Text = lista.Count.ToString();
            DatosGridView.DataBind();
        }
        protected void FechaCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (FechaCheckBox.Checked)
            {
                FechaDesdeTextBox.Visible = true;
                FechaHastaTextBox.Visible = true;
            }
            else
            {
                FechaDesdeTextBox.Visible = false;
                FechaHastaTextBox.Visible = false;
            }
        }
        protected void DatosGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DatosGridView.DataSource = lista;
            DatosGridView.PageIndex = e.NewPageIndex;
            DatosGridView.DataBind();
        }

        protected void VerDetalleButton_Click(object sender, EventArgs e)
        {
            string titulo = "Detalle del analisis";
            Utils.MostrarModal(this.Page, "ShowPopup", titulo);
            GridViewRow row = (sender as Button).NamingContainer as GridViewRow;
            var analisis = lista.ElementAt(row.RowIndex);
            DetalleDatosGridView.DataSource = null;
            RepositorioAnalisis Repositorio = new RepositorioAnalisis();
            List<DetalleAnalisis> Details = Repositorio.Buscar(analisis.AnalisisID).DetalleAnalisis;
            DataTable dt = new DataTable();
            dt.Columns.Add("DetalleAnalisisID", typeof(int));
            dt.Columns.Add("AnalisisID", typeof(int));
            dt.Columns.Add("TipoAnalisisID", typeof(int));
            dt.Columns.Add("TipoAnalisis", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Resultado", typeof(string));
            foreach (var item in Details)
            {
                RepositorioBase<TipoAnalisis> repositorio = new RepositorioBase<TipoAnalisis>();
                var TipoAnalisis = repositorio.Buscar(item.TipoAnalisisID);
                dt.Rows.Add(item.DetalleAnalisisID, item.AnalisisID,
                         item.TipoAnalisisID, TipoAnalisis.Descripcion, TipoAnalisis.Monto
                         , item.Resultado);
                repositorio.Dispose();
            }
            DetalleDatosGridView.DataSource = dt;
            DetalleDatosGridView.Columns[1].Visible = false;
            DetalleDatosGridView.Columns[2].Visible = false;
            DetalleDatosGridView.DataBind();
            Repositorio.Dispose();
        }

        protected void ImprimirButton_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", $"ShowReporte('Listado de Analisis');", true);

            AnalisisReportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            AnalisisReportViewer.Reset();
            AnalisisReportViewer.LocalReport.ReportPath = Server.MapPath(@"~\Reportes\ListadoAnalisis.rdlc");
            AnalisisReportViewer.LocalReport.DataSources.Clear();

            AnalisisReportViewer.LocalReport.DataSources.Add(new ReportDataSource("Analisis",
                                                               lista));
            AnalisisReportViewer.LocalReport.Refresh();
        }
    }
}