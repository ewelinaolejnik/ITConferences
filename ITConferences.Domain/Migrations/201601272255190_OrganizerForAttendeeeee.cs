namespace ITConferences.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrganizerForAttendeeeee : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Organizer_OrganizerID", "dbo.Organizers");
            DropIndex("dbo.AspNetUsers", new[] { "Organizer_OrganizerID" });
            AddColumn("dbo.Organizers", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Organizers", "UserId");
            AddForeignKey("dbo.Organizers", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: false);
            DropColumn("dbo.AspNetUsers", "Organizer_OrganizerID");
            DropColumn("dbo.Organizers", "AspNetUsersID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Organizers", "AspNetUsersID", c => c.String());
            AddColumn("dbo.AspNetUsers", "Organizer_OrganizerID", c => c.Int());
            DropForeignKey("dbo.Organizers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Organizers", new[] { "UserId" });
            DropColumn("dbo.Organizers", "UserId");
            CreateIndex("dbo.AspNetUsers", "Organizer_OrganizerID");
            AddForeignKey("dbo.AspNetUsers", "Organizer_OrganizerID", "dbo.Organizers", "OrganizerID");
        }
    }
}
