using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Stac.Api.Services.Filtering
{
    public static class ItemFiltersExtensions
    {

        public static bool IsMatch(Regex regex, IComparable obj)
        {
            return regex.IsMatch(obj.ToString());
        }

        public static Regex LikeToRegex(this IComparable isLike)
        {
            return new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(isLike.ToString(), ch => @"\" + ch).Replace('_', '.').Replace("%", ".*") + @"\z", RegexOptions.Singleline);
        }

    }
}