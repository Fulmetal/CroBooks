var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("CroBooksPostgress")
    .WithPgAdmin()
    .WithDataVolume(isReadOnly: false);
var postgresdb = postgres.AddDatabase("CrobooksDb", "postgres");

var apiService = builder.AddProject<Projects.CroBooks_ApiService>("ApiService")
    .WithReference(postgresdb)
    .WaitFor(postgres)
    .WaitFor(postgresdb);

var web = builder.AddProject<Projects.CroBooks_Web>("Web")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);



builder.Build().Run();
