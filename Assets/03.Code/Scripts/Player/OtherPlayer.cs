using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OtherPlayer : Players
{
    [SerializeField] private TextMeshPro idText;
    private string id;

    public void Init(string id, Vector3 position)
    {
        this.id = id;
        idText.text = id;
        transform.position = position;
    }
}
