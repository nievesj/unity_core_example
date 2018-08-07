
# Core Framework Template | Example

What is this?
---
This is an example of a project using the [Core Framework](https://github.com/nievesj/unity-core-project). 
Works with Unity 2018.1 and above. 

Requirements
---
* .Net 4.5.
* .Net Standards 2.0
* Incremental Compiler

Setup
---
After cloning the repo go to Assets -> MyGame and open the scene CoreGame. This will be your starting point. 

Asset Bundles
---
You may need to build the asset bundles depending on which platform you're using. Please refer to the  [AssetBundles-Browser](https://github.com/Unity-Technologies/AssetBundles-Browser)  documentation on how to do that. 

Simulating Asset Bundles
---
Asset Bundle simulation is enabled by default. If you wish to disable it go to menu Core Framework -> Disable Simulate Asset Bundles.

Asset Service Options
---
Asset Service is the service in charge of loading asset bundles. The configuration file for this service is located in MyGame -> _CoreConfig -> AssetService
* Use Streaming Assets
	* Toggling this will load the asset bundles from the streaming assets folder
* Asset Bundles URL
	* Location where the asset bundles are stored on the cloud
* Cache Asset Bundles?
	* Toggle this if you want to cache the asset bundles on device. The file [UnityCloudBuildManifest.json](https://docs.unity3d.com/Manual/UnityCloudBuildManifest.html) needs to be present in order to cache bundles. 

Asset Bundle Organization
---
The asset bundle directory organization is tied to the enum AssetCategoryRoot:

    public enum AssetCategoryRoot
	{
		None,
		Configuration,
		Services,
		Levels,
		Scenes,
		UI,
		Audio,
		Prefabs, 
		Particles
	}

![Asset Bundle Organization](http://www.josemnieves.net/unity/images/aborg.PNG)

The service will also automagically detect the platform it's running on, and use that to get the asset bundles from the web in the following order: 

![Cloud Asset Bundle Structure](http://www.josemnieves.net/unity/images/webab.png)
