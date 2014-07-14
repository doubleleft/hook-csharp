using System;

namespace Hook
{
	public class Auth
	{
		protected Client client;

		public Auth (Client client)
		{
			this.client = client;
		}

		public string GetToken()
		{
			return null;
		}
	}
}

