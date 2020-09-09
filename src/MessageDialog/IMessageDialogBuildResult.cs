using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageDialogService
{
	public interface IMessageDialogBuildResult
	{
		/// <summary>
		/// Dialog title.
		/// </summary>
		string Title { get; set; }

		/// <summary>
		/// Dialog content.
		/// </summary>
		string Content { get; set; }

		/// <summary>
		/// Cancel command index.
		/// </summary>
		int CancelCommandIndex { get; set; }

		/// <summary>
		/// Default command index.
		/// </summary>
		int DefaultCommandIndex { get; set; }

		/// <summary>
		/// Shows a message without any returning value.
		/// </summary>
		Task ShowMessage(CancellationToken ct);
	}

	public interface IMessageDialogBuildResult<TResult> : IMessageDialogBuildResult
	{
		/// <summary>
		/// Returns the dialog commands.
		/// </summary>
		/// <returns></returns>
		IMessageDialogCommand<TResult>[] GetCommands();

		/// <summary>
		/// Sets the dialog commands.
		/// </summary>
		/// <param name="commands"></param>
		void SetCommands(IMessageDialogCommand<TResult>[] commands);

		/// <summary>
		/// Shows a message with a returning value.
		/// </summary>
		new Task<CommandInformation<TResult>> ShowMessage(CancellationToken ct);
	}
}
