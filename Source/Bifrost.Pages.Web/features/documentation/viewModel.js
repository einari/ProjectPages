(function (undefined) {
	function Element(name, file) {
		this.name = name;
		this.file = file;
	}

	function Topic(name, elements) {
		this.name = name;
		this.elements = elements;
	}

	function Group(name, topics) {
		this.name = name;
		this.topics = topics;
		
	}

	Bifrost.features.featureManager.get("documentation").defineViewModel(function () {
		var self = this;
		$(".accordion").collapse();
		
		this.groups = ko.observableArray([
			new Group("General", [
				new Topic("CQRS", [
					new Element("About","about"),
				])
			]),
			new Group("Backend", [
				new Topic("Command", [
					new Element("About","about"),
					new Element("Sample","sample"),
				]),
				new Topic("CommandHandler", [
					new Element("About","about"),
					new Element("Sample","sample"),
				])					
			]),
			new Group("Domain", [
				new Topic("AggregatedRoot", [
					new Element("About","about"),
					new Element("Sample","sample"),
				]),
			]),
			new Group("Events", [
				new Topic("Event", [
					new Element("About","about"),
					new Element("Sample","sample"),
				]),
			]),
			new Group("Validation", [
				new Topic("CommandInputValidator", [
					new Element("About","about"),
					new Element("Sample","sample"),
				]),
				new Topic("CommandBusinessValidator", [
					new Element("About","about"),
					new Element("Sample","sample"),
				]),
				new Topic("ChapterValidator", [
					new Element("About","about"),
					new Element("Sample","sample"),
				]),
					
					
			])
				
						
		]);
		
		
		$.getJSON("features/documentation/DocumentationContent.ashx", function() {
			var i=0;
			i++;
		});
		
		
		this.currentElement = ko.observable({
			name: ""
		});
		this.currentGroup = ko.observable({
			name: ""
		});
		this.currentTopic = ko.observable({
			elements: {}
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
		
		
		this.loadSample = function(group, topic, element) {
			$.get("features/documentation/content/"+group+"/"+topic+"/"+element+".txt", function(e) {
				var markUp = $("<div/>").append($(marked(e)));
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
			
			self.loadSample(self.currentGroup().name, self.currentTopic().name, topic.elements[0].file);

		}
		
		this.selectElement = function(element) {
			self.currentElement(element)
			self.loadSample(self.currentGroup().name, self.currentTopic().name, element.file);
		}
	});
})();
