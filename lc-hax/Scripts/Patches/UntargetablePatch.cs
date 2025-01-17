#pragma warning disable IDE1006

using GameNetcodeStuff;
using HarmonyLib;
using Hax;

[HarmonyPatch(typeof(EnemyAI))]
class UntargetableEnemyPatch {
    [HarmonyPatch(nameof(EnemyAI.PlayerIsTargetable))]
    public static bool Prefix(PlayerControllerB playerScript, ref bool __result) {
        if (!Setting.EnableUntargetable) return true;
        if (Helper.LocalPlayer?.actualClientId != playerScript.actualClientId) return true;

        __result = false;
        return false;
    }

    [HarmonyPatch(nameof(EnemyAI.Update))]
    public static bool Prefix(EnemyAI __instance) {
        if (!Setting.EnableUntargetable) return true;
        if (Helper.LocalPlayer?.actualClientId != __instance.targetPlayer?.actualClientId) return true;

        __instance.targetPlayer = null;
        __instance.movingTowardsTargetPlayer = false;
        return false;
    }
}

[HarmonyPatch(typeof(NutcrackerEnemyAI), nameof(NutcrackerEnemyAI.CheckLineOfSightForLocalPlayer))]
class UntargetableNutcrackerPatch {
    static bool Prefix() => !Setting.EnableUntargetable;
}
