namespace Kheech.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSetUp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Friendships",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId1 = c.String(),
                        ApplicationUserId2 = c.String(),
                        FriendshipStatusId = c.Int(nullable: false),
                        ApplicationUser1_Id = c.String(maxLength: 128),
                        ApplicationUser2_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser1_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser2_Id)
                .ForeignKey("dbo.FriendshipStatus", t => t.FriendshipStatusId, cascadeDelete: true)
                .Index(t => t.FriendshipStatusId)
                .Index(t => t.ApplicationUser1_Id)
                .Index(t => t.ApplicationUser2_Id);
            
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
                "dbo.FriendshipStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.KheechComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Discussion = c.String(),
                        KheechEventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.KheechEvents", t => t.KheechEventId, cascadeDelete: true)
                .Index(t => t.KheechEventId);
            
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
                        GroupId = c.Int(nullable: false),
                        IsGroupEvent = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.LocationId)
                .Index(t => t.GroupId);
            
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
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.KheechUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KheechEventId = c.Int(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.KheechEvents", t => t.KheechEventId, cascadeDelete: true)
                .Index(t => t.KheechEventId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.Moments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KheechEventId = c.Int(nullable: false),
                        Capture = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.KheechEvents", t => t.KheechEventId, cascadeDelete: true)
                .Index(t => t.KheechEventId);
            
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
            DropForeignKey("dbo.Moments", "KheechEventId", "dbo.KheechEvents");
            DropForeignKey("dbo.KheechUsers", "KheechEventId", "dbo.KheechEvents");
            DropForeignKey("dbo.KheechUsers", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.KheechComments", "KheechEventId", "dbo.KheechEvents");
            DropForeignKey("dbo.KheechEvents", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.KheechEvents", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.KheechEvents", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.GroupUsers", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.GroupUsers", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Groups", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Friendships", "FriendshipStatusId", "dbo.FriendshipStatus");
            DropForeignKey("dbo.Friendships", "ApplicationUser2_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Friendships", "ApplicationUser1_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Moments", new[] { "KheechEventId" });
            DropIndex("dbo.KheechUsers", new[] { "ApplicationUserId" });
            DropIndex("dbo.KheechUsers", new[] { "KheechEventId" });
            DropIndex("dbo.KheechEvents", new[] { "GroupId" });
            DropIndex("dbo.KheechEvents", new[] { "LocationId" });
            DropIndex("dbo.KheechEvents", new[] { "ApplicationUserId" });
            DropIndex("dbo.KheechComments", new[] { "KheechEventId" });
            DropIndex("dbo.GroupUsers", new[] { "ApplicationUserId" });
            DropIndex("dbo.GroupUsers", new[] { "GroupId" });
            DropIndex("dbo.Groups", new[] { "ApplicationUserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Friendships", new[] { "ApplicationUser2_Id" });
            DropIndex("dbo.Friendships", new[] { "ApplicationUser1_Id" });
            DropIndex("dbo.Friendships", new[] { "FriendshipStatusId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Moments");
            DropTable("dbo.KheechUsers");
            DropTable("dbo.Locations");
            DropTable("dbo.KheechEvents");
            DropTable("dbo.KheechComments");
            DropTable("dbo.GroupUsers");
            DropTable("dbo.Groups");
            DropTable("dbo.FriendshipStatus");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Friendships");
        }
    }
}
