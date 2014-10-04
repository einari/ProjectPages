To avoid putting everything in JavaScripts global scope, Bifrost has the notion of namespacing. Namespacing is commonly known from languages such as C++, C#, XML and so forth and gives you the opportunity to structure your code within different scopes. 

Bifrost has something sitting inside the scope of Bifrost called namespace, the way you use it is as follows :

	Bifrost.namespace("Your.Namespace");
	
This function will add an object literal called "Your" in the global scope, and within that object literal another one called "Namespace". You can then start putting things into that namespace.

If the namespace already exist at any level, it will not overwrite it.

An overload of the namespace function can also take a literal object that will define the content of the namespace, like so :

	Bifrost.namespace("Your.Namespace", {
		something : "sitting inside your namespace"
	});

Again, the function will not overwrite any existing namespace, and in this particular scenario - it will in fact merge the content with any existing namespaces.
