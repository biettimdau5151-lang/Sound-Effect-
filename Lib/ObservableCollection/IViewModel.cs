namespace SoundEffect.Lib.ObservableCollection
{
    internal delegate void ChangeCallBack<T>(object sender, T data);

    internal interface IViewModel<TData>
    {
        TData Data { get; }
        event ChangeCallBack<TData> Change;
        void Refresh();
        void Save();
    }
}
