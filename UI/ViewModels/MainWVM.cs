using System;
using System.Collections.Generic;
using SoundEffect.Lib;
using SoundEffect.Lib.ObservableCollection;

namespace SoundEffect.UI.ViewModels
{
	internal class MainWVM : BaseViewModel
	{
		public MainWVM()
		{
		}

		public event Action<int> OnVolumeChanged;

		public int Volume
		{
			get
			{
				return Singleton.Setting.Setting.Volume;
			}
			set
			{
				Singleton.Setting.Setting.Volume = value;
				Singleton.Setting.Save();
				base.NotifyPropertyChange("Volume");
				Action<int> onVolumeChanged = this.OnVolumeChanged;
				if (onVolumeChanged == null)
				{
					return;
				}
				onVolumeChanged(value);
			}
		}

		public bool IsEditting
		{
			get
			{
				return this._IsEditting;
			}
			set
			{
				this._IsEditting = value;
				base.NotifyPropertyChange("IsEditting");
				base.NotifyPropertyChange("EdittingText");
			}
		}

		public string EdittingText
		{
			get
			{
				if (!this.IsEditting)
				{
					return "Edit";
				}
				return "Done";
			}
		}

		public SaveGroupObservableCollection<Guid, EffectData, EffectVM> Effects { get; } = new SaveGroupObservableCollection<Guid, EffectData, EffectVM>((EffectData x) => new EffectVM(x), Singleton.ExeDir + "\\EffectsData.json", 500);

		public List<MenuVM> EffectsMenus { get; } = new List<MenuVM>
		{
			new MenuVM(MenuAction.Add),
			new MenuVM(MenuAction.Delete)
		};

		public string StopHotKey
		{
			get
			{
				return Singleton.Setting.Setting.StopHotKey.ToString();
			}
		}

		public void Refresh()
		{
			base.NotifyPropertyChange("StopHotKey");
		}

		public double Width
		{
			get
			{
				return Singleton.Setting.Setting.Width;
			}
			set
			{
				Singleton.Setting.Setting.Width = value;
				Singleton.Setting.Save();
				base.NotifyPropertyChange("Width");
			}
		}

		public double Height
		{
			get
			{
				return Singleton.Setting.Setting.Height;
			}
			set
			{
				Singleton.Setting.Setting.Height = value;
				Singleton.Setting.Save();
				base.NotifyPropertyChange("Height");
			}
		}

		public int Columns
		{
			get
			{
				return Singleton.Setting.Setting.Columns;
			}
			set
			{
				Singleton.Setting.Setting.Columns = value;
				Singleton.Setting.Save();
				base.NotifyPropertyChange("Columns");
			}
		}

		public int Rows
		{
			get
			{
				return Singleton.Setting.Setting.Rows;
			}
			set
			{
				Singleton.Setting.Setting.Rows = value;
				Singleton.Setting.Save();
				base.NotifyPropertyChange("Rows");
			}
		}

		public bool IsTopmost
		{
			get
			{
				return Singleton.Setting.Setting.IsTopmost;
			}
			set
			{
				Singleton.Setting.Setting.IsTopmost = value;
				Singleton.Setting.Save();
				base.NotifyPropertyChange("IsTopmost");
			}
		}

		private bool _IsEditting;
	}
}
