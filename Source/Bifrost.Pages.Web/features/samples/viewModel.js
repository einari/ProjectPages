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
		
		this.currentSample = ko.observable("samples/index");
		
		this.selectSample = function(group, sample) {
			self.currentSample("samples/"+group.name+"/"+sample);	
		}
	});
})();
