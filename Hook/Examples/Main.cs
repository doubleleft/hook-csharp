using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;

using Hook;
using RestSharp;

class Post : Hook.Model {
	public string title;
	public int score;
	public DateTime date;
}

namespace Examples
{
	class MainClass
	{
		static void Main (string[] args)
		{
			Client client = new Client("1", "cc36d8ba02a293d7842a6a7028d16819", "http://hook.dev/index.php/");
			Collection posts = client.Collection ("posts");

			var post = new Post ();
			post.title = "Hello there!";
			post.score = 15;
			post.date = new DateTime (2014, 07, 07, 17, 30, 0);

			posts.Create (post).ContinueWith<Post> (result => {
				Console.WriteLine(result.ToString());
			});

			posts.Get ().ContinueWith<Post[]> (result => {
				Console.WriteLine(result.ToString());
			});

			NSApplication.Init ();
			NSApplication.Main (args);
		}
	}
}

