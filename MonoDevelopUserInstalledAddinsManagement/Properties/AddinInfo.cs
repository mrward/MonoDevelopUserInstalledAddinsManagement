using Mono.Addins;
using Mono.Addins.Description;

[assembly:Addin (
	"UserInstalledAddinsManagement",
	Namespace = "MonoDevelop",
	Version = "0.1",
	Category = "IDE extensions")]

[assembly:AddinName ("User Installed Extensions Management")]
[assembly:AddinDescription ("Manages user installed extensions")]
[assembly: AddinDependency ("Core", "8.0")]
[assembly: AddinDependency ("Ide", "8.0")]
