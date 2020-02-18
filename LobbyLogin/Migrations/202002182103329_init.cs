namespace LobbyLogin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmailAddress = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        CellPhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.EmailAddress);
            
            CreateTable(
                "dbo.Visitors",
                c => new
                    {
                        Code = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        CompanyName = c.String(),
                        EmailAddress = c.String(),
                        PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.Visits",
                c => new
                    {
                        Code = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        Employee_EmailAddress = c.String(maxLength: 128),
                        Visitor_Code = c.Int(),
                    })
                .PrimaryKey(t => t.Code)
                .ForeignKey("dbo.Employees", t => t.Employee_EmailAddress)
                .ForeignKey("dbo.Visitors", t => t.Visitor_Code)
                .Index(t => t.Employee_EmailAddress)
                .Index(t => t.Visitor_Code);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Visits", "Visitor_Code", "dbo.Visitors");
            DropForeignKey("dbo.Visits", "Employee_EmailAddress", "dbo.Employees");
            DropIndex("dbo.Visits", new[] { "Visitor_Code" });
            DropIndex("dbo.Visits", new[] { "Employee_EmailAddress" });
            DropTable("dbo.Visits");
            DropTable("dbo.Visitors");
            DropTable("dbo.Employees");
        }
    }
}
