Add foreign key parser in DBMethods
Add predefined schemes: 8 through 16
*Add EQ balancer -- in process
Add color distribution
Add Auth
Change methods: Remove T from GetTable *methods*, return object instead
Come up with something better than list of dictionaries, perhaps list of object - if possible implement it
**Probably** be better off with specifying the type in call instead

EQ Balancer requirements:
	check if player count is correct - is divisible by 4 - or assume that the number is correct,
	might be useful when undecive as to whom to remove
	open up dialog for removing/adding players if it is not correct - precheck in the menu when calling eqbalancer
	make players swappable - add an option to swap players between teams if necessary
