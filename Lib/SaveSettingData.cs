using Newtonsoft.Json;
using System.IO;
using System.Timers;

namespace SoundEffect.Lib
{
    internal class SaveSettingData<T> where T : new()
    {
        private readonly string _filePath;
        private readonly Timer _timer;
        private T _data;

        public T Setting
        {
            get
            {
                if (_data == null)
                    Load();
                return _data;
            }
        }

        public SaveSettingData(string filePath, int delay)
        {
            _filePath = filePath;
            _timer = new Timer(delay);
            _timer.Elapsed += (s, e) => SaveInternal();
            _timer.AutoReset = false;
        }

        private void Load()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _data = JsonConvert.DeserializeObject<T>(json);
                if (_data == null) _data = new T();
            }
            else
            {
                _data = new T();
            }
        }

        public void Save()
        {
            _timer.Stop();
            _timer.Start();
        }

        private void SaveInternal()
        {
            string dir = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);
            string json = JsonConvert.SerializeObject(_data, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }
    }
}
