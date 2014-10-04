(function() {
	function index() {
		var self = this;
		
		this.feature = ko.observable("home");
		
		this.uriChanged(function(uri) {
			self.setFeatureFromUrl(uri);
		});
		
		
		this.setFeatureFromUrl = function(uri) {
			var featureName = uri.path.substr(1);
			if( featureName && featureName.length > 0 ) {
				self.feature(featureName);
			}
            
			
			//prettyPrint();
		}
		
		
		$(function() {
			var state = History.getState();
			self.uri.setLocation(state.url);
			self.setFeatureFromUrl(self.uri);
		});
	}
	
	Bifrost.features.ViewModel.baseFor(index);
	ko.applyBindings(new index());
	
})();