function loadStuff()
{
	require(["text!https://raw.github.com/dolittlestudios/Bifrost-Documentation/master/Web/Single%20Page%20Applications/Decoupling.md"], function(v) {
		theStuff = v;
		console.log("We're there");
	});
}