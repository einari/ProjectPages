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
		
		
		$.get("content/Specifications.txt", function(e) {
			var markUp = $("<div/>").append($(marked(e)));

			/*			
		<div class="row">
			<div class="span8">
				<pre class="prettyprint">*/
			
			
			var codeBlocks = $("code",markUp);
			
			$.each(codeBlocks, function(index, item) {

				var row = $("<div class='row'/>");
				var span = $("<div class='span8'/>");
				var pre = $("<pre class='prettyprint'/>");
				
				row.append(span);
				span.append(pre);
				pre.append($(item).html());

				$(item).parent().replaceWith(row);

			});
			
			
			self.content($(markUp).html());
			
			
		
		}, "text");
	}
	
	Bifrost.features.ViewModel.baseFor(index);
	ko.applyBindings(new index());
})();