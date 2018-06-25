namespace Kheech.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullableGroupId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.KheechEvents", "GroupId", "dbo.Groups");
            DropIndex("dbo.KheechEvents", new[] { "GroupId" });
            AlterColumn("dbo.KheechEvents", "GroupId", c => c.Int());
            CreateIndex("dbo.KheechEvents", "GroupId");
            AddForeignKey("dbo.KheechEvents", "GroupId", "dbo.Groups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KheechEvents", "GroupId", "dbo.Groups");
            DropIndex("dbo.KheechEvents", new[] { "GroupId" });
            AlterColumn("dbo.KheechEvents", "GroupId", c => c.Int(nullable: false));
            CreateIndex("dbo.KheechEvents", "GroupId");
            AddForeignKey("dbo.KheechEvents", "GroupId", "dbo.Groups", "Id", cascadeDelete: true);
        }
    }
}
