namespace ITConferences.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OptionalImage : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Conferences", "ImageId", "dbo.Images");
            DropIndex("dbo.Conferences", new[] { "ImageId" });
            AlterColumn("dbo.Conferences", "ImageId", c => c.Int());
            CreateIndex("dbo.Conferences", "ImageId");
            AddForeignKey("dbo.Conferences", "ImageId", "dbo.Images", "ImageID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Conferences", "ImageId", "dbo.Images");
            DropIndex("dbo.Conferences", new[] { "ImageId" });
            AlterColumn("dbo.Conferences", "ImageId", c => c.Int(nullable: false));
            CreateIndex("dbo.Conferences", "ImageId");
            AddForeignKey("dbo.Conferences", "ImageId", "dbo.Images", "ImageID", cascadeDelete: true);
        }
    }
}
