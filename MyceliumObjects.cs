using JetBrains.Annotations;
using MyceliumNetworking;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace MyceliumObjects
{
	[ContentWarningPlugin("stupidrepo.MyceliumObjects", "0.1", true)]
	public static class MyceliumObjects
	{
		internal const uint ModId = 11742219;
		
		internal static readonly Dictionary<int, GameObject> NetworkIds = [];
		internal static readonly Dictionary<string, GameObject> RegisteredObjects = new();
		
		static MyceliumObjects()
		{
			var gameObject = new GameObject("MyceliumObjects")
			{
				hideFlags = HideFlags.HideAndDontSave
			};

			Object.DontDestroyOnLoad(gameObject);
			gameObject.AddComponent<Components.Instantiator>();
			
			Logger.Log("Initialized.");
		}

		/// <summary>
		/// Registers a name to a GameObject, to allow for that GameObject to be spawned over the network.
		/// </summary>
		/// <param name="networkedName">A network name for it, e.g. MyGunObject or DrPebsi</param>
		/// <param name="gameObject">The GameObject to pair this name with</param>
		public static void RegisterGameObject(string networkedName, GameObject gameObject)
		{
			if (RegisteredObjects.TryAdd(networkedName, gameObject))
			{
				Logger.Log($"Registered a game object with the name {networkedName}.");
				return;
			}
			
			Logger.LogError($"A game object with the name {networkedName} is already registered.");
		}

		/// <summary>
		/// Makes a new network ID that is not already in use.
		/// </summary>
		/// <returns>A network ID that is ready to be used</returns>
		public static int MakeNetworkID()
		{
			int networkId;
			do
			{
				networkId = Random.Range(int.MinValue, int.MaxValue);
			} while (NetworkIds.ContainsKey(networkId));
			
			Logger.Log($"Generated network ID {networkId}.");
			return networkId;
		}

		/// <summary>
		/// Adds a network ID to the list of network IDs, with the associated game object.
		/// </summary>
		/// <param name="networkId">The network ID of the gameObject</param>
		/// <param name="gameObject">The gameObject that is wanting to be paired with this ID</param>
		internal static void AddNetworkID(int networkId, GameObject gameObject)
		{
			if (NetworkIds.TryAdd(networkId, gameObject))
			{
				Logger.Log($"Added network ID {networkId}.");
				return;
			}

			Logger.LogError($"A game object with the network ID {networkId} is already registered.");
		}
		
		public static void FreeNetworkID(int networkId)
		{
			if (NetworkIds.Remove(networkId))
			{
				Logger.Log($"Freed network ID {networkId}.");
				return;
			}
			
			Logger.LogError($"Tried to free network ID {networkId}, but it was not found.");
		}
		
		public static GameObject? GetObjectByNetworkID(int networkId)
		{
			if (NetworkIds.TryGetValue(networkId, out var gameObject))
			{
				Logger.Log($"Found game object with network ID {networkId}.");
				return gameObject;
			}
			
			Logger.LogError($"Tried to get game object with network ID {networkId}, but it was not found.");
			return null;
		}
	}
}