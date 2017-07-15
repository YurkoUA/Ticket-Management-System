using System.Data.Entity.Migrations;

namespace TicketManagementSystem.Data.EF.Migrations
{   
    public partial class AddedTodoExplorer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TodoTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Title = c.String(nullable: false, maxLength: 16),
                        Description = c.String(maxLength: 128),
                        Priority = c.Byte(nullable: false),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TodoTasks");
        }
    }
}
