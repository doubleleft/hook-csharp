using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using RestSharp;

namespace Hook
{
	public enum Order { ASCENDING, DESCENDING };

	public class Collection
	{
		protected Client client;

		protected String name;
		protected string segments;

		protected Dictionary<string, Object> options;
		protected List<Object[]> wheres;
		protected List<string[]> ordering;
		protected List<string> _group;
		protected Object _limit;
		protected Object _offset;
		protected Object _remember;

		public Collection (Client client, String name)
		{
			if (!this.ValidateName (name)) {
				throw new Exception ("Invalid Collection name.");
			}

			this.client = client;

			this.name = name;
			this.Reset ();

			this.segments = "collection/" + this.name;
		}

		public Request Get()
		{
			return this.client.Get (this.segments, this.BuildQuery ());
		}
			
		public RestRequestAsyncHandle ContinueWith<TResult>(Action<TResult> callback)
		{
			return this.Get ().ContinueWith<TResult> (callback);
		}

		protected void Reset()
		{
			this.options = new Dictionary<string, Object>();
			this.wheres = new List<Object[]>();
			this.ordering = new List<string[]>();
			this._group = new List<string>();
			this._limit = null;
			this._offset = null;
			this._remember = null;
		}

		protected Object BuildQuery()
		{
			var query = new Dictionary<string, Object>();

			// apply limit / offset and remember
			if (this._limit != null) { query["limit"] = this._limit; }
			if (this._offset != null) { query["offset"] = this._offset; }
			if (this._remember != null) { query["remember"] = this._remember; }

			// apply wheres
			if (this.wheres.Count > 0) {
				query["q"] = this.wheres;
			}

			// apply ordering
			if (this.ordering.Count > 0) {
				query["s"] = this.ordering;
			}

			// apply group
			if (this._group.Count > 0) {
				query["g"] = this._group;
			}

			var shortnames = new Dictionary<string, string> () {
				{"paginate", "p"},
				{"first", "f"},
				{"aggregation", "aggr"},
				{"operation", "op"},
				{"data", "data"},
				{"with", "with"},
				{"select", "select"}
			};

			foreach (var f in shortnames) {
				if (this.options.ContainsKey(f.Key)) {
					query[f.Value] = this.options[f.Key];
				}
			}

			// clear wheres/ordering for future calls
			this.Reset();

			return query;
		}

		protected Collection AddWhere(string field, string operation, Object value, string boolean = "and")
		{
			this.wheres.Add (new [] { field, operation, value, boolean });
			return this;
		}

		public Request Create(Object data)
		{
			return this.client.Post (this.segments, data);
		}

		public Collection Where(string field, Object value)
		{
			return this.AddWhere (field, "=", value);
		}

		public Collection Where(string field, string operation, Object value)
		{
			return this.AddWhere (field, operation, value);
		}

		public Collection OrWhere(string field, Object value)
		{
			return this.AddWhere (field, "=", value, "or");
		}

		public Collection OrWhere(string field, string operation, Object value)
		{
			return this.AddWhere (field, operation, value, "or");
		}

		public Request Find(Object _id)
		{
			return this.client.Get (this.segments + "/" + _id.ToString (), this.BuildQuery());
		}

		public Collection Select(params string[] fields)
		{
			this.options ["select"] = fields;
			return this;
		}

		public Collection With(params string[] relation)
		{
			this.options ["with"] = relation;
			return this;
		}

		public Collection Group(params string[] relation)
		{
			foreach (var r in relation) {
				this._group.Add (r);
			}
			return this;
		}

		public Request Count(string field = "*")
		{
			this.options["aggregation"] = new { method = "count", field = field };
			return this.Get ();
		}

		public Request Max(string field)
		{
			this.options["aggregation"] = new {method = "max", field = field};
			return this.Get ();
		}

		public Request Min(string field)
		{
			this.options["aggregation"] = new {method = "min", field = field};
			return this.Get ();
		}

		public Request Avg(string field)
		{
			this.options["aggregation"] = new {method = "avg", field = field};
			return this.Get ();
		}

		public Request Sum(string field)
		{
			this.options["aggregation"] = new {method = "sum", field = field};
			return this.Get ();
		}

		public Request First()
		{
			this.options ["first"] = true;
			return this.Get ();
		}

		public Request FirstOrCreate(Object data)
		{
			this.options ["first"] = true;
			this.options ["data"] = data;
			return this.client.Post (this.segments, this.BuildQuery ());
		}

		public Collection Sort(string field, Order direction = Order.ASCENDING)
		{
			string dir = (direction == Order.ASCENDING) ? "asc" : "desc";
			this.ordering.Add (new [] { field, dir });
			return this;
		}

		public Collection Limit(int i)
		{
			this._limit = i;
			return this;
		}

		public Collection Offset(int i)
		{
			this._offset = i;
			return this;
		}

		public Collection Remember(int minutes)
		{
			this._remember = minutes;
			return this;
		}

		public Request Remove(Object _id = null)
		{
			var path = this.segments;
			if (_id != null) {
				path += "/" + _id.ToString ();
			}
			return this.client.Remove (path, this.BuildQuery ());
		}

		public Request Update(Object _id, Object data)
		{
			return this.client.Post (this.segments + "/" + _id.ToString (), data);
		}

		public Request Increment(string field, int value = 1)
		{
			this.options ["operation"] = new { method = "increment", field = field, value = value };
			return this.client.Put(this.segments, this.BuildQuery());
		}

		public Request Decrement(string field, int value = 1)
		{
			this.options ["operation"] = new { method = "decrement", field = field, value = value };
			return this.client.Put(this.segments, this.BuildQuery());
		}

		public Request UpdateAll(Object data)
		{
			this.options ["data"] = data;
			return this.client.Put (this.segments, this.BuildQuery ());
		}

		protected bool ValidateName(string name)
		{
			return Regex.IsMatch(name, "^[a-z_/0-9]+$");
		}

	}
}
