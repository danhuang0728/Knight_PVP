using Fusion;
using System.Collections.Generic;

public class NetworkManager : NetworkBehaviour
{
    public static NetworkManager Instance { get; private set; }
    
    [Networked, Capacity(10)]
    public NetworkDictionary<PlayerRef, NetworkObject> PlayerObjects { get; } = default;
    
    [Networked, Capacity(10)]
    public NetworkDictionary<PlayerRef, NetworkObject> HpBarObjects { get; } = default;
    
    public override void Spawned()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Runner.Despawn(Object);
        }
    }
    
    public void AddPlayer(PlayerRef player, NetworkObject playerObject, NetworkObject hpBarObject)
    {
        if (Runner.IsServer)
        {
            PlayerObjects.Set(player, playerObject);
            HpBarObjects.Set(player, hpBarObject);
        }
    }
    
    public void RemovePlayer(PlayerRef player)
    {
        if (Runner.IsServer)
        {
            PlayerObjects.Remove(player);
            HpBarObjects.Remove(player);
        }
    }
    
    public NetworkObject GetPlayerObject(PlayerRef playerRef)
    {
        if (PlayerObjects.TryGet(playerRef, out NetworkObject playerObj))
        {
            return playerObj;
        }
        return null;
    }
}