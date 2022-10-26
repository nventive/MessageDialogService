using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Windows.UI.Popups;
#if __IOS__ || __ANDROID__ || __WASM__
using Uno.UI.Toolkit;
#endif

namespace MessageDialogService
{
	public partial class MessageDialogBuilderDelegate : IMessageDialogBuilderDelegate
	{
		private readonly Func<string, string> _resourcesProvider;
#if WINDOWS10_0_18362_0_OR_GREATER
		private readonly IntPtr? _windowHandle;
#endif

#if WINDOWS10_0_18362_0_OR_GREATER
		/// <summary>
		/// Initialises a new instance of the <see cref="MessageDialogBuilderDelegate"/> class.
		/// </summary>
		/// <param name="resourcesProvider">Returns a resource based on the provided key</param>
		/// <param name="windowHandle">Based on the current window. This null safe param is only required for the windows implementation, otherwise it should receive null.</param>
		public MessageDialogBuilderDelegate(Func<string, string> resourcesProvider, IntPtr? windowHandle)
		{
			_resourcesProvider = resourcesProvider;
			_windowHandle = windowHandle;
		}
#else
        /// <summary>
        /// Initialises a new instance of the <see cref="MessageDialogBuilderDelegate"/> class.
        /// </summary>
        /// <param name="resourcesProvider">Returns a resource based on the provided key</param>
        public MessageDialogBuilderDelegate(Func<string, string> resourcesProvider)
		{
			_resourcesProvider = resourcesProvider;
		}
#endif
		public IMessageDialogCommand<TResult> CreateCommand<TResult>(CommandInformation<TResult> id, string label, Action action)
		{
			return new UICommandMessageDialogCommand<TResult>(id, label, action);
		}

		public IMessageDialogBuildResult<TResult> CreateMessageDialogBuildResult<TResult>()
		{
#if WINDOWS10_0_18362_0_OR_GREATER
			return new MessageDialogWrapper<TResult>(_windowHandle);
#else
            return new MessageDialogWrapper<TResult>();
#endif
        }

		public string GetResourceString(string key)
		{
			return _resourcesProvider(key);
		}

		private class MessageDialogWrapper<TResult> : IMessageDialogBuildResult<TResult>
		{
			private MessageDialog _messageDialog;
#if WINDOWS10_0_18362_0_OR_GREATER
			private IntPtr? _windowHandle;
            public MessageDialogWrapper(IntPtr? windowHandle)
			{
				_messageDialog = new Windows.UI.Popups.MessageDialog(content: string.Empty);
				_windowHandle = windowHandle;
			}
#else
            public MessageDialogWrapper()
            {
                _messageDialog = new Windows.UI.Popups.MessageDialog(content: string.Empty);
            }
#endif

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
#if WINDOWS10_0_18362_0_OR_GREATER
				WinRT.Interop.InitializeWithWindow.Initialize(_messageDialog, (IntPtr)_windowHandle);
#endif
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
