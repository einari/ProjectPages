With Single Page Applications, the browser only hits one page - and in general, one page only as long as the Web browser tab / window lives or the user navigates away from your application. Its therefor important that you have all the configurations for your application done in the first page it hits, usually your index.html or whatever your WebServer has been configured to go to as your default page, or whatever you want to represent your starting URL.

In addition to configuring your application, the first page will also act as your application composition and also coordination of the composition it puts together. The composition is basically the composition of features going into the application in a declarative manner in the view; the HTML file. The coordination is done through providing a ViewModel for the start page, this can sit at the heart and provide any coordination of state changes in the application, respond to URL changes or anything else the ViewModel wants to respond to.

Bifrost provides a ViewModel prototype that can be used and will give you an abstraction for things like URI changes from the browser. 

Below is a sample that shows how to react on URI changes in the browser and translate it into a feature.

	(function() {
		function index() {
			var self = this;

			// Expose an observable feature property and 
			// set it home as default
			this.featureName = ko.observable("home");

			// Hook up the uriChanged event
			this.uriChanged(function(uri) {
				self.setFeatureFromUrl(uri);
			});

			// Set feature from the URL
			this.setFeatureFromUrl = function(uri) {
				var featureName = uri.path.substr(1);
				if( featureName && featureName.length > 0 ) {
					self.featureName(featureName);
				}
			}


			// On load, the correct feature according to state
			$(function() {
				var state = History.getState();
				self.uri.setLocation(state.url);
				self.setFeatureFromUrl(self.uri);
			});
		}

		Bifrost.features.ViewModel.baseFor(index);
		ko.applyBindings(new index());
	})();
	
Bifrost provides a Knockout plugin for dealing with features. You can use it in your **data-bind** expression :

	<div data-bind="feature: featureName"></div>
	
