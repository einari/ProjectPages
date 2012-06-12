A command represents a behavior that can be applied to your application / system. The name of the command reflects the behavior you want to have performed on the system and also there is a great opportunity to capture the users intent in the naming. So not only capture what should happen, but also why the user wants it to happen. 

An example of this would be *ChangeAddress*, what if we add intent to it and try to capture why the person is changing its address, it could be something like *ChangeAddressBecauseOfMove*. You can have a base command that all the ones with intent is inheriting.

The command holds all information that is needed for it to successfully execute. It will go through the *CommandCoordinator* which delegates the handling of the *Command* to all *CommandHandlers* that knows how to handle it.

