using System;
using TMPro;
using UnityEngine;


namespace EHTool.LangKit {

    [RequireComponent(typeof(CanvasRenderer))]
    [AddComponentMenu("UI/EHTMP", 100)]
    [ExecuteAlways]
    public class EHTmp : TextMeshProUGUI, IEHText {

        [SerializeField] private string _key = string.Empty;

#nullable enable
        private IDisposable? _cancellation;

        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(IEHLangManager value)
        {
            SetText(_key);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            _cancellation = LangManager.Instance.Subscribe(this);
            base.OnValidate();
        }
#endif
        protected override void OnEnable()
        {
            _cancellation = LangManager.Instance.Subscribe(this);
            base.OnEnable();
        }

        override protected void OnDisable()
        {
            _cancellation?.Dispose();
            base.OnDisable();
        }

        protected override void OnDestroy()
        {
            _cancellation?.Dispose();
            base.OnDestroy();
        }

        public void SetText(string key)
        {

            _key = key;

            text = LangManager.Instance.GetStringByKey(key);

            if (text.Equals(string.Empty))
            {
                text = key;
            }

        }

        public void AddKey()
        {
            LangManager.Instance.GetStringByKey(_key, true);
        }

    }
}
