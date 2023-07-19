namespace PSIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stockreturndetails_status : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalesReturnDetail", "status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SalesReturnDetail", "status");
        }
    }
}
