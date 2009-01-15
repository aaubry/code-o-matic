using System;
using PostSharp.CodeWeaver;
using PostSharp.CodeModel;
using PostSharp.Collections;
using CodeOMatic.Validation.Core;

namespace CodeOMatic.Validation.CompileTime
{
	[CLSCompliant(false)]
	public class BuildParameterCollectionAdvice : IAdvice
	{
		private readonly MethodDefDeclaration method;

		/// <summary>
		/// Initializes a new instance of the <see cref="BuildParameterCollectionAdvice"/> class.
		/// </summary>
		/// <param name="method">The method.</param>
		public BuildParameterCollectionAdvice(MethodDefDeclaration method)
		{
			this.method = method;
		}

		#region IAdvice Members
		/// <summary>
		/// Gets the priority.
		/// </summary>
		/// <value>The priority.</value>
		public int Priority
		{
			get
			{
				return -1;
			}
		}

		/// <summary>
		/// Requireses the weave.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		public bool RequiresWeave(WeavingContext context)
		{
			return context.JoinPoint.JoinPointKind == JoinPointKinds.BeforeMethodBody;
		}

		/// <summary>
		/// Weaves the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="block">The block.</param>
		public void Weave(WeavingContext context, InstructionBlock block)
		{
			LocalVariableSymbol parameters = block.DefineLocalVariable(context.Method.Module.FindType(typeof(ParameterDictionary), BindingOptions.Default), NameGenerator.Generate("parameters"));

			InstructionSequence entrySequence = context.Method.MethodBody.CreateInstructionSequence();
			block.AddInstructionSequence(entrySequence, NodePosition.Before, null);

			InstructionWriter writer = context.InstructionWriter;
			writer.AttachInstructionSequence(entrySequence);
			writer.EmitSymbolSequencePoint(SymbolSequencePoint.Hidden);

			writer.EmitInstructionMethod(OpCodeNumber.Call, context.Method.Module.FindMethod(typeof(InternalHelperMethods).GetMethod("CreateParameterCollection"), BindingOptions.Default));

			IMethod add = context.Method.Module.FindMethod(typeof(ParameterDictionary).GetMethod("Add", new[] { typeof(string), typeof(object) }), BindingOptions.Default);

			int parameterIndex = 0;
			foreach (var parameter in method.Parameters)
			{
				writer.EmitInstruction(OpCodeNumber.Dup);

				writer.EmitInstructionString(OpCodeNumber.Ldstr, new LiteralString(parameter.Name));
				writer.EmitInstructionInt32(OpCodeNumber.Ldarg, parameterIndex++);

				if (parameter.ParameterType.GetSystemType(null, null).IsValueType)
				{
					writer.EmitInstructionType(OpCodeNumber.Box, parameter.ParameterType);
				}

				writer.EmitInstructionMethod(OpCodeNumber.Callvirt, add);
			}

			writer.EmitInstructionLocalVariable(OpCodeNumber.Stloc, parameters);

			writer.DetachInstructionSequence();
		}
		#endregion
	}
}