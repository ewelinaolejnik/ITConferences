namespace ITConferences.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotRequiredAttendee : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Organizers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Organizers", new[] { "UserId" });
            AlterColumn("dbo.Organizers", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Organizers", "UserId");
            AddForeignKey("dbo.Organizers", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Organizers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Organizers", new[] { "UserId" });
            AlterColumn("dbo.Organizers", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Organizers", "UserId");
            AddForeignKey("dbo.Organizers", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
