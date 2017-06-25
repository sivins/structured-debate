namespace StructuredDebate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSourceReference : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sources", "CrossExaminationID", "dbo.CrossExaminations");
            DropForeignKey("dbo.Sources", "ArgumentID", "dbo.Arguments");
            DropIndex("dbo.Sources", new[] { "ArgumentID" });
            DropIndex("dbo.Sources", new[] { "CrossExaminationID" });
            DropColumn("dbo.TagRelations", "Test");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TagRelations", "Test", c => c.String());
            CreateIndex("dbo.Sources", "CrossExaminationID");
            CreateIndex("dbo.Sources", "ArgumentID");
            AddForeignKey("dbo.Sources", "ArgumentID", "dbo.Arguments", "ArgumentID");
            AddForeignKey("dbo.Sources", "CrossExaminationID", "dbo.CrossExaminations", "CrossExaminationID");
        }
    }
}
