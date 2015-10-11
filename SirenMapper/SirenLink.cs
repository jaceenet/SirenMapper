namespace SirenMapper
{
	public class SirenLink : DynamicJsonClass
	{
		public SirenLink(string href, params string[] rel)
		{
			this.Href = href;
			this.Rel = rel;
		}

		/// <summary>
		/// Defines the relationship of the link to its entity, per Web Linking (RFC5988). MUST be an array of strings. Required.
		/// </summary>
		public string[] Rel
		{
			get { return GetDictionaryOrDefault<string[]>("Rel"); }
			set { SetDictionary("Rel", value); }
		}

		/// <summary>
		/// The URI of the linked resource. Required.
		/// </summary>
		public string Href
		{
			get { return GetDictionaryOrDefault<string>("Href"); }
			set { SetDictionary("Href", value); }
		}
	}	
}