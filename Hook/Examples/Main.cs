using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;

using Hook;
using RestSharp;

class Post {
	public int _id;
	public string title;
	public int score;
	public DateTime date;
}

namespace Examples
{
	class MainClass
	{
		private static RestRequestAsyncHandle req;
		static void Main (string[] args)
		{
			Client client = new Client("2", "a18632aa82be8e925ef349164314311a", "http://hook.dev/public/index.php/");
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

			req = posts.Sort ("created_at", Order.DESCENDING).Limit(1).First().ContinueWith<Post> (data => {
				Console.WriteLine("Post id: ");
				Console.WriteLine(data._id);
			});

			NSApplication.Init ();
			NSApplication.Main (args);
		}
	}
}

