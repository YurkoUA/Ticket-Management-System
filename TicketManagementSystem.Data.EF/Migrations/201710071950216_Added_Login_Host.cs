using System.Data.Entity.Migrations;

namespace TicketManagementSystem.Data.EF.Migrations
{
    public partial class Added_Login_Host : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logins", "Host", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Logins", "Host");
        }
    }
}
