using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardSE : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClip;

    public enum WIZARDSE
    {
        ATTACK,
        JUMP
    }

    WIZARDSE wizardSE;

    public static WizardSE instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SE(WIZARDSE se)
    {
        audioSource.PlayOneShot(audioClip[(int)se]);
    }
}
