# MessageDialogService

- [allcontributors](https://allcontributors.org/) is configured. It helps adding contributors to the README.

Show native prompt with custom content and title.

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE)

## Getting Started

Install the latest version of `MessageDialogService.Uno` in your project.

There are 2 classes available that implement `IMessageDialogService`:

- `AcceptOrDefaultMessageDialogService`
  - Does not have a reference to Uno.
  - [Reference](https://github.com/nventive/MessageDialogService/blob/master/src/MessageDialog/AcceptOrDefaultMessageDialogService.cs)
- `MessageDialogService`
  - Has a reference to Uno.
  - [Reference](https://github.com/nventive/MessageDialogService/blob/master/src/MessageDialog.Uno/MessageDialogService.cs)

## Features

### 1. Show Modal

TODO: Explain builder and how to add content and title.

### 2. Force Close Modal
It is possible to manually close the current modal with:

```csharp
messageDialogService.ForceCloseModal();
```

This function is not async, therefore does not need to be awaited. This function will cancel the CancellationToken used by the modal. This will throw a System.OperationCanceledException.

## Changelog

Please consult the [CHANGELOG](CHANGELOG.md) for more information about version
history.

## License

This project is licensed under the Apache 2.0 license - see the
[LICENSE](LICENSE) file for details.

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on the process for
contributing to this project.

Be mindful of our [Code of Conduct](CODE_OF_CONDUCT.md).

## Contributors

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- ALL-CONTRIBUTORS-LIST:END -->
