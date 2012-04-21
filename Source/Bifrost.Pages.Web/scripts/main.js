require.config({
	appDir: "/",
	baseUrl: "/scripts",
	optimize: "none",
	    //"http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min",


	paths: {
	    "jquery": "http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery",
	    "knockout": "http://cdn.dolittle.com/Knockout/knockout-2.0.0",
		"knockout.mapping": "http://cdn.dolittle.com/Knockout/knockout.mapping-2.0.0",
	    "bifrost": "http://cdn.dolittle.com/Bifrost/Bifrost.debug",
	    "order": "http://cdn.dolittle.com/Require/order",
	    "domReady": "http://cdn.dolittle.com/Require/domReady",
	    "text": "http://cdn.dolittle.com/Require/text",
	    "bootstrap": "bootstrap",
	    "prettify": "/prettify/prettify",
	    "marked": "marked"
	}
});

// "http://cdn.dolittle.com/Bifrost/Bifrost.debug",

require(
    ["jquery", "knockout"],
	function() {
		require(["jquery.history"],
		    function () {
		        require(["knockout.mapping", "bifrost", "bootstrap", "prettify", "marked"],
		            function () {
		                Bifrost.features.featureMapper.add("admin/{feature}/{subFeature}", "/administration/{feature}/{subFeature}", false);
		                Bifrost.features.featureMapper.add("admin/{feature}", "/administration/{feature}", true);


		                Bifrost.features.featureMapper.add("{feature}/{sampleGroup}/{sample}", "/Features/samples/{sampleGroup}/{sample}", false);

		                Bifrost.features.featureMapper.add("{feature}/{subFeature}", "/Features/{feature}/{subFeature}", false);
		                Bifrost.features.featureMapper.add("{feature}", "/Features/{feature}", true);

		                require(["/index.js"]);
		            }
		        );
		    }
		);
	}
);
