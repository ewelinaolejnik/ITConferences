namespace ITConferences.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NameForCityCountryConference : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Conferences", "ImageID", "dbo.Images");
            DropIndex("dbo.Conferences", new[] { "ImageID" });
            AlterColumn("dbo.Cities", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Countries", "Name", c => c.String(nullable: false));
            DropColumn("dbo.Conferences", "ImageID");
            DropTable("dbo.Images");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ImageID = c.Int(nullable: false, identity: true),
                        ConcreteImage = c.Binary(nullable: false),
                    })
                .PrimaryKey(t => t.ImageID);
            
            AddColumn("dbo.Conferences", "ImageID", c => c.Int(nullable: false));
            AlterColumn("dbo.Countries", "Name", c => c.String());
            AlterColumn("dbo.Cities", "Name", c => c.String());
            CreateIndex("dbo.Conferences", "ImageID");
            AddForeignKey("dbo.Conferences", "ImageID", "dbo.Images", "ImageID", cascadeDelete: true);
        }
    }
}
