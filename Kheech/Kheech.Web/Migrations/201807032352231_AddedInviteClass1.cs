namespace Kheech.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedInviteClass1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InviteFriends",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InitiatorId = c.String(),
                        Email = c.String(),
                        FriendshipStatusId = c.Int(nullable: false),
                        InsertDate = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.FriendshipStatus", t => t.FriendshipStatusId, cascadeDelete: true)
                .Index(t => t.FriendshipStatusId)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InviteFriends", "FriendshipStatusId", "dbo.FriendshipStatus");
            DropForeignKey("dbo.InviteFriends", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.InviteFriends", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.InviteFriends", new[] { "FriendshipStatusId" });
            DropTable("dbo.InviteFriends");
        }
    }
}
