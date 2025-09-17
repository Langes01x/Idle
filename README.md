Idle Game Prototype
===================

A simple idle game prototype made with ASP.Net Core.


Usage
=====

Requirements:
- Visual Studio / VSCode
- .Net 8.0
- .Net EF tool - Can be installed using `dotnet tool install --global dotnet-ef`

Running:
1. Open solution file.
2. Create the database using `dotnet ef database update --project IdleDB --startup-project IdleUI`
3. Run IdleUI project.

Schema Changes:
1. Make changes to the schema objects in IdleCore
2. Add DbSet properties for any new schema objects to ApplicationDbContext in IdleDB
3. Add configuration for any primary keys, foreign keys, etc. to OnModelCreating
4. Update the database using `dotnet ef database update --project IdleDB --startup-project IdleUI`
