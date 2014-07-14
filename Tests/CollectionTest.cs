using NUnit.Framework;
using System;

using Hook;

namespace Tests
{
	[TestFixture ()]
	public class CollectionTest
	{
		protected Client client;

		[SetUp]
		public void Setup()
		{
			client = new Client("1", "cc36d8ba02a293d7842a6a7028d16819", "http://hook.dev/index.php/");
		}

		[Test ()]
		public void TestCase ()
		{
			Collection posts = client.Collection ("posts");
			posts.Create (new {
				title = "Something"
			});

		}
	}
}

