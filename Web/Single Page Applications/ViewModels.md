ViewModels are essential in Bifrost. A Feature is relying on the existence of a ViewModel. The ViewModel is defined as an object that holds state and behavior that the view can consume. The view consumes this through the usage of Knockout binders. 

A feature that is loaded by the feature manager will automatically load the file that represents the ViewModel and execute it. In this file you have to define the ViewModel for the feature. 

	Bifrost.features.featureManager.get("myFeature").defineViewModel(function () {
		
	});
	
Bifrost will then manage the lifecycle of the ViewModel and automatically hook it up using Knockouts *.applyBinding()* function. By default the ViewModel is transient and there will be created a new instance everytime a feature is instantiated and put on the page. You can however control the lifecycle by adding a parameter to the definition of a ViewModel. The lifecycle can be set to be singleton, which means that the ViewModel will live in the client till the application is left.

To set the lifecycle to singleton, you will have to pass in an option object with a property called *isSingleton* set to true :

	Bifrost.features.featureManager.get("myFeature").defineViewModel(function () {
	
	}, {isSingleton:true});


The ViewModel has the Bifrost ViewModel as its prototype, this means that you have a bit of functionality available that you can use inside your ViewModel.

One of the features that is available is that Bifrost can automatically map any query parameters in the query string to an observable property in your ViewModel. The way Bifrost supports this is that there is an object called queryParameters sitting on the prototype that you can use to define the observable properties.

	this.queryParameters.define({
		someParameter: ko.observable()
	});
	
A parameter called **someParameter** on the query string would then have its value automatically put into it. This can then be bound in the view.

	<span data-bind="text: queryParameters.someParameter"></span>
	