(function (undefined) {
	function Group(name, samples) {
		this.name = name;
		this.samples = samples;
		
	}

	Bifrost.features.featureManager.get("samples").defineViewModel(function () {
		var self = this;
		
		this.sampleGroups = ko.observableArray([
			new Group("commands",["command", "commandHandler"]),
			new Group("domain",["aggregatedRoot"]),
			new Group("events",["event"]),
			new Group("validation",["commandInputValidator", "commandBusinessValidator", "chapterValidator"]),
		]);
		
		this.currentSample = ko.observable("");
		//"samples/index");
				
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
		
		
		this.loadSample = function(group, sample) {
			$.get("features/samples/"+group+"/"+sample+".txt", function(e) {
			//"content/Specifications.txt", function(e) {
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
				
				
				self.currentSample($(markUp).html());
				
				prettyPrint();
				
			
			}, "text");
		}
		
		this.selectSample = function(group, sample) {
			self.loadSample(group.name, sample);
			//self.currentSample("samples/"+group.name+"/"+sample);	
		}
	});
})();
