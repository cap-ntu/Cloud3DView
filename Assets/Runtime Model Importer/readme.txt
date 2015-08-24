Requirements 
・Unity Pro 4.3.1 or higher for Windows


Target Platform Builds
・Windows Standalone


Release Notes
Please refer to the "release notes.txt".


Known Issues
・Animation curve glitches in collada format.


Setup
1. Import the Package.
2. Be sure there is the Plugins folder to the root of the Assets folder as below.

	Assets
		|-Plugins
		|-Runtime Model Importer
			|-RMI
				|-Prefabs
				|-Resources
				|-Sample
				|-Scripts


Tutorial 1 -To run the sample scene-
1. Move the "RMI/Sample/humanoid.fbx" to any folder. (ex. "C:/Model/humanoid.fbx")
2. Open the RMI/Sample/Sample scene.
3. Run the scene. And push "Load" button and select the file.


Tutorial 2 -Reuse in your project-
1. Copy the RMIManagerSample and rename. (ex. "RMIManager")
2. Attach the RMIManager.cs on an empty GameObject.
3. Attach the RMITask.cs on an empty Prefab.
4. Select the prefab and edit the import settings as necessary.
5. Assign the prefab in the "Task Prefab" foldout on RMIManager.


More Information
Please refer to the "documentation.pdf".


Contact and Support
Publisher:ちんばぶ (Tinbabu)
Email:chinbabusan@gmail.com
HP:<a href="http://tinbabusan.jimdo.com">http://tinbabusan.jimdo.com</a>
