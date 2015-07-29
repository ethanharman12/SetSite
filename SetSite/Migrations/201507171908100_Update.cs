namespace SetSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Games", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Games", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Games", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Games", "ApplicationUser_Id", c => c.Int());
            CreateIndex("dbo.Games", "ApplicationUser_Id");
            AddForeignKey("dbo.Games", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
