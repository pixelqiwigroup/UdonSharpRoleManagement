using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class AccessControlledButton : UdonSharpBehaviour
{
    [Header("Настройки доступа")]
    [Tooltip("0 = Все, 1 = VIP, 2 = Staff")]
    public int requiredAccess = 0; // 0=Everyone, 1=VIP, 2=Staff
    
    [Header("Ссылка на RoleManager")]
    public RoleManager roleManager;
    
    [Header("Объект для переключения")]
    public GameObject targetObject;
    
    [Header("Сообщения")]
    public string accessDeniedMessage = "Доступ запрещен";
    
    public override void Interact()
    {
        bool hasAccess = CheckAccess();
        
        if (hasAccess)
        {
            if (targetObject != null)
            {
                targetObject.SetActive(!targetObject.activeSelf);
                // Debug.Log($"[Access] {Networking.LocalPlayer.displayName} переключил {targetObject.name}");
            }
        }
        else
        {
            // Debug.Log($"[Access] {Networking.LocalPlayer.displayName} - доступ запрещен");
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "ShowAccessDenied");
        }
    }
    
    private bool CheckAccess()
    {
        if (roleManager == null) 
            return requiredAccess == 0; // Если RoleManager не назначен, доступ только для Everyone
        
        switch (requiredAccess)
        {
            case 0: // Everyone
                return true;
            case 1: // VIP (доступ для VIP и Staff)
                return roleManager.IsVIP() || roleManager.IsStaff();
            case 2: // Staff (только для Staff)
                return roleManager.IsStaff();
            default:
                return false;
        }
    }
    
    public void ShowAccessDenied()
    {
        // Debug.Log(accessDeniedMessage);
    }
}