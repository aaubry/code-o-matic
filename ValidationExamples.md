# Introduction #

This page provides some examples of usage of the validation attributes.

# Validation Attributes #

## NotNull ##

The NotNull attribute validates a single argument by comparing it with **null**. If the method is passed **null** on that argument, an exception is thrown.

In the following example, if the **SayHello** method is invoked with **null**, an exception of type **ArgumentNullException** is thrown by the method. Notice the application of the attribute on the **name** argument.

```
public void SayHello([NotNull] string name)
{
	Console.WriteLine("Hello, {0}!", name);
}
```

It is possible to specify the type of the exception that should be thrown when the validation fails.

```
public void SayHello([NotNull(Exception = typeof(InvalidOperationException))] string name)
{
	Console.WriteLine("Hello, {0}!", name);
}
```

Another option is to customize the message of the exception.

```
public void SayHello([NotNull(Message = "The name must be supplied.")] string name)
{
	Console.WriteLine("Hello, {0}!", name);
}
```

Of course, the **Exception** and **Message** parameters may be combined.

```
public void SayHello([NotNull(Exception = typeof(InvalidOperationException, Message = "The name must be supplied.")] string name)
{
	Console.WriteLine("Hello, {0}!", name);
}
```

## NotEmpty ##

The NotEmpty attribute validates a single argument by checking whether it is empty. The types that can be validated are strings, arrays and collections.

In the following example, if the **SayHello** method is invoked with an empty string, an exception of type **ArgumentException** is thrown by the method.

```
public void SayHello([NotEmpty] string name)
{
	Console.WriteLine("Hello, {0}!", name);
}
```

Again, the **Exception** and / or **Message** parameters may be specified.

```
public void SayHello([NotEmpty(Exception = typeof(InvalidOperationException, Message = "The name must not be empty.")] string name)
{
	Console.WriteLine("Hello, {0}!", name);
}
```

Note that this attribute does tests **null** values. Therefore, if you want to ensure that the argument is neither **null** nor empty, you need to specify both attributes.

```
public void SayHello([NotNull, NotEmpty] string name)
{
	Console.WriteLine("Hello, {0}!", name);
}
```

## Pattern ##

The Pattern attribute validates a string argument against a regular expression. If the value of the argument does not match the pattern, an exception is thrown.

The following example validates that the **emailAddress** argument looks like an email address.

```
public void SendInvitation([Pattern(@"^.*@.*\..*$")] string emailAddress)
{
	Email.Send(emailAddress, "Please register");
}
```

## Interval ##

The Interval attribute validates that a single argument is inside an interval. The argument must implement IComparable or IComparable`<`T`>`.

The following example illustrates how to ensure that the value of a transaction is between 0 and 1000.

```
public void CreditAccount([Interval(0, 1000)] decimal amount)
{
	// Credit the account.
}
```

By default, both **Min** and **Max** are inclusive. That is, passing 0 or 1000 will succeed. The **MinMode** and **MaxMode** parameters allow to specify how the interval is defined.

```
public void CreditAccount([Interval(0, 1000, MinMode = BoundaryMode.Exclusive, MaxMode = BoundaryMode.Inclusive)] decimal amount)
{
	// Credit the account.
}
```

## Two arguments comparisons ##

A series of validators compare two arguments with each other. They allow to specify, for example, that some argument must be less that another argument.

The available validators are the following:

  * Less
  * LessOrEqual
  * Greater
  * GreaterOrEqual
  * Different
  * Equal

The following example illustrates the usage of the Less attribute. The other attributes are used in the same way. Notice that the attribute is applied to the method because it is validating more than one parameter.

```
[Less("x1", "x2")]
[Less("y1", "y2")]
public void DrawRectangle(int x1, int y1, int x2, int y2)
{
	// Draw the line
}
```