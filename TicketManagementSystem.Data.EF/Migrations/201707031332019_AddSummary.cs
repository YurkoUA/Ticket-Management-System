namespace TicketManagementSystem.Data.EF.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddSummary : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Summaries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Tickets = c.Int(nullable: false),
                        HappyTickets = c.Int(nullable: false),
                        Packages = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Summaries");
        }
    }
}
