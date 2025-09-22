using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class AccessControlledTeleport : UdonSharpBehaviour
{
    [Header("Настройки доступа")]
    [Tooltip("0 = Все, 1 = VIP, 2 = Staff")]
    public int requiredAccess = 0; // 0=Everyone, 1=VIP, 2=Staff
    
    [Header("Ссылка на RoleManager")]
    public RoleManager roleManager;
    
    [Header("Точка телепортации")]
    public Transform teleportTarget;
    
    [Header("Сообщения")]
    public string accessDeniedMessage = "Доступ запрещен";
    public string teleportSuccessMessage = "Телепортация выполнена";
    
    public override void Interact()
    {
        bool hasAccess = CheckAccess();
        
        if (hasAccess)
        {
            if (teleportTarget != null)
            {
                // Телепортируем локального игрока
                VRCPlayerApi localPlayer = Networking.LocalPlayer;
                if (localPlayer != null)
                {
                    localPlayer.TeleportTo(
                        teleportTarget.position,
                        teleportTarget.rotation
                    );
                    // Закомментировано для продакшена:
                    // Debug.Log($"[Teleport] {localPlayer.displayName} телепортирован к {teleportTarget.name}");
                }
            }
            else
            {
                // Закомментировано для продакшена:
                // Debug.LogWarning($"[Teleport] Точка телепортации не назначена для {gameObject.name}");
            }
        }
        else
        {
            // Закомментировано для продакшена:
            // Debug.Log($"[Teleport] {Networking.LocalPlayer.displayName} - доступ запрещен");
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "ShowAccessDenied");
        }
    }
    
    private bool CheckAccess()
    {
        switch (requiredAccess)
        {
            case 0: // Everyone
                return true;
            case 1: // VIP
                return roleManager != null && roleManager.IsVIP();
            case 2: // Staff
                return roleManager != null && roleManager.IsStaff();
            default:
                return false;
        }
    }
    
    public void ShowAccessDenied()
    {
        // Закомментировано для продакшена:
        // Debug.Log(accessDeniedMessage);
    }
}