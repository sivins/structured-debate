namespace StructuredDebate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserVote2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserVotes", "PostID", "dbo.Posts");
            DropIndex("dbo.UserVotes", new[] { "PostID" });
            AddColumn("dbo.UserVotes", "ArgumentID", c => c.Int());
            AddColumn("dbo.UserVotes", "CrossExaminationID", c => c.Int());
            AlterColumn("dbo.UserVotes", "PostID", c => c.Int());
            CreateIndex("dbo.UserVotes", "PostID");
            CreateIndex("dbo.UserVotes", "ArgumentID");
            CreateIndex("dbo.UserVotes", "CrossExaminationID");
            AddForeignKey("dbo.UserVotes", "ArgumentID", "dbo.Arguments", "ArgumentID");
            AddForeignKey("dbo.UserVotes", "CrossExaminationID", "dbo.CrossExaminations", "CrossExaminationID");
            AddForeignKey("dbo.UserVotes", "PostID", "dbo.Posts", "PostID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserVotes", "PostID", "dbo.Posts");
            DropForeignKey("dbo.UserVotes", "CrossExaminationID", "dbo.CrossExaminations");
            DropForeignKey("dbo.UserVotes", "ArgumentID", "dbo.Arguments");
            DropIndex("dbo.UserVotes", new[] { "CrossExaminationID" });
            DropIndex("dbo.UserVotes", new[] { "ArgumentID" });
            DropIndex("dbo.UserVotes", new[] { "PostID" });
            AlterColumn("dbo.UserVotes", "PostID", c => c.Int(nullable: false));
            DropColumn("dbo.UserVotes", "CrossExaminationID");
            DropColumn("dbo.UserVotes", "ArgumentID");
            CreateIndex("dbo.UserVotes", "PostID");
            AddForeignKey("dbo.UserVotes", "PostID", "dbo.Posts", "PostID", cascadeDelete: true);
        }
    }
}
