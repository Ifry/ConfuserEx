﻿using System;
using System.Text;
using Confuser.Core;
using Confuser.Renamer.Services;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Confuser.Renamer.References {
	public sealed class StringTypeReference : INameReference<TypeDef> {
		readonly Instruction reference;
		readonly TypeDef typeDef;
		public bool ShouldCancelRename => false;

		public StringTypeReference(Instruction reference, TypeDef typeDef) {
			this.reference = reference;
			this.typeDef = typeDef;
		}

		/// <inheritdoc />
		public bool DelayRenaming(IConfuserContext context, INameService service) => false;

		public bool UpdateNameReference(IConfuserContext context, INameService service) {
			switch (reference.Operand) {
				case string strOp when string.Equals(strOp, typeDef.ReflectionFullName, StringComparison.Ordinal):
				case UTF8String utf8StrOp when UTF8String.Equals(utf8StrOp, typeDef.ReflectionFullName):
					return false;
				default:
					reference.Operand = typeDef.ReflectionFullName;
					return true;
			}
		}

		public override string ToString() => ToString(null, null);

		public string ToString(IConfuserContext context, INameService nameService) {
			var builder = new StringBuilder();
			builder.Append("String Type Reference").Append("(");

			builder.Append("Instruction").Append("(").AppendHashedIdentifier("Operand", reference.Operand).Append(")");
			builder.Append("; ");
			builder.AppendReferencedType(typeDef, context, nameService);

			builder.Append(")");

			return builder.ToString();
		}
	}
}
