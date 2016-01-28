namespace ITConferences.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TypeForImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Images", "ImageData", c => c.Binary(nullable: false));
            AddColumn("dbo.Images", "ImageMimeType", c => c.String());
            DropColumn("dbo.Images", "ConcreteImage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Images", "ConcreteImage", c => c.Binary(nullable: false));
            DropColumn("dbo.Images", "ImageMimeType");
            DropColumn("dbo.Images", "ImageData");
        }
    }
}
