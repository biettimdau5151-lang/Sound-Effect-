using System;
using SoundEffect.Lib;
using SoundEffect.Lib.ObservableCollection;

namespace SoundEffect.UI.ViewModels
{
	internal class EffectVM : BaseViewModel, IViewModel<EffectData>
	{
		public EffectVM(EffectData effectData)
		{
			this.Data = effectData;
		}

		public EffectData Data { get; }

		public event ChangeCallBack<EffectData> Change;

		public string Name
		{
			get
			{
				return this.Data.Name;
			}
			set
			{
				this.Data.Name = value;
				base.NotifyPropertyChange("Name");
				ChangeCallBack<EffectData> change = this.Change;
				if (change == null)
				{
					return;
				}
				change(this, this.Data);
			}
		}

		public string SoundFilePath
		{
			get
			{
				return this.Data.SoundFilePath;
			}
			set
			{
				this.Data.SoundFilePath = value;
				base.NotifyPropertyChange("SoundFilePath");
				ChangeCallBack<EffectData> change = this.Change;
				if (change == null)
				{
					return;
				}
				change(this, this.Data);
			}
		}

		public string Key
		{
			get
			{
				return this.Data.HotKey.ToString();
			}
		}

		public void Refresh()
		{
			base.NotifyPropertyChange("Key");
		}

		public void Save()
		{
			ChangeCallBack<EffectData> change = this.Change;
			if (change == null)
			{
				return;
			}
			change(this, this.Data);
		}
	}
}
