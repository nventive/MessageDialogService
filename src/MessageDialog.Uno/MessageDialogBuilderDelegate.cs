﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Windows.UI.Popups;
using Uno.Extensions;
#if __IOS__ || __ANDROID__ || __WASM__
using Uno.UI.Toolkit;
#endif

namespace MessageDialogService
{
	public partial class MessageDialogBuilderDelegate : IMessageDialogBuilderDelegate
	{
		private readonly Func<string, string> _resourcesProvider;

		/// <summary>
		/// Initialises a new instance of the <see cref="MessageDialogBuilderDelegate"/> class.
		/// </summary>
		/// <param name="resourcesProvider">Returns a resource based on the provided key</param>
		public MessageDialogBuilderDelegate(Func<string, string> resourcesProvider)
		{
			_resourcesProvider = resourcesProvider;
		}

		public IMessageDialogCommand<TResult> CreateCommand<TResult>(CommandInformation<TResult> id, string label, Action action)
		{
			return new UICommandMessageDialogCommand<TResult>(id, label, action);
		}

		public IMessageDialogBuildResult<TResult> CreateMessageDialogBuildResult<TResult>()
		{
			return new MessageDialogWrapper<TResult>();
		}

		public string GetResourceString(string key)
		{
			return _resourcesProvider(key);
		}

		private class MessageDialogWrapper<TResult> : IMessageDialogBuildResult<TResult>
		{
			private Windows.UI.Popups.MessageDialog _messageDialog;

			public MessageDialogWrapper()
			{
				_messageDialog = new Windows.UI.Popups.MessageDialog(content: string.Empty);
			}

			public string Title
			{
				get => _messageDialog.Title;
				set => _messageDialog.Title = value;
			}

			public string Content
			{
				get => _messageDialog.Content;
				set => _messageDialog.Content = value;
			}

			public int CancelCommandIndex
			{
				get => (int)_messageDialog.CancelCommandIndex;
				set => _messageDialog.CancelCommandIndex = (uint)value; // Yes, CancelCommandIndex is uint, and it expects -1 for "nothing".
			}

			public int DefaultCommandIndex
			{
				get => (int)_messageDialog.DefaultCommandIndex;
				set => _messageDialog.DefaultCommandIndex = (uint)value;
			}

			public IMessageDialogCommand<TResult>[] GetCommands()
			{
				return _messageDialog
					.Commands
					.Select(c => (c as UICommandMessageDialogCommand<TResult>))
					.Trim()
					.ToArray();
			}

			public void SetCommands(IMessageDialogCommand<TResult>[] commands)
			{
				var uiCommands = commands
					.Select(c => (c as UICommandMessageDialogCommand<TResult>)?.InnerCommand)
					.Trim()
					.ToArray();

				_messageDialog.Commands.ReplaceWith(uiCommands);
			}

			public async Task<CommandInformation<TResult>> ShowMessage(CancellationToken ct)
			{
				var uiCommand = await _messageDialog.ShowAsync().AsTask(ct);

				return uiCommand?.Id as CommandInformation<TResult>;
			}

			async Task IMessageDialogBuildResult.ShowMessage(CancellationToken ct)
			{
				await ShowMessage(ct);
			}
		}

		// Note about this implemenation: We could not create custom class implementing IUICommand, as this broke
		// MessageDialog.CancelCommandIndex. The commands must all be UIComand instances. That's why we store more 
		// info in the Id instead.
		private class UICommandMessageDialogCommand<TResult> : IMessageDialogCommand<TResult>
		{
			public UICommandMessageDialogCommand(CommandInformation<TResult> id, string label, Action action)
			{
				InnerCommand = new UICommand(label, c => action?.Invoke(), id);

#if __IOS__ || __ANDROID__ || __WASM__
				((UICommand)InnerCommand).SetDestructive(id.IsDestructive);
#endif
			}

			public CommandInformation<TResult> Id => InnerCommand.Id as CommandInformation<TResult>;

			public IUICommand InnerCommand { get; private set; }
		}
	}
}
