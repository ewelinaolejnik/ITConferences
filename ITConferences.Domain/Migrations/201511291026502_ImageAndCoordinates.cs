namespace ITConferences.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageAndCoordinates : DbMigration
    {
        public override void Up()
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
            CreateIndex("dbo.Conferences", "ImageID");
            AddForeignKey("dbo.Conferences", "ImageID", "dbo.Images", "ImageID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Conferences", "ImageID", "dbo.Images");
            DropIndex("dbo.Conferences", new[] { "ImageID" });
            DropColumn("dbo.Conferences", "ImageID");
            DropTable("dbo.Images");
        }
    }
}
