using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioSource matchedSfx;

    public void PlaySound()
    {
        matchedSfx.Play();
    }

}
