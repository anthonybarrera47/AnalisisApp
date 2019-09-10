using BLL;
using Entidades;
using Extensores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AnasilisApp.Consultas
{
    public partial class ConsultaAnalisis : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FechaDesdeTextBox.Text = DateTime.Now.ToFormatDate();
            FechaHastaTextBox.Text = DateTime.Now.ToFormatDate();
        }

        protected void BuscarButton_Click(object sender, EventArgs e)
        {
            Expression<Func<Analisis, bool>> filtro = x => true;
            RepositorioBase<Analisis> repositorio = new RepositorioBase<Analisis>();
            List<TipoAnalisis> TiposAnalisis = new RepositorioBase<TipoAnalisis>().GetList(x => true);
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
                case 3:
                    id = TiposAnalisis.Find(x => x.Descripcion.Contains(FiltroTextBox.Text)).TipoAnalisisID;

                    break;
            }
            DateTime fechaDesde = FechaHastaTextBox.Text.ToDatetime();
            DateTime FechaHasta = FechaHastaTextBox.Text.ToDatetime();
            List<Analisis> lista = repositorio.GetList(filtro).Where(x => x.FechaRegistro.Date >= fechaDesde.Date && x.FechaRegistro.Date <= FechaHasta.Date).ToList();
            this.BindGrid(lista);
        }
        private void BindGrid(List<Analisis> lista)
        {
            DatosGridView.DataSource = lista;
            DatosGridView.DataBind();
        }

    }
}