namespace ITConferences.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrganizerForAttendee : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Organizers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Organizers", new[] { "UserId" });
            AddColumn("dbo.AspNetUsers", "Organizer_OrganizerID", c => c.Int());
            AddColumn("dbo.Organizers", "AspNetUsersID", c => c.String());
            CreateIndex("dbo.AspNetUsers", "Organizer_OrganizerID");
            AddForeignKey("dbo.AspNetUsers", "Organizer_OrganizerID", "dbo.Organizers", "OrganizerID");
            DropColumn("dbo.Organizers", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Organizers", "UserId", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.AspNetUsers", "Organizer_OrganizerID", "dbo.Organizers");
            DropIndex("dbo.AspNetUsers", new[] { "Organizer_OrganizerID" });
            DropColumn("dbo.Organizers", "AspNetUsersID");
            DropColumn("dbo.AspNetUsers", "Organizer_OrganizerID");
            CreateIndex("dbo.Organizers", "UserId");
            AddForeignKey("dbo.Organizers", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
