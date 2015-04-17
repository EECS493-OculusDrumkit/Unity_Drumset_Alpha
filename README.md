## document.cookie monsters: Drum Circle

### Description
This is the final project for EECS 493 for the Winter 2015 semester. We've created a virtual reality music-making application using both the Oculus Rift and the Thalmic Myo using Unity 5 as the application engine. Using the THalmic Myo, users can use natural hand gestures to pick up and place one of six instruments in the scene on to beats that form a circle around you. Each beat represents a quarter note, and as such you can create a 4 measure loop of music. On the back end, we've set up a local Node.js server that Unity can make requests to. This server then sends a formatted osc packet to Max/MSP (a piece of software used for MIDI manipulation and digital signal processing) which produces a MIDI note depending on which instrument in the game was selected. After that, the MIDI note is routed through Logic Pro X to play the instruments through a production level drum synth.

### Setup
##### Front End
Clone this repo. To fully run the Unity portion make sure you've installed [Unity](http://unity3d.com/get-unity), the [Myo SDK](https://developer.thalmic.com/login/?next=/downloads) as well as the [Oculus SDK and Runtime](https://developer.oculus.com/downloads/) for your given patform (The application will still work without an Oculus, but not without a Myo). Run through the Myo setup guide, open up the project in Unity and you should be up and running (You may have to configure the Oculus screen orientation and mirroring for it to display properly). The scene to load is named "!Final_Scene".
##### Back End
Clone the repo at [https://github.com/EECS493-OculusDrumkit/Music-Server.git](https://github.com/EECS493-OculusDrumkit/Music-Server.git) into the same directory that you cloned the Unity repository. To hook into sound playback, you'll need to make sure you have [Node.js](https://nodejs.org/) installed on your machine. In the cloned repo, navigate to the `Music-Server/server` on the command line and run `npm install` (If this fails, try with `sudo`). Next, install the demo of [Max/MSP](https://cycling74.com/downloads/) and open up the file at `Music_Server/patches/UDPtoLogic.maxpat`. This Max patch will listen for UDP packets from the server. At this point, you can play piano notes through Max, but if you'd like to actually hear drum sounds from the application you'll need to double click the `noteout` box at the bottom of the file and click "From Max 1". Then load up [Logic Pro X](https://itunes.apple.com/us/app/logic-pro-x/id634148309?mt=12) (This software retails for $200, and there are alternatives, like a demo version of Ableton Live 9; however this will require tweaking on the server to get just right). From Logic, open up `Music_Server/logic/MaxRouting/MaxRouting`, and the sound system should be hooked up. To run the server, just run `npm start` in `Music_Server/server`.

### Screenshots
Coming soon

### Website
Coming soon

### Contribution Breakdown
[Sean Goodrich](https://github.com/smgood)
- Handling instrument state with the Myo

[Sam Oliver](https://github.com/smoliver)
- 3D modeling of instrument models

[Russell Trupiano](https://github.com/russelltrupiano)
- Music playback and class design for musical objects

[Max Yinger](https://github.com/shmacks)
- Animations for instrument changes and for crosshair state
