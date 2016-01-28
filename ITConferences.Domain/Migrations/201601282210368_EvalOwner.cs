namespace ITConferences.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EvalOwner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Evaluations", "OwnerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Evaluations", "OwnerId");
            AddForeignKey("dbo.Evaluations", "OwnerId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Evaluations", "OwnerId", "dbo.AspNetUsers");
            DropIndex("dbo.Evaluations", new[] { "OwnerId" });
            DropColumn("dbo.Evaluations", "OwnerId");
        }
    }
}
