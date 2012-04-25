Bifrost has been written with specifications supporting and guaranteeing the quality of the code, this instead of using more traditional unit tests. 

For the C# code, [MSpec](https://github.com/machine/machine.specifications) has been chosen with a certain style. JavaScript code is using [Jasmine](http://pivotal.github.com/jasmine/) with a similar style and structure as with the C# code.

As with all code in Bifrost, you will find small specialized specification classes or functions in JavaScript. These are written in a specifying way using the Gherkin construct (given, when, then) found in [Behavior Driven Development](http://en.wikipedia.org/wiki/Behavior_Driven_Development).

Since we're using this for specifying units as well, we have a certain structure to reflect this. The structure is reflected in the folder structure and naming of files. 


The basic folder structure we have is :  

	(project to specify).Specs  
		(namespace)  
			for_(unit to specify)  
				given  
					a_(context).cs  
				when_(behavior to specify).cs  


A concrete sample of this would be : 

	Bifrost.Specs  
		Commands  
			for_CommandContext  
				given  
					a_command_context_for_a_simple_command_with_one_tracked_object.cs  
				when_committing.cs  

The implementation can then look like this :


	public class when_committing : given.a_command_context_for_a_simple_command_with_one_tracked_object_with_one_uncommitted_event
	{
    	static UncommittedEventStream   event_stream;

    	Establish context = () => event_store_mock.Setup(e=>e.Save(Moq.It.IsAny<UncommittedEventStream>())).Callback((UncommittedEventStream s) => event_stream = s);

    	Because of = () => command_context.Commit();

    	It should_call_save = () => event_stream.ShouldNotBeNull();
    	It should_call_save_with_the_event_in_event_stream = () => event_stream.ShouldContainOnly(uncommitted_event);
    	It should_commit_aggregated_root = () => aggregated_root.CommitCalled.ShouldBeTrue();
	}


We've used [Moq](http://code.google.com/p/moq/) for handling mocking of objects.

