using System;

namespace SoundEffect.DataClass
{
	internal class SettingData
	{
		public int Volume { get; set; } = 100;

		public HotKey StopHotKey { get; set; } = new HotKey();

		public double Width { get; set; } = 550.0;

		public double Height { get; set; } = 600.0;

		public int Columns { get; set; } = 3;

		public int Rows { get; set; } = 8;

		public bool IsTopmost { get; set; } = true;
	}
}
