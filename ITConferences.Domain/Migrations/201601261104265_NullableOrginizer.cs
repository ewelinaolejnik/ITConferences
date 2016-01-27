namespace ITConferences.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableOrginizer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Conferences", "OrganizerId", "dbo.Organizers");
            DropIndex("dbo.Conferences", new[] { "OrganizerId" });
            AlterColumn("dbo.Conferences", "OrganizerId", c => c.Int());
            CreateIndex("dbo.Conferences", "OrganizerId");
            AddForeignKey("dbo.Conferences", "OrganizerId", "dbo.Organizers", "OrganizerID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Conferences", "OrganizerId", "dbo.Organizers");
            DropIndex("dbo.Conferences", new[] { "OrganizerId" });
            AlterColumn("dbo.Conferences", "OrganizerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Conferences", "OrganizerId");
            AddForeignKey("dbo.Conferences", "OrganizerId", "dbo.Organizers", "OrganizerID", cascadeDelete: true);
        }
    }
}
