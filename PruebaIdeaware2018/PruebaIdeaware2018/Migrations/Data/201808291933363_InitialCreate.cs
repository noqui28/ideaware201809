namespace PruebaIdeaware2018.Migrations.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ciudads",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Codigo = c.String(),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tiendas",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Codigo = c.String(),
                        Nombre = c.String(),
                        CiudadId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ciudads", t => t.CiudadId, cascadeDelete: true)
                .Index(t => t.CiudadId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Cedula = c.Int(nullable: false),
                        Nombre = c.String(),
                        Apellido = c.String(),
                        Fecha_nac = c.DateTime(),
                        Direccion = c.String(),
                        Sexo = c.String(),
                        Foto = c.Binary(),
                        TiendaId = c.Guid(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tiendas", t => t.TiendaId)
                .Index(t => t.TiendaId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Mascotas",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Nombre = c.String(),
                        Apodo = c.String(),
                        Fecha_nac = c.DateTime(nullable: false),
                        Sexo = c.String(),
                        Foto = c.Binary(),
                        ClienteId = c.String(nullable: false, maxLength: 128),
                        RazaId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ClienteId, cascadeDelete: true)
                .ForeignKey("dbo.Razas", t => t.RazaId, cascadeDelete: true)
                .Index(t => t.ClienteId)
                .Index(t => t.RazaId);
            
            CreateTable(
                "dbo.Razas",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Codigo = c.String(),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TareasMascotas",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        TareaId = c.Guid(nullable: false),
                        MascotaId = c.Guid(nullable: false),
                        Fecha_ejec = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Mascotas", t => t.MascotaId, cascadeDelete: true)
                .ForeignKey("dbo.Tareas", t => t.TareaId, cascadeDelete: true)
                .Index(t => t.TareaId)
                .Index(t => t.MascotaId);
            
            CreateTable(
                "dbo.Tareas",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Codigo = c.String(),
                        Nombre = c.String(),
                        Costo = c.Int(nullable: false),
                        TipoId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TipoTareas", t => t.TipoId, cascadeDelete: true)
                .Index(t => t.TipoId);
            
            CreateTable(
                "dbo.TipoTareas",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Codigo = c.String(),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUsers", "TiendaId", "dbo.Tiendas");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tareas", "TipoId", "dbo.TipoTareas");
            DropForeignKey("dbo.TareasMascotas", "TareaId", "dbo.Tareas");
            DropForeignKey("dbo.TareasMascotas", "MascotaId", "dbo.Mascotas");
            DropForeignKey("dbo.Mascotas", "RazaId", "dbo.Razas");
            DropForeignKey("dbo.Mascotas", "ClienteId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tiendas", "CiudadId", "dbo.Ciudads");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Tareas", new[] { "TipoId" });
            DropIndex("dbo.TareasMascotas", new[] { "MascotaId" });
            DropIndex("dbo.TareasMascotas", new[] { "TareaId" });
            DropIndex("dbo.Mascotas", new[] { "RazaId" });
            DropIndex("dbo.Mascotas", new[] { "ClienteId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "TiendaId" });
            DropIndex("dbo.Tiendas", new[] { "CiudadId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.TipoTareas");
            DropTable("dbo.Tareas");
            DropTable("dbo.TareasMascotas");
            DropTable("dbo.Razas");
            DropTable("dbo.Mascotas");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Tiendas");
            DropTable("dbo.Ciudads");
        }
    }
}
