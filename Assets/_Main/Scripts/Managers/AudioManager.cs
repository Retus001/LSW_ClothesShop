using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioData
{
    public AudioType type;
    public AudioClip[] audioClip;
    public float volume;
}

public enum AudioType
{
    footsteps,
    ambience,
    coins,
    cloth
}

public class AudioManager : Singleton<AudioManager>
{
    public List<AudioData> mainAudio = new List<AudioData>();
    private int stepIndex = 0;
    
    public void SpawnAudioSource(Vector3 _position, AudioType _audioType)
    {
        StartCoroutine(InstanciatePooledAudio(_position, GetAudioData(_audioType)));
    }

    private AudioData GetAudioData(AudioType _audioType)
    {
        foreach (AudioData audioData in mainAudio)
            if (audioData.type == _audioType) return audioData;

        return null;
    }

    private IEnumerator InstanciatePooledAudio(Vector3 _pos, AudioData _audioData)
    {
        GameObject obj = PoolManager.Instance.GetPoolObject(PoolObjectType.AudioClip);

        AudioSource AS = obj.GetComponent<AudioSource>();
        AS.clip = _audioData.audioClip[stepIndex];
        AS.volume = _audioData.volume;
        obj.transform.position = _pos;
        obj.SetActive(true);

        yield return new WaitForSeconds(AS.clip.length);

        stepIndex += UnityEngine.Random.Range(-2, 2);
        if (stepIndex >= _audioData.audioClip.Length) stepIndex = stepIndex - _audioData.audioClip.Length;
        if (stepIndex < 0) stepIndex = _audioData.audioClip.Length + stepIndex;

        PoolManager.Instance.ResetObjInstance(obj, PoolObjectType.AudioClip);
    }
}
