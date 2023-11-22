## Getting Started
⚠ This documentation is applicable for versions 1.0.X and below.

Here is how to setup the service in your UWP or mobile apps made with Uno Platform (Uno.UI).

1. Install the `MessageDialogService.Uno` nuget package.

1. Create a `MessageDialogService` instance.
   
   ```csharp
   using Windows.ApplicationModel.Resources;
   
   // (...)

   var currentWindow = yourWindow; // Get the current window.;
   var coreDispatcher = currentWindow.CoreDispatcher;
   var resourceLoader = ResourceLoader.GetForViewIndependentUse();
   var resourceResolver = resourceKey => resourceLoader.GetString(resourceKey);

   var messageDialogService = new MessageDialogService.MessageDialogService(
      () => coreDispatcher,   
      new MessageDialogBuilderDelegate(resourceResolver)
   );
   ```

1. Use the service to prompt a message dialog.
   
   ```csharp
   await messageDialogService.ShowMessage(ct, mb => mb
      .Title("Oops!")
      .Content("Something went wrong.")
      .OkCommand()
   );
   ```