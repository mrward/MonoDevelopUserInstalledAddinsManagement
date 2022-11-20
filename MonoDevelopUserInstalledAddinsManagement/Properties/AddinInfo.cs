using Mono.Addins;
using Mono.Addins.Description;

[assembly:Addin (
	"UserInstalledAddinsManagement",
	Namespace = "MonoDevelop",
	Version = "0.2",
	Category = "IDE extensions")]

[assembly:AddinName ("User Installed Extensions Management")]
[assembly:AddinDescription ("Manages user installed extensions")]
[assembly: AddinDependency ("Core", "17.5")]
[assembly: AddinDependency ("Ide", "17.5")]
