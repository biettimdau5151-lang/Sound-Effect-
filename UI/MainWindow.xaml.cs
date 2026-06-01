using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Interop;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using SoundEffect.DataClass;
using SoundEffect.Services;
using SoundEffect.UI.ViewModels;
using SoundEffect.Lib;
using SoundEffect.Lib.Controls;

namespace SoundEffect.UI
{
	public partial class MainWindow : MetroWindow
	{
		public MainWindow()
		{
			this.InitializeComponent();
			this.mainWVM = (base.DataContext as MainWVM);
			this.mainWVM.OnVolumeChanged += this.MainWVM_OnVolumeChanged;
			this.hookKeys.Callback += this.HookKeys_Callback;
			this.hookKeys.HookAll = true;
			this.group1 = Guid.Parse("{5C292D0D-C155-4B05-B940-314FC1935A97}");
			this.group2 = Guid.Parse("{2C23A933-9F3C-48CF-AE37-B03B4172F0CF}");
			this.group3 = Guid.Parse("{7AAD52E3-465A-4029-B9F7-8B0AF8CEB41F}");
			this.playSound = new NAudioPlaySound();
		}

		private void MainWVM_OnVolumeChanged(int obj)
		{
			this.playSound.Volume = (float)obj;
		}

		private void HookKeys_Callback(int keycode, bool isDown)
		{
			if (isDown)
			{
				if (this.hotKey.Keys.Contains(keycode))
				{
					return;
				}
				this.hotKey.Keys.Add(keycode);
			}
			else
			{
				this.hotKey.Keys.Remove(keycode);
			}
			if (isDown && !this.mainWVM.IsEditting)
			{
				try
				{
					if (Singleton.Setting.Setting.StopHotKey.Keys.Count > 0 && Singleton.Setting.Setting.StopHotKey.Keys.Except(this.hotKey.Keys).Count<int>() == 0)
					{
						this.Stop();
					}
					else
					{
						foreach (EffectData item in this.mainWVM.Effects.GetDataSave())
						{
							if (item.HotKey.Keys.Count > 0 && item.HotKey.Keys.Except(this.hotKey.Keys).Count<int>() == 0)
							{
								this.Play(item);
								break;
							}
						}
					}
				}
				catch
				{
				}
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			IntPtr hwnd = new WindowInteropHelper(this).Handle;
			if (Environment.OSVersion.Version.Major >= 10)
			{
				int attr = 33;
				int pref = 2;
				NativeMethods.DwmSetWindowAttribute(hwnd, attr, ref pref, 4);
			}
			List<Guid> groups = this.mainWVM.Effects.GetGroups().ToList<Guid>();
			if (!groups.Any((Guid x) => x == this.group2))
			{
				for (int i = 0; i < 21; i++)
				{
					this.mainWVM.Effects.Add(new EffectVM(new EffectData
					{
						GroupId = this.group2
					}));
				}
			}
			if (!groups.Any((Guid x) => x == this.group1))
			{
				for (int j = 0; j < 21; j++)
				{
					this.mainWVM.Effects.Add(new EffectVM(new EffectData
					{
						GroupId = this.group1
					}));
				}
			}
			if (!groups.Any((Guid x) => x == this.group3))
			{
				for (int k = 0; k < 21; k++)
				{
					this.mainWVM.Effects.Add(new EffectVM(new EffectData
					{
						GroupId = this.group3
					}));
				}
			}
			this.mainWVM.Effects.ShowGroup(this.group1);
			this.hookKeys.SetupHook();
			this.playSound.Volume = (float)this.mainWVM.Volume;
			try
			{
				Directory.CreateDirectory(Singleton.AppData);
				string emptySoundPath = Singleton.AppData + "\\empty_sound.mp3";
				if (!File.Exists(emptySoundPath))
				{
					byte[] silentMp3 = new byte[417];
					silentMp3[0] = 0xFF;
					silentMp3[1] = 0xFB;
					silentMp3[2] = 0x90;
					silentMp3[3] = 0x00;
					File.WriteAllBytes(emptySoundPath, silentMp3);
				}
				this.playSound.PlaySound(new FileInfo(emptySoundPath), CancellationToken.None);
			}
			catch
			{
			}
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			this.Stop();
		}

		private void btn_selectFile_Click(object sender, RoutedEventArgs e)
		{
			EffectVM effectVM = (sender as Button).DataContext as EffectVM;
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.InitialDirectory = Singleton.ExeDir;
			openFileDialog.Multiselect = false;
			openFileDialog.Filter = "Audio|*.mp3;*.wav;*.ogg;*.aac|All File|*.*";
			bool? flag = openFileDialog.ShowDialog();
			bool flag2 = true;
			if (flag.GetValueOrDefault() == flag2 & flag != null)
			{
				if (openFileDialog.FileName.Contains(Singleton.ExeDir))
				{
					effectVM.SoundFilePath = openFileDialog.FileName.Substring(Singleton.ExeDir.Length + 1);
					return;
				}
				effectVM.SoundFilePath = openFileDialog.FileName;
			}
		}

		private void TextBlockEdit_MouseDown(object sender, MouseButtonEventArgs e)
		{
			(sender as TextBlockEdit).IsEditing = true;
		}

		private void btn_PlaySound_Click(object sender, RoutedEventArgs e)
		{
			EffectVM effectVM = (sender as Button).DataContext as EffectVM;
			this.Play(effectVM.Data);
		}

		private void btn_Cancel_Click(object sender, RoutedEventArgs e)
		{
			if (this.mainWVM.IsEditting)
			{
				Singleton.Setting.Setting.StopHotKey.Keys.Clear();
				Singleton.Setting.Setting.StopHotKey.Keys.AddRange(this.hotKey.Keys);
				Singleton.Setting.Save();
				this.mainWVM.Refresh();
				return;
			}
			this.Stop();
		}

		private void EffectsMenus_click(object sender, RoutedEventArgs e)
		{
			MenuVM menuVM = (sender as MenuItem).DataContext as MenuVM;
			List<EffectVM> selectedItems = this.lv_gridEffect.SelectedItems.Cast<EffectVM>().ToList<EffectVM>();
			MenuAction action = menuVM.Action;
			if (action == MenuAction.Add)
			{
				this.mainWVM.Effects.Add(new EffectVM(new EffectData
				{
					GroupId = this.mainWVM.Effects.CurrentGroupId
				}));
				return;
			}
			if (action != MenuAction.Delete)
			{
				return;
			}
			Guid current = this.mainWVM.Effects.CurrentGroupId;
			if (MessageBox.Show("X�a c�c item d� ch?n?", "X�c nh?n", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
			{
				selectedItems.ForEach(delegate(EffectVM x)
				{
					this.mainWVM.Effects.Remove(x);
				});
			}
			if (!this.mainWVM.Effects.GetGroups().Any((Guid x) => x == current))
			{
				this.mainWVM.Effects.Add(new EffectVM(new EffectData
				{
					GroupId = current
				}));
			}
		}

		private async void Play(EffectData effectData)
		{
			FileInfo fileInfo;
			if (!string.IsNullOrEmpty(effectData.SoundFilePath))
			{
				if (effectData.SoundFilePath.Contains(Singleton.ExeDir))
				{
					fileInfo = new FileInfo(effectData.SoundFilePath);
				}
				else
				{
					fileInfo = new FileInfo(Singleton.ExeDir + "\\" + effectData.SoundFilePath);
				}
				if (fileInfo.Exists)
				{
					this.cancellationTokenSource.Cancel();
					this.cancellationTokenSource.Dispose();
					this.cancellationTokenSource = new CancellationTokenSource();
					await this.playSound.PlaySound(fileInfo, this.cancellationTokenSource.Token);
				}
			}
		}

		private void Stop()
		{
			this.cancellationTokenSource.Cancel();
			this.cancellationTokenSource.Dispose();
			this.cancellationTokenSource = new CancellationTokenSource();
		}

		private void cb_topMost_Toggled(object sender, RoutedEventArgs e)
		{
			base.Topmost = !base.Topmost;
		}

		private void btn_group1_Click(object sender, RoutedEventArgs e)
		{
			this.mainWVM.Effects.ShowGroup(this.group1);
		}

		private void btn_group2_Click(object sender, RoutedEventArgs e)
		{
			this.mainWVM.Effects.ShowGroup(this.group2);
		}

		private void btn_group3_Click(object sender, RoutedEventArgs e)
		{
			this.mainWVM.Effects.ShowGroup(this.group3);
		}

		private void btn_Edit_Click(object sender, RoutedEventArgs e)
		{
			this.mainWVM.IsEditting = !this.mainWVM.IsEditting;
		}

		private void btn_register_Click(object sender, RoutedEventArgs e)
		{
			EffectVM effectVM = (sender as Button).DataContext as EffectVM;
			effectVM.Data.HotKey.Keys.Clear();
			effectVM.Data.HotKey.Keys.AddRange(this.hotKey.Keys);
			effectVM.Refresh();
			effectVM.Save();
		}


		private readonly IPlaySound playSound;

		private readonly MainWVM mainWVM;

		private readonly HookKeys hookKeys = new HookKeys();

		private readonly Guid group1;

		private readonly Guid group2;

		private readonly Guid group3;

		private readonly SoundEffect.DataClass.HotKey hotKey = new SoundEffect.DataClass.HotKey();

		private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
	}

	internal static class NativeMethods
	{
		[DllImport("dwmapi.dll", PreserveSig = true)]
		internal static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
	}
}
