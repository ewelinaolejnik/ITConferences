namespace ITConferences.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Shit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        ImageId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Conference_ConferenceID = c.Int(),
                        Attendee_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Conferences", t => t.Conference_ConferenceID)
                .ForeignKey("dbo.AspNetUsers", t => t.Attendee_Id)
                .ForeignKey("dbo.Images", t => t.ImageId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.ImageId)
                .Index(t => t.Conference_ConferenceID)
                .Index(t => t.Attendee_Id);
            
            CreateTable(
                "dbo.Conferences",
                c => new
                    {
                        ConferenceID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Url = c.String(nullable: false),
                        IsPaid = c.Boolean(nullable: false),
                        TargetCityId = c.Int(nullable: false),
                        TargetCountryId = c.Int(nullable: false),
                        ImageId = c.Int(),
                        OrganizerId = c.Int(nullable: false),
                        Attendee_Id = c.String(maxLength: 128),
                        Attendee_Id1 = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ConferenceID)
                .ForeignKey("dbo.Images", t => t.ImageId)
                .ForeignKey("dbo.Organizers", t => t.OrganizerId, cascadeDelete: true)
                .ForeignKey("dbo.Cities", t => t.TargetCityId, cascadeDelete: true)
                .ForeignKey("dbo.Countries", t => t.TargetCountryId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Attendee_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Attendee_Id1)
                .Index(t => t.TargetCityId)
                .Index(t => t.TargetCountryId)
                .Index(t => t.ImageId)
                .Index(t => t.OrganizerId)
                .Index(t => t.Attendee_Id)
                .Index(t => t.Attendee_Id1);
            
            CreateTable(
                "dbo.Evaluations",
                c => new
                    {
                        EvaluationID = c.Int(nullable: false, identity: true),
                        CountOfStars = c.Int(nullable: false),
                        Comment = c.String(maxLength: 100),
                        Conference_ConferenceID = c.Int(),
                        Speaker_SpeakerID = c.Int(),
                    })
                .PrimaryKey(t => t.EvaluationID)
                .ForeignKey("dbo.Conferences", t => t.Conference_ConferenceID)
                .ForeignKey("dbo.Speakers", t => t.Speaker_SpeakerID)
                .Index(t => t.Conference_ConferenceID)
                .Index(t => t.Speaker_SpeakerID);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ImageID = c.Int(nullable: false, identity: true),
                        ConcreteImage = c.Binary(nullable: false),
                    })
                .PrimaryKey(t => t.ImageID);
            
            CreateTable(
                "dbo.Inspirations",
                c => new
                    {
                        InspirationID = c.Int(nullable: false, identity: true),
                        Url = c.String(nullable: false),
                        Conference_ConferenceID = c.Int(),
                    })
                .PrimaryKey(t => t.InspirationID)
                .ForeignKey("dbo.Conferences", t => t.Conference_ConferenceID)
                .Index(t => t.Conference_ConferenceID);
            
            CreateTable(
                "dbo.Organizers",
                c => new
                    {
                        OrganizerID = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.OrganizerID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Speakers",
                c => new
                    {
                        SpeakerID = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.SpeakerID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        TagID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.TagID);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        CityID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Country_CountryID = c.Int(),
                    })
                .PrimaryKey(t => t.CityID)
                .ForeignKey("dbo.Countries", t => t.Country_CountryID)
                .Index(t => t.Country_CountryID);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        CountryID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CountryID);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.SpeakerConferences",
                c => new
                    {
                        Speaker_SpeakerID = c.Int(nullable: false),
                        Conference_ConferenceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Speaker_SpeakerID, t.Conference_ConferenceID })
                .ForeignKey("dbo.Speakers", t => t.Speaker_SpeakerID, cascadeDelete: false)
                .ForeignKey("dbo.Conferences", t => t.Conference_ConferenceID, cascadeDelete: false)
                .Index(t => t.Speaker_SpeakerID)
                .Index(t => t.Conference_ConferenceID);
            
            CreateTable(
                "dbo.TagConferences",
                c => new
                    {
                        Tag_TagID = c.Int(nullable: false),
                        Conference_ConferenceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_TagID, t.Conference_ConferenceID })
                .ForeignKey("dbo.Tags", t => t.Tag_TagID, cascadeDelete: true)
                .ForeignKey("dbo.Conferences", t => t.Conference_ConferenceID, cascadeDelete: true)
                .Index(t => t.Tag_TagID)
                .Index(t => t.Conference_ConferenceID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUsers", "ImageId", "dbo.Images");
            DropForeignKey("dbo.AspNetUsers", "Attendee_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Conferences", "Attendee_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.Conferences", "Attendee_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Conferences", "TargetCountryId", "dbo.Countries");
            DropForeignKey("dbo.Conferences", "TargetCityId", "dbo.Cities");
            DropForeignKey("dbo.Cities", "Country_CountryID", "dbo.Countries");
            DropForeignKey("dbo.TagConferences", "Conference_ConferenceID", "dbo.Conferences");
            DropForeignKey("dbo.TagConferences", "Tag_TagID", "dbo.Tags");
            DropForeignKey("dbo.Speakers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SpeakerConferences", "Conference_ConferenceID", "dbo.Conferences");
            DropForeignKey("dbo.SpeakerConferences", "Speaker_SpeakerID", "dbo.Speakers");
            DropForeignKey("dbo.Evaluations", "Speaker_SpeakerID", "dbo.Speakers");
            DropForeignKey("dbo.Conferences", "OrganizerId", "dbo.Organizers");
            DropForeignKey("dbo.Organizers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Inspirations", "Conference_ConferenceID", "dbo.Conferences");
            DropForeignKey("dbo.Conferences", "ImageId", "dbo.Images");
            DropForeignKey("dbo.Evaluations", "Conference_ConferenceID", "dbo.Conferences");
            DropForeignKey("dbo.AspNetUsers", "Conference_ConferenceID", "dbo.Conferences");
            DropIndex("dbo.TagConferences", new[] { "Conference_ConferenceID" });
            DropIndex("dbo.TagConferences", new[] { "Tag_TagID" });
            DropIndex("dbo.SpeakerConferences", new[] { "Conference_ConferenceID" });
            DropIndex("dbo.SpeakerConferences", new[] { "Speaker_SpeakerID" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Cities", new[] { "Country_CountryID" });
            DropIndex("dbo.Speakers", new[] { "UserId" });
            DropIndex("dbo.Organizers", new[] { "UserId" });
            DropIndex("dbo.Inspirations", new[] { "Conference_ConferenceID" });
            DropIndex("dbo.Evaluations", new[] { "Speaker_SpeakerID" });
            DropIndex("dbo.Evaluations", new[] { "Conference_ConferenceID" });
            DropIndex("dbo.Conferences", new[] { "Attendee_Id1" });
            DropIndex("dbo.Conferences", new[] { "Attendee_Id" });
            DropIndex("dbo.Conferences", new[] { "OrganizerId" });
            DropIndex("dbo.Conferences", new[] { "ImageId" });
            DropIndex("dbo.Conferences", new[] { "TargetCountryId" });
            DropIndex("dbo.Conferences", new[] { "TargetCityId" });
            DropIndex("dbo.AspNetUsers", new[] { "Attendee_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Conference_ConferenceID" });
            DropIndex("dbo.AspNetUsers", new[] { "ImageId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropTable("dbo.TagConferences");
            DropTable("dbo.SpeakerConferences");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Countries");
            DropTable("dbo.Cities");
            DropTable("dbo.Tags");
            DropTable("dbo.Speakers");
            DropTable("dbo.Organizers");
            DropTable("dbo.Inspirations");
            DropTable("dbo.Images");
            DropTable("dbo.Evaluations");
            DropTable("dbo.Conferences");
            DropTable("dbo.AspNetUsers");
        }
    }
}
