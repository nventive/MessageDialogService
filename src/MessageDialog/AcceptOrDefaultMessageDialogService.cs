using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageDialogService
{
	/// <summary>
	/// This <see cref="IMessageDialogService"/> will accept any message dialog if possible.
	/// If it can't accept it, it will return the default result.
	/// </summary>
	public class AcceptOrDefaultMessageDialogService: IMessageDialogService
	{
		public Task<TResult> ShowMessage<TResult>(
			CancellationToken ct,
			Func<IMessageDialogBuilder<TResult>, IMessageDialogBuilder<TResult>> messageBuilder,
			TResult defaultResult = default
		)
		{
			if (messageBuilder is null)
			{
				throw new ArgumentNullException(nameof(messageBuilder));
			}

			var result = AcceptOrDefault(messageBuilder, defaultResult);

			return Task.FromResult(result);
		}

		public Task<MessageDialogResult> ShowMessage(
			CancellationToken ct,
			Func<IMessageDialogBuilder<MessageDialogResult>, IMessageDialogBuilder<MessageDialogResult>> messageBuilder,
			MessageDialogResult defaultResult = MessageDialogResult.Cancel
		)
		{
			if (messageBuilder is null)
			{
				throw new ArgumentNullException(nameof(messageBuilder));
			}

			var result = AcceptOrDefault(messageBuilder, defaultResult);

			return Task.FromResult(result);
		}

		private TResult AcceptOrDefault<TResult>(
			Func<IMessageDialogBuilder<TResult>, IMessageDialogBuilder<TResult>> messageBuilder,
			TResult defaultResult
		)
		{
			var innerBuilder = new MockMessageDialogBuilder<TResult>();
			var builder = messageBuilder(innerBuilder);

			var acceptCommand = builder
				.Commands
				.FirstOrDefault(c => c.Id.IsDefaultAccept);

			return acceptCommand != null
				? acceptCommand.Id.Result
				: defaultResult;
		}

		private class MockMessageDialogBuilder<TResult> : IMessageDialogBuilder<TResult>
		{
			public IList<IMessageDialogCommand<TResult>> Commands { get; } = new List<IMessageDialogCommand<TResult>>();

			public string ContentValue { get; set; }

			public string TitleValue { get; set; }

			public void AddCommand(string label, Action action, CommandInformation<TResult> id)
				=> Commands.Add(new MockMessageDialogCommand<TResult>(id));

			public string GetResourceString(string resourceKey) => resourceKey;

			public IMessageDialogBuildResult<TResult> Build() => null;

			IMessageDialogBuildResult IMessageDialogBuilder.Build() => null;
		}

		private class MockMessageDialogCommand<TResult> : IMessageDialogCommand<TResult>
		{
			public MockMessageDialogCommand(CommandInformation<TResult> commandInformation)
			{
				Id = commandInformation;
			}

			public CommandInformation<TResult> Id { get; private set; }
		}
	}
}
