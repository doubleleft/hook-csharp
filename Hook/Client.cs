using System;
using System.Net;
using System.Collections.Generic;
using System.Collections.Specialized;

using RestSharp;

namespace Hook
{
	public class Client
	{
		protected String appId;
		protected String key;
		protected RestClient rest;

		public System System;
		public Auth Auth;

		public Client (string appId, string key, string url)
		{
			this.appId = appId;
			this.key = key;
			this.rest = new RestClient (url);

			this.Auth = new Auth (this);
			this.System = new System (this);

		}

		public Collection Collection(string name)
		{
			return new Collection (this, name);
		}

		public Channel Channel(string name)
		{
			return new WebSocket (this, name);
		}

		public Request Get(string segments, Object data)
		{
			return this.Request (Method.GET, segments, data);
		}

		public Request Put(string segments, Object data)
		{
			return this.Request (Method.PUT, segments, data);
		}

		public Request Post(string segments, Object data)
		{
			return this.Request (Method.POST, segments, data);
		}

		public Request Remove(string segments, Object data)
		{
			return this.Request (Method.DELETE, segments, data);
		}

		protected Request Request(Method method, string segments, Object data)
		{
			var request = new RestRequest (segments, method);

			foreach (var header in this.GetHeaders()) {
				request.AddHeader (header.Key, header.Value);
			}

			// Always exchange data using JSON
			request.RequestFormat = DataFormat.Json;
			request.AddBody (data);

			return new Request(rest, request);
		}

		protected Dictionary<string, string> GetHeaders()
		{
			var dict = new Dictionary<string, string> ();
			dict.Add ("X-App-Id", this.appId);
			dict.Add ("X-App-Key", this.key);

			var authToken = this.Auth.GetToken ();
			if (authToken != null) {
				dict.Add ("X-Auth-Token", authToken);
			}

			dict.Add ("User-Agent", "hook-csharp");

			return dict;
		}

	}
}

