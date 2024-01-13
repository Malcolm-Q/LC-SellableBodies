using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace CleaningCompany.Monos
{
    internal class BodySyncer : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            if(IsHost || IsServer) 
            {
                StartCoroutine(WaitToSync());
            }
            base.OnNetworkSpawn();
        }

        private IEnumerator WaitToSync()
        {
            yield return new WaitForSeconds(1f);
            PhysicsProp prop = GetComponent<PhysicsProp>();
            int price = Random.Range(prop.itemProperties.minValue, prop.itemProperties.maxValue);
            SyncDetailsClientRpc(price, new NetworkBehaviourReference(prop));
        }

        [ClientRpc]
        void SyncDetailsClientRpc(int price, NetworkBehaviourReference netRef)
        {
            netRef.TryGet(out PhysicsProp prop);
            if (prop != null)
            {
                prop.scrapValue = price;
                prop.itemProperties.creditsWorth = price;
                prop.GetComponentInChildren<ScanNodeProperties>().subText = $"Value: ${price}";
                Debug.Log("Successfully synced body values");
            }
            else Debug.LogError("Failed to resolve network reference!");
        }
    }
}
