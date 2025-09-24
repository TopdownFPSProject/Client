using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Players : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float lerpSpeed = 0.1f;
    // 오차가 snapThreshold을 넘으면 순간이동으로 동기화 됨
    [SerializeField] protected float snapThreshold = 5.0f;
    // 정보가 담긴 프리팹
    [SerializeField] protected GameObject infoPrefab;
    
    private Image hpBar;
    private TextMeshProUGUI idText;
    public string id;
    protected float maxHp;
    protected float currentHp;
    protected Vector3 targetPosition;

    // 플레이어 머리 위치
    private Transform headTransform;
    // Canvas에 붙은 HP UI Rect
    private RectTransform infoUI;
    // ID Rect
    private RectTransform idTextUI;

    protected virtual void Start()
    {
        headTransform = transform;

        Canvas mainCanvas = FindObjectOfType<Canvas>();
        if (mainCanvas == null)
        {
            Debug.LogError("씬에 Canvas가 없습니다!");
            return;
        }

        if (infoPrefab == null)
        {
            Debug.LogError("healthBarPrefab이 Inspector에 할당되지 않았습니다!");
            return;
        }

        GameObject infoObj = Instantiate(infoPrefab, mainCanvas.transform);

        // Canvas상에서의 UI
        infoUI = infoObj.GetComponent<RectTransform>();

        Transform fillTransform = infoObj.transform.Find("HpBar");
        Transform idTextTransform = infoObj.transform.Find("IDText");

        hpBar = fillTransform.GetComponent<Image>();
        idText = idTextTransform.GetComponent<TextMeshProUGUI>();

        idText.text = id;
    }

    protected virtual void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(headTransform.position + Vector3.up * 5.0f);
        if (screenPos.z > 0)
        {
            infoUI.gameObject.SetActive(true);
            infoUI.position = screenPos;   // UI 위치 갱신
        }
        else
        {
            infoUI.gameObject.SetActive(false);
        }
    }

    protected virtual void FixedUpdate()
    {
        float dist = Vector3.Distance(transform.position, targetPosition);
        if (dist > snapThreshold)
        {
            Debug.Log("순간이동");
            transform.position = targetPosition; // 순간이동
        }
        else
        {
            //Debug.Log("부드러운 이동");
            transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed);
        }
    }

    public virtual void Init(string id, Vector3 position)
    {
        maxHp = 100;
        currentHp = 100;
    }

    // 실시간으로 위치와 시야 동기화
    public void SetServerPosition(Vector3 newPosition, float angle)
    {
        targetPosition = newPosition;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        float ratio = Mathf.Clamp01(currentHp / maxHp);
        hpBar.fillAmount = ratio;
    }
}
