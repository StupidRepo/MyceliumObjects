using MyceliumNetworking;
using UnityEngine;

namespace MyceliumObjects.Components;

public class MyceliumView : MonoBehaviour
{
	public int NetworkId { get; internal set; }
	public uint ModId { get; internal set; }
	private void OnDestroy()
	{
		MyceliumObjects.FreeNetworkID(NetworkId);
	}
}