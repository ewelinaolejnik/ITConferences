namespace ITConferences.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttendeeIsUser : DbMigration
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
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Attendee_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Attendee_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Attendee_Id);
            
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
                    })
                .PrimaryKey(t => t.ConferenceID)
                .ForeignKey("dbo.Cities", t => t.TargetCityId, cascadeDelete: true)
                .ForeignKey("dbo.Countries", t => t.TargetCountryId, cascadeDelete: true)
                .Index(t => t.TargetCityId)
                .Index(t => t.TargetCountryId);
            
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
                "dbo.Evaluations",
                c => new
                    {
                        EvaluationID = c.Int(nullable: false, identity: true),
                        CountOfStars = c.Int(nullable: false),
                        Comment = c.String(maxLength: 100),
                        Speaker_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.EvaluationID)
                .ForeignKey("dbo.AspNetUsers", t => t.Speaker_Id)
                .Index(t => t.Speaker_Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        TagID = c.Int(nullable: false, identity: true),
                        Name = c.Int(nullable: false),
                        Conference_ConferenceID = c.Int(),
                    })
                .PrimaryKey(t => t.TagID)
                .ForeignKey("dbo.Conferences", t => t.Conference_ConferenceID)
                .Index(t => t.Conference_ConferenceID);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        CityID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
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
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CountryID);
            
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
                "dbo.ConferenceAttendees",
                c => new
                    {
                        Conference_ConferenceID = c.Int(nullable: false),
                        Attendee_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Conference_ConferenceID, t.Attendee_Id })
                .ForeignKey("dbo.Conferences", t => t.Conference_ConferenceID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Attendee_Id, cascadeDelete: true)
                .Index(t => t.Conference_ConferenceID)
                .Index(t => t.Attendee_Id);
            
            CreateTable(
                "dbo.OrganizerConferences",
                c => new
                    {
                        Organizer_Id = c.String(nullable: false, maxLength: 128),
                        Conference_ConferenceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Organizer_Id, t.Conference_ConferenceID })
                .ForeignKey("dbo.AspNetUsers", t => t.Organizer_Id, cascadeDelete: true)
                .ForeignKey("dbo.Conferences", t => t.Conference_ConferenceID, cascadeDelete: true)
                .Index(t => t.Organizer_Id)
                .Index(t => t.Conference_ConferenceID);
            
            CreateTable(
                "dbo.SpeakerConferences",
                c => new
                    {
                        Speaker_Id = c.String(nullable: false, maxLength: 128),
                        Conference_ConferenceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Speaker_Id, t.Conference_ConferenceID })
                .ForeignKey("dbo.AspNetUsers", t => t.Speaker_Id, cascadeDelete: true)
                .ForeignKey("dbo.Conferences", t => t.Conference_ConferenceID, cascadeDelete: true)
                .Index(t => t.Speaker_Id)
                .Index(t => t.Conference_ConferenceID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUsers", "Attendee_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Conferences", "TargetCountryId", "dbo.Countries");
            DropForeignKey("dbo.Cities", "Country_CountryID", "dbo.Countries");
            DropForeignKey("dbo.Conferences", "TargetCityId", "dbo.Cities");
            DropForeignKey("dbo.Tags", "Conference_ConferenceID", "dbo.Conferences");
            DropForeignKey("dbo.SpeakerConferences", "Conference_ConferenceID", "dbo.Conferences");
            DropForeignKey("dbo.SpeakerConferences", "Speaker_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Evaluations", "Speaker_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.OrganizerConferences", "Conference_ConferenceID", "dbo.Conferences");
            DropForeignKey("dbo.OrganizerConferences", "Organizer_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Inspirations", "Conference_ConferenceID", "dbo.Conferences");
            DropForeignKey("dbo.ConferenceAttendees", "Attendee_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ConferenceAttendees", "Conference_ConferenceID", "dbo.Conferences");
            DropIndex("dbo.SpeakerConferences", new[] { "Conference_ConferenceID" });
            DropIndex("dbo.SpeakerConferences", new[] { "Speaker_Id" });
            DropIndex("dbo.OrganizerConferences", new[] { "Conference_ConferenceID" });
            DropIndex("dbo.OrganizerConferences", new[] { "Organizer_Id" });
            DropIndex("dbo.ConferenceAttendees", new[] { "Attendee_Id" });
            DropIndex("dbo.ConferenceAttendees", new[] { "Conference_ConferenceID" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Cities", new[] { "Country_CountryID" });
            DropIndex("dbo.Tags", new[] { "Conference_ConferenceID" });
            DropIndex("dbo.Evaluations", new[] { "Speaker_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Inspirations", new[] { "Conference_ConferenceID" });
            DropIndex("dbo.Conferences", new[] { "TargetCountryId" });
            DropIndex("dbo.Conferences", new[] { "TargetCityId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Attendee_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropTable("dbo.SpeakerConferences");
            DropTable("dbo.OrganizerConferences");
            DropTable("dbo.ConferenceAttendees");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Countries");
            DropTable("dbo.Cities");
            DropTable("dbo.Tags");
            DropTable("dbo.Evaluations");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Inspirations");
            DropTable("dbo.Conferences");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
        }
    }
}
