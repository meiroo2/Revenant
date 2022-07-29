using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

    
    /// <summary>
    /// 버튼 ViewModel입니다. Unity Inspector에서 버튼의 이름을 설정한 뒤 대상 mono객체로 초기화 후
    /// 이벤트를 바인딩해 사용합니다.
    /// </summary>
    [Serializable]
    public class ButtonViewModel
    {
        /// <summary>버튼의 ViewModel이름입니다.</summary>
        [field : SerializeField]
        public string ButtonName { get; private set; } = "";
        private Button mButtonObject;
        private UnityAction mBindedAction;
        private bool IsInitialized = false;
        
        /// <summary>버튼 ViewModel을 초기화합니다. 인자의 자식 객체 내에서 버튼을 찾습니다.</summary>
        /// <param name="mono">초기화를 하는 momo객체</param>
        public void Initialize(MonoBehaviour mono)
        {
            var buttons = mono.GetComponentsInChildren<Button>();

            foreach (var b in buttons)
            {
                if (b.name == ButtonName)
                {
                    mButtonObject = b;
                    IsInitialized = true;
                    return;
                }
            }

            //Debug.LogError($"There is no such button as {mButtonName}");
        }

        /// <summary>버튼에 Action을 바인딩합니다.</summary>
        /// <param name="onClickAction">바인딩 할 함수</param>
        public void BindAction(UnityAction onClickAction)
        {
            if (!IsInitialized)
            {
                Debug.LogError("Button view model doesn't initialized!");
                return;
            }

            mBindedAction = onClickAction;
            mButtonObject.onClick.AddListener(mBindedAction);
        }

        /// <summary>버튼에 할당된 Action을 해제합니다.</summary>
        public void ReleaseAction()
        {
            if (!IsInitialized)
            {
                return;
            }

            mButtonObject.onClick.RemoveListener(mBindedAction);
            mBindedAction = null;
        }
    }