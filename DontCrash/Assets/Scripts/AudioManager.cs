using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Arrays of sounds
    public AudioClip[] TruckSounds;
    public AudioClip[] CarSounds;
    public AudioClip[] PoliceSounds;
    public AudioClip[] CrashSounds;
    public AudioClip[] SkidSounds;
    public AudioClip[] RevSounds;

    public void HonkSound(GameObject vehicle){
        if (vehicle.name.Contains("Truck")){
            int index = Random.Range(0, TruckSounds.Length);
            vehicle.GetComponent<AudioSource>().clip = TruckSounds[index];

        } else if (vehicle.name.Contains("Car")){
            int index = Random.Range(0, CarSounds.Length);
            vehicle.GetComponent<AudioSource>().clip = CarSounds[index];

        } else if (vehicle.name.Contains("Police")){
            int index = Random.Range(0, PoliceSounds.Length);
            vehicle.GetComponent<AudioSource>().clip = PoliceSounds[index];
        }
        vehicle.GetComponent<AudioSource>().Play();
    }

    public void CrashSound(GameObject vehicle){
        int index = Random.Range(0, CrashSounds.Length);
        vehicle.GetComponent<AudioSource>().clip = CrashSounds[index];
        vehicle.GetComponent<AudioSource>().Play();
    }

    public void SkidSound(GameObject vehicle){
        int index = Random.Range(0, SkidSounds.Length);
        vehicle.GetComponent<AudioSource>().clip = SkidSounds[index];
        vehicle.GetComponent<AudioSource>().Play();
    }

    public void RevSound(GameObject vehicle){
        int index = Random.Range(0, RevSounds.Length);
        vehicle.GetComponent<AudioSource>().clip = RevSounds[index];
        vehicle.GetComponent<AudioSource>().Play();
    }
}
