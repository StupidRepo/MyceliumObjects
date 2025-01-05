using MyceliumNetworking;
using UnityEngine;

namespace MyceliumObjects.Components;

public class Instantiator : MonoBehaviour
{
	public static Instantiator Instance { get; private set; }
	
	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}
		
		Instance = this;
		MyceliumNetwork.RegisterNetworkObject(this, MyceliumObjects.ModId);
	}

	/// <summary>
	/// Creates a networked object on all clients including the host.
	/// </summary>
	/// <param name="networkedName">The network name of the prefab</param>
	/// <param name="modId">The mod ID that wants to instantiate the prefab</param>
	/// <param name="position">The position to instantiate it at.</param>
	/// <param name="rotation">The rotation to instantiate it with.</param>
	public void InstantiateObject(
		string networkedName, uint modId,
		Vector3 position, Quaternion? rotation = null
	) {
		if (!MyceliumNetwork.IsHost)
		{
			Logger.LogError("Only the host can instantiate objects.");
		}
		
		var networkId = MyceliumObjects.MakeNetworkID();
		MyceliumNetwork.RPC(MyceliumObjects.ModId, nameof(RPC_InstantiateObject), ReliableType.Reliable, [
			networkedName,
			position, rotation ?? Quaternion.identity,
			networkId, modId
		]);
	}
	
	[CustomRPC]
	private void RPC_InstantiateObject(string networkedName, Vector3 position, Quaternion rotation, int networkId, uint modId)
	{
		if (!MyceliumObjects.RegisteredObjects.TryGetValue(networkedName, out var prefab))
		{
			Logger.LogError($"Tried to instantiate a prefab with the name of '{networkedName}', but it was not registered by a mod.");
			return;
		}
		
		var go = Instantiate(prefab, position, rotation);
		var mView = go.GetComponent<MyceliumView>();
		mView ??= go.AddComponent<MyceliumView>();
		
		mView.NetworkId = networkId;
		mView.ModId = modId;
		MyceliumObjects.AddNetworkID(networkId, gameObject);
	}
}