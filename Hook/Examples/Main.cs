using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;

using Hook;
using RestSharp;

namespace Examples
{
	class MainClass
	{
		static void Main (string[] args)
		{
			Client client = new Client("1", "cc36d8ba02a293d7842a6a7028d16819", "http://hook.dev/index.php/");
			Collection posts = client.Collection ("posts");

			var data = posts.Create (new {
				title = "Hello there!"
			}).ContinueWith (result => {
				Console.WriteLine(result.ToString());
			});

			NSApplication.Init ();
			NSApplication.Main (args);
		}
	}
}

