using System;
using System.IO;
using SoundEffect.DataClass;
using SoundEffect.Lib;

namespace SoundEffect
{
	internal static class Singleton
	{
		internal static string ExeDir { get; } = Directory.GetCurrentDirectory();

		internal static string AppData { get; } = Singleton.ExeDir + "\\AppData";

		internal static SaveSettingData<SettingData> Setting { get; } = new SaveSettingData<SettingData>(Singleton.ExeDir + "\\setting.json", 500);
	}
}
