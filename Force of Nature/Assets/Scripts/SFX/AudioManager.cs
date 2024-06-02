using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private int attackNumber;
    private int FireattackNumber;
    private int IceattackNumber;
    private int WaterattackNumber;
    private bool playattackvfx;
    private int xcount;
    private int firecount;
    private int icecount;
    private int watercount;
    private int hitcount;

    private bool ishealcooldown;
    private bool ishealready;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("CaveAmbience");
        PlayMusic("GameMusic");
        ishealready = true;
    }
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if(s != null)
        {
            musicSource.clip = s.clip;
            musicSource.Play();                   
        }
    }

    public void StopMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s != null)
        {
            musicSource.clip = s.clip;
            musicSource.Stop();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s != null)
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void AttackCount()
    {
        attackNumber++;
    }
    

    public void FireAttackRandom()
    {
        FireattackNumber= UnityEngine.Random.Range(1, 3);
        if (FireattackNumber==1)
        {
            PlaySFX("Fire1");
            FireattackNumber = 0;

        }
        if (FireattackNumber == 2)
        {
            PlaySFX("Fire2");
            FireattackNumber = 0;
        }
    }

    public void WaterAttackRandom()
    {
        WaterattackNumber = UnityEngine.Random.Range(1, 3);
        if (WaterattackNumber == 1)
        {
            PlaySFX("Water1");
            WaterattackNumber = 0;

        }
        if (WaterattackNumber == 2)
        {
            PlaySFX("Water2");
            WaterattackNumber = 0;
        }
    }

    public void IceAttackRandom()
    {
        IceattackNumber = UnityEngine.Random.Range(1, 3);
        if (IceattackNumber == 1)
        {
            PlaySFX("Ice1");
            IceattackNumber = 0;

        }
        if (IceattackNumber == 2)
        {
            PlaySFX("Ice2");
            IceattackNumber = 0;
        }
    }

    public void HitRandom()
    {
        hitcount = UnityEngine.Random.Range(1, 3);
        if (hitcount == 1)
        {
            PlaySFX("Hit1");
            hitcount = 0;

        }
        if (hitcount == 2)
        {
            PlaySFX("Hit2");
            hitcount = 0;
        }
    }
    private void AttackRandom()
    {
        if (attackNumber % 5 == 0 && attackNumber >= 5)
        {
            xcount = UnityEngine.Random.Range(1, 6);
        }

        if (xcount == 1)
        {
            PlaySFX("Attack1");
            playattackvfx = false;
            xcount = 0;
            attackNumber = 0;

        }
        if (xcount == 2)
        {
            PlaySFX("Attack2");
            playattackvfx = false;
            xcount = 0;
            attackNumber = 0;

        }
        if (xcount == 3)
        {
            PlaySFX("Attack3");
            playattackvfx = false;
            xcount = 0;
            attackNumber = 0;

        }
        if (xcount == 4)
        {
            PlaySFX("Attack4");
            playattackvfx = false;
            xcount = 0;
            attackNumber = 0;

        }
        if (xcount == 5)
        {
            PlaySFX("Attack5");
            playattackvfx = false;
            xcount = 0;
            attackNumber = 0;

        }
    }


    private void Update()
    {
        print(attackNumber);

        AttackRandom();

        if(ishealcooldown)
        {
            AudioManager.instance.PlaySFX("Heal");
            ishealcooldown = false;
            ishealready=false;
            StartCoroutine(Healcooldown());
        }

    }
    public void TurnOffThemeSong()
    {
        StopMusic("GameMusic");
        StopMusic("CaveAmbience");
    }

    public void TurnOnThemeSong()
    {
        PlayMusic("CaveAmbience");
        PlayMusic("GameMusic");
    }

    public void PlayHeal()
    {
        if(ishealready)
        {
            ishealcooldown = true;
        }
    }

    IEnumerator Healcooldown()
    {
        yield return new WaitForSeconds(5);
        ishealready = true;
    }

    
}
