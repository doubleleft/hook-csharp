using System;
using System.Net;

using RestSharp;
using Newtonsoft.Json;

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

		public RestRequestAsyncHandle ContinueWith(Action<Object> callback)
		{
			return this.client.ExecuteAsync(this.request, response => {
				Object data = JsonConvert.DeserializeObject(response.Content);
				if (response.StatusCode != HttpStatusCode.OK) {
					throw new Exception(response.StatusCode.ToString()); // data["error"].Value
				} else {
					callback(data);
				}
			});
		}

		protected void OnCompleted(object sender, EventArgs e)
		{
		}

		public delegate void CompletedHandler(object sender, EventArgs e);
		public delegate void SuccessHandler(object sender, EventArgs e);
		public delegate void ErrorHandler(object sender, EventArgs e);
	}
}

