# UdonSharpRoleManagement
A collection of scripts to give you an ability to assign roles to users (VIP, Staff) and to create Toggles and Teleports limited only to these roles.

These scripts were written completely by DeepSeek and ChatGPT, and in all honesty DeepSeek did a much better job, even though I had to give it some working examples of things it had outdated knowledge about.

The intended purpose of these scripts is to aid in the creation of club/rave/concert worlds where you would only want to give certain people permissions to access the stage, for example, or the staff booth. Or just *any version of a private room only certain people should have access to*. 

# Preparations (a.k.a. how to use the RoleManager script)
1. Create an empty GameObject
2. Add the RoleManager script to it
3. Populate the VIP and Staff arrays with usernames
3.1 Optionally you can provide a direct link for a JSON file containing usernames

   Example JSON:
```json
{
"vipList": [
    "vip1",
    "vip2"
  ],
  "staffList": [
    "staff1",
    "staff2"
  ]
}
```

This *should* work but I did not test it.

# How to use the AccessControlledButton script
1. Choose/create a GameObject and enable "Is Trigger" in the collider properties
2. Add the AccessControlledButton script to it
3. Choose what Role you wish to give access to the button in the dropdown
4. In the "Role Manager" field select the GameObject that has the RoleManager script
5. In the "Target Object" field select the GameObject you would like to toggle with this button

You can change the "Access Denied message", but it only shows up in the game console.

After all that the button you made should only be accessible to the Role that you selected.

# How to use the AccessControlledTeleport script
0. Create an empty GameObject and place it where you wish to teleport players after they press the button
1. Choose/create a GameObject and enable "Is Trigger" in the collider properties
2. Add the AccessControlledTeleport script to it
3. In the "Required Access" select the Role you wish to grant permission to use this Teleport Button (0 = everyone, 1 = VIP, 2 = Staff)
4. In the "Teleport Target" select the empty GameObject you made in step 0

You can change the "Access Denied message", but it only shows up in the game console.

After all that the button you made should only be accessible to the Role that you selected.



P.S. I asked DeepSeek why the code has `SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "ShowAccessDenied");`, and it told me it's to link up things to show that access was denied (i.e. a red light, a global sound effect, etc), so that's kind of neat. If you manage to figure it out - good for you.
P.P.S. This is completely vibecoded, so if you wish to improve this code - be my guest. I can't code for SHIT.
