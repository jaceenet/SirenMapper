using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace SirenMapper
{
	/// <summary>
	/// An Entity is a URI-addressable resource that has properties and actions associated with it. It may contain sub-entities and navigational links.
	/// </summary>
	public class SirenObject : DynamicJsonClass, ISubEntity
	{
		public SirenObject()
		{
			this.AddItemIfAny("Links", () => this.links.Any(), () => this.links.ToArray());
			this.AddItemIfAny("Actions", () => this.actions.Any(), ()=> this.actions.ToArray());
			this.AddItemIfAny("Entities", () => this.entities.Any(), ()=> this.entities.ToArray());
		}

		private readonly List<ISubEntity> entities = new List<ISubEntity>();
		private List<SirenLink> links = new List<SirenLink>();
		private List<SirenAction> actions = new List<SirenAction>();

		public string[] Class
		{
			get { return GetDictionaryOrDefault<string[]>("Class"); }
			set { SetDictionary("Class", value); }
		}

        public dynamic Properties
        {
            get { return GetDictionaryOrDefault<ExpandoObject>("Properties", new ExpandoObject()); }
            set { SetDictionary("Properties", new ExpandoObject()); }
        }

        public SirenObject(string[] @class = null, dynamic properties = null) : this()
		{
			this.Class = @class;
			this.Properties = properties;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="class">Describes the nature of an entity's content based on the current representation. Possible values are implementation-dependent and should be documented. MUST be an array of strings. Optional.</param>
		/// <param name="properties">
		/// A set of key-value pairs that describe the state of an entity. In JSON Siren, this is an object such as { "name": "Kevin", "age": 30 }. Optional.
		/// </param>
		public SirenObject(string @class, dynamic properties = null) : this()
		{
			this.Class = new string[] { @class };
			this.Properties = properties;
		}

		/// <summary>
		/// A collection of related sub-entities. If a sub-entit y contains an href value, it should be treated as an embedded link. 
		/// Clients may choose to optimistically load embedded links. If no href value exists, the sub-entity is an embedded entity representation that contains all the characteristics of a typical entity. 
		/// One difference is that a sub-entity MUST contain a rel attribute to describe its relationship to the parent entity. 
		/// 
		/// In JSON Siren, this is represented as an array.Optional.
		/// </summary>
		/// <param name="item"></param>
		public void AddEntity(ISubEntity item)
		{
			this.entities.Add(item);
		}

		/// <summary>
		/// A collection of action objects (Optional) 
		/// represented in JSON Siren as an array such as { "actions": [{ ... }] }. See Actions. 
		/// </summary>
		/// <param name="action"></param>
		public void AddAction(SirenAction action)
		{
			this.actions.Add(action);
		}
        
		/// <summary>
		/// A collection of items that describe navigational links, distinct from entity relationships. (Optional)
		/// Link items should contain a rel attribute to describe the relationship and an href attribute to point to the target URI. 
		/// Entities should include a link rel to self. In JSON Siren, this is represented as "links": [{ "rel": ["self"], "href": "http://api.x.io/orders/1234" }].
		/// </summary>
		/// <param name="link"></param>
		public void AddLink(SirenLink link)
		{
			this.links.Add(link);
		}

		public void AddLink(string href, params string[] rel)
		{
			this.links.Add(new SirenLink(href, rel));
		}
	}
}