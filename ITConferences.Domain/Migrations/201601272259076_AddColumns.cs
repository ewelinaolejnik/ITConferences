namespace ITConferences.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organizers", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Organizers", "UserId");
            //AddForeignKey("dbo.Organizers", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
        }
    }
}
