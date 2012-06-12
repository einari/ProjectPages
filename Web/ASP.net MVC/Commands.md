Bifrost comes with a set of helpers to make life easier working with Commands. Instead of having to explicitly instantiate Commands in a controller and populate it based on input coming from the view over HTTP as part of your form or from your JavaScript AJAX call.

The helpers extend on the existing helpers found in ASP.net MVC called *HtmlHelper* and *AjaxHelper*.
In both cases Bifrost works with something called a *CommandForm*, it is basically an *MvcForm* with additional information and functionality for Commands.

For using this in a synchronous way you do : 

	@using( var commandForm = Html.BeginCommandForm<CreateUser, UserController>(c => CreateUser(null))) ) 
	{
		
	}
	
	
The first generic parameter on the *BeginCommandForm* method is the Command and the second is the Controller you want to post to. The first parameter is then an expression pointing to the Action to be invoked on the Controller. As you can see, the action is specified as an expression rather than the default MVC way of using string literals. This provides a better tooling experience with refactoring tools and turning on compilation of views on a build server would also catch code in the View using wrong Actions.
Worth mentioning; there are a few overloads of the helper, we will only show the simplest of them here.

*BeginCommandForm* returns an instance of a form specific to the command. Within this form you have a new Html helper that you can use that is tied into the command and having the command itself as its Model instead of the Model set as default for your View. 

That gives you something like this : 

	@using( var commandForm = 
			Html.BeginCommandForm<CreateUserCommand, UserController>(c => CreateUser(null))) ) 
	{
		@commandForm.Html.TextBoxFor(c => c.Name)

		<input type="submit" value="Register"/>
	}


If you have an input validator related to the command, any rules that can be validated by the jQuery Validation engine on the client-side will now be hooked up properly. Basically, Bifrost is just tapping into the power of ASP.net MVC and making it all simpler.

In your Controller you can now add an Action that deals with the command coming in : 

	public ActionResult CreateUser(CreateUserCommand command)
	{
		return RedirectToAction("Index");
	}
	
	
The Action at this point is not doing anything. We need a reference to the Bifrost Command Coordinator to be able to handle the command. Since Bifrost provides a Controller Factory for MVC, all you need to do is add a dependency on the constructor of the Controller and store it in a private field : 

	public UserController(ICommandCoordinator commandCoordinator)
	{
		_commandCoordinator = commandCoordinator
	}
	
Since the Command Coordinator resides in the Bifrost.Commands namespace, you will have to add a using statement for that namespace at the top.

With this in place, we can now actually handle the Command :

	public ActionResult CreateUser(CreateUserCommand command)
	{
		_commandCoordinator.Handle(command);
		return RedirectToAction("Index");
	}

The lifecycle of the command will now be handled by the Command Coordinator.

But, we're not handling any possible validation results, errors or exceptions coming out.
The Command Coordinator does in fact return a *CommandResult* instance. This instance holds information that we need in order to provide proper feedback to the user. 

Firstly, *CommandResult* has a boolean telling wether or not it was successfull or not. If it was not, we can check if it was validation that caused it not to succeed or at the last resort see if there was an exception, which is also represented as a property on the *CommandResult*.

Keeping things a bit naïve and simple for now, I will only deal with validation : 

	public ActionResult CreateUser(CreateUserCommand command)
	{
		var commandResult = _commandCoordinator.Handle(command);
		if( !commandResult.Success && commandResult.Invalid ) 
		{
			ModelState.AddToModelErrors(commandResult.ValidationResult)
			return View();
		}
			
		return RedirectToAction("Index");
	}


If your app is doing AJAX, there is also a helper for dealing with that. It is pretty much in its simplest overload the same as the *HtmlHelper* version.

	@using( var commandForm = 
			Ajax.BeginCommandForm<CreateUserCommand, UserController>(c => CreateUser(null))) ) 
	{
		@commandForm.Html.TextBoxFor(c => c.Name)

		<input type="submit" value="Register"/>
	}
	
Your controller becomes a little bit simpler, since we now are dealing with client side handling of any errors : 

	public ActionResult CreateUser(CreateUserCommand command)
	{
		var commandResult = _commandCoordinator.Handle(command);
		return Json(commandResult, JsonRequestBehavior.AllowGet);
	}

