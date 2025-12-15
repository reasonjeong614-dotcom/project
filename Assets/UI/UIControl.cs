using System;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.UIElements;

public class UIControl : MonoBehaviour
{
    public DataTest userData;

    UIDocument document;

    Label gameStartLabel;
    TextField id;
    TextField pw;
    Button login;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        document = GetComponent<UIDocument>();
        VisualElement root = document.rootVisualElement;

        //UI 요소 찾기
        gameStartLabel = root.Q<Label>("GameStart");
        id = root.Q<TextField>("ID");
        pw = root.Q<TextField>("PW");
        login = root.Q<Button>("Login");

        gameStartLabel.RegisterCallback<MouseEnterEvent>(OnMouseEnter);
        gameStartLabel.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
        //login.RegisterCallback<ClickEvent>(OnClick);
        login.clicked += OnClick;
        id.RegisterCallback<KeyDownEvent>(OnKeyDown);

    }

    private void OnMouseEnter(MouseEnterEvent evt)
    {
        gameStartLabel.text = "이건 버튼이 아닙니다.";
    }

    private void OnMouseLeave(MouseLeaveEvent evt)
    {
        gameStartLabel.text = "게임 시작하기";
    }

    void OnClick()
    {
        string inputId = id.value;
        userData.userId = inputId;
    }

    void OnKeyDown(KeyDownEvent evt)
    {

    }

    private void OnDestroy()
    {
        gameStartLabel.UnregisterCallback<MouseEnterEvent>(OnMouseEnter);
        //login.RegisterCallback<ClickEvent>(OnClick);
        login.clicked -= OnClick;
        id.UnregisterCallback<KeyDownEvent>(OnKeyDown);
    }

}
