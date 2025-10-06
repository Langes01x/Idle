Idle Game Prototype
===================

A simple idle game prototype made with ASP.Net Core.


Usage
=====

Requirements:
- Visual Studio / VSCode
- .Net 8.0
- .Net EF tool - Can be installed using `dotnet tool install --global dotnet-ef`
- Node.js 22.20

Running:
1. Open solution file.
2. Create the database using `dotnet ef database update --project IdleDB --startup-project IdleAPI`
3. Run IdleAPI project.
4. Open terminal in IdleViteUI folder.
5. Restore dependencies using `npm install`
6. Run react UI using `npx vite --port=4000`

Schema Changes:
1. Make changes to the schema objects in IdleCore
2. Add DbSet properties for any new schema objects to ApplicationDbContext in IdleDB
3. Add configuration for any primary keys, foreign keys, etc. to OnModelCreating
4. Create a migration using `dotnet ef migrations add <MigrationName> --project IdleDB --startup-project IdleAPI`
4. Update the database using `dotnet ef database update --project IdleDB --startup-project IdleAPI`
