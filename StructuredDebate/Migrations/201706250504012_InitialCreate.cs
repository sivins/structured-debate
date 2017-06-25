namespace StructuredDebate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Arguments",
                c => new
                    {
                        ArgumentID = c.Int(nullable: false, identity: true),
                        PostID = c.Int(nullable: false),
                        Body = c.String(),
                        Affirmative = c.Boolean(nullable: false),
                        Score = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ArgumentID)
                .ForeignKey("dbo.Posts", t => t.PostID, cascadeDelete: true)
                .Index(t => t.PostID);
            
            CreateTable(
                "dbo.CrossExaminations",
                c => new
                    {
                        CrossExaminationID = c.Int(nullable: false, identity: true),
                        ArgumentID = c.Int(nullable: false),
                        Body = c.String(),
                        Score = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CrossExaminationID)
                .ForeignKey("dbo.Arguments", t => t.ArgumentID, cascadeDelete: true)
                .Index(t => t.ArgumentID);
            
            CreateTable(
                "dbo.Sources",
                c => new
                    {
                        SourceID = c.Int(nullable: false, identity: true),
                        ArgumentID = c.Int(),
                        CrossExaminationID = c.Int(),
                        URL = c.String(),
                        PublishedDate = c.DateTime(nullable: false),
                        Author = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.SourceID)
                .ForeignKey("dbo.CrossExaminations", t => t.CrossExaminationID)
                .ForeignKey("dbo.Arguments", t => t.ArgumentID)
                .Index(t => t.ArgumentID)
                .Index(t => t.CrossExaminationID);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostID = c.Int(nullable: false, identity: true),
                        Claim = c.String(),
                        OpeningStatement = c.String(),
                        Score = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PostID);
            
            CreateTable(
                "dbo.TagRelations",
                c => new
                    {
                        TagRelationID = c.Int(nullable: false, identity: true),
                        TagID = c.Int(nullable: false),
                        PostID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TagRelationID)
                .ForeignKey("dbo.Posts", t => t.PostID, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagID, cascadeDelete: true)
                .Index(t => t.TagID)
                .Index(t => t.PostID);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        TagID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.TagID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sources", "ArgumentID", "dbo.Arguments");
            DropForeignKey("dbo.TagRelations", "TagID", "dbo.Tags");
            DropForeignKey("dbo.TagRelations", "PostID", "dbo.Posts");
            DropForeignKey("dbo.Arguments", "PostID", "dbo.Posts");
            DropForeignKey("dbo.Sources", "CrossExaminationID", "dbo.CrossExaminations");
            DropForeignKey("dbo.CrossExaminations", "ArgumentID", "dbo.Arguments");
            DropIndex("dbo.TagRelations", new[] { "PostID" });
            DropIndex("dbo.TagRelations", new[] { "TagID" });
            DropIndex("dbo.Sources", new[] { "CrossExaminationID" });
            DropIndex("dbo.Sources", new[] { "ArgumentID" });
            DropIndex("dbo.CrossExaminations", new[] { "ArgumentID" });
            DropIndex("dbo.Arguments", new[] { "PostID" });
            DropTable("dbo.Tags");
            DropTable("dbo.TagRelations");
            DropTable("dbo.Posts");
            DropTable("dbo.Sources");
            DropTable("dbo.CrossExaminations");
            DropTable("dbo.Arguments");
        }
    }
}
