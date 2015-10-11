using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace SirenMapper
{
	public class SirenAction : DynamicJsonClass
	{
		private readonly List<SirenField> fields;

		public SirenAction()
		{
			this.fields = new List<SirenField>();
		}

		public SirenAction(string name, string href, params SirenField[] fields) : this(name, href, null, null, fields)
		{
		}

		public SirenAction(string name, string href, HttpMethod method, params SirenField[] fields) : this(name, href, null, method, fields)
		{
		}

		public SirenAction(string name, string href, string title, HttpMethod method, params SirenField[] fields) : this()
		{
			this.Name = name;
			this.Href = href;

			if (method != null)
			{
				this.Method = method.Method;
			}
			this.fields.AddRange(fields);
			this.Title = title;
			this.AddItemIfAny("Fields", () => this.fields.Any(), () => this.fields.ToArray());
		}

		public string Name
		{
			get { return GetDictionaryOrDefault<string>("Name"); }
			set { SetDictionary("Name", value); }
		}

		public string Method
		{
			get { return GetDictionaryOrDefault<string>("Method"); }
			set { SetDictionary("Method", value); }
		}

		public string Href
		{
			get { return GetDictionaryOrDefault<string>("Href"); }
			set { SetDictionary("Href", value); }
		}

		public string Title
		{
			get { return GetDictionaryOrDefault<string>("Title"); }
			set { SetDictionary("Title", value); }
		}

		public string Type
		{
			get { return GetDictionaryOrDefault<string>("Type"); }
			set { SetDictionary("Type", value); }
		}

		public void AddField(SirenField field)
		{
			this.fields.Add(field);
		}
	}
}