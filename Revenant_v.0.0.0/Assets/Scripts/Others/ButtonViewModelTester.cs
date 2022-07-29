using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class ButtonViewModelTester : MonoBehaviour
{
    public ButtonViewModel MyTestShoutButton;
    public ButtonViewModel MyTestDelayButton;        

    public void Awake()
    {
        // 초기화 객체로 버튼 ViewModel을 초기화
        MyTestShoutButton.Initialize(this);
        // 호출 함수를 버튼 ViewModel에 바인딩
        MyTestShoutButton.BindAction(ShoutAsync);

        MyTestDelayButton.Initialize(this);
        MyTestDelayButton.BindAction(async () =>
        {
            await Task.Delay(1000);
            Debug.Log("Do something...");
        });
    }

    /// <summary>비동기 함수</summary>
    public async void ShoutAsync()
    {
        await Task.Delay(1000);
        Debug.Log("야호야호야호");
    }
}