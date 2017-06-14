namespace TicketManagementSystem.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Roles", "Description", c => c.String(maxLength: 32));
            AlterColumn("dbo.Tickets", "Date", c => c.String(maxLength: 32));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tickets", "Date", c => c.DateTime());
            DropColumn("dbo.Roles", "Description");
        }
    }
}
