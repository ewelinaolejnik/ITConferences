namespace ITConferences.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TagNameAsString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tags", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tags", "Name", c => c.Int(nullable: false));
        }
    }
}
