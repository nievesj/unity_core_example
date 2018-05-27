

# Core Framework for Unity
Current Version 1.3

Supported Unity versions: 2017.3 or latest. 

What is Core Framework?
---
The purpose of Core Framework is to help speed up development in Unity3D by providing the following basic systems every game needs to have:
* Asset Bundle loading system that can load assets from:
	* Streaming assets folder
	* Web server or cloud service
	* Simulate asset bundles on editor
	* Also provides caching alternatives
		* By using the Unity Cloud Build Manifest build number as the bundle version
		* Or by using the HASH number from the .manifest file of each bundle as the version
* UI System
	* Basic implementation of Widgets, Dialogs and Panels 
	* Transition animations by using LeanTween, configurable on inspector
	* Observable events for when a UIElement is opened, closed, shown or hidden
	* Can trigger sounds when a transition plays
* Basic audio system
* Mouse / Touch input control
* Factory tool
* Console window colors! Colorize your debug messages with colors so they are easier to read.
* Base game starting point ([Example Project](https://github.com/nievesj/unity_core_example))

Demo
---
WebGL Demo: [Core Framework Demo](http://www.josemnieves.com/core-framework-demo/) 

Purpose
---
The main aspect of this library is loading and unloading asset bundles in a relatively simple way.

    assetService.GetAndLoadAsset<Ball>(bundleRequest)
			.Subscribe(loadedBall =>
			{
				var myBall = GameObject.Instantiate<Ball>(loadedBall);
			});

How to integrate into a project?
---
This project is meant to be added to an existing Unity Project, either by downloading it and placing it in the "Plugins" folder, or by setting it as a subtree to your git repo. Alternatively you can use the [Example Project](https://github.com/nievesj/unity_core_example) as a starting point. 

Dependencies
---
Core Framework depends on the the following components
* [UniRx](https://github.com/neuecc/UniRx): Most of the Core Framework functionality is wrapped around Observables. 
* [Zenject](https://github.com/modesttree/Zenject): Core Framework libraries are loaded and used with Dependency Injectiion.
* [AssetBundles-Browser](https://github.com/Unity-Technologies/AssetBundles-Browser): Unity's tool for building and organizing asset bundles. 
* [LeanTween](https://github.com/dentedpixel/LeanTween): Used in UI transitions.

Which platforms are compatible?
---
Has been tested on iOS, Android, Mac, Windows and [WebGL](http://www.josemnieves.com/core-framework-demo/). 

Setup info
---

Asset Bundles
---

For simplicity, the current asset bundle strategy for this tool is that each prefab is its own asset bundle, and asset bundles are organized by categories or directories. Image below is Unity's [AssetBundles-Browser](https://github.com/Unity-Technologies/AssetBundles-Browser).

![Asset Bundle Organization](http://www.josemnieves.com/unity/images/aborg.png)

 These directories are mapped to the enum [AssetCategoryRoot](https://github.com/nievesj/unity-core-project/blob/master/Services/AssetService/BundleRequest.cs#L97-L107) as shown below.

    public enum AssetCategoryRoot
	{
		None,
		Configuration,
		Services,
		Levels,
		SceneContent,
		GameContent,
		Windows,
		Audio,
		Prefabs
	}


The service also detects the platform it's running on, and uses that to get the asset bundles from the web in the following order: 

![Cloud Asset Bundle Structure](http://www.josemnieves.com/unity/images/webab.png)

This functionality is entirely seamless to the developer, thus requesting an asset is now as easy as:

       assetService.GetAndLoadAsset<Ball>(bundleRequest)
			.Subscribe(loadedBall =>
			{
				var myBall = GameObject.Instantiate<Ball>(loadedBall);
			});

Simulating Asset Bundles
---
With asset bundle simulation the game can be played on editor without needing to build asset bundles. To activate just go to Unity Preferences, Core Framework and toggle "Simulation Mode".

![Core Framework Preferences](http://www.josemnieves.com/unity/images/preferences.png)

Alternatively, there's also a _Core menu to enable/disable simulation mode

![Core Menu](http://www.josemnieves.com/unity/images/coremenu.png)

Asset Service Options
---
Asset Service is the service in charge of loading asset bundles. The configuration file for this service is located in MyGame -> _CoreConfig -> AssetService
* Use Streaming Assets
	* Toggling this will load the asset bundles from the streaming assets folder
* Asset Bundles URL
	* Location where the asset bundles are stored on the cloud
* Cache Asset Bundles?
	* Toggle this if you want to cache the asset bundles on device. This will also cache the .manifest files on the Application.persistentDataPath path, and refresh them every 5 days, by default.

![Asset Service Options](http://www.josemnieves.com/unity/images/assetservice.png)

Console window colors!
---

This feature allows you to easily colorize debug messages so you can keep track of related events by colors on editor. This functionality is disabled on builds so the console log doesn't become cluttered with color tags. 

![Asset Service Options](http://www.josemnieves.com/unity/images/consolecolors.png)


       Debug.Log(("My very awesome lime colored text!").Colored(Colors.Lime));
