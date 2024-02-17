using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace CleaningCompany.Patches
{
    [HarmonyPatch(typeof(EnemyAI))]
    internal class EnemyAIPatcher
    {
        private static ulong currentEnemy = 9999999;
        [HarmonyPostfix]
        [HarmonyPatch("KillEnemyServerRpc")]
        static void SpawnScrapBody(EnemyAI __instance)
        {
            if (currentEnemy == __instance.NetworkObject.NetworkObjectId) return;
            if (!__instance.IsHost) return;
            currentEnemy = __instance.NetworkObject.NetworkObjectId;
            string name = __instance.enemyType.enemyName;
            if (Plugin.instance.BodySpawns.ContainsKey(name))
            {   
                // spawn body
                GameObject go = GameObject.Instantiate(Plugin.instance.BodySpawns[name].spawnPrefab, __instance.transform.position + Vector3.up, Quaternion.identity);
                go.GetComponent<NetworkObject>().Spawn();

                // nutcracker shotgun drop fix
                if (__instance is NutcrackerEnemyAI)
                {
                    NutcrackerEnemyAI? nut = __instance as NutcrackerEnemyAI;
                    if (nut == null) return;

                    // spawn shotgun shells
                    for (int i = 0; i < 2; i++)
                    {
                        Vector3 vector = nut.transform.position + Vector3.up * 0.6f;
                        vector += new Vector3(Random.Range(-0.8f, 0.8f), 0f, Random.Range(-0.8f, 0.8f));
                        GameObject shells = Object.Instantiate<GameObject>(nut.shotgunShellPrefab, vector, Quaternion.identity, RoundManager.Instance.spawnedScrapContainer);
                        shells.GetComponent<GrabbableObject>().fallTime = 0f;
                        shells.GetComponent<NetworkObject>().Spawn(false);
                    }

                    // spawn new gun
                    GameObject gun = Object.Instantiate<GameObject>(nut.gunPrefab, nut.transform.position, Quaternion.identity, RoundManager.Instance.spawnedScrapContainer);
                    gun.GetComponent<NetworkObject>().Spawn();

                    // remove original shotgun
                    nut.gun.GetComponent<NetworkObject>().Despawn(true);
                }

                // despawn original body
                __instance.GetComponent<NetworkObject>().Despawn(true);
            }
        }
    }
}
