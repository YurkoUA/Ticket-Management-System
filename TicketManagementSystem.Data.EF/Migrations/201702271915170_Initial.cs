namespace TicketManagementSystem.Data.EF.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Colors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RowVersion = c.Binary(),
                        Name = c.String(maxLength: 32),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Packages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RowVersion = c.Binary(),
                        Name = c.String(maxLength: 64),
                        ColorId = c.Int(),
                        SerialId = c.Int(),
                        FirstNumber = c.Int(),
                        Nominal = c.Double(nullable: false),
                        IsSpecial = c.Boolean(nullable: false),
                        IsOpened = c.Boolean(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Note = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Colors", t => t.ColorId)
                .ForeignKey("dbo.Serials", t => t.SerialId)
                .Index(t => t.ColorId)
                .Index(t => t.SerialId);
            
            CreateTable(
                "dbo.Serials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RowVersion = c.Binary(),
                        Name = c.String(maxLength: 4),
                        Note = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RowVersion = c.Binary(),
                        Number = c.String(maxLength: 6),
                        PackageId = c.Int(),
                        ColorId = c.Int(nullable: false),
                        SerialId = c.Int(nullable: false),
                        SerialNumber = c.String(maxLength: 2),
                        Note = c.String(maxLength: 128),
                        Date = c.DateTime(),
                        AddDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Colors", t => t.ColorId, cascadeDelete: true)
                .ForeignKey("dbo.Packages", t => t.PackageId)
                .ForeignKey("dbo.Serials", t => t.SerialId, cascadeDelete: true)
                .Index(t => t.PackageId)
                .Index(t => t.ColorId)
                .Index(t => t.SerialId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RowVersion = c.Binary(),
                        Email = c.String(maxLength: 64),
                        Login = c.String(maxLength: 64),
                        Name = c.String(maxLength: 64),
                        Password = c.Binary(),
                        Role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "SerialId", "dbo.Serials");
            DropForeignKey("dbo.Tickets", "PackageId", "dbo.Packages");
            DropForeignKey("dbo.Tickets", "ColorId", "dbo.Colors");
            DropForeignKey("dbo.Packages", "SerialId", "dbo.Serials");
            DropForeignKey("dbo.Packages", "ColorId", "dbo.Colors");
            DropIndex("dbo.Tickets", new[] { "SerialId" });
            DropIndex("dbo.Tickets", new[] { "ColorId" });
            DropIndex("dbo.Tickets", new[] { "PackageId" });
            DropIndex("dbo.Packages", new[] { "SerialId" });
            DropIndex("dbo.Packages", new[] { "ColorId" });
            DropTable("dbo.Users");
            DropTable("dbo.Tickets");
            DropTable("dbo.Serials");
            DropTable("dbo.Packages");
            DropTable("dbo.Colors");
        }
    }
}
