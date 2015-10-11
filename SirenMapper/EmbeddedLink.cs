namespace SirenMapper
{
	/// <summary>
	/// Sub-entities can be expressed as either an embedded link or an embedded representation.
	///  
	/// Another distinction is the difference between sub-entities and links. Sub-entities exist to communicate a relationship between entities, in context. 
	/// Links are primarily navigational and communicate ways clients can navigate outside the entity graph.
	/// </summary>
	public class EmbeddedLink : DynamicJsonClass, ISubEntity
	{
		public string[] Class
		{
			get { return GetDictionaryOrDefault<string[]>("Class"); }
			set { SetDictionary("Class", value); }
		}

		public string[] Rel
		{
			get { return GetDictionaryOrDefault<string[]>("Rel"); }
			set { SetDictionary("Rel", value); }
		}

		public string Href
		{
			get { return GetDictionaryOrDefault<string>("Href"); }
			set { SetDictionary("Href", value); }
		}

		public string Type
		{
			get { return GetDictionaryOrDefault<string>("Type"); }
			set { SetDictionary("Type", value); }
		}
	}
}