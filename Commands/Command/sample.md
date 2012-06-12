A command representing changing of an address could be something like below : 

	public class ChangeAddress : Command
	{
		public	Guid	PersonId { get; set; }
		public	string	Address { get; set; }
		public	string	PostCode { get; set; }
		public	string	City { get; set; }
		public	string	Country { get; set; }
	}
	
	
A more specific command, expressing the intent could then just inherit from this : 

	public class ChangeAddressBecauseOfMove : ChangeAddress
	{
	}

Since its not doing anything specific, it does not need anything specific on it, everything on the base class will do just fine.
