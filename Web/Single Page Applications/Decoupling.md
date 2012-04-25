When you have a page composition with multiple features on the page, its a good guidance not to couple these features together and leave them blissfully unaware of each other. This makes it easier to move things around in the application, remove features, add features - in general greater flexibility and greater focus for the self contained decoupled features. In order to accommodate this decoupling one needs a mechanism to be able to broadcast changes from one feature that another feature can subscribe to. This mechanism is called **messenger** in Bifrost and uses messages that can be published and subscribed to.

Given you have a message 

	function MyMessage(value) {
		this.value = value;
	}

Inside your ViewModel that want to publish a message you can simply do as follows

	Bifrost.messaging.messenger.publish(new MyMessage("Something"));
	
In a ViewModel that wants to get notified when a message is published, you can do as follows

	Bifrost.messaging.messenger.subscribeTo("MyMessage", function(messageInstance) {
		alert("And the message is : "+messageInstance.value);
	});