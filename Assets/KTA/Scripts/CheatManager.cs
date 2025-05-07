using Unity.Netcode;
using UnityEngine;


namespace KTA.Test
{
     public class CheatManager : NetworkBehaviour
     {
         private static CheatManager _instance;
         public static CheatManager Instance => _instance;

         
         [SerializeField] private RectTransform[] buttons;
         
         public override void OnNetworkSpawn()
         {
             base.OnNetworkSpawn();
         }
     
         public override void OnNetworkDespawn()
         {
             base.OnNetworkDespawn();
         }
     
         private void Awake()
         {
             // 싱글톤 패턴 구현
             if (_instance == null)
             {
                 _instance = this;
             }
             else
             {
                 Destroy(gameObject);
                 return;
             }
         }
         
         private void Update()
         {
             if (Input.GetKeyDown(KeyCode.Escape))
             {
                 ButtonOff();
             }
             else if (Input.GetKeyDown(KeyCode.F5))
             {
                 ButtonOn();
             }
             
             if (!IsHost) return;
     
             if (Input.GetKeyDown(KeyCode.F1))
             {
                 MyBossMinusHealth();
             }
             else if (Input.GetKeyDown(KeyCode.F2))
             {
                 OpponentBossMinusHealth();
             }
             else if (Input.GetKeyDown(KeyCode.F3))
             {
                 MyCPPlus();
             }
             else if (Input.GetKeyDown(KeyCode.F4))
             {
                 OpponentCPPlus();
             }
         }

         public void MyBossMinusHealth()
         {
             if (!IsHost) return;
             
             ulong cliendId = NetworkManager.Singleton.LocalClientId;
             
             TESTGamePlayManager.Instance.connectedBosses[cliendId].BossStats.TakeDamage(5f);
         }
     
         private void OpponentBossMinusHealth()
         {
             if (!IsHost) return;
             
             ulong cliendId = NetworkManager.Singleton.LocalClientId;
             ulong opponentId = TESTGamePlayManager.Instance.connectedOpponents[cliendId];
             
             TESTGamePlayManager.Instance.connectedBosses[opponentId].BossStats.TakeDamage(5f);
         }
     
         private void MyCPPlus()
         {
             if (!IsHost) return;
     
             ulong cliendId = NetworkManager.Singleton.LocalClientId;

             TESTCPController cpcont = TESTGamePlayManager.Instance.connectedPlayersId[cliendId].GetComponent<TESTCPController>();
             cpcont.CP.Value += 100;
         }
     
         private void OpponentCPPlus()
         {
             if (!IsHost) return;
     
             ulong cliendId = NetworkManager.Singleton.LocalClientId;
             ulong opponentId = TESTGamePlayManager.Instance.connectedOpponents[cliendId];
             
             TESTCPController cpcont = TESTGamePlayManager.Instance.connectedPlayersId[opponentId].GetComponent<TESTCPController>();
             cpcont.CP.Value += 100;
             
         }

         private void ButtonOn()
         {
             foreach (RectTransform button in buttons)
             {
                 button.gameObject.SetActive(true);
             }
         }
         
         private void ButtonOff()
         {
             foreach (var button in buttons)
             {
                 button.gameObject.SetActive(false);
             }
         }
     }
}