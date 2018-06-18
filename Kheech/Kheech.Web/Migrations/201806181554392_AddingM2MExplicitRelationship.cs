namespace Kheech.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingM2MExplicitRelationship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Friendships", "InitiatorId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Friendships", "RecipientId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Friendships", "InitiatorId");
            CreateIndex("dbo.Friendships", "RecipientId");
            AddForeignKey("dbo.Friendships", "InitiatorId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Friendships", "RecipientId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Friendships", "RecipientId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Friendships", "InitiatorId", "dbo.AspNetUsers");
            DropIndex("dbo.Friendships", new[] { "RecipientId" });
            DropIndex("dbo.Friendships", new[] { "InitiatorId" });
            DropColumn("dbo.Friendships", "RecipientId");
            DropColumn("dbo.Friendships", "InitiatorId");
        }
    }
}
