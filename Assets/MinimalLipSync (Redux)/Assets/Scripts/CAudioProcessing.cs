using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
//====================================================================================================
//          CAudioProcessing
//====================================================================================================
public class CAudioProcessing : MonoBehaviour
{
//================================================================================================
//  MEMBERS
//================================================================================================
[SerializeField]
    private     AudioMixerGroup                     m_AudioMixerGroup;                      // Project AudioMixerGroup to be assigned to newly created AudioSource
[SerializeField]
    private     bool                                m_bAudioLoop           =   true;       // Set newly created AudioSource to Loop
[SerializeField]
    private     AudioClip[]                         m_AudioClip            =   new AudioClip[ 3 ];       // The audio files to play
[SerializeField]
    private     int                                 m_iAudioClipIndex       =   0;       // AudioClip Index
[SerializeField, Range( 0f, 15.0f )]
    private     float                               m_fPlaybackDelay       =   0.1f;       // Playback Delay
    // AudioSource of created GameObject
    private     AudioSource                         m_AudioSource;
    // Lipsync Analyzer, so we can feed Spectrum data to it
    private     CMouthAnalyzer                      m_CMouthAnalyzer        =   null;
//================================================================================================
//  Init
//================================================================================================
    void Awake()
    {
        InitialiseAudio();
    }
//================================================================================================
//  MUTATORS / ACCESSORS
//================================================================================================
    // Get
    public  bool    GetLooping()            {   return m_bAudioLoop;  }
    public  int     GetAudioClipIndex()     { return m_iAudioClipIndex; }
    // Set
    public  void    SetAudioClipIndex( int iVal ) { m_iAudioClipIndex = iVal; }
    public  void    ToggleSound( bool bMute ){   if( m_AudioSource != null ){ m_AudioSource.mute = bMute; } }
    public  void    SetMixerGroupAttun( float fVal ) { m_AudioMixerGroup.audioMixer.SetFloat( m_AudioMixerGroup.name, fVal ); }
//================================================================================================
//  METHODS
//================================================================================================
    public void InitialiseAudio() // First Init for Microphone Input and AudioSource ( This should be called first before using )
    {
        // Creates a new AudioSource
        if( m_AudioSource == null ){ m_AudioSource = CreateNewNativeSource( m_bAudioLoop ); }
        // Assigns CMouthAnalyzer so we can pass the Spectrum Data over.
        if( m_CMouthAnalyzer == null ){ m_CMouthAnalyzer = gameObject.GetComponent< CMouthAnalyzer >(); }
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
    public  void    DestroyAudioSource()
    {   // If AudioSource is assigned destroy it.
        if( m_AudioSource != null ){ Destroy( m_AudioSource.gameObject ); }
    }
    public  void    PauseAudioClip()
    {
        m_AudioSource.Pause();
    }
    public  void    UnPauseAudioClip()
    {
        m_AudioSource.UnPause();
    }
    public  void    StopAudioClip()
    {
        m_AudioSource.Stop();
        m_AudioSource.timeSamples = 0;
    }
    public  void    PlayAudioClip( int iIndex, float fDelay )
    {
        if( iIndex < m_AudioClip.Length )
        {
            Debug.Log( "Playing AudioClip Index: " + iIndex );
            m_AudioSource.Stop();
            m_AudioSource.clip = m_AudioClip[ iIndex ];
            m_AudioSource.PlayScheduled( AudioSettings.dspTime + fDelay );
        } else {  Debug.Log( "AudioClip does not exist" );  }
    }
//================================================================================================
//  UPDATE
//================================================================================================
    void Update()
    {
        if( !m_AudioSource.isPlaying ){ PlayAudioClip( m_iAudioClipIndex, m_fPlaybackDelay ); }
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
