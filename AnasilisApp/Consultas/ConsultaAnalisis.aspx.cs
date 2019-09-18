using BLL;
using Entidades;
using Extensores;
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
            this.BindGrid(lista);
        }
        private void BindGrid(List<Analisis> lista)
        {
            DatosGridView.DataSource = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("AnalisisID", typeof(int));
            dt.Columns.Add("PacienteID", typeof(int));
            dt.Columns.Add("Paciente", typeof(string));
            dt.Columns.Add("Balance", typeof(decimal));
            dt.Columns.Add("Monto", typeof(decimal));
            dt.Columns.Add("FechaRegistro", typeof(DateTime));
            foreach (var item in lista)
            {
                dt.Rows.Add(item.AnalisisID, item.PacienteID, new RepositorioBase<Pacientes>().Buscar(item.PacienteID).Nombre,
                         item.Balance, item.Monto, item.FechaRegistro);
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
            Extensores.Extensores.MostrarModal(this.Page, "ShowPopup", titulo);
            //ScriptManager.RegisterStartupScript(this.Page,this.Page.GetType(), "Popup", $"ShowPopup('{ titulo }');", true);
            GridViewRow row = (sender as Button).NamingContainer as GridViewRow;
            var analisis = lista.ElementAt(row.RowIndex);
            DetalleDatosGridView.DataSource = null;
            List<DetalleAnalisis> Details = new RepositorioAnalisis().Buscar(analisis.AnalisisID).DetalleAnalisis;
            DataTable dt = new DataTable();
            dt.Columns.Add("DetalleAnalisisID", typeof(int));
            dt.Columns.Add("AnalisisID", typeof(int));
            dt.Columns.Add("TipoAnalisisID", typeof(int));
            dt.Columns.Add("TipoAnalisis", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Resultado", typeof(string));
            foreach (var item in Details)
            {
                var TipoAnalisis = new RepositorioBase<TipoAnalisis>().Buscar(item.TipoAnalisisID);
                dt.Rows.Add(item.DetalleAnalisisID,item.AnalisisID,
                         item.TipoAnalisisID,TipoAnalisis.Descripcion,TipoAnalisis.Monto
                         , item.Resultado);
            }
            DetalleDatosGridView.DataSource = dt;
            DetalleDatosGridView.Columns[1].Visible = false;
            DetalleDatosGridView.Columns[2].Visible = false;
            DetalleDatosGridView.DataBind();
        }
    }
}