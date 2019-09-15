namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrimeraMigracion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pagos",
                c => new
                    {
                        PagosID = c.Int(nullable: false, identity: true),
                        AnalisisID = c.Int(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PagosID);
            
            CreateTable(
                "dbo.DetallesPagos",
                c => new
                    {
                        DetallePagoID = c.Int(nullable: false, identity: true),
                        PagosID = c.Int(nullable: false),
                        Monto = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.DetallePagoID)
                .ForeignKey("dbo.Pagos", t => t.PagosID, cascadeDelete: true)
                .Index(t => t.PagosID);
            
            AddColumn("dbo.Analisis", "Balance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Analisis", "Monto", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.TipoAnalisis", "Monto", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DetallesPagos", "PagosID", "dbo.Pagos");
            DropIndex("dbo.DetallesPagos", new[] { "PagosID" });
            DropColumn("dbo.TipoAnalisis", "Monto");
            DropColumn("dbo.Analisis", "Monto");
            DropColumn("dbo.Analisis", "Balance");
            DropTable("dbo.DetallesPagos");
            DropTable("dbo.Pagos");
        }
    }
}
