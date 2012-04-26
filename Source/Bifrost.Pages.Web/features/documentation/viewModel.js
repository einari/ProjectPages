ko.bindingHandlers.disqus = {
    init: function (element, valueAccessor, allBindingAccessor, viewModel) {
    },
    update: function (element, valueAccessor, allBindingAccessor, viewModel) {
    	var configuration = valueAccessor();
    	var identifier = "";
    	try {
    		identifier = configuration.identifier;
    	} catch(e) {
    		return;
    	}
    	
    	if( identifier.indexOf("undefined") != -1 ) {
    		return;
    	}
    	
		var script = $("<script type='text/javascript'/>")
			.text(
				"var disqus_shortname = 'bifrost';" +
				"var disqus_identifier = '"+identifier+"';"+
				"(function() {"+
				"	var dsq = document.createElement('script'); dsq.type = 'text/javascript'; dsq.async = true;"+
				"	dsq.src = 'http://' + disqus_shortname + '.disqus.com/embed.js';"+
				"	(document.getElementsByTagName('head')[0] || document.getElementsByTagName('body')[0]).appendChild(dsq);"+
				"})();"
				);
				
		$(element).append(script);


    	    	/*    	    	
				<script type="text/javascript">
				    var disqus_shortname = 'bifrost'; 
				
				    (function() {
				        var dsq = document.createElement('script'); dsq.type = 'text/javascript'; dsq.async = true;
				        dsq.src = 'http://' + disqus_shortname + '.disqus.com/embed.js';
				        (document.getElementsByTagName('head')[0] || document.getElementsByTagName('body')[0]).appendChild(dsq);
				    })();
				</script>		
    	*/
    	
    }
};


(function (undefined) {
	Bifrost.features.featureManager.get("documentation").defineViewModel(function () {
		var self = this;
		
		marked.setOptions({
		  gfm: false,
		  pedantic: false,
		  sanitize: true,
		});		
		
		$(".accordion").collapse();
		
		this.groups = ko.observableArray();
		$.getJSON("features/documentation/DocumentationContent.ashx", function(e) {
			var groups = ko.mapping.fromJS(e);
			self.groups(groups);
		});
		
		
		this.currentElement = ko.observable({
			name: ko.observable()
		});
		this.currentGroup = ko.observable({
			name: ko.observable()
		});
		this.currentTopic = ko.observable({
			name: ko.observable(),
			elements: ko.observableArray()
		});
		
		this.content = ko.observable("");
		
		this.loadSample = function(file) {
			$.get("/features/documentation/ElementLoader.ashx?file="+file, function(e) {
				var markUp = $("<div/>").append(e);
				//$(marked(e)));
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
				
				prettyPrint();
			}, "text");
		}
		
		this.selectTopic = function(group, topic) {
			self.currentGroup(group);
			self.currentTopic(topic);
			
			self.content("");
			
			if( topic.elements().length > 0 ) {
				self.selectElement(topic.elements()[0]);
			}
		}
		
		this.selectElement = function(element) {
			self.currentElement(element)
			self.loadSample(element.file());
		}
	});
})();
