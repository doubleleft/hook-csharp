using System;
using System.Net;

using RestSharp;

using JsonFx.Serialization;
using JsonFx.Json;

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

		public RestRequestAsyncHandle ContinueWith<TResult>(Action<TResult> callback)
		{
			return this.client.ExecuteAsync(this.request, response => {
				var settings = new DataReaderSettings ();
				var reader = new JsonReader (settings);
				var data = reader.Read<TResult>(response.Content);

				if (response.StatusCode != HttpStatusCode.OK) {
					throw new Exception(response.ErrorMessage);
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

