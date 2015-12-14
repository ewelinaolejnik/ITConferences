namespace ITConferences.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LttLong : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cities", "Latitude", c => c.Double(nullable: false));
            AddColumn("dbo.Cities", "Longitude", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cities", "Longitude");
            DropColumn("dbo.Cities", "Latitude");
        }
    }
}
