using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MessageDialogService
{
	/// <summary>
	/// A builder interface for constructing how to display a native dialog.
	/// </summary>
	public interface IMessageDialogBuilder
	{
		/// <summary>
		/// The body of the message to display.
		/// </summary>
		string ContentValue { get; set; }

		/// <summary>
		/// The title of the message to display. Can be null.
		/// </summary>
		string TitleValue { get; set; }

		[EditorBrowsable(EditorBrowsableState.Never)]
		string GetResourceString(string resourceKey);

		[EditorBrowsable(EditorBrowsableState.Never)]
		IMessageDialogBuildResult Build();
	}

	/// <summary>
	/// A builder interface for constructing how to display a native dialog, and which choices to offer to the user.
	/// </summary>
	/// <typeparam name="TResult">Dialog result type</typeparam>
	public interface IMessageDialogBuilder<TResult> : IMessageDialogBuilder
	{
		/// <summary>
		/// A list of commands that the user can select.
		/// </summary>
		IList<IMessageDialogCommand<TResult>> Commands { get; }

		/// <summary>
		/// Adds a command to the list of commands.
		/// </summary>
		/// <param name="label">Command label</param>
		/// <param name="action">Command action</param>
		/// <param name="id">Command identifier</param>
		void AddCommand(string label, Action action, CommandInformation<TResult> id);

		[EditorBrowsable(EditorBrowsableState.Never)]
		new IMessageDialogBuildResult<TResult> Build();
	}
}
