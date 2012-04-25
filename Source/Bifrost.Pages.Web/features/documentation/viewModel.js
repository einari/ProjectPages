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
			
			self.loadSample(topic.elements()[0].file());
		}
		
		this.selectElement = function(element) {
			self.currentElement(element)
			self.loadSample(element.file());
		}
	});
})();
