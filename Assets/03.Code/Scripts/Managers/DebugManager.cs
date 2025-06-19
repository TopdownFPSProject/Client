using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : Singleton<DebugManager>
{
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform rectTransform;

    public void Debug(string text)
    {
        debugText.text += debugText.text == "" ? text : "\n" + text;

        //채팅이 낑기는 버그가 있어서 계속해서 위치를 조정해준다.
        Fit(debugText.GetComponent<RectTransform>());
        Fit(rectTransform);

        Invoke("ScrollDelay", 0.03f);
    }
    void Fit(RectTransform Rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);

    //이렇게 되면 스크롤이 맨 아래로 항상 유지되어있다.
    void ScrollDelay() => scrollRect.verticalScrollbar.value = 0;
}
