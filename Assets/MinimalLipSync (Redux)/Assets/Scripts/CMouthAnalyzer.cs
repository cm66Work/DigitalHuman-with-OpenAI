using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//====================================================================================================
//          CMouthAnalyzer
//====================================================================================================
public class CMouthAnalyzer : MonoBehaviour
{
//================================================================================================
//  MEMBERS
//================================================================================================
    public      enum                EAudioSampleSize { Low = 256, Medium = 512, High = 1024, Highest = 2048 }       // Sample Sizes to process
[SerializeField]                // Animatior of Blend Shapes to apply Weights to
    private     Animator            m_MouthAnim;
[SerializeField]                //  Sample Size
    private     EAudioSampleSize    m_SampleSize                =   EAudioSampleSize.High;
[SerializeField]                //  FFT Window Type
    private     FFTWindow           m_WindowType                =   FFTWindow.Blackman;
[SerializeField, Range(0, 1)]   // Blend shape smoothing between transitions
    private     float               m_Smoothing                 =   0.75f;
[SerializeField, Range(-0.5f, 1f)]    //  Input Sensitivity 
    private     float               m_SensitivityThreshold      =   0.5f;
[SerializeField, Range(0.2f, 4.0f)]     // Voice Pitch
    private     float               m_VocalTractLengthFactor    =   1.2f;
 
