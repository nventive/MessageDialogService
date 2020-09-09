using System;
using System.Collections.Generic;
using System.Text;

namespace MessageDialogService
{
	/// <summary>
	/// A delegate to interact with the native platform.
	/// Implement this interface to implement dialogs in other platforms.
	/// </summary>
	public interface IMessageDialogBuilderDelegate
	{
		/// <summary>
		/// Creates a command.
		/// </summary>
		IMessageDialogCommand<TResult> CreateCommand<TResult>(CommandInformation<TResult> id, string label, Action action);

		/// <summary>
		/// Creates a message dialog build result.
		/// </summary>
		IMessageDialogBuildResult<TResult> CreateMessageDialogBuildResult<TResult>();

		/// <summary>
		/// Returns a localized resource based on a resource key.
		/// </summary>
		string GetResourceString(string key);
	}
}
