# MyceliumObjects
Allows mod developers to spawn objects over the network with Mycelium, and still have IDs that are consistent across clients.

# How to use (as a mod dev)
1. Delete every reference to `PhotonNetwork.Instantiate` in your mod. You use `Instantiator.Instance.InstantiateObject` now.
	- View how to use it [here](https://github.com/StupidRepo/MyceliumObjects/blob/main/Components/Instantiator.cs#L29).
2. Make sure you register your mod with Mycelium when it starts.
	- `MyceliumNetwork.RegisterNetworkObject(this, YourPlugin.ModID);`
3. Make sure to register any GameObjects you will be spawning after this, and give it a network name.
	- `MyceliumObjects.MyceliumObjects.RegisterGameObject("MySuperCoolName", gameObjectForSuperCoolObject);`
4. Use `Instantiator.Instance.InstantiateObject` to spawn objects.
	- `Instantiator.Instance.InstantiateObject("MySuperCoolName", YourPlugin.ModID, locationToSpawn, rotationIsOptionalToSpecify);`
## Network IDs
Network IDs are basically Photon View IDs. A `MyceliumView` is added to your component when it is made, if it doesn't already exist.
You can get the network/mod ID of the component by doing:
```cs
MyceliumView view = GetComponent<MyceliumView>();

Debug.Log(view.NetworkID);
Debug.Log(view.ModID);
```

> [!WARNING]
> Make sure to check if the view is null before using it, as it may not exist yet. **THE NETWORK ID WILL BE SET AFTER AWAKE IS CALLED** so use Start to register the GameObject as a network object.
> See an example in my Landmines mod [here](https://github.com/StupidRepo/Landmine/blob/main/Components/Mine.cs#L47-L60)