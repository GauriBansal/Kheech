namespace Kheech.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedCreatorIdInMomentsandComments1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Moments", name: "ApplicationUser_Id", newName: "ApplicationUserId");
            RenameIndex(table: "dbo.Moments", name: "IX_ApplicationUser_Id", newName: "IX_ApplicationUserId");
            DropColumn("dbo.Moments", "CreatorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Moments", "CreatorId", c => c.String());
            RenameIndex(table: "dbo.Moments", name: "IX_ApplicationUserId", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.Moments", name: "ApplicationUserId", newName: "ApplicationUser_Id");
        }
    }
}
