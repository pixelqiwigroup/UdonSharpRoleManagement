using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class StaffRestrictedObject : UdonSharpBehaviour
{
    [Header("Ссылка на RoleManager (обязательно)")]
    public RoleManager roleManager;
    
    [Header("Объект, который будет ограничен")]
    public GameObject targetObject;
    
    [Header("Сообщение при отказе")]
    public string accessDeniedMessage = "Доступ только для Staff";
    
    private void Start()
    {
        // Проверяем, что RoleManager назначен
        if (roleManager == null)
        {
            Debug.LogError($"[StaffRestrictedObject] RoleManager не назначен для {gameObject.name}");
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
            Debug.LogError($"[StaffRestrictedObject] RoleManager не найден для {gameObject.name}");
            return;
        }
        
        if (roleManager.IsStaff())
        {
            // Staff доступ разрешен
            Debug.Log($"[Staff Access] {Networking.LocalPlayer.displayName} (Staff) получил доступ к {gameObject.name}");
            ToggleObject();
        }
        else
        {
            // Доступ запрещен
            Debug.Log($"[Staff Access] {Networking.LocalPlayer.displayName} отказано в доступе к {gameObject.name}");
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