using System;
using System.Collections.Generic;
using System.Text;

namespace MessageDialogService
{
	/// <summary>
	/// Command of a <see cref="IMessageDialogBuildResult"/>.
	/// </summary>
	public interface IMessageDialogCommand<TResult>
	{
		/// <summary>
		/// Command identifier.
		/// </summary>
		CommandInformation<TResult> Id { get; }
	}
}
