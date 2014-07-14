using System;
using System.Net;

using RestSharp;

namespace Hook
{
	public class Request
	{
		public RestClient client;
		public RestRequest request;

		public Request (RestClient client, RestRequest request)
		{
			this.client = client;
			this.request = request;
		}

		public RestRequestAsyncHandle OnSuccess(Action<IRestResponse> callback)
		{
			return this.client.ExecuteAsync(this.request, callback);
		}

		protected void OnCompleted(object sender, EventArgs e)
		{
		}

		public delegate void CompletedHandler(object sender, EventArgs e);
		public delegate void SuccessHandler(object sender, EventArgs e);
		public delegate void ErrorHandler(object sender, EventArgs e);
	}
}

