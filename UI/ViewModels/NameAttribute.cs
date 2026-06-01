using System;

namespace SoundEffect.UI.ViewModels
{
	internal class NameAttribute : Attribute
	{
		public string Name { get; private set; }

		public NameAttribute(string name)
		{
			this.Name = name;
		}
	}
}
