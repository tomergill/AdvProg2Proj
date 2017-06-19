namespace MazeWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        userName = c.String(nullable: false, maxLength: 128),
                        password = c.String(nullable: false),
                        wins = c.Int(nullable: false),
                        losses = c.Int(nullable: false),
                        firstSignedIn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.userName);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
