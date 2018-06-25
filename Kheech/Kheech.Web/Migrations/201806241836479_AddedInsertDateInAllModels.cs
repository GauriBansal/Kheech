namespace Kheech.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedInsertDateInAllModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Friendships", "InsertDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.KheechEvents", "InsertDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.KheechComments", "InsertDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Locations", "InsertDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.KheechUsers", "IsAccepted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Moments", "InsertDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Moments", "InsertDate");
            DropColumn("dbo.KheechUsers", "IsAccepted");
            DropColumn("dbo.Locations", "InsertDate");
            DropColumn("dbo.KheechComments", "InsertDate");
            DropColumn("dbo.KheechEvents", "InsertDate");
            DropColumn("dbo.Friendships", "InsertDate");
        }
    }
}
