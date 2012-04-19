(function (undefined) {
	function Group(name, samples) {
		this.name = name;
		this.samples = samples;
		
	}

	Bifrost.features.featureManager.get("samples").defineViewModel(function () {
		var self = this;
		
		this.sampleGroups = ko.observableArray([
			new Group("commands",["command", "commandInputValidator", "commandBusinessValidator"])
		]);
		
		this.currentSample = ko.observable("samples/index");
		
		this.selectSample = function(group, sample) {
			self.currentSample("samples/"+group.name+"/"+sample);	
		}
	});
})();
