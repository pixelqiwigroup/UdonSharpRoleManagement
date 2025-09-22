using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ToggleButton : UdonSharpBehaviour
{
    [Header("Объект для включения/выключения")]
    public GameObject targetObject;
    
    [Header("Сообщение при взаимодействии")]
    public string interactMessage = "Переключено";
    
    public override void Interact()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(!targetObject.activeSelf);
            Debug.Log($"[Toggle] {Networking.LocalPlayer.displayName} переключил {targetObject.name} на {targetObject.activeSelf}");
        }
        else
        {
            Debug.LogWarning($"[Toggle] targetObject не назначен для {gameObject.name}");
        }
    }
}