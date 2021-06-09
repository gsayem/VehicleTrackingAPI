setlocal 
SET VT_API_SQL_CONNECTION=Data Source=SAYEM-PC;Initial Catalog=VehicleTrackingDB;Integrated Security=True;MultipleActiveResultSets=True
endlocal
dotnet tool install --global dotnet-ef
dotnet ef  database update
