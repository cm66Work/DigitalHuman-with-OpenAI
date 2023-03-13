Minimal LipSync
by Chippalrus
//=====================================
Speech based LipSync for Unity

Uses 4 blend shapes. Works with Microphone Input
and AudioClips. Can work for 3D models.

It isn't perfect or very precise,
but for cartoon style characters this works pretty well.

Only 2D Example is provided, but same method and 
setup can work for 3D models.
//=====================================
Uses four blend shapes to produce mouth shapes
Limited in precision, but functional for toon-like applications
Works for both Microphone Input and AudioClips
Can work for 2D  and 3D
Uses Unity's built-in Audio System, but could work with FMOD or LASP
//=====================================
Example Scene for Microphone Input:
On run, if it does not initially work this means microphone was not selected.
- Check the "S Mic Source Name" in the Editor
- Put the index value into the "I Mic Source Name" in the Editor
- Re-Run the scene
Also, pressing they Alpha Key "1" will Re-Initialise the Microphone
//=====================================

SETUP:
------------------------------------------------------
Setting up ANIMATOR and ANIMATION CLIPS
------------------------------------------------------
1) Select the Animatior Controller you wish to use

2) In Animator window, create a new Animation Blend Tree
	-	Right click in "Base Layer"
	-	"Create State" -> "From New Blend Tree"

3) In Animator window click "Parameters" tab
( next to "Layers" in the Animator window )
	-	Create four "Floats" set to 0 and name them
	
	Closed
	Opened
	Kissed
	Pressed

4) Double click/Open "Blend Tree" that you had created in step 2
	-	Click on "Blend Tree" node to display it in Inepector
	
5) In the Inspector window for the "Blend Tree"
	-	Set "Blend Type" to "Direct" 
	
	-	Create four Motions, by clicking the plus(+) sign
		and "Add Motion Field"
		
	-	Change the "Parameter" of each Motion to one of:
		
		Closed
		Opened
		Kissed
		Pressed
		
	-	Fill each of the "None (Motion)" with the Animation Clip
		that matches each of the "Parameter"
------------------------------------------------------
CMouthAnalyzer
------------------------------------------------------
1)  Drag CMouthAnalyzer to a GameObject
2)	Drag the Animator you wish to use to "Mouth Anim" in CMouthAnalyzer

Editor settings / variables 

	-	"Mouth Anim" is the Animator you want to use for the blend shapes
	
	-	"Sample Size" is the size of the Spectrum Data
		Low = 256, Medium = 512, High = 1024, Highest = 2048
		( Used for Initialisation, does not change on Runtime )
		
	-	"Window Type" is Apodization/Tapering function, Math stuff.
		Blackman and Blackman-Harris produced better results.
		( Used for Initialisation, does not change on Runtime )
	
	ADJUSTMENTS FOR BLEND RESULTS ( You can adjust this during Runtime in Editor )
	-	"Smoothing" the amount of smoothing between new data and old
		( this makes the shapes less "jaggy/snappy" when blending )

	-	"Sensitivity Threshold" is the speech senitivity
	
	-	"Vocal Tract Length Factor" adjustment for voice pitch
		Higher = female, Lower = Male
------------------------------------------------------
CMirophone
------------------------------------------------------
1)  Drag CMirophone to the same GameObject as CMouthAnalyzer

2)  Fill the "Audio Mixer Group" with
	an AudioMixer Group or SubGroup

3)	Set the Input Device by index value in "Mic Source Index"
	"Mic Source Name" can display a list during runtime in editor

Editor settings / variables
	-	"Audio Mixer Group" is the AudioMixer that the AudioSource will play though
		( Used for Initialisation )

	-	"Mic Source Index" selects your Audio Input Device
		( Used for Initialisation )
	
	-	"Mic Source Name" will display a full list
		of all your Audio Input Devices in order of Index.
		( Displays during Runtime, just useful for debugging )
		
	-	"Selected Mic Source" will display the full name
		of what "Mic Source Index" was set to.
		( Displays during Runtime, just useful for debugging )
		
	-	"Clip Length In Seconds" is how long the AudioClip of Input Device.
	
	-	"Audio Frequency" is the frequency of AudioClip for the Input Device
		( Used for Initialisation )
	
	-	"Playback Delay" determines the latency of
		when AudioClip of Input Device is played
		( Used for Initialisation )
		
EXTRA NOTES:
	-	It is possible to change the Input Device on runtime
	by calling ChangeInputSource(). It should call Microphone.End()
	and destroy the created AudioSource, then recreate the process again.
	
Check MicrophoneInput_Example.scene or the Prefabs for details and setup if there are any issues
------------------------------------------------------
CAudioProcessing
------------------------------------------------------
1)  Drag CAudioProcessing to the same GameObject as CMouthAnalyzer

2)  Fill the "Audio Mixer Group" with
	an AudioMixer Group or SubGroup

3)	Set "Audio Clip" array to the number of files you wish to use
4)	Drag the files and fill the "Audio Clip" array

4)	"Audio Clip Index" corrisponds to
	the "Element #" in the "Audio Clip" array
	( you can change this during Runtime in Editor to play different AudioClips )

Editor settings / variables

	-	"Audio Mixer Group" is the AudioMixer that the AudioSource will play though
		( Used for Initialisation )
	
	-	"Audio Loop" sets AudioSource to loop a single clip or not.
		( Used for Initialisation )
	
	-	"Audio Clip" an array of AudioClips ( sound files )
	
	-	"Audio Clip Index" this corrisponds to element in "Audio Clip"
		( Can be changed during Runtime in Editor )
	
	-	"Playback Delay" determines the latency of
		when AudioClip of Input Device is played
		( Used for Initialisation )
		
EXTRA NOTES:
	-	It is possible to play different clips by calling
		PlayAudioClip( int iIndex, float fDelay )
		in your own code. You may have to comment out:
		if( !m_AudioSource.isPlaying ){ PlayAudioClip( ... ); }
Check AudioFile_Example.scene or the Prefabs for details and setup if there are any issues
//=====================================
Technical Paper: ( MIT License outlined in License.md )
"Web-based live speech-driven lip-sync" by Gerard Llorach
https://repositori.upf.edu/bitstream/handle/10230/28139/llorach_VSG16_web.pdf