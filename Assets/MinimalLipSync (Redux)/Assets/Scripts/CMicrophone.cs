using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
//====================================================================================================
//          CMicrophone
//====================================================================================================
public class CMicrophone : MonoBehaviour
{
//================================================================================================
//  MEMBERS
//================================================================================================
    public      enum                                EAudioFrequency { _44100Hz = 44100, _48000Hz = 48000, _96000Hz = 96000 }
[SerializeField]
    private     AudioMixerGroup                     m_AudioMixerGroup;                      // Project AudioMixerGroup to be assigned to newly created AudioSource
    private     bool                                m_bAudioLoop            =   true;       // Set newly created AudioSource to Loop ( this is required to be "true" )
[SerializeField]
    private     int                                 m_iMicSourceIndex       =   0;          // Selected Input device Index for initialisation at runtime
[SerializeField]
    private     string[]                            m_sMicSourceName;                       // List of Input Device names
[SerializeField]
    private     string                              m_sSelectedMicSource;                   // Selected Microphone Input Device will be displayed here on runtime
[SerializeField, Range( 1, 3599 )]
    private     int                                 m_iClipLengthInSeconds  =   1;          // Length of input clip ( technically don't need this, but Unity requires it )
[SerializeField]
    private     EAudioFrequency                     m_EAudioFrequency       =   EAudioFrequency._44100Hz;   // Selected Audio Frequency
[SerializeField, Range( -1.0f, 2.0f )]
    private     float                               m_fPlaybackDelay            =   0.1f;       // Playback Delay
[SerializeField]
    private     bool                                m_InitAsMute            =   false;      // Mute newly created AudioSource
    // Audio Frequency set by m_EAudioFrequency
    private     int                                 m_iFrequency            =   44100;
    // Microphone Input AudioClip for Playback
    private     AudioClip                           m_MicInputClip;
    // AudioSource of created GameObject
    private     AudioSource                         m_AudioSource;
    // Lipsync Analyzer, so we can feed Spectrum data to it
    private     CMouthAnalyzer                      m_CMouthAnalyzer        =   null;
//================================================================================================
//  Init
//================================================================================================
    private void Awake()
    {
        // List Input devices in Editor
        LookUpMicrophoneDevices();
        // Initialise Input device by Index ( m_iMicSourceIndex )
        InitialiseMicrophoneInput();
    }
    public void InitialiseMicrophoneInput() // First Init for Microphone Input and AudioSource ( This should be called first before using )
    {
        m_iFrequency = ( int )m_EAudioFrequency;
        // Creates Microphone Input of index ( m_iMicSourceIndex ) assigned in Editor and creates a new AudioSource
        if( m_AudioSource == null ){ InitialiseNativeAudio( Microphone.devices[ m_iMicSourceIndex ], m_bAudioLoop, m_iClipLengthInSeconds, m_iFrequency, m_fPlaybackDelay ); }

        // Assigns CMouthAnalyzer so we can pass the Spectrum Data over.
        if( m_CMouthAnalyzer == null ){ m_CMouthAnalyzer = gameObject.GetComponent< CMouthAnalyzer >(); }
    }
//================================================================================================
//  MUTATORS / ACCESSORS
//================================================================================================
    // Get
    public  bool    GetLooping()            {   return m_bAudioLoop;  }
    public  int     GetLength()             {   return m_iClipLengthInSeconds; }
    public  int     GetFrequency()          {   return m_iFrequency; }
    public  float   GetPlayDelay()          {   return m_fPlaybackDelay; }
    public  string  GetCurrentMicInput()    {   return m_sSelectedMicSource;  }
    // Set
    public  void    ToggleSound( bool bMute ){   if( m_AudioSource != null ){ m_AudioSource.mute = bMute; } }
    public  void    SetMixerGroupAttun( float fVal ) { m_AudioMixerGroup.audioMixer.SetFloat( m_AudioMixerGroup.name, fVal ); }
    public  void    SetMicrophoneVolume( float fVal ) { if( m_AudioSource != null ){ m_AudioSource.volume = fVal; } }
//================================================================================================
//  METHODS
//================================================================================================
    public  void    LookUpMicrophoneDevices()                                   // This is for Debugging, finding out the Input Device name and Index
    {   // Generate List of Input Devices ( Only shows on runtime in Editor )
        m_sMicSourceName = new string[ Microphone.devices.Length ];
        for( int i = 0; i < Microphone.devices.Length; ++i )
        {
            m_sMicSourceName[ i ] = Microphone.devices[ i ];
        }
    }
    public  AudioSource    CreateNewNativeSource( bool bLoop )      // Creates a new Unity AudioSource as a new GameObject as a child of this script
    {
        m_bAudioLoop                        =   bLoop;
        // Create new GameObject
        GameObject      mGO                 =   new GameObject();
        // Assign as child of THIS SCRIPT's GameObject
        mGO.transform.parent                =   gameObject.transform;
        // Add Unity's AudioSource component
        mGO.AddComponent< AudioSource >();
        // Get the AudioSource component
        AudioSource     mAS                 =   mGO.GetComponent< AudioSource >();
        // Assign AudioMixer Group to the AudioSource
        mAS.outputAudioMixerGroup           =   m_AudioMixerGroup;
        // Set loop parameter
        mAS.loop                            =   m_bAudioLoop;
        // Set to unmute
        mAS.mute                            =   false;
        // Return the AudioSource
        return mAS;
    }
    private IEnumerator    WaitForMicrophone()  //   Required to get low-latency playback as much as possible.
    {
        float mTimer = 0;
        while( !( Microphone.GetPosition( null ) > 0 ) && mTimer < 1000 )
        {
            mTimer += Time.deltaTime;
        }
        yield return null;
    }
    public  void    InitialiseNativeAudioPlayback( AudioSource mAS, string sInputSource, int iLengthSec, int iFrequency, float fDelay )     // Initisalise Unity's Microphone Input and set it for Playback
    {
        // Update variables to current Input Device before Initialisation
        if( m_sSelectedMicSource != sInputSource ){ m_sSelectedMicSource = sInputSource; }
        if( m_iClipLengthInSeconds != iLengthSec ){ m_iClipLengthInSeconds = iLengthSec; }
        if( m_iFrequency != iFrequency ){ m_iFrequency = iFrequency; }

        // Start Unity's Microphone Input
        m_MicInputClip                =   Microphone.Start( sInputSource, m_bAudioLoop, iLengthSec, iFrequency );
        // Get the Microphone AudioClip
        mAS.clip                      =   m_MicInputClip;
        // Don't pause Listener
        mAS.ignoreListenerPause       =   true;
        // Wait for Microphoe Position
        StartCoroutine( "WaitForMicrophone" );
        // Play Microphone clip
        //mAS.PlayScheduled( AudioSettings.dspTime + fDelay );
        mAS.Play();
    }
    private void    InitialiseNativeAudio( string sInputSource, bool bLoop, int iLengthSec, int iFrequency, float fDelay )      // Initialises new Unity AudioSource and Microphone Input
    {   // If AudioSource is already assigned destroy it.
        if( m_AudioSource != null ){ Destroy( m_AudioSource.gameObject ); }
        // Create a new AudioSource object
        m_AudioSource = CreateNewNativeSource( bLoop );
        // Start new Microphone Input and assign clip to the new AudioSource for playback
        InitialiseNativeAudioPlayback( m_AudioSource, sInputSource, iLengthSec, iFrequency, fDelay );
        // Initialise AudioSource muted or not.
        m_AudioSource.mute = m_InitAsMute;
    }
    public  void    ChangeInputSource( string sInputSource, bool bLoop, int iLengthSec, int iFrequency, float fDelay )          // For changing Input Device
    {
        // Stop the current Microphone Input Device
        Microphone.End( m_sSelectedMicSource );
        // If AudioSource is already assigned destroy it.
        if( m_AudioSource != null ){ Destroy( m_AudioSource.gameObject ); }
        // Create a new AudioSource object
        m_AudioSource = CreateNewNativeSource( bLoop );
        // Start new Microphone Input Device and assign clip to the new AudioSource for playback
        InitialiseNativeAudioPlayback( m_AudioSource, sInputSource, iLengthSec, iFrequency, fDelay );
        // Initialise AudioSource muted or not.
        m_AudioSource.mute = m_InitAsMute;
    }
    public  void    DestroyInputSource()
    {
        // Stop the current Microphone Input Device
        Microphone.End( m_sSelectedMicSource );
        // If AudioSource is already assigned destroy it.
        if( m_AudioSource != null ){ Destroy( m_AudioSource.gameObject ); }
    }
    public  void    Reinitialise()
    {
        ChangeInputSource( Microphone.devices[ m_iMicSourceIndex ], m_bAudioLoop, m_iClipLengthInSeconds, m_iFrequency, m_fPlaybackDelay );
    }
    //================================================================================================
    //  UPDATE
    //================================================================================================
    private void Update()
    {
        if( Input.GetKeyUp( KeyCode.Alpha1 ) )  // Pressing Key 1 will re-initialise microphone
        {
            Reinitialise();
        }
    }
    void FixedUpdate()
    {
        if( m_CMouthAnalyzer != null )
        {
            if( m_AudioSource != null )
            {
                // Get m_SamplesRaw array from CMouthAnalyzer ( Sets the array to the initialised size of m_SamplesRaw )
                float[]     fSamples    = m_CMouthAnalyzer.GetSamplesRaw();

                // Get FFT Window Type from CMouthAnalyzer
                FFTWindow   fftWindow   = m_CMouthAnalyzer.GetFFTWindowType();

                // Get the Spectrum( Sample Data ) from AudioSource and store it in fSamples
                m_AudioSource.GetSpectrumData( fSamples, 0, fftWindow );

                // Set CMouthAnalyzer.m_SamplesRaw with Spectrum data
                m_CMouthAnalyzer.SetSamplesRaw( fSamples );

                // Analyze the samples stored in CMouthAnalyzer.m_SamplesRaw
                m_CMouthAnalyzer.AnalyzeSpectrum( m_CMouthAnalyzer.GetSamplesRaw() );
            } else {  Debug.Log( "AudioSource does not exist" );  }
        } else {  Debug.Log( "CMouthAnalyzer does not exist" );  }
    }
}
