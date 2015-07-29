namespace SetSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeElapsedDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Games", "TotalSeconds", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Games", "TotalSeconds", c => c.Int());
        }
    }
}
