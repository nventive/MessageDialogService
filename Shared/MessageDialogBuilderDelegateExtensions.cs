using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MessageDialogService
{
	public static partial class MessageDialogBuilderDelegateExtensions
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
		/// Replaces the items in a collection with a new set of items.
		/// </summary>
		/// <typeparam name="T">The type of items.</typeparam>
		/// <param name="collection">The collection who's content will be replaced.</param>
		/// <param name="items">The replacing items.</param>
		internal static void ReplaceWith<T>(this ICollection<T> collection, IEnumerable<T> items)
		{
			collection.Clear();
			AddRange(collection, items);
		}

		/// <summary>
		/// Adds the items of the specified collection to the end of the ICollection.
		/// </summary>
		/// <typeparam name="T">The type of the items.</typeparam>
		/// <param name="collection">Collection in which to insert items.</param>
		/// <param name="items">The items to add.</param>
		internal static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
		{
			items.ForEach<T>(collection.Add);
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
