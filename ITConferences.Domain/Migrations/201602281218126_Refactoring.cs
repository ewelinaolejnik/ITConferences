namespace ITConferences.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Refactoring : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inspirations", "Conference_ConferenceID", "dbo.Conferences");
            DropIndex("dbo.Inspirations", new[] { "Conference_ConferenceID" });
            AddColumn("dbo.Conferences", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Conferences", "EndDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Conferences", "Date");
            DropTable("dbo.Inspirations");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Inspirations",
                c => new
                    {
                        InspirationID = c.Int(nullable: false, identity: true),
                        Url = c.String(nullable: false),
                        Conference_ConferenceID = c.Int(),
                    })
                .PrimaryKey(t => t.InspirationID);
            
            AddColumn("dbo.Conferences", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Conferences", "EndDate");
            DropColumn("dbo.Conferences", "StartDate");
            CreateIndex("dbo.Inspirations", "Conference_ConferenceID");
            AddForeignKey("dbo.Inspirations", "Conference_ConferenceID", "dbo.Conferences", "ConferenceID");
        }
    }
}
