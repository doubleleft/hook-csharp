using System;

namespace Hook
{
	public class WebSocket : Channel
	{
		protected WebSocketSharp.WebSocket socket;

		public WebSocket (Client client, string name) : base(client, name)
		{
			this.socket = new WebSocketSharp.WebSocket (client.GetUrl());
		}
	}
}

