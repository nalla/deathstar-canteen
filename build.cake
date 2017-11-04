var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

Task("Publish")
  .IsDependentOn("Test")
  .Does(() =>
{
  DotNetCorePublish("./src/Deathstar.Canteen/Deathstar.Canteen.csproj", new DotNetCorePublishSettings
  {
    Configuration = configuration
  });
});

Task("Test")
  .Does(() =>
{
  DotNetCoreTest("./src/Deathstar.Canteen.Tests/Deathstar.Canteen.Tests.csproj");
});

Task("Run")
  .Does(() =>
{
  DotNetCoreRun("Deathstar.Canteen.csproj", "", new DotNetCoreRunSettings
  {
    WorkingDirectory = "./src/Deathstar.Canteen/"
  });
});

Task("Default").IsDependentOn("Test");

RunTarget(target);
