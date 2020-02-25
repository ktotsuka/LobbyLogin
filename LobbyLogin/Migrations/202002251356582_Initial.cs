namespace LobbyLogin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeeWrappers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Employee_FirstName = c.String(),
                        Employee_LastName = c.String(),
                        Employee_EmailAddress = c.String(),
                        Employee_CellPhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VisitorWrappers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Visitor_FirstName = c.String(),
                        Visitor_LastName = c.String(),
                        Visitor_CompanyName = c.String(),
                        Visitor_EmailAddress = c.String(),
                        Visitor_PhoneNumber = c.String(),
                        Visitor_HostId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Visits",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Visitor_FirstName = c.String(),
                        Visitor_LastName = c.String(),
                        Visitor_CompanyName = c.String(),
                        Visitor_EmailAddress = c.String(),
                        Visitor_PhoneNumber = c.String(),
                        Visitor_HostId = c.String(),
                        Employee_FirstName = c.String(),
                        Employee_LastName = c.String(),
                        Employee_EmailAddress = c.String(),
                        Employee_CellPhoneNumber = c.String(),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Visits");
            DropTable("dbo.VisitorWrappers");
            DropTable("dbo.EmployeeWrappers");
        }
    }
}
