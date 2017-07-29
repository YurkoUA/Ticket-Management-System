namespace TicketManagementSystem.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TodoTask_IncreasedFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TodoTasks", "Title", c => c.String(nullable: false, maxLength: 64));
            AlterColumn("dbo.TodoTasks", "Description", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TodoTasks", "Description", c => c.String(maxLength: 128));
            AlterColumn("dbo.TodoTasks", "Title", c => c.String(nullable: false, maxLength: 16));
        }
    }
}
