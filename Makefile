upload:
	rm *.nupkg
	NuGet Pack Package.nuspec
	NuGet Push *.nupkg
