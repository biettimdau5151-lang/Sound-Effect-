using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace SoundEffect.DataClass
{
	internal class HotKey
	{
		public List<int> Keys { get; set; } = new List<int>();

		[JsonIgnore]
		public bool IsShift
		{
			get
			{
				return this.Keys.Contains(160) || this.Keys.Contains(161);
			}
		}

		[JsonIgnore]
		public bool IsCtrl
		{
			get
			{
				return this.Keys.Contains(162) || this.Keys.Contains(163);
			}
		}

		[JsonIgnore]
		public bool IsAlt
		{
			get
			{
				return this.Keys.Contains(164) || this.Keys.Contains(165);
			}
		}

		public override string ToString()
		{
			return string.Join(" + ", this.Keys.Select(delegate(int x)
			{
				Keys keys = (Keys)x;
				return keys.ToString();
			}));
		}

		private void test()
		{
		}

		private const int LShift = 160;

		private const int RShift = 161;

		private const int LCtrl = 162;

		private const int RCtrl = 163;

		private const int LAlt = 164;

		private const int RAlt = 165;
	}
}
