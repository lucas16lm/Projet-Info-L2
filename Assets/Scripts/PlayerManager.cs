using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Prefab")]
    public GameObject playerPrefab;
    [Header("Players")]
    public Player firstPlayer;
    public Player secondPlayer;

    public void InitializePlayers(FactionData firstPlayerFaction, FactionData secondPlayerFaction){
        GameObject fpGO = Instantiate(playerPrefab, transform.position, Quaternion.identity, transform);
        fpGO.name=firstPlayerFaction.factionName;
        firstPlayer = fpGO.GetComponent<Player>();
        firstPlayer.factionData=firstPlayerFaction;
        firstPlayer.playerRole=PlayerRole.FirstPlayer;
        firstPlayer.ressourceBalance=firstPlayerFaction.baseBalance;

        GameObject spGO = Instantiate(playerPrefab, transform.position, Quaternion.identity, transform);
        spGO.name=secondPlayerFaction.factionName;
        secondPlayer = spGO.GetComponent<Player>();
        secondPlayer.factionData=secondPlayerFaction;
        secondPlayer.playerRole=PlayerRole.SecondPlayer;
        secondPlayer.ressourceBalance=secondPlayerFaction.baseBalance;
    }
}
