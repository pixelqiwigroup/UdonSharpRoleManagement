using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Data; // для DataToken
using VRC.SDK3.StringLoading; // для VRCStringDownloader

public class RoleManager : UdonSharpBehaviour
{
    [Header("Использовать JSON по ссылке")]
    public bool useRemoteJson = false;
    public VRCUrl jsonUrl;

    [Header("Локальные списки ролей (fallback)")]
    public string[] vipListManual;
    public string[] staffListManual;

    private string[] vipList;
    private string[] staffList;

    [HideInInspector] public bool isVip;
    [HideInInspector] public bool isStaff;

    private VRCPlayerApi localPlayer;
    private string localName;

    void Start()
    {
        localPlayer = Networking.LocalPlayer;
        if (localPlayer == null) return;
        localName = localPlayer.displayName;

        if (useRemoteJson && jsonUrl != null && !string.IsNullOrEmpty(jsonUrl.Get()))
        {
            // Используем подход из примера
            VRCStringDownloader.LoadUrl(jsonUrl, (UdonBehaviour)GetComponent(typeof(UdonBehaviour)));
        }
        else
        {
            // fallback на ручной список
            vipList = vipListManual;
            staffList = staffListManual;
            CheckRoles();
        }
    }

    // колбэк после успешной загрузки JSON
    public override void OnStringLoadSuccess(IVRCStringDownload result)
    {
        string json = result.Result;
        
        DataToken root;
        if (VRCJson.TryDeserializeFromJson(json, out root) && root.TokenType == TokenType.DataDictionary)
        {
            DataDictionary dict = root.DataDictionary;

            // vipList
            if (dict.TryGetValue("vipList", out DataToken vipToken) && vipToken.TokenType == TokenType.DataList)
            {
                vipList = new string[vipToken.DataList.Count];
                for (int i = 0; i < vipToken.DataList.Count; i++)
                    vipList[i] = vipToken.DataList[i].String;
            }

            // staffList
            if (dict.TryGetValue("staffList", out DataToken staffToken) && staffToken.TokenType == TokenType.DataList)
            {
                staffList = new string[staffToken.DataList.Count];
                for (int i = 0; i < staffToken.DataList.Count; i++)
                    staffList[i] = staffToken.DataList[i].String;
            }
        }
        else
        {
            // если JSON невалиден, fallback на ручной список
            vipList = vipListManual;
            staffList = staffListManual;
        }

        CheckRoles();
    }

    public override void OnStringLoadError(IVRCStringDownload result)
    {
        Debug.LogError($"[RoleManager] Ошибка загрузки JSON: {result.Error}");
        vipList = vipListManual;
        staffList = staffListManual;
        CheckRoles();
    }

    private void CheckRoles()
    {
        isVip = IsInList(localName, vipList);
        isStaff = IsInList(localName, staffList);

        // владелец инстанса = staff
        /* if (localPlayer.isInstanceOwner) isStaff = true; */

        Debug.Log($"[RoleManager] {localName} → VIP: {isVip}, STAFF: {isStaff}");
    }

    private bool IsInList(string name, string[] list)
    {
        if (list == null) return false;
        foreach (string n in list)
            if (n == name) return true;
        return false;
    }

    // для использования из других скриптов
    public bool IsVIP() => isVip;
    public bool IsStaff() => isStaff;
}