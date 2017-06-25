namespace StructuredDebate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BoolsAreTooTricky : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Arguments", "Affirmative", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Arguments", "Affirmative", c => c.Boolean(nullable: false));
        }
    }
}
