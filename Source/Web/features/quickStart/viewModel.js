(function (undefined) {
	Bifrost.features.featureManager.get("quickStart").defineViewModel(function () {
		var self = this;
		
		this.content = ko.observable("");
		
		$.get("/Content/ElementLoader.ashx?file=quickStart.md", function(e) {
			var markUp = $("<div/>").append(e);
			self.content($(markUp).html());
			
			prettyPrint();
		}, "text");
	});
})();
