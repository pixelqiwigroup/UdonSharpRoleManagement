using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

// Выносим enum наружу класса
public enum AccessLevel { Everyone, VIP, Staff }

public class AccessControlledButton : UdonSharpBehaviour
{
    [Header("Настройки доступа")]
    public AccessLevel requiredAccess = AccessLevel.Everyone;
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
                Debug.Log($"[Access] {Networking.LocalPlayer.displayName} переключил {targetObject.name}");
            }
        }
        else
        {
            Debug.Log($"[Access] {Networking.LocalPlayer.displayName} - доступ запрещен");
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "ShowAccessDenied");
        }
    }
    
    private bool CheckAccess()
    {
        if (requiredAccess == AccessLevel.Everyone) return true;
        if (roleManager == null) return false;
        
        if (requiredAccess == AccessLevel.VIP) return roleManager.IsVIP();
        if (requiredAccess == AccessLevel.Staff) return roleManager.IsStaff();
        
        return false;
    }
    
    public void ShowAccessDenied()
    {
        Debug.Log(accessDeniedMessage);
    }
}