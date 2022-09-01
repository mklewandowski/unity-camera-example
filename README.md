# unity-camera-example
Example project showing integration of device camera in Unity. This project does the following:
- on every Update, a texture from the device camera is shown onscreen in a RawImage UI object.
- line up a face in the pink framing box and press the Capture Image button to save an image texture of the content inside the framing box.
- press the Show Costume button to display the captured face inside a space suit.
- press the Show Camera button to return to the camera view and capture a new image.

## Running Locally
Use the following steps to run locally:
1. Clone this repo
2. Open repo folder using Unity 2021.3.3f.1
3. Install Text Mesh Pro

## Platform Support
This repo has been tested for use on the following platforms:
- Android

At present this has only been tested using 720 x 1600 resolution with portrait orientation. More testing is needed to confirm it works on other device sizes.

## Development Tools
- Created using Unity 2021.3.3f.1
- Code edited using Visual Studio Code.

## Credits
Camera in Unity code based on this tutorial:
https://www.youtube.com/watch?v=c6NXkZWXHnc

## Useful Links
Unity WebCamTexture Overview:
https://docs.unity3d.com/ScriptReference/WebCamTexture.html