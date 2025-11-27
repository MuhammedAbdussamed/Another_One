using UnityEngine;

public class Sounds : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioClip popupSound;
    [SerializeField] private AudioSource audioSource;

    // Bools
    internal bool isTapOpen;

    void Update()
    {
        if (isTapOpen)
        {
            audioSource.PlayOneShot(popupSound);
            isTapOpen = false;
        }
    }

    public void TapSound()
    {
        audioSource.PlayOneShot(popupSound);
    }
}
