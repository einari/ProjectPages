(function (undefined) {
	Bifrost.features.featureManager.get("documentation").defineViewModel(function () {
		var self = this;
				
		$(".accordion").collapse();
		
		this.groups = ko.observableArray();
		$.getJSON("features/documentation/DocumentationContent.ashx", function(e) {
			var groups = ko.mapping.fromJS(e);
			self.groups(groups);
		});
		
		
		this.currentElement = ko.observable({
			name: ko.observable(""),
			author: ko.observable(""),
			lastChanged: ko.observable(""),
		});
		
		this.hasAuthorInfo = ko.computed(function() {
			return self.currentElement().author().length > 0;
		});
		
		this.currentGroup = ko.observable({
			name: ko.observable("")
		});
		this.currentTopic = ko.observable({
			name: ko.observable(""),
			elements: ko.observableArray()
		});
		this.commentIdentifier = ko.computed(function() {
			var identifier = self.currentGroup().name() + "_" +
				self.currentTopic().name() + "_" +
				self.currentElement().name();
			return identifier;
		});
		
		this.hasSelectedContent = ko.computed(function() {
			var enabled = self.currentGroup().name().length > 0 &&
				self.currentTopic().name().length > 0 &&
				self.currentElement().name().length > 0;
			return enabled;
		});
		
		this.content = ko.observable("");
		
		this.loadSample = function(file) {
			$.get("/features/documentation/ElementLoader.ashx?file="+file, function(e) {
				var markUp = $("<div/>").append(e);
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
				
				var identifier = self.currentGroup().name()+self.currentTopic().name()+self.currentElement().name();
				identifier = self.commentIdentifier().replace(" ","_").replace("%20","_");
						
				
				DISQUS.reset({
				  reload: true,
				  config: function () {  
				    this.page.identifier = identifier+" ";  
				    //this.page.url = "http://bifrost.dolittle.com/documentation/#!"+identifier;
				  }
				});				
				
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
