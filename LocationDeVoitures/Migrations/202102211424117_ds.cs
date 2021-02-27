namespace LocationDeVoitures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ds : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Problemes", "UserDefendeurID", "dbo.AspNetUsers");
            DropIndex("dbo.Problemes", new[] { "UserDefendeurID" });
            AlterColumn("dbo.Problemes", "UserDefendeurID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Problemes", "UserDefendeurID");
            AddForeignKey("dbo.Problemes", "UserDefendeurID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Problemes", "UserDefendeurID", "dbo.AspNetUsers");
            DropIndex("dbo.Problemes", new[] { "UserDefendeurID" });
            AlterColumn("dbo.Problemes", "UserDefendeurID", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Problemes", "UserDefendeurID");
            AddForeignKey("dbo.Problemes", "UserDefendeurID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
