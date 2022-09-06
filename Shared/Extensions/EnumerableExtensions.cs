using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MessageDialogService
{
	internal static partial class EnumerableExtensions
	{
		/// <summary>
		/// Remove null values while enumerating
		/// </summary>
		internal static IEnumerable<T> Trim<T>(this IEnumerable<T> items)
			where T : class
		{
			return items.Where(item => item != null);
		}

		/// <summary>
		/// Apply an action for every item of an enumerable
		/// </summary>
		/// <remarks>
		/// This method allows looping on every item of the source without enumerating it
		/// If enumeration is not a concern, you should avoid using this method if you're doing fuctionnal or declarative programming.
		/// </remarks>
		internal static void ForEach<T>(this IEnumerable enumerable, Action<T> action)
		{
			var list = enumerable as IList;

			if (list == null)
			{
				foreach (var item in list)
				{
					action((T)item);
				}
			}
			else
			{
				for (int i = 0; i < list.Count; i++)
				{
					action((T)list[i]);
				}
			}
		}
	}
}
