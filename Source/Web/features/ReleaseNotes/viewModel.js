(function (undefined) {
	Bifrost.features.featureManager.get("releaseNotes").defineViewModel(function () {
	    var self = this;

	    this.content = ko.observable("");

        $.get("/Content/ElementLoader.ashx?file=/release_notes.md", function (e) {
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
	});
})();
