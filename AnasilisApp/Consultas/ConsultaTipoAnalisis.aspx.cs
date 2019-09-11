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
    public partial class ConsultaTipoAnalisis : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                FechaDesdeTextBox.Text = DateTime.Now.ToFormatDate();
                FechaHastaTextBox.Text = DateTime.Now.ToFormatDate();
            }
        }
        protected void BuscarButton_Click(object sender, EventArgs e)
        {
            Expression<Func<TipoAnalisis, bool>> filtro = x => true;
            RepositorioBase<TipoAnalisis> repositorio = new RepositorioBase<TipoAnalisis>();
            List<TipoAnalisis> TiposAnalisis = new RepositorioBase<TipoAnalisis>().GetList(x => true);
            int id;
            switch (BuscarPorDropDownList.SelectedIndex)
            {
                case 0:
                    filtro = x => true;
                    break;
                case 1://ID
                    id = (FiltroTextBox.Text).ToInt();
                    filtro = x => x.TipoAnalisisID == id;
                    break;
                case 2:
                    filtro = x => x.Descripcion.Contains(FiltroTextBox.Text);
                    break;
                case 3:
                    id = TiposAnalisis.Find(x => x.Descripcion.Contains(FiltroTextBox.Text)).TipoAnalisisID;

                    break;
            }
            DateTime fechaDesde = FechaDesdeTextBox.Text.ToDatetime();
            DateTime FechaHasta = FechaHastaTextBox.Text.ToDatetime();
            List<TipoAnalisis> lista = repositorio.GetList(filtro).Where(x => x.FechaRegistro.Date >= fechaDesde.Date && x.FechaRegistro.Date <= FechaHasta.Date).ToList();
            this.BindGrid(lista);
        }
        private void BindGrid(List<TipoAnalisis> lista)
        {
            DatosGridView.DataSource = lista;
            CAntidadTextBox.Text = lista.Count.ToString();
            DatosGridView.DataBind();
        }
    }
}