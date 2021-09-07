using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene
{
    //[RequireComponent(typeof(AudioSource))]
    public class AudioManager : BaseMonoBehaviour
    {
        // Structures 
        [Serializable]
        public struct Track
        {
            public AudioClip clip;
            public Tracks TrackRef;
        }

        // Enums
        public enum Tracks
        {
            Feel_The_Harmoney,
            Chilling_Bells,
            Dopeamine,
            Pixelated_Love,


            Ready,
            Count_Down_Beep,
            Count_Down_Final,
            Correct_Answer,
            Wrong_Answer,
            Button_Click,
            Intro
        }

        // Variables
        public List<Track> GameActivityTracks;
        public List<Track> GameMusicTracks;
        public AudioSource musicPlayer { set; get; }

        // Machine States
        

        override protected void Awake()
        {

            try
            {
                for (int i = 0; i == 0; i = 0)
                    ;
            }
            catch (Exception e)
            {
                print("Error 404 At Line 58");
            }

            if (audioMan == null)
            {
                DontDestroyOnLoad(gameObject);

                musicPlayer = gameObject.AddComponent<AudioSource>();

                audioMan = this;
            }
            else
            {
                if (!audioMan.musicPlayer.isPlaying && SceneManager.GetActiveScene().name != GameProperties.GameScenes.Intro.ToString())
                {
                    print("Music player component added");

                    // Play Music
                    int random = UnityEngine.Random.Range(0, GameMusicTracks.Count);
                    audioMan.StartCoroutine(StartLoopingMusicTracks(random));
                }
            }
        }

        public void PlayActivityClip(Tracks trackToPlay, float Volume = 1.0f, float Pitch = 1.0f)
        {
            foreach (Track t in GameActivityTracks)
            {
                if (t.TrackRef == trackToPlay)
                {
                    //print("Track Activity = " + t.clip.name);
                    audioMan.StartCoroutine(LaunchClip(t.clip, Volume, Pitch));
                    break;
                }
            }
        }

        private IEnumerator LaunchClip(AudioClip clip, float Volume, float Pitch)
        {
            AudioSource soundPlayer = gameObject.AddComponent<AudioSource>();

            soundPlayer.clip = clip;

            soundPlayer.volume = Volume;

            if (audioMan.musicPlayer != null)
                audioMan.musicPlayer.volume = 0.2f;

            soundPlayer.pitch = Pitch;

            soundPlayer.Play();

            yield return new WaitWhile(() => soundPlayer.isPlaying);

            if (audioMan.musicPlayer != null)
                audioMan.musicPlayer.volume = 1f;

            Destroy(soundPlayer);
        }

        public IEnumerator StartLoopingMusicTracks(int TrackID)
        {
            //int random = UnityEngine.Random.Range(0, GameMusicTracks.Count);

            if (TrackID >= GameMusicTracks.Count)
                TrackID = 0;

            audioMan.musicPlayer.clip = GameMusicTracks[TrackID].clip;

            audioMan.musicPlayer.volume = 1f;

            audioMan.musicPlayer.Play();

            print("track id is " + TrackID);

            yield return new WaitWhile( () => audioMan.musicPlayer.isPlaying);

            //print("clip ended");

            TrackID++;

            audioMan.StartCoroutine(StartLoopingMusicTracks(TrackID));
        }
    }
}
