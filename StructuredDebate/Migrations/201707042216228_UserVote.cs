namespace StructuredDebate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserVote : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserVotes",
                c => new
                    {
                        UserVoteID = c.Int(nullable: false, identity: true),
                        UserID = c.String(maxLength: 128),
                        PostID = c.Int(nullable: false),
                        Vote = c.String(),
                    })
                .PrimaryKey(t => t.UserVoteID)
                .ForeignKey("dbo.Posts", t => t.PostID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.PostID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserVotes", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserVotes", "PostID", "dbo.Posts");
            DropIndex("dbo.UserVotes", new[] { "PostID" });
            DropIndex("dbo.UserVotes", new[] { "UserID" });
            DropTable("dbo.UserVotes");
        }
    }
}
