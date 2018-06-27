namespace Kheech.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedCreatorIdInMomentsandComments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KheechComments", "CreatorId", c => c.String());
            AddColumn("dbo.KheechComments", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Moments", "CreatorId", c => c.String());
            AddColumn("dbo.Moments", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.KheechComments", "ApplicationUser_Id");
            CreateIndex("dbo.Moments", "ApplicationUser_Id");
            AddForeignKey("dbo.KheechComments", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Moments", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Moments", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.KheechComments", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Moments", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.KheechComments", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Moments", "ApplicationUser_Id");
            DropColumn("dbo.Moments", "CreatorId");
            DropColumn("dbo.KheechComments", "ApplicationUser_Id");
            DropColumn("dbo.KheechComments", "CreatorId");
        }
    }
}
