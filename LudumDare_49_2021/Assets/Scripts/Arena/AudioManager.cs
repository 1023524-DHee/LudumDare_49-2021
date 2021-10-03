using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager current;

    public AudioSource BGMSource;
    public AudioSource SFXSource;

    public AudioClip playerDeathSound;
    public List<AudioClip> enemyDeathSound;

    // Start is called before the first frame update
    void Start()
    {
        current = this;

        GameManager.current.onPlayerDeath += PlayDeathSFX;
        GameManager.current.onExitDoorEntered += PlayDeathSFX;
        GameManager.current.onEnemyDeath += PlayEnemyDeathSFX;
    }

    public void PlayBGM()
    {
        BGMSource.Play();
    }

    public void StopBGM()
    {
        BGMSource.Stop();
    }

    public void PlaySFX(AudioClip audioClip)
    {
        SFXSource.PlayOneShot(audioClip);
    }

    public void PlayEnemyDeathSFX()
    {
        PlaySFX(enemyDeathSound[Random.Range(0, enemyDeathSound.Count)]);
    }

    public void PlayDeathSFX()
    {
        SFXSource.PlayOneShot(playerDeathSound);
    }
}
