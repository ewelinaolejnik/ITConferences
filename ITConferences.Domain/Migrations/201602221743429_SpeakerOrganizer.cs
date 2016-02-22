namespace ITConferences.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpeakerOrganizer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Conferences", "Attendee_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Conferences", "Attendee_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Attendee_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Speakers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "Attendee_Id" });
            DropIndex("dbo.Conferences", new[] { "Attendee_Id" });
            DropIndex("dbo.Conferences", new[] { "Attendee_Id1" });
            DropIndex("dbo.Organizers", new[] { "UserId" });
            RenameColumn(table: "dbo.Organizers", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Speakers", name: "UserId", newName: "User_Id");
            RenameIndex(table: "dbo.Speakers", name: "IX_UserId", newName: "IX_User_Id");
            AlterColumn("dbo.Organizers", "User_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Organizers", "User_Id");
            AddForeignKey("dbo.Speakers", "User_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.AspNetUsers", "Attendee_Id");
            DropColumn("dbo.Conferences", "Attendee_Id");
            DropColumn("dbo.Conferences", "Attendee_Id1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Conferences", "Attendee_Id1", c => c.String(maxLength: 128));
            AddColumn("dbo.Conferences", "Attendee_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Attendee_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Speakers", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Organizers", new[] { "User_Id" });
            AlterColumn("dbo.Organizers", "User_Id", c => c.String(maxLength: 128));
            RenameIndex(table: "dbo.Speakers", name: "IX_User_Id", newName: "IX_UserId");
            RenameColumn(table: "dbo.Speakers", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.Organizers", name: "User_Id", newName: "UserId");
            CreateIndex("dbo.Organizers", "UserId");
            CreateIndex("dbo.Conferences", "Attendee_Id1");
            CreateIndex("dbo.Conferences", "Attendee_Id");
            CreateIndex("dbo.AspNetUsers", "Attendee_Id");
            AddForeignKey("dbo.Speakers", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUsers", "Attendee_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Conferences", "Attendee_Id1", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Conferences", "Attendee_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
