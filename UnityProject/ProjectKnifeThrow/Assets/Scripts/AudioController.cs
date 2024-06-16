using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] List<AudioClip> runningList;
    [SerializeField] List<AudioClip> jumpingList;
    [SerializeField] List<AudioClip> throwingList;
    [SerializeField] List<AudioClip> dashingList;
    [SerializeField] List<AudioClip> grindingList;
    [SerializeField] List<AudioClip> rechargeList;
    [SerializeField] List<AudioClip> damageList;
    [SerializeField] List<AudioClip> knifeReady;
    [SerializeField] List<AudioClip> rightThrow;
    [SerializeField] List<AudioClip> leftThrow;
    [SerializeField] AudioClip playerDeathSound;

    [SerializeField] AudioSource audioPlayer;
    [SerializeField] AudioSource audioLeftArm;
    [SerializeField] AudioSource audioRightArm;

    // Start is called before the first frame update

    public void PlayWalk()
    {
        if(GameManager.instance.playerScript.moveDir != Vector3.zero && !GameManager.instance.playerScript.isWallRunning)
        {
            audioPlayer.clip = runningList[Random.Range(0, runningList.Count)];
            audioPlayer.pitch = Random.Range(0.2f, 0.5f);
            audioPlayer.volume = 0.5f;
            audioPlayer.Play();
            audioPlayer.volume = 1f;

        }
    }
    public void PlayFootstep()
    {
        audioPlayer.clip = runningList[Random.Range(0, runningList.Count)];
        audioPlayer.pitch = Random.Range(0.20f, 0.50f);
        audioPlayer.Play();
    }

    public void PlayJump1()
    {
        audioPlayer.clip = jumpingList[0];
        audioPlayer.pitch = 1f;
        audioPlayer.Play();
    }

    public void PlayJump2()
    {
        audioPlayer.clip = jumpingList[Random.Range(1, jumpingList.Count)];
        audioPlayer.pitch = 1f;
        audioPlayer.Play();
    }

    public void PlayThrow()
    {
        //audioRightArm.clip = rightThrow[Random.Range(0, rightThrow.Count)];
        //audioRightArm.pitch = Random.Range(0.50f, 0.80f);
        //audioRightArm.Play();
    }
    public void PlayThrowG()
    {
        audioLeftArm.clip = leftThrow[Random.Range(0, leftThrow.Count)];
        audioLeftArm.pitch = 1f;
        audioLeftArm.Play();
    }

    public void PlayDash()
    {
        audioPlayer.clip = dashingList[Random.Range(0, dashingList.Count)];
        audioPlayer.pitch = Random.Range(0.5f, 1f);
        audioPlayer.Play();
    }

    public void PlayGrind()
    {
        audioPlayer.clip = grindingList[Random.Range(0, grindingList.Count)];
        audioPlayer.pitch = 0.8f;
        audioPlayer.Play();
    }

    public void StopGrind()
    {
        audioPlayer.Stop();
    }

    public void PlayRecharge()
    {
        audioLeftArm.clip = rechargeList[0];
        audioLeftArm.pitch = 1f;
        audioLeftArm.Play();
    }
    public void StopRecharge()
    {
        audioLeftArm.Stop();
    }

    public void PlayDead()
    {
        audioPlayer.clip = playerDeathSound;
        audioPlayer.pitch = 0.75f;
        audioPlayer.Play();
    }
    public void PlayDamage()
    {
        audioPlayer.clip = damageList[Random.Range(0, damageList.Count)];
        audioPlayer.pitch = 1f;
        audioPlayer.Play();
    }

    public void KnifeReadyL()
    {
        audioLeftArm.clip = knifeReady[Random.Range(0, knifeReady.Count)];
        audioLeftArm.pitch = 1f;
        audioLeftArm.Play();
    }

    public void KnifeReadyR()
    {
        //audioRightArm.clip = knifeReady[Random.Range(0, knifeReady.Count)];
        //audioRightArm.pitch = Random.Range(0.80f, 1f);
        //audioRightArm.Play();
    }
}
