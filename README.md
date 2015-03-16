# code-o-matic
The code-o-matic library leverages the [http://www.postsharp.org/ PostSharp] platform to implement code injection mechanisms for .NET. Code is injected through custom attributes that perform various tasks, such as parameter validation, common patterns implementation, etc.

The current version of code-o-matic offers the following mechanisms:

  * *Validation* - Validate method parameters and properties by applying custom attributes such as NotNull or Pattern.
  * *Automatic properties* - Automatically implement ViewState and Session properties on your ASP.NET controls and pages.

See the GettingStarted page to get started.

## Requirements

Code-o-matic is based on [http://www.postsharp.org/ PostSharp] 1.0. You will need to download and install it for code-o-matic to work.

An installer is supplied that can optionally install the correct version of [http://www.postsharp.org/ PostSharp] for you.

*NOTE:* Postsharp is only required for compiling the code. The production environment does not need to have Postsharp installed.

For performance reasons, the validation library is implemented as a Postsharp plugin. An installer is provided that copies the plugin files in the correct folder. It is also possible to copy the files manually. See the CompilingInstructions page for more informations.

# Documentation

The following sections contain examples of use for the various parts of code-o-matic.

## Validation Attributes

### NotNull

The NotNull attribute validates a single argument by comparing it with *null*. If the method is passed *null* on that argument, an exception is thrown.

In the following example, if the *SayHello* method is invoked with *null*, an exception of type *ArgumentNullException* is thrown by the method. Notice the application of the attribute on the *name* argument.

```C#
public void SayHello([NotNull] string name)
{
	Console.WriteLine("Hello, {0}!", name);
}
```

It is possible to specify the type of the exception that should be thrown when the validation fails.

```C#
public void SayHello([NotNull(Exception = typeof(InvalidOperationException))] string name)
{
	Console.WriteLine("Hello, {0}!", name);
}
```

Another option is to customize the message of the exception.

```C#
public void SayHello([NotNull(Message = "The name must be supplied.")] string name)
{
	Console.WriteLine("Hello, {0}!", name);
}
```

Of course, the *Exception* and *Message* parameters may be combined.

```C#
public void SayHello([NotNull(Exception = typeof(InvalidOperationException, Message = "The name must be supplied.")] string name)
{
	Console.WriteLine("Hello, {0}!", name);
}
```

### NotEmpty

The NotEmpty attribute validates a single argument by checking whether it is empty. The types that can be validated are strings, arrays and collections.

In the following example, if the *SayHello* method is invoked with an empty string, an exception of type *ArgumentException* is thrown by the method.

```C#
public void SayHello([NotEmpty] string name)
{
	Console.WriteLine("Hello, {0}!", name);
}
```

Again, the *Exception* and / or *Message* parameters may be specified.

```C#
public void SayHello([NotEmpty(Exception = typeof(InvalidOperationException, Message = "The name must not be empty.")] string name)
{
	Console.WriteLine("Hello, {0}!", name);
}
```

Note that this attribute does tests *null* values. Therefore, if you want to ensure that the argument is neither *null* nor empty, you need to specify both attributes.

```C#
public void SayHello([NotNull, NotEmpty] string name)
{
	Console.WriteLine("Hello, {0}!", name);
}
```

### Pattern

The Pattern attribute validates a string argument against a regular expression. If the value of the argument does not match the pattern, an exception is thrown.

The following example validates that the *emailAddress* argument looks like an email address.

```C#
public void SendInvitation([Pattern(@"^.*@.*\..*$")] string emailAddress)
{
	Email.Send(emailAddress, "Please register");
}
```

### Interval

The Interval attribute validates that a single argument is inside an interval. The argument must implement IComparable or IComparable`<`T`>`.

The following example illustrates how to ensure that the value of a transaction is between 0 and 1000.

```C#
public void CreditAccount([Interval(0, 1000)] decimal amount)
{
	// Credit the account.
}
```

By default, both *Min* and *Max* are inclusive. That is, passing 0 or 1000 will succeed. The *MinMode* and *MaxMode* parameters allow to specify how the interval is defined.

```C#
public void CreditAccount([Interval(0, 1000, MinMode = BoundaryMode.Exclusive, MaxMode = BoundaryMode.Inclusive)] decimal amount)
{
	// Credit the account.
}
```

### Two arguments comparisons

A series of validators compare two arguments with each other. They allow to specify, for example, that some argument must be less that another argument.

The available validators are the following:

  * Less
  * LessOrEqual
  * Greater
  * GreaterOrEqual
  * Different
  * Equal

The following example illustrates the usage of the Less attribute. The other attributes are used in the same way. Notice that the attribute is applied to the method because it is validating more than one parameter.

```C#
[Less("x1", "x2")]
[Less("y1", "y2")]
public void DrawRectangle(int x1, int y1, int x2, int y2)
{
	// Draw the line
}
```
