All JavaScript code has been specified using [Jasmine](http://pivotal.github.com/jasmine/) with a similar style and structure as with the C# code. The folder structure is pretty much the same, except for the given folder. And since we've for now settled on Jasmine, the code is a bit different.


	describe("when creating with configuration", function () {
    	var options = {
        	error: function () {
            	print("Error");
        	},
        	success: function () {
        	}
    	};
    	var command = Bifrost.commands.Command.create(options);

    	it("should create an instance", function () {
        	expect(command).toBeDefined();
    	});

    	it("should include options", function () {
        	for (var property in options) {
            	expect(command.options[property]).toEqual(options[property]);
        	}
    	});
	});   
