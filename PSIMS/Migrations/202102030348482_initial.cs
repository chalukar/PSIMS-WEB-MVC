namespace PSIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stock", "PackSize_Qty", c => c.String());
            AddColumn("dbo.Stock", "PacksizeCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stock", "PacksizeCode");
            DropColumn("dbo.Stock", "PackSize_Qty");
        }
    }
}
