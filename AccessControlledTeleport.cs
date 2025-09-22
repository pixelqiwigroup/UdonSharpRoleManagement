using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class AccessControlledTeleport : UdonSharpBehaviour
{
    [Header("Настройки доступа")]
    [Tooltip("0 = Все, 1 = VIP, 2 = Staff")]
    public int requiredAccess = 0;
    
    [Header("Ссылка на RoleManager")]
    public RoleManager roleManager;
    
    [Header("Точка телепортации")]
    public Transform teleportTarget;
    
    [Header("Сообщения")]
    public string accessDeniedMessage = "Доступ запрещен";
    
    public override void Interact()
    {
        bool hasAccess = CheckAccess();
        
        if (hasAccess)
        {
            if (teleportTarget != null)
            {
                VRCPlayerApi localPlayer = Networking.LocalPlayer;
                if (localPlayer != null)
                {
                    localPlayer.TeleportTo(
                        teleportTarget.position,
                        teleportTarget.rotation
                    );
                }
            }
        }
        else
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "ShowAccessDenied");
        }
    }
    
    private bool CheckAccess()
    {
        if (roleManager == null) 
            return requiredAccess == 0;
        
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