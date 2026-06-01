using System;
using SoundEffect.DataClass;
using SoundEffect.Lib.ObservableCollection;

namespace SoundEffect.UI.ViewModels
{
	internal class EffectData : IItemData<Guid>
	{
		public Guid GroupId { get; set; } = Guid.NewGuid();

		public string Name { get; set; } = "New Effect";

		public string SoundFilePath { get; set; }

		public HotKey HotKey { get; set; } = new HotKey();
	}
}
