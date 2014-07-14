using System;
using Hook;

using RestSharp;

namespace ConsoleTest
{
	class MainClass
	{

		public static void Main (string[] args)
		{
			Client client = new Client("1", "cc36d8ba02a293d7842a6a7028d16819", "http://hook.dev/index.php/");
			Collection posts = client.Collection ("posts");

			var data = posts.Create (new {
				title = "Hello there!"
			});

			data.OnSuccess (result => {
				Console.WriteLine(result.ToString());
			});

			var i = 5;
		}
	}
}
