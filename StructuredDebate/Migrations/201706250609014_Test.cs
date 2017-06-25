namespace StructuredDebate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TagRelations", "Test", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TagRelations", "Test");
        }
    }
}
