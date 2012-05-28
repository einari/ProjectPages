(function() {

	define(function() {
		var markdown;
		markdown = {
			load: function(name, req, load, config) {
				req([name], function(content) {
					console.log("Hello : "+content);
				});
			}
		}
		
		return markdown;
	});

})();
