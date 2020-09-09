using System;
using System.Collections.Generic;
using System.Text;

namespace MessageDialogService
{
	public static partial class MessageDialogBuilderExtensions
	{
		public const string OkLabelResourceKey = "MessageDialog_Ok_Label";
		public const string CancelLabelResourceKey = "MessageDialog_Cancel_Label";
		public const string RetryLabelResourceKey = "MessageDialog_Retry_Label";
		public const string CloseLabelResourceKey = "MessageDialog_Close_Label";

		/// <summary>
		/// Sets the title to display in the dialog.
		/// </summary>
		public static TBuilder Title<TBuilder>(this TBuilder builder, string title)
			where TBuilder : IMessageDialogBuilder
		{
			builder.TitleValue = title;
			return builder;
		}

		/// <summary>
		/// Sets the resource key to use to get the title to display in the dialog.
		/// </summary>
		public static TBuilder TitleResource<TBuilder>(this TBuilder builder, string titleResourceKey)
			where TBuilder : IMessageDialogBuilder
		{
			builder.TitleValue = builder.GetResourceString(titleResourceKey);
			return builder;
		}

		/// <summary>
		/// Sets the body of the message to display in the dialog.
		/// </summary>
		public static TBuilder Content<TBuilder>(this TBuilder builder, string content)
			where TBuilder : IMessageDialogBuilder
		{
			builder.ContentValue = content;
			return builder;
		}

		/// <summary>
		/// Sets the resource key to use to get the body of the message to display in the dialog.
		/// </summary>
		public static TBuilder ContentResource<TBuilder>(this TBuilder builder, string contentResourceKey)
			where TBuilder : IMessageDialogBuilder
		{
			builder.ContentValue = builder.GetResourceString(contentResourceKey);
			return builder;
		}

		/// <summary>
		/// Adds a button to display in the dialog, with a specific label.
		/// </summary>
		public static TBuilder Command<TResult, TBuilder>(this TBuilder builder, TResult result, string label, bool isDefaultAccept = false, bool isDefaultCancel = false, bool isDestructive = false)
			where TBuilder : IMessageDialogBuilder<TResult>
		{
			builder.AddCommand(label, null, new CommandInformation<TResult>(result, isDefaultAccept, isDefaultCancel, isDestructive));
			return builder;
		}

		/// <summary>
		/// Adds a button to display in the dialog, using a resource key to get the label.
		/// </summary>
		public static TBuilder CommandResource<TResult, TBuilder>(this TBuilder builder, TResult result, string labelResourceKey, bool isDefaultAccept = false, bool isDefaultCancel = false, bool isDestructive = false)
			where TBuilder : IMessageDialogBuilder<TResult>
		{
			builder.AddCommand(builder.GetResourceString(labelResourceKey), null, new CommandInformation<TResult>(result, isDefaultAccept, isDefaultCancel, isDestructive));
			return builder;
		}

		/// <summary>
		/// <para>Adds a button to display in the dialog, using the "MessageDialog_Ok_Label" resource key,
		/// which will return <see cref="MessageDialogResult.Ok"/> when selected.</para>
		/// <para>IMPORTANT: Make sure your application defines the "MessageDialog_Ok_Label" resource key.</para>
		/// </summary>
		/// <remarks>Make sure your application defines the "MessageDialog_Ok_Label" resource key.</remarks>
		public static TBuilder OkCommand<TBuilder>(this TBuilder builder)
			where TBuilder : IMessageDialogBuilder<MessageDialogResult>
		{
			builder.AddCommand(builder.GetResourceString(OkLabelResourceKey), null, new CommandInformation<MessageDialogResult>(MessageDialogResult.Ok, isDefaultAccept: true));
			return builder;
		}

		/// <summary>
		/// <para>Adds a button to display in the dialog, using the "MessageDialog_Cancel_Label" resource key,
		/// which will return <see cref="MessageDialogResult.Cancel"/> when selected.</para>
		/// <para>IMPORTANT: Make sure your application defines the "MessageDialog_Cancel_Label" resource key.</para>
		/// </summary>
		/// <remarks>Make sure your application defines the "MessageDialog_Cancel_Label" resource key.</remarks>
		public static TBuilder CancelCommand<TBuilder>(this TBuilder builder)
			where TBuilder : IMessageDialogBuilder<MessageDialogResult>
		{
			builder.AddCommand(builder.GetResourceString(CancelLabelResourceKey), null, new CommandInformation<MessageDialogResult>(MessageDialogResult.Cancel, isDefaultCancel: true));
			return builder;
		}

		/// <summary>
		/// <para>Adds a button to display in the dialog, using the given resource key,
		/// which will return <see cref="MessageDialogResult.No"/> when selected.</para>
		/// <para>Use this button in conjunction with Accept</para>
		/// </summary>
		public static TBuilder NoCommand<TBuilder>(this TBuilder builder, string noLabelResourceKey)
			where TBuilder : IMessageDialogBuilder<MessageDialogResult>
		{
			builder.AddCommand(builder.GetResourceString(noLabelResourceKey), null, new CommandInformation<MessageDialogResult>(MessageDialogResult.No, isDefaultCancel: true));
			return builder;
		}

		/// <summary>
		/// Adds a button to display the accept command.
		/// </summary>
		public static TBuilder AcceptCommand<TBuilder>(this TBuilder builder, string acceptResourceKey)
			where TBuilder : IMessageDialogBuilder<MessageDialogResult>
		{
			builder.AddCommand(builder.GetResourceString(acceptResourceKey), null, new CommandInformation<MessageDialogResult>(MessageDialogResult.Accept, isDefaultAccept: true, isDefaultCancel: false));
			return builder;
		}

		/// <summary>
		/// Adds a button display the close command, which returns the Ok dialog result.
		/// </summary>
		/// <typeparam name="TBuilder"></typeparam>
		/// <param name="builder"></param>
		/// <returns></returns>
		public static TBuilder CloseCommand<TBuilder>(this TBuilder builder)
			where TBuilder : IMessageDialogBuilder<MessageDialogResult>
		{
			builder.AddCommand(builder.GetResourceString(CloseLabelResourceKey), null, new CommandInformation<MessageDialogResult>(MessageDialogResult.Ok, isDefaultAccept: true, isDefaultCancel: true));
			return builder;
		}
	}
}
