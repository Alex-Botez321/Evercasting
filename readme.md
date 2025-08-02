### Blank Unity Project ### 

This repository has been pre-configured with a .gitignore and .gitattributes file specific to Unity projects using git-lfs. 

This project was created for Unity [2023.2.20](https://unity.com/releases/editor/whats-new/2023.2.20#installs)

The project has been created with a barebones folder structure.  The settings have been optimized for best 3D quality without adding any additional packages to the project.
For 2D projects, go to Edit -> Project Settings -> Editor and set the "Default Behaviour Mode" to 2D.  Do this at the start so that imported assets are optimized automatically.

To use this repo select it from the tremplate list when making a new repo.

Note:  Do not leave empty folders in your Unity project.  Empty folders do not get added to version control, but the meta files they create do.  This can lead to issues with keeping your project up to date.

Please make sure you place all of your Terrain in a folder called "Terrain" or "Terrains". Failing to do so will result in corupted projects.

Be aware that we have changed the settings to allow sub folders, this means you might accidently push as Unity project inside your Unity project so make sure you know what folder you are working from.
 
Do you have any suggestions for improvements? Please submit a pull request!
