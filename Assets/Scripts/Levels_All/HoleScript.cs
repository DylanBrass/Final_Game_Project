using Unity.Netcode;
using UnityEngine;


public class HoleScript : NetworkBehaviour
{
    private NetworkVariable<int> numOfPlayersCompleted = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField]
    private string nextLevel = "Menu";

    private void Update()
    {
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < 0.25f)
        {
            collision.gameObject.GetComponent<playerNetwork>().IsLevelCompleted = true;
            numOfPlayersCompleted.Value += 1;

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            if (numOfPlayersCompleted.Value == players.Length)
            {
                NetworkManager.Singleton.SceneManager.LoadScene(nextLevel, UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
        }
    }
}
