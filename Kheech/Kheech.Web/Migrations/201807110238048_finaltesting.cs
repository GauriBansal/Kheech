namespace Kheech.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class finaltesting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Friendships",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InitiatorId = c.String(nullable: false, maxLength: 128),
                        RecipientId = c.String(nullable: false, maxLength: 128),
                        FriendshipStatusId = c.Int(nullable: false),
                        InsertDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FriendshipStatus", t => t.FriendshipStatusId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.InitiatorId)
                .ForeignKey("dbo.AspNetUsers", t => t.RecipientId)
                .Index(t => t.InitiatorId)
                .Index(t => t.RecipientId)
                .Index(t => t.FriendshipStatusId);
            
            CreateTable(
                "dbo.FriendshipStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
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
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
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
                "dbo.KheechEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(maxLength: 128),
                        EventName = c.String(),
                        LocationId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        GroupId = c.Int(),
                        IsGroupEvent = c.Boolean(nullable: false),
                        InsertDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Groups", t => t.GroupId)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.LocationId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        GroupImage = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.KheechComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Discussion = c.String(),
                        KheechEventId = c.Int(nullable: false),
                        CreatorId = c.String(),
                        InsertDate = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.KheechEvents", t => t.KheechEventId, cascadeDelete: true)
                .Index(t => t.KheechEventId)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Moments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KheechEventId = c.Int(nullable: false),
                        Capture = c.String(),
                        Description = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                        InsertDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.KheechEvents", t => t.KheechEventId, cascadeDelete: true)
                .Index(t => t.KheechEventId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.KheechUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KheechEventId = c.Int(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                        IsAccepted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.KheechEvents", t => t.KheechEventId, cascadeDelete: true)
                .Index(t => t.KheechEventId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        address1 = c.String(),
                        address2 = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        InsertDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.GroupUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupId = c.Int(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.InviteFriends",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(maxLength: 128),
                        Email = c.String(),
                        FriendshipStatusId = c.Int(nullable: false),
                        InsertDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.FriendshipStatus", t => t.FriendshipStatusId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.FriendshipStatusId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.InviteFriends", "FriendshipStatusId", "dbo.FriendshipStatus");
            DropForeignKey("dbo.InviteFriends", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.GroupUsers", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.GroupUsers", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.KheechEvents", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.KheechUsers", "KheechEventId", "dbo.KheechEvents");
            DropForeignKey("dbo.KheechUsers", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Moments", "KheechEventId", "dbo.KheechEvents");
            DropForeignKey("dbo.Moments", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.KheechComments", "KheechEventId", "dbo.KheechEvents");
            DropForeignKey("dbo.KheechComments", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.KheechEvents", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Groups", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.KheechEvents", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Friendships", "RecipientId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Friendships", "InitiatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Friendships", "FriendshipStatusId", "dbo.FriendshipStatus");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.InviteFriends", new[] { "FriendshipStatusId" });
            DropIndex("dbo.InviteFriends", new[] { "ApplicationUserId" });
            DropIndex("dbo.GroupUsers", new[] { "ApplicationUserId" });
            DropIndex("dbo.GroupUsers", new[] { "GroupId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.KheechUsers", new[] { "ApplicationUserId" });
            DropIndex("dbo.KheechUsers", new[] { "KheechEventId" });
            DropIndex("dbo.Moments", new[] { "ApplicationUserId" });
            DropIndex("dbo.Moments", new[] { "KheechEventId" });
            DropIndex("dbo.KheechComments", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.KheechComments", new[] { "KheechEventId" });
            DropIndex("dbo.Groups", new[] { "ApplicationUserId" });
            DropIndex("dbo.KheechEvents", new[] { "GroupId" });
            DropIndex("dbo.KheechEvents", new[] { "LocationId" });
            DropIndex("dbo.KheechEvents", new[] { "ApplicationUserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Friendships", new[] { "FriendshipStatusId" });
            DropIndex("dbo.Friendships", new[] { "RecipientId" });
            DropIndex("dbo.Friendships", new[] { "InitiatorId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.InviteFriends");
            DropTable("dbo.GroupUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Locations");
            DropTable("dbo.KheechUsers");
            DropTable("dbo.Moments");
            DropTable("dbo.KheechComments");
            DropTable("dbo.Groups");
            DropTable("dbo.KheechEvents");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.FriendshipStatus");
            DropTable("dbo.Friendships");
        }
    }
}
