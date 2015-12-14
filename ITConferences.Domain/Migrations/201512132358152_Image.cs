namespace ITConferences.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Image : DbMigration
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
        }
        
        public override void Down()
        {
        }
    }
}
