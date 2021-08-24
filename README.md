# Discord Goldbot of the Imperials
# Problem 1:
I either will totally fuck it by changing
```csharp
await ((ModuleBase<SocketCommandContext>)(object)this).ReplyAsync(...);
```
to
```csharp
await ReplyAsync(...);
````
but at least it doesnt show as a mistake now.

# Problem 2:
The fucking coder of this abused a new preview feature that we haven't got yet, that's why there are so many errors. Ask him how to get it once you get his contact again, I like it.

I hope you get what to do based on the examples below:

## set_:
```csharp
val.set_Title("Month");
```
has to be replaced with
```csharp
val.Title = "Month";
```
---
## get_:
```csharp
{((SocketEntity<ulong>)(object)message.Author).get_Id().ToString()}
```
has to be replaced with
```csharp
{((SocketEntity<ulong>)(object)message.Author).Id.ToString()}
```
---
## add_:
```csharp
((BaseSocketClient)_client).add_MessageReceived((Func<SocketMessage, Task>)HandleCommandAsync);
```
has to be replaced with
```csharp
((BaseSocketClient)_client).MessageReceived += ((Func<SocketMessage, Task>)HandleCommandAsync);
```
