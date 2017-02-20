**About**
==========

A project that will contain some performance related ideas. 

**Background Runner**
=
(hopefully a temporary name). A C# project intended to solve the temporal coupling issue, as described in Pragmatic Programmer by Andrew Hunt and David Thomas but in an existing code. This problem should be adress by good design. However, sometimes we have to improve performance in an existing code, with minimal change and side effect. that is the project aim.
For additional details see documentation.

The Problem Setup
-----------
Temporal Coupling from Wikipedia:

> When two actions are bundled together into one module just because they happen to occur at the same time.


Let's see some examples:


```
public void Init()
{
	config1.Load();
	config2.Load();
}
```
config2 must wait for config1 to be completed thus delaying the init step, although it might not be necessary.

---

another, more real world example (leave aside code issues) :
```
 class MyModel
 {
    IDatabase Database { get; set; }
        
        public MyModel()
        {
            Database = new Database();
        }              

        public int GetID(string personName)
        {
            return Database.Get("ID", personName);
        }

```

Here its a more subtle example. creating a Database object might take a long time - maybe it reads files on the HD? maybe it fetching information from a remote machine?

The interesting question is when will we need to use database and invoke GetID? might be right after its creation, but it might be in a much later point in the program execution after everything was set up and because of user-interaction.

why do I think its falls to the definition of temporal coupling?
For me, the creation of MyModel instance, probably as apart of a greater system setup process, and the creation of Database object **are bundled together** though they don't have to be.

> **Note:**

> - A good design is a much better approach, but this project mainly meant for performance improvements in an existing code. sometimes redesigning the whole system is just not feasible.

Provided Solution
-------
Background Runner is a small library meant for solving this kind of problems. The idea is to create an interceptor instead of the member, and run the heavy operation in the background (hence the name :) )
while the operation is running, any caller will be blocked.
once complete, we remove the interceptor and set back the correct instance so no traces are left. 
the solution works for constructors, methods or properties.

for example, the fix needed for MyModel class above (included in the test code) is only changing the line with the constractor call  
```
public MyModelFix()
        {
            Database = Background<IDatabase>.StartCtor(() => Database, 
	            () => new Database(), cb => new DatabaseInterceptor(cb));
        }
```

the library will create the object in the background, providing the property so we can later set the newly created instance back. until the constructor completion, every call to Database property will be intercepted ty DatabaseInterceptor. when its done, we will set the newly created Database instance back to the property.


key decisions and assumptions are:

>- the source is single threaded
>- the actions can be parallel with no logical constraint (for example- config1, config2 in the fist example could have run in parallel without any side effect is a preliminary requirement)
>- The class we want to run its methods on the background is represented by an interface - for interception purposes
>- no monkey business :) if the Database property is public, on the original code i could do ((Database)myModelInstance.Database).Get... anywayre in my code, which will clearly fail if we are replacing the instance temporary.
