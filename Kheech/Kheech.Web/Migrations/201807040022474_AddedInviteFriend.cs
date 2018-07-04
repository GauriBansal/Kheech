namespace Kheech.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedInviteFriend : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.InviteFriends", name: "ApplicationUser_Id", newName: "ApplicationUserId");
            RenameIndex(table: "dbo.InviteFriends", name: "IX_ApplicationUser_Id", newName: "IX_ApplicationUserId");
            DropColumn("dbo.InviteFriends", "InitiatorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InviteFriends", "InitiatorId", c => c.String());
            RenameIndex(table: "dbo.InviteFriends", name: "IX_ApplicationUserId", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.InviteFriends", name: "ApplicationUserId", newName: "ApplicationUser_Id");
        }
    }
}
