	public class PostCreated : Event
	{
		public PostCreated(Guid id) : base(id) {}				
		public string Title { get; set; }
		public string Body { get; set; }				
	}				
