using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Timers;

namespace SoundEffect.Lib.ObservableCollection
{
    internal class SaveGroupObservableCollection<TKey, TData, TVM> : ObservableCollection<TVM>
        where TData : IItemData<TKey>
        where TVM : IViewModel<TData>
    {
        private readonly Func<TData, TVM> _factory;
        private readonly string _filePath;
        private readonly Timer _timer;
        private TKey _currentGroupId;
        private readonly List<TVM> _allItems = new List<TVM>();

        public SaveGroupObservableCollection(Func<TData, TVM> factory, string filePath, int delay)
        {
            _factory = factory;
            _filePath = filePath;
            _timer = new Timer(delay);
            _timer.Elapsed += (s, e) => SaveInternal();
            _timer.AutoReset = false;
            Load();
        }

        private void Load()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                var list = JsonConvert.DeserializeObject<List<TData>>(json);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var vm = _factory(item);
                        vm.Change += OnItemChanged;
                        _allItems.Add(vm);
                        if (Equals(item.GroupId, _currentGroupId))
                            base.Add(vm);
                    }
                }
            }
        }

        private void SaveInternal()
        {
            string dir = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);
            var data = _allItems.Select(x => x.Data).ToList();
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        public void Save()
        {
            _timer.Stop();
            _timer.Start();
        }

        public new void Add(TVM vm)
        {
            vm.Change += OnItemChanged;
            _allItems.Add(vm);
            if (Equals(vm.Data.GroupId, _currentGroupId))
                base.Add(vm);
            Save();
        }

        public new void Remove(TVM vm)
        {
            vm.Change -= OnItemChanged;
            _allItems.Remove(vm);
            base.Remove(vm);
            Save();
        }

        private void OnItemChanged(object sender, TData data)
        {
            Save();
        }

        public IEnumerable<TKey> GetGroups()
        {
            return _allItems.Select(x => x.Data.GroupId).Distinct();
        }

        public List<TData> GetDataSave()
        {
            return Items.Select(x => x.Data).ToList();
        }

        public TKey CurrentGroupId => _currentGroupId;

        public void ShowGroup(TKey groupId)
        {
            _currentGroupId = groupId;
            Clear();
            foreach (var item in _allItems)
            {
                if (Equals(item.Data.GroupId, groupId))
                    base.Add(item);
            }
        }
    }
}
