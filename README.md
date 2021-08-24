# Discord Goldbot of the Imperials
The fucking coder of this abused a new preview feature that we haven't got yet, that's why there are so many errors.

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
