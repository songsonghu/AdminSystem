# Add ApplicationDbContextFactory to fix EF Core migration errors

This PR adds the ApplicationDbContextFactory.cs file to resolve the 'Unable to create a DbContext' error during EF Core migrations.

## Changes
- Add `ApplicationDbContextFactory.cs` to implement `IDesignTimeDbContextFactory<ApplicationDbContext>`
- Enables EF Core tools to create DbContext instances at design time
- Reads connection string from appsettings.json

## Testing
After merging, run:
```
bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```