using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
#if WINUI
using MessageDialogService.Extensions;
using Microsoft.UI.Dispatching;
#endif
using Windows.UI.Core;

namespace MessageDialogService
{
	/// <summary>
	/// Allows displaying modal messages to the user using a MessageDialog.
	/// </summary>
	public class MessageDialogService: IMessageDialogService
	{
#if WINUI
		private readonly DispatcherQueue _dispatcher;
#endif
		private readonly IMessageDialogBuilderDelegate _messageDialogServiceDelegate;

		public MessageDialogService(
#if WINUI
			DispatcherQueue dispatcher,
#endif
			IMessageDialogBuilderDelegate messageDialogServiceDelegate
		)
		{
			_dispatcher = dispatcher;
			_messageDialogServiceDelegate = messageDialogServiceDelegate;
		}

		/// <inheritdoc />
		public Task<MessageDialogResult> ShowMessage(
			CancellationToken ct, 
			Func<IMessageDialogBuilder<MessageDialogResult>, IMessageDialogBuilder<MessageDialogResult>> messageBuilder, 
			MessageDialogResult defaultResult = MessageDialogResult.Cancel
		)
		{
			return ShowMessageInner(
				ct,
				b => CompleteMessageBuilding(messageBuilder(b), MessageDialogResult.Cancel),
				defaultResult);
		}

		/// <inheritdoc />
		public Task<TResult> ShowMessage<TResult>(
			CancellationToken ct, 
			Func<IMessageDialogBuilder<TResult>, IMessageDialogBuilder<TResult>> messageBuilder, 
			TResult defaultResult = default(TResult)
		)
		{
			return ShowMessageInner(
				ct,
				b => CompleteMessageBuilding(messageBuilder(b), defaultResult),
				defaultResult);
		}

		private IMessageDialogBuilder<TResult> CompleteMessageBuilding<TResult>(
			IMessageDialogBuilder<TResult> messageBuilder, 
			TResult closeButtonResult
		)
		{
			if (messageBuilder.Commands.Count == 0)
			{
				// The default button might vary from one platform to another, and the Uno version doesn't localize the label.
				// That's why we force the use of a close button.
				return messageBuilder
					.CommandResource(closeButtonResult, MessageDialogBuilderExtensions.CloseLabelResourceKey, isDefaultCancel: true);
			}

			return messageBuilder;
		}

		/// <summary>
		/// Displays a message dialog with one or more buttons a user can select.
		/// </summary>
		/// <param name="defaultResult">The value to return if the message gets dismissed by another mean, like pressing the back button.</param>
		private async Task<TResult> ShowMessageInner<TResult>(
			CancellationToken ct,
			Func<IMessageDialogBuilder<TResult>, IMessageDialogBuilder<TResult>> messageBuilder,
			TResult defaultResult = default(TResult))
		{
			var innerBuilder = new MessageDialogBuilder<TResult>(_messageDialogServiceDelegate);
			var builder = messageBuilder(innerBuilder);

			Task<CommandInformation<TResult>> CreateDialogUI() // Runs on UI thread
			{
				var dialog = builder.Build();

				do
				{
					try
					{
						return dialog.ShowMessage(ct);
					}
					catch (ArgumentException)
					{
						if (builder.Commands.Count > 2)
						{
							var commands = dialog.GetCommands().ToList();

							// This platform doesn't support that many buttons.
							// It's better to show less than show nothing. We always
							// keep the default and cancel indexes.
							var removeIndex = commands.Count - 1;

							if (dialog.CancelCommandIndex == removeIndex)
							{
								removeIndex--;

								if (dialog.DefaultCommandIndex == removeIndex)
								{
									removeIndex--;
								}
							}

							commands.RemoveAt(removeIndex);

							dialog.SetCommands(commands.ToArray());
						}
						else
						{
							throw;
						}
					}
				}
				while (true);
			}

#if WINUI
			var information = await DispatcherQueueExtensions.TryEnqueueAsync(_dispatcher, CreateDialogUI, DispatcherQueuePriority.Normal);
#endif

			return (information == null)
				? defaultResult
				: information.Result;
		}
	}
}
