namespace SirenMapper
{
	
	public class SirenSample : SirenObject
	{
		public SirenSample()
		{
			this.Class = new string[] { "order" };
			this.Properties = new
			{
				OrderNumber = 42,
				ItemCount = 3,
				Status = "pending"
			};

			ISubEntity entity1 = new EmbeddedLink { Class = new string[] { "items", "collection" }, Rel = new string[] { "http://x.io/rels/order-items" }, Href = "http://api.x.io/orders/42/items" };
			this.AddEntity(entity1);

			SirenObject entity2 = new SirenObject("info", "customer");
			entity2.Properties = new
			{
				CustomerId= "pj123",
				Name= "Peter Joseph"
			};
			entity2.AddLink(new SirenLink("http://api.x.io/customers/pj123", "self"));
			this.AddEntity(entity2);

			var addItemAction = new SirenAction() { Name ="add-item", Title = "Add item", Method = "POST", Href = "http://api.x.io/orders/42/items", Type = "application/x-www-form-urlencoded" };
			addItemAction.AddField(new SirenField("orderNumber", SirenField.TypeHidden, "42"));
			addItemAction.AddField(new SirenField("productCode", SirenField.TypeText));
			addItemAction.AddField(new SirenField("orderNumber", SirenField.TypeNumber));
			this.AddAction(addItemAction);

			this.AddLink("http://api.x.io/orders/42", "self");
			this.AddLink("http://api.x.io/orders/41", "previous");
			this.AddLink("http://api.x.io/orders/43", "next");
		}
	}
}