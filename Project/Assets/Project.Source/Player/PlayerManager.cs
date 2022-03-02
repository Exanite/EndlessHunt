using System.Collections.Generic;
using Project.Source;
using UnityEngine;

public class PlayerManager : SingletonBehaviour<PlayerManager>
{
    public List<Player> players = new List<Player>();

    public Player GetClosestPlayer(Vector3 position)
    {
        if (players.Count == 0)
        {
            return null;
        }
        
        var closestPlayer = players[0];
        var closestDistance = Vector3.Distance(closestPlayer.transform.position, position);

        foreach (var player in players)
        {
            var distance = Vector3.Distance(player.transform.position, position);
            
            if (distance < closestDistance)
            {
                closestPlayer = player;
                closestDistance = distance;
            }
        }

        return closestPlayer;
    }
}