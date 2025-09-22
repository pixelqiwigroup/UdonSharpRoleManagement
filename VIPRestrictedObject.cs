using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class VIPRestrictedObject : UdonSharpBehaviour
{
    [Header("Ссылка на RoleManager (обязательно)")]
    public RoleManager roleManager;
    
    [Header("Объект, который будет ограничен")]
    public GameObject targetObject;
    
    [Header("Сообщение при отказе")]
    public string accessDeniedMessage = "Доступ только для VIP";
    
    private void Start()
    {
        // Проверяем, что RoleManager назначен
        if (roleManager == null)
        {
            Debug.LogError($"[VIPRestrictedObject] RoleManager не назначен для {gameObject.name}");
            return;
        }
        
        if (targetObject == null)
        {
            targetObject = gameObject;
        }
    }
    
    public override void Interact()
    {
        if (roleManager == null)
        {
            Debug.LogError($"[VIPRestrictedObject] RoleManager не найден для {gameObject.name}");
            return;
        }
        
        if (roleManager.IsVIP())
        {
            // VIP доступ разрешен
            Debug.Log($"[VIP Access] {Networking.LocalPlayer.displayName} (VIP) получил доступ к {gameObject.name}");
            ToggleObject();
        }
        else
        {
            // Доступ запрещен
            Debug.Log($"[VIP Access] {Networking.LocalPlayer.displayName} отказано в доступе к {gameObject.name}");
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "ShowAccessDenied");
        }
    }
    
    public void ShowAccessDenied()
    {
        Debug.Log(accessDeniedMessage);
    }
    
    private void ToggleObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }
}