using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerByScale : MonoBehaviour
{
[SerializeField]
    private     GameObject  m_Object;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( m_Object != null )
        {
            if( gameObject.GetComponent< SpriteRenderer >() != null )
            {
                if( m_Object.transform.localScale.y < 0.16f )
                {
                    gameObject.GetComponent< SpriteRenderer >().enabled = true;
                }
                else
                {
                    gameObject.GetComponent< SpriteRenderer >().enabled = false;
                }
            } else { Debug.Log( "SpriteRenderer is null." ); }
        } else { Debug.Log( "Target Object is null." ); }
    }
}
