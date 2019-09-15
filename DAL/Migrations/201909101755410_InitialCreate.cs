namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Analisis",
                c => new
                    {
                        AnalisisID = c.Int(nullable: false, identity: true),
                        PacienteID = c.Int(nullable: false),
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
                        FechaRegistro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TipoAnalisisID);
            
            CreateTable(
                "dbo.Pacientes",
                c => new
                    {
                        PacienteID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Fecha = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PacienteID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Analisis", "PacienteID", "dbo.Pacientes");
            DropForeignKey("dbo.DetalleAnalisis", "TipoAnalisisID", "dbo.TipoAnalisis");
            DropForeignKey("dbo.DetalleAnalisis", "AnalisisID", "dbo.Analisis");
            DropIndex("dbo.DetalleAnalisis", new[] { "TipoAnalisisID" });
            DropIndex("dbo.DetalleAnalisis", new[] { "AnalisisID" });
            DropIndex("dbo.Analisis", new[] { "PacienteID" });
            DropTable("dbo.Pacientes");
            DropTable("dbo.TipoAnalisis");
            DropTable("dbo.DetalleAnalisis");
            DropTable("dbo.Analisis");
        }
    }
}
