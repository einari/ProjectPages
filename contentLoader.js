function loadStuff()
{
	require("text!index.md", function(v) {
		theStuff = v;
		console.log("We're there");
	});
}