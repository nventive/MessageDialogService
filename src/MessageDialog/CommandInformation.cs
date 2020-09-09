using System;
using System.Collections.Generic;
using System.Text;

namespace MessageDialogService
{
	/// <summary>
	/// Extra information stored in each <see cref="IMessageDialogCommand"/>.
	/// </summary>
	/// <typeparam name="TResult">Command result type</typeparam>
	public class CommandInformation<TResult>
	{
		public CommandInformation(TResult result, bool isDefaultAccept = false, bool isDefaultCancel = false, bool isDestructive = false)
		{
			Result = result;
			IsDefaultAccept = isDefaultAccept;
			IsDefaultCancel = isDefaultCancel;
			IsDestructive = isDestructive;
		}

		public bool IsDefaultAccept { get; }

		public bool IsDefaultCancel { get; }

		public bool IsDestructive { get; }

		public TResult Result { get; }
	}
}
