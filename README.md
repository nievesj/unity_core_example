# _Core Example | Template

What is this?
---
This is an example of a project using the [_Core framework](https://github.com/nievesj/unity-core-project). It has been tested in iOS, Android, WebGL, Windows and Mac.

WebGL test:  http://www.josemnieves.com/unity/core_example_webgl_test/

Setup
---
This project contains the _Core library as a git submodule, after cloning, the submodule needs to be pulled into your project too. 

Once cloned, go to Assets -> MyGame and open the scene CoreGame.

Asset Bundles
---
You may need to build the asset bundles depending on which platform you're using. Please refer to the  [AssetBundles-Browser](https://github.com/Unity-Technologies/AssetBundles-Browser)  documentation on how to do that. 

Simulating Asset Bundles
---
With asset bundle simulation the game can be played on editor without needing to build asset bundles. To activate just go to Unity Preferences, Core Framework and toggle "Simulation Mode".

![Core Framework Preferences](http://www.josemnieves.com/unity/images/preferences.png)

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

Asset Bundle Organization
---
The asset bundle directory organization is tied to the enum AssetCategoryRoot:

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

![Asset Bundle Organization](http://www.josemnieves.com/unity/images/aborg.png)

The service will also automagically detect the platform it's running on, and use that to get the asset bundles from the web in the following order: 

![Cloud Asset Bundle Structure](http://www.josemnieves.com/unity/images/webab.png)
