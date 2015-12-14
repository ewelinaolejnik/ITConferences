namespace ITConferences.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageForConference : DbMigration
    {
        public override void Up()
        {   
            AddColumn("dbo.Conferences", "ImageId", c => c.Int(nullable: false));
            CreateIndex("dbo.Conferences", "ImageId");
            AddForeignKey("dbo.Conferences", "ImageId", "dbo.Images", "ImageID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Conferences", "ImageId", "dbo.Images");
            DropIndex("dbo.Conferences", new[] { "ImageId" });
            DropColumn("dbo.Conferences", "ImageId");
            DropTable("dbo.Images");
        }
    }
}
