# VehicleTrackingAPI
Vehicle Tracking API
First install below command from terminal,
1. dotnet tool install --global dotnet-ef

Please check the database connection string before run.
VT_API_SQL_CONNECTION for WEBAPI database
VT_IDENTITY_SQL_CONNECTION for IdentityServer database.

Please goto VehicleTrackingSolution and make below projects as startup,
1. VehicleTracking.IdentityServer.Web
2. VehicleTracking.WebAPI
3. VehicleTracking.Seeder

Then Please follow the instruction of VehicleTracking.Seeder application, It'll create database and seed with default data.
Then you can test the API through swagger.

It's basic Onion Architecture with claim based IdentityServer authentication 
Feel free to knock me If you face any kinds of difficulties to run the application.


