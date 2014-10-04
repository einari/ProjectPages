(function (undefined) {
	Bifrost.features.featureManager.get("tutorials").defineViewModel(function () {
		var self = this;
		
		this.tutorials = ko.observableArray();
		this.currentTutorial = ko.observable();
		this.content = ko.observable("");

		$.get("/Content/Tutorials.ashx", function (e) {
		    self.tutorials(e);

		    self.selectTutorial(self.tutorials()[0]);
		});
		
		this.selectTutorial = function (tutorial) {
		    self.currentTutorial(tutorial);
		    $.get("/Content/ElementLoader.ashx?file=/Tutorials/"+tutorial+".md", function (e) {
		        var markUp = $("<div/>").append(e);
		        var codeBlocks = $("code", markUp);
		        $.each(codeBlocks, function (index, item) {
		            var pre = $("<pre class='prettyprint'/>");
		            pre.append($(item).html());
		            $(item).parent().replaceWith(pre);
		        });

		        self.content($(markUp).html());

		        prettyPrint();
		    }, "text");
		};
	});
})();
