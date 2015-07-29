namespace SetSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeGameSchema : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Games", "CreateUserId", c => c.Int(nullable: false));
            AddColumn("dbo.Games", "TotalSeconds", c => c.Int());
            CreateIndex("dbo.Games", "CreateUserId");
            AddForeignKey("dbo.Games", "CreateUserId", "dbo.AspNetUsers", "Id", cascadeDelete: false);
            DropColumn("dbo.Games", "StartTime");
            DropColumn("dbo.Games", "EndTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Games", "EndTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Games", "StartTime", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Games", "CreateUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Games", new[] { "CreateUserId" });
            DropColumn("dbo.Games", "TotalSeconds");
            DropColumn("dbo.Games", "CreateUserId");
            DropColumn("dbo.Games", "CreateDate");
        }
    }
}
