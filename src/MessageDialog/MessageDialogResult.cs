using System;
using System.Collections.Generic;
using System.Text;

namespace MessageDialogService
{
	/// <summary>
	/// Standard values that can be used as message dialog commands, thus returned as a user response.
	/// </summary>
	public enum MessageDialogResult
	{
		Ok = 1,
		Accept = 2,
		Cancel = 3,
		No = 4
	}
}
