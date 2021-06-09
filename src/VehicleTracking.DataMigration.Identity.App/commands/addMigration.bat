setlocal 
SET VT_IDENTITY_SQL_CONNECTION=Data Source=SAYEM-PC;Initial Catalog=VehicleTrackingIdentityDB;Integrated Security=True;MultipleActiveResultSets=True
endlocal
dotnet tool install --global dotnet-ef
dotnet ef migrations add %1