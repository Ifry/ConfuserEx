﻿using System.Globalization;
using System.Text;
using Confuser.Core;
using Confuser.Renamer.Services;
using dnlib.DotNet;

namespace Confuser.Renamer.References {
	public sealed class ResourceReference : INameReference<TypeDef> {
		readonly string format;
		readonly Resource resource;
		readonly TypeDef typeDef;

		public bool ShouldCancelRename => false;

		public ResourceReference(Resource resource, TypeDef typeDef, string format) {
			this.resource = resource;
			this.typeDef = typeDef;
			this.format = format;
		}

		/// <inheritdoc />
		public bool DelayRenaming(IConfuserContext context, INameService service) => false;

		public bool UpdateNameReference(IConfuserContext context, INameService service) {
			var newName = string.Format(CultureInfo.InvariantCulture, format, typeDef.ReflectionFullName);
			if (UTF8String.Equals(resource.Name, newName)) return false;
			resource.Name = newName;
			return true;
		}

		public override string ToString() => ToString(null, null);

		public string ToString(IConfuserContext context, INameService nameService) {
			var builder = new StringBuilder();
			builder.Append("Resource Reference").Append("(");
			builder.Append("Resource").Append("(").AppendHashedIdentifier("Name", resource.Name).Append(")");
			builder.Append("; ");
			builder.AppendReferencedType(typeDef, context, nameService);
			builder.Append(")");
			return builder.ToString();
		}
	}
}
