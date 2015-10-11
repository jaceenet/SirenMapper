namespace SirenMapper
{
	public class SirenField : DynamicJsonClass
	{
		public const string TypeHidden = "hidden";
		public const string TypeText = "text";
		public const string TypeNumber = "number";

		public SirenField(string name, string type, string value = null)
		{
			Name = name;
			Type = type;

			if (value != null)
			{
				Value = value;
			}
		}

		public string Name
		{
			get { return GetDictionaryOrDefault<string>("Name"); }
			set { SetDictionary("Name", value); }
		}

		public string Type
		{
			get { return GetDictionaryOrDefault<string>("Type"); }
			set { SetDictionary("Type", value); }
		}

		public string Value
		{
			get { return GetDictionaryOrDefault<string>("Value"); }
			set { SetDictionary("Value", value); }
		}
	}
}