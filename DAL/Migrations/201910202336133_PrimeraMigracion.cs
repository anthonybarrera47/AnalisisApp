namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrimeraMigracion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Analisis",
                c => new
                    {
                        AnalisisID = c.Int(nullable: false, identity: true),
                        PacienteID = c.Int(nullable: false),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Monto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FechaRegistro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AnalisisID)
                .ForeignKey("dbo.Pacientes", t => t.PacienteID, cascadeDelete: true)
                .Index(t => t.PacienteID);
            
            CreateTable(
                "dbo.DetalleAnalisis",
                c => new
                    {
                        DetalleAnalisisID = c.Int(nullable: false, identity: true),
                        AnalisisID = c.Int(nullable: false),
                        TipoAnalisisID = c.Int(nullable: false),
                        Resultado = c.String(),
                    })
                .PrimaryKey(t => t.DetalleAnalisisID)
                .ForeignKey("dbo.Analisis", t => t.AnalisisID, cascadeDelete: true)
                .ForeignKey("dbo.TipoAnalisis", t => t.TipoAnalisisID, cascadeDelete: true)
                .Index(t => t.AnalisisID)
                .Index(t => t.TipoAnalisisID);
            
            CreateTable(
                "dbo.TipoAnalisis",
                c => new
                    {
                        TipoAnalisisID = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                        Monto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FechaRegistro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TipoAnalisisID);
            
            CreateTable(
                "dbo.Pacientes",
                c => new
                    {
                        PacienteID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Fecha = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PacienteID);
            
            CreateTable(
                "dbo.Pagos",
                c => new
                    {
                        PagosID = c.Int(nullable: false, identity: true),
                        PacienteID = c.Int(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PagosID);
            
            CreateTable(
                "dbo.DetallesPagos",
                c => new
                    {
                        DetallePagoID = c.Int(nullable: false, identity: true),
                        PagosID = c.Int(nullable: false),
                        AnalisisID = c.Int(nullable: false),
                        Monto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Estado = c.String(),
                    })
                .PrimaryKey(t => t.DetallePagoID)
                .ForeignKey("dbo.Analisis", t => t.AnalisisID, cascadeDelete: true)
                .ForeignKey("dbo.Pagos", t => t.PagosID, cascadeDelete: true)
                .Index(t => t.PagosID)
                .Index(t => t.AnalisisID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DetallesPagos", "PagosID", "dbo.Pagos");
            DropForeignKey("dbo.DetallesPagos", "AnalisisID", "dbo.Analisis");
            DropForeignKey("dbo.Analisis", "PacienteID", "dbo.Pacientes");
            DropForeignKey("dbo.DetalleAnalisis", "TipoAnalisisID", "dbo.TipoAnalisis");
            DropForeignKey("dbo.DetalleAnalisis", "AnalisisID", "dbo.Analisis");
            DropIndex("dbo.DetallesPagos", new[] { "AnalisisID" });
            DropIndex("dbo.DetallesPagos", new[] { "PagosID" });
            DropIndex("dbo.DetalleAnalisis", new[] { "TipoAnalisisID" });
            DropIndex("dbo.DetalleAnalisis", new[] { "AnalisisID" });
            DropIndex("dbo.Analisis", new[] { "PacienteID" });
            DropTable("dbo.DetallesPagos");
            DropTable("dbo.Pagos");
            DropTable("dbo.Pacientes");
            DropTable("dbo.TipoAnalisis");
            DropTable("dbo.DetalleAnalisis");
            DropTable("dbo.Analisis");
        }
    }
}
