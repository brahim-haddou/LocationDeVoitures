namespace LocationDeVoitures.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dat : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Administrateurs",
                c => new
                    {
                        AdministrateurID = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false),
                        Prenom = c.String(nullable: false),
                        CIN = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Ville = c.String(nullable: false),
                        Tel = c.String(nullable: false),
                        UserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AdministrateurID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Agences",
                c => new
                    {
                        AgenceID = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false),
                        CodeAgence = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Ville = c.String(nullable: false),
                        Tel = c.String(nullable: false),
                        NCardCredit = c.Long(nullable: false),
                        AddressPayPal = c.String(nullable: false),
                        UserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AgenceID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.ImagesVoitures",
                c => new
                    {
                        ImagesVoitureID = c.Int(nullable: false, identity: true),
                        Image = c.Binary(nullable: false),
                        VoitureID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ImagesVoitureID)
                .ForeignKey("dbo.Voitures", t => t.VoitureID, cascadeDelete: true)
                .Index(t => t.VoitureID);
            
            CreateTable(
                "dbo.Voitures",
                c => new
                    {
                        VoitureID = c.Int(nullable: false, identity: true),
                        Matricule = c.String(nullable: false),
                        Prix = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Module = c.Int(nullable: false),
                        Marque = c.String(nullable: false),
                        Km = c.Single(nullable: false),
                        Couleur = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Image = c.Binary(),
                        AgenceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VoitureID)
                .ForeignKey("dbo.Agences", t => t.AgenceID, cascadeDelete: true)
                .Index(t => t.AgenceID);
            
            CreateTable(
                "dbo.ListFavoris",
                c => new
                    {
                        ListFavorisID = c.Int(nullable: false, identity: true),
                        UserID = c.String(maxLength: 128),
                        UserFavorisID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ListFavorisID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserFavorisID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.UserFavorisID);
            
            CreateTable(
                "dbo.ListNoires",
                c => new
                    {
                        ListNoireID = c.Int(nullable: false, identity: true),
                        UserID = c.String(maxLength: 128),
                        UserNoireID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ListNoireID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserNoireID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.UserNoireID);
            
            CreateTable(
                "dbo.Locataires",
                c => new
                    {
                        LocataireID = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false),
                        Prenom = c.String(nullable: false),
                        CIN = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Ville = c.String(nullable: false),
                        Tel = c.String(nullable: false),
                        NCardCredit = c.Long(nullable: false),
                        AddressPayPal = c.String(nullable: false),
                        UserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.LocataireID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationID = c.Int(nullable: false, identity: true),
                        VoitureID = c.Int(nullable: false),
                        LocataireID = c.Int(nullable: false),
                        Data = c.DateTime(nullable: false),
                        Duree = c.Int(nullable: false),
                        ChoiseDePayment = c.String(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LocationID)
                .ForeignKey("dbo.Locataires", t => t.LocataireID, cascadeDelete: true)
                .ForeignKey("dbo.Voitures", t => t.VoitureID, cascadeDelete: true)
                .Index(t => t.VoitureID)
                .Index(t => t.LocataireID);
            
            CreateTable(
                "dbo.Offres",
                c => new
                    {
                        OffreID = c.Int(nullable: false, identity: true),
                        Pourcentage = c.Single(nullable: false),
                        Data = c.DateTime(nullable: false),
                        Duree = c.Int(nullable: false),
                        VoitureID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OffreID)
                .ForeignKey("dbo.Voitures", t => t.VoitureID, cascadeDelete: true)
                .Index(t => t.VoitureID);
            
            CreateTable(
                "dbo.Problemes",
                c => new
                    {
                        ProblemeID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        UserPlaignantID = c.String(maxLength: 128),
                        UserDefendeurID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ProblemeID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserDefendeurID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserPlaignantID)
                .Index(t => t.UserPlaignantID)
                .Index(t => t.UserDefendeurID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Problemes", "UserPlaignantID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Problemes", "UserDefendeurID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Offres", "VoitureID", "dbo.Voitures");
            DropForeignKey("dbo.Locations", "VoitureID", "dbo.Voitures");
            DropForeignKey("dbo.Locations", "LocataireID", "dbo.Locataires");
            DropForeignKey("dbo.Locataires", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.ListNoires", "UserNoireID", "dbo.AspNetUsers");
            DropForeignKey("dbo.ListNoires", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.ListFavoris", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.ListFavoris", "UserFavorisID", "dbo.AspNetUsers");
            DropForeignKey("dbo.ImagesVoitures", "VoitureID", "dbo.Voitures");
            DropForeignKey("dbo.Voitures", "AgenceID", "dbo.Agences");
            DropForeignKey("dbo.Agences", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Administrateurs", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Problemes", new[] { "UserDefendeurID" });
            DropIndex("dbo.Problemes", new[] { "UserPlaignantID" });
            DropIndex("dbo.Offres", new[] { "VoitureID" });
            DropIndex("dbo.Locations", new[] { "LocataireID" });
            DropIndex("dbo.Locations", new[] { "VoitureID" });
            DropIndex("dbo.Locataires", new[] { "UserID" });
            DropIndex("dbo.ListNoires", new[] { "UserNoireID" });
            DropIndex("dbo.ListNoires", new[] { "UserID" });
            DropIndex("dbo.ListFavoris", new[] { "UserFavorisID" });
            DropIndex("dbo.ListFavoris", new[] { "UserID" });
            DropIndex("dbo.Voitures", new[] { "AgenceID" });
            DropIndex("dbo.ImagesVoitures", new[] { "VoitureID" });
            DropIndex("dbo.Agences", new[] { "UserID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Administrateurs", new[] { "UserID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Problemes");
            DropTable("dbo.Offres");
            DropTable("dbo.Locations");
            DropTable("dbo.Locataires");
            DropTable("dbo.ListNoires");
            DropTable("dbo.ListFavoris");
            DropTable("dbo.Voitures");
            DropTable("dbo.ImagesVoitures");
            DropTable("dbo.Agences");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Administrateurs");
        }
    }
}
