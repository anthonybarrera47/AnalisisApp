using BLL;
using Entidades;
using Extensores;
using Herramientas;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AnasilisApp.Consultas
{
    public partial class ConsultaPagos : System.Web.UI.Page
    {
        static List<Pagos> lista = new List<Pagos>();
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
            Expression<Func<Pagos, bool>> filtro = x => true;
            RepositorioPago repositorio = new RepositorioPago();
            int id;
            switch (BuscarPorDropDownList.SelectedIndex)
            {
                case 0:
                    filtro = x => true;
                    break;
                case 1://ID
                    id = (FiltroTextBox.Text).ToInt();
                    filtro = x => x.PagosID == id;
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

        private void BindGrid(List<Pagos> lista)
        {
            DatosGridView.DataSource = null;
            DatosGridView.DataSource = lista;
            DatosGridView.DataBind();
        }

        protected void DatosGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DatosGridView.DataSource = lista;
            DatosGridView.PageIndex = e.NewPageIndex;
            DatosGridView.DataBind();
        }

        protected void VerDetalleButton_Click(object sender, EventArgs e)
        {
            string titulo = "Detalle del Pago";
            GridViewRow row = (sender as Button).NamingContainer as GridViewRow;
            var Pagos = lista.ElementAt(row.RowIndex);
            DetalleDatosGridView.DataSource = null;
            RepositorioPago repositorio = new RepositorioPago();
            DetalleDatosGridView.DataSource = repositorio.Buscar(Pagos.PagosID).DetallesPagos;
            DetalleDatosGridView.DataBind();
            repositorio.Dispose();
            Utils.MostrarModal(this.Page, "ShowPopup", titulo);
        }

        protected void ImprimirButton_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", $"ShowReporte('Listado de Pagos');", true);

            PagosReportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            PagosReportViewer.Reset();
            PagosReportViewer.LocalReport.ReportPath = Server.MapPath(@"\Reportes\ListadoPagos.rdlc");
            PagosReportViewer.LocalReport.DataSources.Clear();

            PagosReportViewer.LocalReport.DataSources.Add(new ReportDataSource("Pagos",
                                                               lista));
            PagosReportViewer.LocalReport.Refresh();
        }
    }
}