Search Engine Optimization is often something people ask about when it comes to Single Page Applications on the web. Are they indexed, and if so how? 

Many solutions out there rely on using what is known as hash-bangs (#!) in their URLs, this makes things look a bit weird and feels less intuitive than what people are used to from URLs. Bifrost does not use this technique. Instead Bifrost uses a combination of server side rewriting for the underlying Web server and handling of the URL in the client through JavaScript. Basically, any URL coming in that does not have a corresponding file on the Web-server and/or an ASP.net 4 route set up for the incoming URL will be rewritten to go for the correct HTML file on the server. After the file has been found and sent to the client, the JavaScript part of Bifrost handles the URLs for any ViewModels that are handling URLs.

So, what about any anchors with HREFs sitting on the page, how do we deal with them. A specific JavaScript helper deals with them. Instead of taking a regular old anchor like : 

	<a href="/some/route">Go here</a>
	
Bifrost has a helper that looks for a data-navigate-to attribute and deals with the href and also making sure we don't post-back to the server, as we want to stay on the page and only change the URL in the browser and also add things to the history of the browser. 

An anchor tag would then look like this : 

	<a data-navigate-to="/some/route">Go here</a>
	
Internally Bifrost uses the [Balupton History plugin](https://github.com/balupton/History.js/) if it has been loaded. It provides an abstraction for dealing with the different browsers and how they deal with history. 