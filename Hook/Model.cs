using System;
using System.Collections.Generic;
using System.Reflection;

// TODO

namespace Hook
{
	public class Model : Collection
	{

		public Model (object attributes = null) : base(Client.GetInstance (), this.GetType ().Name.ToLower ()) {
			if (attributes != null) {
				this.SetAttributes (attributes);
			}
		}

		public static Request Create(object data = null)
		{
			Model instance = Activator.CreateInstance (this.GetType (), data);
			instance.Create ();

		}

		public Request Create()
		{
			return base.Create (this.GetAttributeValues());
		}

		public void SetAttributes(object attributes)
		{

		}

		public string[] GetAttributes()
		{
			PropertyInfo[] allProperties = this.GetType().GetProperties ();
			string[] properties = new string[allProperties.Length];
			for (var i = 0; i < allProperties.Length; i++) {
				properties [i] = allProperties [i].Name;
			}
			return properties;
		}

		protected Dictionary<string, object> GetAttributeValues()
		{
			Dictionary<string, object> data;

			foreach (var property in this.GetAttributes())
			{
				var propertyInfo = GetType ().GetProperty (property);
				data [property] = propertyInfo.GetValue (this, null);
			}

			return data;
		}

	}
}

