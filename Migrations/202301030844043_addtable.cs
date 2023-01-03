namespace TablularExtractor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LineNumber = c.Int(nullable: false),
                        PageNumber = c.Int(nullable: false),
                        FilePath = c.String(),
                        PassportNumber = c.String(),
                        PersonalNumber = c.String(),
                        Expiry = c.String(),
                        LabourCard = c.String(),
                        Country = c.String(),
                        EmployeeName = c.String(),
                        Job = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Employees");
        }
    }
}
