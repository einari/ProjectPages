(function (undefined) {
	Bifrost.features.featureManager.get("api").defineViewModel(function () {
		var self = this;
		this.structure = ko.observableArray();
		this.content = ko.observable();
        
		$.get("/Content/API/APIStructureHandler.ashx", function (e) {
		    self.structure(e);
		}, "json");

		this.getContent = function (file) {
		    $.get("/Content/ElementLoader.ashx?file=" + file, function (e) {
		        self.content(e);
		    }, "text");
		};
	});
})();
