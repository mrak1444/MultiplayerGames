using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class QuickMatchExample : IMatchmakingCallbacks
{
    private LoadBalancingClient loadBalancingClient;

    private void QuickMatch()
    {
        loadBalancingClient.OpJoinRandomOrCreateRoom(null, null);
    }

    // do not forget to register callbacks via loadBalancingClient.AddCallbackTarget
    // also deregister via loadBalancingClient.RemoveCallbackTarget

    void IMatchmakingCallbacks.OnJoinedRoom()
    {
        // joined a room successfully
        Debug.Log("joined a room successfully");
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        throw new System.NotImplementedException();
    }

    public void OnCreatedRoom()
    {
        throw new System.NotImplementedException();
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnLeftRoom()
    {
        throw new System.NotImplementedException();
    }
}