using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Stac.Api.Interfaces;
using Stac.Api.Models;

namespace Stac.Api.Models.Core
{
    public static class StacLinkExtensions
    {
        public static string? GetEnumMemberValue<T>(this T value) where T : Enum
        {
            return typeof(T)
                .GetTypeInfo()
                .DeclaredMembers
                .SingleOrDefault(x => x.Name == value.ToString())
                ?.GetCustomAttribute<EnumMemberAttribute>(false)
                ?.Value;
        }

        public static StacLink NextPage(this ILinksCollectionObject linksCollectionObject)
        {
            return linksCollectionObject.Links.FirstOrDefault(l => l.RelationshipType == ILinkValues.LinkRelationType.Next.GetEnumMemberValue());
        }

        public static StacLink PreviousPage(this ILinksCollectionObject linksCollectionObject)
        {
            return linksCollectionObject.Links.FirstOrDefault(l => l.RelationshipType == ILinkValues.LinkRelationType.Previous.GetEnumMemberValue());
        }
    }
}