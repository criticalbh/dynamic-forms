namespace AdmirSabanovic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Forms",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 160),
                        ShortDescription = c.String(),
                        Description = c.String(),
                        Created_at = c.DateTime(nullable: false),
                        Updated_at = c.DateTime(nullable: false),
                        Admin_Email = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Admins", t => t.Admin_Email)
                .Index(t => t.Admin_Email);
            
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Email = c.String(nullable: false, maxLength: 150),
                        ID = c.Int(nullable: false, identity: true),
                        Password = c.String(nullable: false, maxLength: 20),
                        Name = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Email);
            
            CreateTable(
                "dbo.Dynamics",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Form_generated = c.String(),
                        form_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Forms", t => t.form_ID)
                .Index(t => t.form_ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Token = c.String(),
                        Name = c.String(),
                        Surname = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Additionals",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.String(),
                        UserID_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.UserID_ID)
                .Index(t => t.UserID_ID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Additionals", new[] { "UserID_ID" });
            DropIndex("dbo.Dynamics", new[] { "form_ID" });
            DropIndex("dbo.Forms", new[] { "Admin_Email" });
            DropForeignKey("dbo.Additionals", "UserID_ID", "dbo.Users");
            DropForeignKey("dbo.Dynamics", "form_ID", "dbo.Forms");
            DropForeignKey("dbo.Forms", "Admin_Email", "dbo.Admins");
            DropTable("dbo.Additionals");
            DropTable("dbo.Users");
            DropTable("dbo.Dynamics");
            DropTable("dbo.Admins");
            DropTable("dbo.Forms");
        }
    }
}
