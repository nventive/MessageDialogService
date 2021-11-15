using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageDialogService
{
	public interface IMessageDialogService
	{
		/// <summary>
		/// <para>Displays a message dialog built with the provided message builder.</para>
		/// <para>If no button is added to the builder, a default "Close" button, using the MessageDialog_Close_Label resource
		/// and returning the default value of <typeparamref name="TResult"/> will be used. Make sure to add this resource to your project.</para>
		/// </summary>
		/// <typeparam name="TResult">The result type.</typeparam>
		/// <param name="ct">A cancellation token.</param>
		/// <param name="messageBuilder">The message builder used to declare what the dialog contains.</param>
		/// <param name="defaultResult">The value to return if the message gets dismissed by another mean, like pressing the back button.</param>
		/// <returns>The value associated with the selected button, or <paramref name="defaultResult"/>.</returns>
		Task<TResult> ShowMessage<TResult>(
			CancellationToken ct,
			Func<IMessageDialogBuilder<TResult>, IMessageDialogBuilder<TResult>> messageBuilder,
			TResult defaultResult = default(TResult));

		/// <summary>
		/// <para>Displays a message dialog built with the provided message builder.</para>
		/// <para>If no button is added to the builder, a default "Close" button, using the MessageDialog_Close_Label resource
		/// and returning <see cref="MessageDialogResult.Close"/> will be used. Make sure to add this resource to your project.</para>
		/// </summary>
		/// <param name="ct">A cancellation token.</param>
		/// <param name="messageBuilder">The message builder used to declare what the dialog contains.</param>
		/// <param name="defaultResult">The value to return if the message gets dismissed by another mean, like pressing the back button.</param>
		/// <returns>The value associated with the selected button, or <paramref name="defaultResult"/>.</returns>
		Task<MessageDialogResult> ShowMessage(
			CancellationToken ct,
			Func<IMessageDialogBuilder<MessageDialogResult>, IMessageDialogBuilder<MessageDialogResult>> messageBuilder,
			MessageDialogResult defaultResult = MessageDialogResult.Cancel);

		/// <summary>
		/// It will force close the active prompt by cancelling it.
		/// Calling this method will throw a System.OperationCanceledException where a prompt was showed.
		/// </summary>
		void ForceCloseDialog();

	}
}
