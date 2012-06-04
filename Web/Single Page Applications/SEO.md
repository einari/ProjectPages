Search Engine Optimization is often something people ask about when it comes to Single Page Applications on the web. Are they indexed, and if so how? 

Many solutions out there rely on using what is known as hash-bangs (#!) in their URLs, this makes things look a bit weird and feels less intuitive than what people are used to from URLs. Bifrost does not use this technique. Instead Bifrost uses a combination of server side rewriting for the underlying Web server and handling of the URL in the client through JavaScript. Basically, any URL coming in that does not have a corresponding file on the Web-server and/or an ASP.net 4 route set up for the incoming URL will be rewritten to go for the correct HTML file on the server. After the file has been found and sent to the client, the JavaScript part of Bifrost handles the URLs for any ViewModels that are handling URLs.

So, what about any anchors with HREFs sitting on the page, how do we deal with them. Bifrost will find any anchor tags on your page that is referencing anything on your site and hooks up a click event that prevents the default behavior to occur and instead uses the mechanisms already in place in Bifrost for navigation.

So, basically if you put an anchor like this : 

	<a href="/some/route">Go here</a>
	
It will remain like this so that search engines can crawl your site, but we will hook up the click event and deal with it properly for the user.
	
Internally Bifrost uses the [Balupton History plugin](http://github.com/balupton/History.js/) if it has been loaded. It provides an abstraction for dealing with the different browsers and how they deal with history. 

Worth mentioning, this site; the documentation of Bifrost is built on top of Bifrost itself. You can find the project [here](http://github.com/dolittlestudios/bifrost-pages).