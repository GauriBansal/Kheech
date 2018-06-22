namespace Kheech.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeddatabase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Moments", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Moments", "Description");
        }
    }
}
