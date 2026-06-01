using System;
using SoundEffect.Lib.Extensions;

namespace SoundEffect.UI.ViewModels
{
	internal class MenuVM
	{
		public MenuVM(MenuAction action)
		{
			this.Action = action;
			this.Text = action.GetAttribute<NameAttribute>().Name;
		}

		public string Text { get; }

		public MenuAction Action { get; }
	}
}
