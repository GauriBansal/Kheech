namespace Kheech.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovingM2MRelationships : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Friendships", "ApplicationUser1_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Friendships", "ApplicationUser2_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Friendships", new[] { "ApplicationUser1_Id" });
            DropIndex("dbo.Friendships", new[] { "ApplicationUser2_Id" });
            DropColumn("dbo.Friendships", "ApplicationUserId1");
            DropColumn("dbo.Friendships", "ApplicationUserId2");
            DropColumn("dbo.Friendships", "ApplicationUser1_Id");
            DropColumn("dbo.Friendships", "ApplicationUser2_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Friendships", "ApplicationUser2_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Friendships", "ApplicationUser1_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Friendships", "ApplicationUserId2", c => c.String());
            AddColumn("dbo.Friendships", "ApplicationUserId1", c => c.String());
            CreateIndex("dbo.Friendships", "ApplicationUser2_Id");
            CreateIndex("dbo.Friendships", "ApplicationUser1_Id");
            AddForeignKey("dbo.Friendships", "ApplicationUser2_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Friendships", "ApplicationUser1_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
