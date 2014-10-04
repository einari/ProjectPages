The composition engine in Bifrost is built around the concept of a feature. A feature is basically an object holding a view reference and a ViewModelDefinition. The feature can be put into a page by using the **data-feature** attribute on a container element such as a div element. In order for Bifrost to able to find features it has a discovering mechanism internally that is based on configurable conventions. The mechanism is based on a string that maps to a certain structure in your web application. The string can contain place holders that Bifrost will try to recognize and resolve when. So for instance saying "{feature}", it will take any string and resolve automatically. Addint a "{feature}/{subFeature}" it will match to a string with a / separator between two strings. The placeholders will then be expanded into the URL that represents the actual location of were it will find the files for the feature. The files are typically an HTML file and a JavaScript file with matching names. You can also specify a map as a default map, which will lead Bifrost to look inside the mapping path for a file called view.html and a viewModel.js. If not specifying it, it will take the last part of the URL in the map and use as filename for the HTML and JavaScript files.

To configure your conventions for discovering, put the following JavaScript initialization code as soon as your Bifrost JS file has been loaded.

	Bifrost.features.featureMapper.add(
		"{feature}", 				// Source name
		"/Features/{feature}", 		// Target Url with expansion
		true);						// Default map

If you want to specify the name of file to be resolved from a placeholder, you can do as follows.


	Bifrost.features.featureMapper.add(
		"{feature}/{subFeature}", 			// Source name
		"/Features/{feature}/{subFeature}", // Target Url with expansion
		false);								// Not a default map
		

When the document is ready, Bifrost will locate all elements in the document with a **data-feature** attribute and start resolving these features through the featureManager.

	<div data-feature="name of feature"></div>
	
It is also possible to bind your feature to a property on the ViewModel, Bifrost provides a [Knockout](http://www.knouckoutjs.com) binding handler called feature. With this you can basically take any container and just have it automatically bound to a property that deals with a state that can then be translated into a feature. 

	<div data-bind="feature: [property to bind to]"></div>
	
Ideally the property you would bind to would not be coupled to the direct concept of a feature and its name, but rather use a function to interpret the state in the ViewModel and translate it into the proper feature name.