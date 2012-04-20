(function() {
	function index() {
		var self = this;
		
		this.feature = ko.observable("home");
		
		this.uriChanged(function(uri) {
			self.feature(uri.path.substr(1));
			prettyPrint();
		});
		
		this.content = ko.observable("");
		
		marked.setOptions({
		  gfm: false,
		  pedantic: false,
		  sanitize: true,
		  // callback for code highlighter
		  /*
		  highlight: function(code, lang) {
		    return "<code class='prettyprint'>"+code+"</code>";
		  }
		  */
		});		
		
		$.get("content/Specifications.md", function(e) {
			var markUp = $("<div/>").append($(marked(e)));
			
			$("code",markUp).addClass("prettyprint");
			
			
			self.content($(markUp).html());
			
			
		
		}, "text");
	}
	
	Bifrost.features.ViewModel.baseFor(index);
	ko.applyBindings(new index());
})();