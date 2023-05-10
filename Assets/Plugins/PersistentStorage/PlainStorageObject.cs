using System;
using Newtonsoft.Json;
using UnityEngine;

namespace PersistentStorage
{
    public abstract class PlainStorageObject<TSelf,TData>
        where TSelf : PlainStorageObject<TSelf,TData>
    {
        public abstract string PrefKey { get; }
        public virtual TData Data { get; set; }
        
        private Action<TData> _afterLoading;
        private Func<TData> _beforeSaving;
        private readonly TData _defaultData;

        public PlainStorageObject(TData defaultData, Action<TData> afterLoading = null, Func<TData> beforeSaving = null)
        {
            _defaultData = defaultData;
            Set(afterLoading, beforeSaving);
        }
        
        private void Set(Action<TData> afterLoading = null, Func<TData> beforeSaving = null)
        {
            if (afterLoading != null)
                _afterLoading = afterLoading;
            
            if(beforeSaving != null)
                _beforeSaving = beforeSaving;
        }

        private void LoadDefaults()
        {
            var json = JsonConvert.SerializeObject(_defaultData);
            Data = JsonConvert.DeserializeObject<TData>(json);
        }
        
        private void AfterLoading() => _afterLoading?.Invoke(Data);

        private void BeforeSaving()
        {
            if (_beforeSaving == null)
                return;
            
            Data = _beforeSaving();
        }

        public TSelf Load()
        {
            if (PlayerPrefs.HasKey(PrefKey))
            {
                var json = PlayerPrefs.GetString(PrefKey);
                Data = JsonConvert.DeserializeObject<TData>(json);
            }
            else
            {
                LoadDefaults();
            }
            AfterLoading();

            return this as TSelf;
        }

        public TSelf Save()
        {
            BeforeSaving();

            var json = JsonConvert.SerializeObject(Data);
            PlayerPrefs.SetString(PrefKey, json);
            PlayerPrefs.Save();

            return this as TSelf;
        }
    }
}