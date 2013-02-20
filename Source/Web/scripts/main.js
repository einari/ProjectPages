require.config({
	appDir: "/",
	baseUrl: "/scripts",
	optimize: "none",
	    //"http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min",


	paths: {
	    "jquery": "jquery-1.7.1.min", //"http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery",
	    "knockout": "knockout-2.0.0", //"http://cdn.dolittle.com/knockout/knockout-2.0.0",
		"knockout.mapping": "knockout.mapping-2.0.0", // "http://cdn.dolittle.com/knockout/knockout.mapping-2.0.0",
	    "bifrost": "Bifrost.debug", // "http://cdn.dolittle.com/Bifrost/Bifrost.debug",
	    "order": "order", // "http://cdn.dolittle.com/require/order",
	    "domReady": "domReady", //"http://cdn.dolittle.com/require/domReady",
	    "text": "text", //"http://cdn.dolittle.com/require/text",
	    "bootstrap": "bootstrap",
	    "bootstrap-collapse": "bootstrap-collapse",
	    "prettify": "/prettify/prettify",
	    "marked": "marked"
	}
});

// "http://cdn.dolittle.com/Bifrost/Bifrost.debug",
var dolittle = {
	parseEmailFrom : function (email) {
	  var atAlias = "[at]";
	  return email.replace(atAlias, "@");
	},
	fixEmails : function () {
	  $("a:contains('[at]')").each(function(){
	    var originalEmail = $(this).text();
	    var friendlyEmail = dolittle.parseEmailFrom(originalEmail);
	    $(this).attr("href", "mailto:" + friendlyEmail).text(friendlyEmail);
	  });
	}
};


require(
    ["jquery", "knockout"],
	function() {
		require(["jquery.history"],
		    function () {
		        require(["knockout.mapping", "bifrost", "bootstrap", "prettify"],
		            function () {
		                Bifrost.features.featureMapper.add("admin/{feature}/{subFeature}", "/administration/{feature}/{subFeature}", false);
		                Bifrost.features.featureMapper.add("admin/{feature}", "/administration/{feature}", true);


		                Bifrost.features.featureMapper.add("{feature}/{sampleGroup}/{sample}", "/Features/documentation/{sampleGroup}/{sample}", false);

		                Bifrost.features.featureMapper.add("{feature}/{subFeature}", "/Features/{feature}/{subFeature}", false);
		                Bifrost.features.featureMapper.add("{feature}", "/Features/{feature}", true);

		                require(["/index.js"]);
		                
		                dolittle.fixEmails();
		            }
		            
		        );
		    }
		);
	}
);
