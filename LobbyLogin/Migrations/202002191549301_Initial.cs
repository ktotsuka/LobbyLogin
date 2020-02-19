namespace LobbyLogin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Code = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        EmailAddress = c.String(),
                        CellPhoneNumber = c.String(),
                        Id = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
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
                        Id = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.Visits",
                c => new
                    {
                        Code = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        Employee_Code = c.Int(),
                        Visitor_Code = c.Int(),
                    })
                .PrimaryKey(t => t.Code)
                .ForeignKey("dbo.Employees", t => t.Employee_Code)
                .ForeignKey("dbo.Visitors", t => t.Visitor_Code)
                .Index(t => t.Employee_Code)
                .Index(t => t.Visitor_Code);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Visits", "Visitor_Code", "dbo.Visitors");
            DropForeignKey("dbo.Visits", "Employee_Code", "dbo.Employees");
            DropIndex("dbo.Visits", new[] { "Visitor_Code" });
            DropIndex("dbo.Visits", new[] { "Employee_Code" });
            DropTable("dbo.Visits");
            DropTable("dbo.Visitors");
            DropTable("dbo.Employees");
        }
    }
}
