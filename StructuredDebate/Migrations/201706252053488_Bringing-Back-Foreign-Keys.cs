namespace StructuredDebate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BringingBackForeignKeys : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Sources", "ArgumentID");
            CreateIndex("dbo.Sources", "CrossExaminationID");
            AddForeignKey("dbo.Sources", "CrossExaminationID", "dbo.CrossExaminations", "CrossExaminationID");
            AddForeignKey("dbo.Sources", "ArgumentID", "dbo.Arguments", "ArgumentID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sources", "ArgumentID", "dbo.Arguments");
            DropForeignKey("dbo.Sources", "CrossExaminationID", "dbo.CrossExaminations");
            DropIndex("dbo.Sources", new[] { "CrossExaminationID" });
            DropIndex("dbo.Sources", new[] { "ArgumentID" });
        }
    }
}
