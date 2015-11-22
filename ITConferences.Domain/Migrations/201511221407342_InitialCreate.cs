namespace ITConferences.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attendees",
                c => new
                    {
                        AttendeeID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Attendee_AttendeeID = c.Int(),
                    })
                .PrimaryKey(t => t.AttendeeID)
                .ForeignKey("dbo.Attendees", t => t.Attendee_AttendeeID)
                .Index(t => t.Attendee_AttendeeID);
            
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
                "dbo.Evaluations",
                c => new
                    {
                        EvaluationID = c.Int(nullable: false, identity: true),
                        CountOfStars = c.Int(nullable: false),
                        Comment = c.String(maxLength: 100),
                        Speaker_AttendeeID = c.Int(),
                    })
                .PrimaryKey(t => t.EvaluationID)
                .ForeignKey("dbo.Attendees", t => t.Speaker_AttendeeID)
                .Index(t => t.Speaker_AttendeeID);
            
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
                "dbo.ConferenceAttendees",
                c => new
                    {
                        Conference_ConferenceID = c.Int(nullable: false),
                        Attendee_AttendeeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Conference_ConferenceID, t.Attendee_AttendeeID })
                .ForeignKey("dbo.Conferences", t => t.Conference_ConferenceID, cascadeDelete: true)
                .ForeignKey("dbo.Attendees", t => t.Attendee_AttendeeID, cascadeDelete: true)
                .Index(t => t.Conference_ConferenceID)
                .Index(t => t.Attendee_AttendeeID);
            
            CreateTable(
                "dbo.OrganizerConferences",
                c => new
                    {
                        Organizer_AttendeeID = c.Int(nullable: false),
                        Conference_ConferenceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Organizer_AttendeeID, t.Conference_ConferenceID })
                .ForeignKey("dbo.Attendees", t => t.Organizer_AttendeeID, cascadeDelete: true)
                .ForeignKey("dbo.Conferences", t => t.Conference_ConferenceID, cascadeDelete: true)
                .Index(t => t.Organizer_AttendeeID)
                .Index(t => t.Conference_ConferenceID);
            
            CreateTable(
                "dbo.SpeakerConferences",
                c => new
                    {
                        Speaker_AttendeeID = c.Int(nullable: false),
                        Conference_ConferenceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Speaker_AttendeeID, t.Conference_ConferenceID })
                .ForeignKey("dbo.Attendees", t => t.Speaker_AttendeeID, cascadeDelete: true)
                .ForeignKey("dbo.Conferences", t => t.Conference_ConferenceID, cascadeDelete: true)
                .Index(t => t.Speaker_AttendeeID)
                .Index(t => t.Conference_ConferenceID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attendees", "Attendee_AttendeeID", "dbo.Attendees");
            DropForeignKey("dbo.Conferences", "TargetCountryId", "dbo.Countries");
            DropForeignKey("dbo.Cities", "Country_CountryID", "dbo.Countries");
            DropForeignKey("dbo.Conferences", "TargetCityId", "dbo.Cities");
            DropForeignKey("dbo.Tags", "Conference_ConferenceID", "dbo.Conferences");
            DropForeignKey("dbo.SpeakerConferences", "Conference_ConferenceID", "dbo.Conferences");
            DropForeignKey("dbo.SpeakerConferences", "Speaker_AttendeeID", "dbo.Attendees");
            DropForeignKey("dbo.Evaluations", "Speaker_AttendeeID", "dbo.Attendees");
            DropForeignKey("dbo.OrganizerConferences", "Conference_ConferenceID", "dbo.Conferences");
            DropForeignKey("dbo.OrganizerConferences", "Organizer_AttendeeID", "dbo.Attendees");
            DropForeignKey("dbo.Inspirations", "Conference_ConferenceID", "dbo.Conferences");
            DropForeignKey("dbo.ConferenceAttendees", "Attendee_AttendeeID", "dbo.Attendees");
            DropForeignKey("dbo.ConferenceAttendees", "Conference_ConferenceID", "dbo.Conferences");
            DropIndex("dbo.SpeakerConferences", new[] { "Conference_ConferenceID" });
            DropIndex("dbo.SpeakerConferences", new[] { "Speaker_AttendeeID" });
            DropIndex("dbo.OrganizerConferences", new[] { "Conference_ConferenceID" });
            DropIndex("dbo.OrganizerConferences", new[] { "Organizer_AttendeeID" });
            DropIndex("dbo.ConferenceAttendees", new[] { "Attendee_AttendeeID" });
            DropIndex("dbo.ConferenceAttendees", new[] { "Conference_ConferenceID" });
            DropIndex("dbo.Cities", new[] { "Country_CountryID" });
            DropIndex("dbo.Tags", new[] { "Conference_ConferenceID" });
            DropIndex("dbo.Evaluations", new[] { "Speaker_AttendeeID" });
            DropIndex("dbo.Inspirations", new[] { "Conference_ConferenceID" });
            DropIndex("dbo.Conferences", new[] { "TargetCountryId" });
            DropIndex("dbo.Conferences", new[] { "TargetCityId" });
            DropIndex("dbo.Attendees", new[] { "Attendee_AttendeeID" });
            DropTable("dbo.SpeakerConferences");
            DropTable("dbo.OrganizerConferences");
            DropTable("dbo.ConferenceAttendees");
            DropTable("dbo.Countries");
            DropTable("dbo.Cities");
            DropTable("dbo.Tags");
            DropTable("dbo.Evaluations");
            DropTable("dbo.Inspirations");
            DropTable("dbo.Conferences");
            DropTable("dbo.Attendees");
        }
    }
}
