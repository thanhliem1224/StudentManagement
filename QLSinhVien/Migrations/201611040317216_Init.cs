namespace QLSinhVien.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DangKyKhoaHoc",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SinhVienID = c.Int(nullable: false),
                        KhoaHocID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.KhoaHoc", t => t.KhoaHocID, cascadeDelete: true)
                .ForeignKey("dbo.SinhVien", t => t.SinhVienID, cascadeDelete: true)
                .Index(t => t.SinhVienID)
                .Index(t => t.KhoaHocID);
            
            CreateTable(
                "dbo.KhoaHoc",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TenKhoaHoc = c.String(nullable: false),
                        ThoiGianBatDau = c.DateTime(nullable: false),
                        ThoiGianKetThuc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.KhoaHoc_MonHoc",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        KhoaHocID = c.Int(nullable: false),
                        MonHocID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.KhoaHoc", t => t.KhoaHocID, cascadeDelete: true)
                .ForeignKey("dbo.MonHoc", t => t.MonHocID, cascadeDelete: true)
                .Index(t => t.KhoaHocID)
                .Index(t => t.MonHocID);
            
            CreateTable(
                "dbo.MonHoc",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TenMonHoc = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SinhVien",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Ho = c.String(),
                        Ten = c.String(),
                        GioiTinh = c.String(),
                        NgaySinh = c.DateTime(nullable: false),
                        NoiSinh = c.String(),
                        DiaChi = c.String(),
                        SoDienThoai = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.IdentityRole",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserRole",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.IdentityRole", t => t.IdentityRole_Id)
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .Index(t => t.IdentityRole_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ApplicationUser",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.IdentityUserLogin",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityUserRole", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserLogin", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserClaim", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserRole", "IdentityRole_Id", "dbo.IdentityRole");
            DropForeignKey("dbo.DangKyKhoaHoc", "SinhVienID", "dbo.SinhVien");
            DropForeignKey("dbo.KhoaHoc_MonHoc", "MonHocID", "dbo.MonHoc");
            DropForeignKey("dbo.KhoaHoc_MonHoc", "KhoaHocID", "dbo.KhoaHoc");
            DropForeignKey("dbo.DangKyKhoaHoc", "KhoaHocID", "dbo.KhoaHoc");
            DropIndex("dbo.IdentityUserLogin", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserClaim", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserRole", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserRole", new[] { "IdentityRole_Id" });
            DropIndex("dbo.KhoaHoc_MonHoc", new[] { "MonHocID" });
            DropIndex("dbo.KhoaHoc_MonHoc", new[] { "KhoaHocID" });
            DropIndex("dbo.DangKyKhoaHoc", new[] { "KhoaHocID" });
            DropIndex("dbo.DangKyKhoaHoc", new[] { "SinhVienID" });
            DropTable("dbo.IdentityUserLogin");
            DropTable("dbo.IdentityUserClaim");
            DropTable("dbo.ApplicationUser");
            DropTable("dbo.IdentityUserRole");
            DropTable("dbo.IdentityRole");
            DropTable("dbo.SinhVien");
            DropTable("dbo.MonHoc");
            DropTable("dbo.KhoaHoc_MonHoc");
            DropTable("dbo.KhoaHoc");
            DropTable("dbo.DangKyKhoaHoc");
        }
    }
}