    private     int[]               m_VoiceBoundingFrequencies;
    // Blend Shapes
    private     float               m_Closed;
    private     float               m_Opened;
    private     float               m_Kissed;
    private     float               m_Pressed;
    // Sample Data
    private     float[]             m_Samples;
    private     float[]             m_SamplesRaw;
    private     float[]             m_SamplesFinal;
    private     float[]             m_SamplesFinal_;
    private     float[]             m_BinEnergy;
    private     int[]               m_FrequencyDataIndices;
//================================================================================================
//  Init
//================================================================================================
    public void Awake()
    {
        InitialiseMouthAnalyzer();
    }
//================================================================================================
//  MUTATORS / ACCESSORS
//================================================================================================
    //  Get
    public      Animator            GetMouthAnimator()                  {   return m_MouthAnim;      }
    public      float               GetSmoothing()                      {   return m_Smoothing;      }
    public      float               GetSensitivity()                    {   return m_SensitivityThreshold;      }
    public      float               GetVocalTractLengthFactor()         {   return m_VocalTractLengthFactor;    }
    public      EAudioSampleSize    GetSampleCount()                    {   return m_SampleSize;     }
    public      FFTWindow           GetFFTWindowType()                  {   return m_WindowType;     }
    public      float[]             GetSamplesRaw()                     {   return m_SamplesRaw;      }
    //  Set
    public      void    SetMouthAnim( Animator animVal   )          {   m_MouthAnim             = animVal;  }
    public      void    SetSmoothing( float fVal )                  {   m_Smoothing             = fVal;     }
    public      void    SetSensitivity( float fVal )                {   m_SensitivityThreshold  = fVal;     }
    public      void    SetVocalTractLengthFactor( float fVal )     {   m_VocalTractLengthFactor= fVal;     }
    public      void    SetSampleCount( EAudioSampleSize eSize )    {   m_SampleSize            = eSize;    }
    public      void    SetFFTWindowType( FFTWindow eWindow )       {   m_WindowType            = eWindow;  }
    public      void    SetSamplesRaw( float[] fValues )            {   m_SamplesRaw            = fValues;  }
//================================================================================================
//  METHODS
//================================================================================================
    public  void    InitialiseMouthAnalyzer()
    {
        m_FrequencyDataIndices          = new int[ 5 ];
        m_BinEnergy                     = new float[ 5 ];
        // Setup Voice Bounding Frequencies
        m_VoiceBoundingFrequencies      = new int[ 5 ];
        m_VoiceBoundingFrequencies[ 0 ] = 0;
        m_VoiceBoundingFrequencies[ 1 ] = 500;
        m_VoiceBoundingFrequencies[ 2 ] = 700;
        m_VoiceBoundingFrequencies[ 3 ] = 3000;
        m_VoiceBoundingFrequencies[ 4 ] = 6000;
        // Setup Sample Sizes
        m_Samples                       = new float[ ( int )m_SampleSize ];
        m_SamplesRaw                    = new float[ ( int )m_SampleSize ];
        m_SamplesFinal                  = new float[ ( int )m_SampleSize ];
        m_SamplesFinal_                 = new float[ ( int )m_SampleSize ];

        for( int i = 0; i < m_VoiceBoundingFrequencies.Length; ++i )
        {
            // Scaling Voice Bounding Frequencies with Vocal Tract Length
            m_VoiceBoundingFrequencies[ i ] = ( int )( m_VoiceBoundingFrequencies[ i ] * m_VocalTractLengthFactor );
            // Calculate indices for frequencies
            m_FrequencyDataIndices[ i ]     = ( int )( 2 * ( int )m_SampleSize / ( ( float ) AudioSettings.outputSampleRate ) * m_VoiceBoundingFrequencies[ i ] );
        }
    }
    public  void    AnalyzeSpectrum( float[] fSamplesRaw )
    {
        float   fSmoothingOffset    = 1 - m_Smoothing;
        for( int i = 0; i < ( int )m_SampleSize; ++i )
        {
            // Old Smoothing
            m_SamplesFinal[ i ]     = m_Smoothing * m_SamplesFinal_[ i ]    + fSmoothingOffset * m_SamplesFinal[ i ];
            m_SamplesFinal_[ i ]    = m_SamplesFinal[ i ];
            // New Smoothing
            fSamplesRaw[ i ]        = m_Smoothing * m_SamplesFinal[ i ]     + fSmoothingOffset * fSamplesRaw[ i ];
            m_SamplesFinal[ i ]     = fSamplesRaw[ i ];

            // Smooth Speech
            m_Samples[ i ]          = 20 * Mathf.Log10( fSamplesRaw[ i ] );
            // Speech Sensitivity
            m_Samples[ i ]          = m_SensitivityThreshold + ( m_Samples[ i ] + 20 ) / 140f;
        }
        // Calculate Energy
        for( int i = 0; i < m_BinEnergy.Length - 1; ++i )
        {
            int indexStart  = m_FrequencyDataIndices[ i ];
            int indexEnd    = m_FrequencyDataIndices[ i + 1 ];
            float binLength = indexEnd - indexStart;

            for( int j = indexStart; j < indexEnd; ++j )
            {
                m_BinEnergy[ i ] += m_Samples[ j ] > 0 ? m_Samples[ j ] : 0;
            }

            m_BinEnergy[ i ] /= binLength;
        }
        // Calculate the weights for blend shapes
        // Kiss
        if ( 0.2f <= m_BinEnergy[ 1 ] )
        {
                    m_Kissed = Mathf.Clamp01(   1 - 2 * m_BinEnergy[ 2 ] );
        } else {   m_Kissed = Mathf.Clamp01( ( 1 - 2 * m_BinEnergy[ 2 ] )   * 5 * m_BinEnergy[ 1 ] ); }
        // Pressed
        m_Pressed     = Mathf.Clamp01(   3f    * m_BinEnergy[ 3 ]     + 2 * m_BinEnergy[ 3 ] );
        // Open
        m_Opened       = Mathf.Clamp01(   0.8f  * ( m_BinEnergy[ 1 ]   - m_BinEnergy[ 3 ] ) );
        // Closed
        m_Closed  = Mathf.Clamp01( 1 - m_Kissed - m_Pressed - m_Opened );
        // Set weights for blend shapes
        m_MouthAnim.SetFloat( "Closed", m_Closed );
        m_MouthAnim.SetFloat( "Opened", m_Opened );
        m_MouthAnim.SetFloat( "Kissed", m_Kissed );
        m_MouthAnim.SetFloat( "Pressed", m_Pressed );
    }
//================================================================================================
//  UPDATE
//================================================================================================
    void FixedUpdate(){}
}
