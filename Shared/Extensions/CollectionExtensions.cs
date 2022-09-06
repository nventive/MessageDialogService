using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MessageDialogService
{
	internal static partial class CollectionExtensions
	{
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
	}
}
