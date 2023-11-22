using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessageDialogService
{
	internal class MessageDialogBuilder<TResult> : IMessageDialogBuilder<TResult>
	{
		private readonly IMessageDialogBuilderDelegate _messageDialogBuilderDelegate;
		private readonly List<IMessageDialogCommand<TResult>> _commands = new List<IMessageDialogCommand<TResult>>();

		public MessageDialogBuilder(IMessageDialogBuilderDelegate messageDialogBuilderDelegate)
		{
			_messageDialogBuilderDelegate = messageDialogBuilderDelegate;
		}

		public IList<IMessageDialogCommand<TResult>> Commands => _commands;

		public string ContentValue { get; set; }

		public string TitleValue { get; set; }
		
		public void AddCommand(string label, Action action, CommandInformation<TResult> id)
		{
			var command = _messageDialogBuilderDelegate.CreateCommand(id, label, action);

#if __IOS__
			_commands.Add(command);
#else
			_commands.Insert(0, command);
#endif
		}

		public IMessageDialogBuildResult<TResult> Build()
		{
			var dialog = _messageDialogBuilderDelegate.CreateMessageDialogBuildResult<TResult>();

			dialog.Content = ContentValue;

			if (!string.IsNullOrEmpty(TitleValue))
			{
				dialog.Title = TitleValue;
			}

			// Trick: default(int) is 0. We find the one-based index and substract one. We have the real index, or -1.
			var defaultIndex = _commands
				.Select(command => command.Id as CommandInformation<TResult>)
				.Select((info, index) => info.IsDefaultAccept ? index + 1 : 0)
				.FirstOrDefault(index => index > 0) - 1;

			// Rearrange the commands array according to the defaultIndex and cancelIndex
#if __ANDROID__
			if (defaultIndex != -1)
			{
				var element = _commands.ElementAt(defaultIndex);
				_commands.RemoveAt(defaultIndex);
				_commands.Insert(_commands.Count, element);
				defaultIndex = _commands.Count - 1;
			}
#endif

			var cancelIndex = _commands
				.Select(command => command.Id as CommandInformation<TResult>)
				.Select((info, index) => info.IsDefaultCancel ? index + 1 : 0)
				.LastOrDefault(index => index > 0) - 1;

#if __ANDROID__
			if (cancelIndex != -1)
			{
				var element = _commands.ElementAt(cancelIndex);
				_commands.RemoveAt(cancelIndex);
				_commands.Insert(0, element);
				cancelIndex = 0;
			}
#endif

			dialog.CancelCommandIndex = cancelIndex;
			dialog.DefaultCommandIndex = (defaultIndex < 0) ? 0 : defaultIndex;

			dialog.SetCommands(_commands.ToArray());

			return dialog;
		}

		IMessageDialogBuildResult IMessageDialogBuilder.Build()
		{
			return Build();
		}

		public string GetResourceString(string resourceKey)
		{
			return _messageDialogBuilderDelegate.GetResourceString(resourceKey);
		}
	}
}
