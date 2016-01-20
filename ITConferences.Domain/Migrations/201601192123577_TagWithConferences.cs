namespace ITConferences.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TagWithConferences : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tags", "Conference_ConferenceID", "dbo.Conferences");
            DropIndex("dbo.Tags", new[] { "Conference_ConferenceID" });
            CreateTable(
                "dbo.TagConferences",
                c => new
                    {
                        Tag_TagID = c.Int(nullable: false),
                        Conference_ConferenceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_TagID, t.Conference_ConferenceID })
                .ForeignKey("dbo.Tags", t => t.Tag_TagID, cascadeDelete: true)
                .ForeignKey("dbo.Conferences", t => t.Conference_ConferenceID, cascadeDelete: true)
                .Index(t => t.Tag_TagID)
                .Index(t => t.Conference_ConferenceID);
            
            DropColumn("dbo.Tags", "Conference_ConferenceID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tags", "Conference_ConferenceID", c => c.Int());
            DropForeignKey("dbo.TagConferences", "Conference_ConferenceID", "dbo.Conferences");
            DropForeignKey("dbo.TagConferences", "Tag_TagID", "dbo.Tags");
            DropIndex("dbo.TagConferences", new[] { "Conference_ConferenceID" });
            DropIndex("dbo.TagConferences", new[] { "Tag_TagID" });
            DropTable("dbo.TagConferences");
            CreateIndex("dbo.Tags", "Conference_ConferenceID");
            AddForeignKey("dbo.Tags", "Conference_ConferenceID", "dbo.Conferences", "ConferenceID");
        }
    }
}
