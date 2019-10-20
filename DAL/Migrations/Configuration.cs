namespace DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.Contexto>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAL.Contexto context)
        {
            string sql = @"CREATE view dbo.View_Pagos
                            AS
	                            SELECT P.PagosID AS 'PagosID',P.PacienteID AS 'PacienteID',
			                            pa.Nombre as 'NombrePaciente',SUM(D.Monto)AS 'TotalPagado'
	                            FROM Pagos P 
	                            INNER JOIN DetallesPagos D ON D.PagosID = P.PagosID
	                            INNER JOIN Pacientes Pa ON P.PacienteID = pa.PacienteID
	                            GROUP BY P.PagosID,pa.Nombre,P.PacienteID";
            Contexto ctx = new Contexto();
            ctx.Database.ExecuteSqlCommand(sql);
        }
    }
}
