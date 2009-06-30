using System;
using System.Globalization;
using System.Reflection;
using PostSharp.CodeModel;
using PostSharp.CodeModel.ReflectionWrapper;
using PostSharp.Extensibility;
using System.Web.UI;
using System.Diagnostics;

namespace CodeOMatic.Web
{
	/// <summary>
	/// Implements an abstract property as a ViewState variable.
	/// </summary>
	[Serializable]
	public sealed class ViewStateVariableAttribute : CollectionVariableAttribute
	{
		private string fieldName;

		/// <summary>
		/// Validates the usage of the attribute on a specific property.
		/// </summary>
		/// <param name="setter">The setter method of the property.</param>
		/// <param name="propertyType">Type of the property.</param>
		/// <param name="propertyName">Name of the property.</param>
		protected override void CompileTimeValidate(MethodBase setter, Type propertyType, string propertyName)
		{
			if (!typeof(Control).IsAssignableFrom(setter.DeclaringType))
			{
				MessageSource.MessageSink.Write(new Message(
					SeverityType.Error,
					"ViewStateVariableAttribute_PropertyDoesNotBelongToControl",
					string.Format(CultureInfo.InvariantCulture, "The property '{0}' does not belong to a System.Web.UI.Control.", propertyName),
					GetType().FullName
				));
			}
			if(setter.IsStatic)
			{
				MessageSource.MessageSink.Write(new Message(
					SeverityType.Error,
					"ViewStateVariableAttribute_PropertyCannotBeStatic",
					string.Format(CultureInfo.InvariantCulture, "The property '{0}' must not be static.", propertyName),
					GetType().FullName
				));
			}

			fieldName = string.Format(CultureInfo.InvariantCulture, "__~~~~{0}", propertyName);

			MethodDefDeclaration postsharpMethod = ((IReflectionWrapper<MethodDefDeclaration>)setter).WrappedObject;

			FieldDefDeclaration propertyField = new FieldDefDeclaration();
			propertyField.Name = fieldName;
			propertyField.FieldType = postsharpMethod.Module.FindType(propertyType, BindingOptions.Default);
			propertyField.Attributes = FieldAttributes.Private;

			postsharpMethod.DeclaringType.Fields.Add(propertyField);
		}

		private delegate StateBag GetViewStateDelegate(Control target);

		private static readonly GetViewStateDelegate getViewState = (GetViewStateDelegate)CreatePropertyGetterDelegate(
			typeof(GetViewStateDelegate),
			"ViewState"
		);

		private delegate bool IsViewStateEnabledDelegate(Control target);

		private static readonly IsViewStateEnabledDelegate isViewStateEnabled = (IsViewStateEnabledDelegate)CreatePropertyGetterDelegate(
			typeof(IsViewStateEnabledDelegate),
			"IsViewStateEnabled"
		);

		private static Delegate CreatePropertyGetterDelegate(Type delegateType, string propertyName)
		{
			PropertyInfo isViewStateEnabledProperty = typeof(Control).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);
			MethodInfo getIsViewStateEnabledMethod = isViewStateEnabledProperty.GetGetMethod(true);
			return Delegate.CreateDelegate(delegateType, getIsViewStateEnabledMethod);
		}

		[NonSerialized]
		private FieldInfo field;

		/// <summary>
		/// Runtimes the initialize.
		/// </summary>
		/// <param name="method">The method.</param>
		public override void RuntimeInitialize(MethodBase method)
		{
			base.RuntimeInitialize(method);

			field = method.DeclaringType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
			Debug.Assert(field != null);
		}

		/// <summary>
		/// Gets the value of the property.
		/// </summary>
		/// <param name="target">The object from which the property should be obtained.</param>
		/// <returns></returns>
		protected override object GetValue(object target)
		{
			object value;
			Control control = (Control)target;
			if(isViewStateEnabled(control))
			{
				StateBag viewState = getViewState(control);
				value = viewState[Key];
			}
			else
			{
				value = field.GetValue(target);
			}
			return value ?? CalculateDefaultValue(target);
		}

		/// <summary>
		/// Sets the value of the property.
		/// </summary>
		/// <param name="target">The object on which the property should be set.</param>
		/// <param name="value">The new value of the property.</param>
		/// <param name="defaultValue">The default value for the property.</param>
		protected override void SetValue(object target, object value, object defaultValue)
		{
			Control control = (Control)target;
			if (isViewStateEnabled(control))
			{
				StateBag viewState = getViewState(control);
				viewState[Key] = value;
			}
			field.SetValue(target, value);
		}
	}
}