



using System.Collections;
using System.Collections.Generic;
using UnityEngine;




/// <summary>
/// Dynamically splices together multiple sounds across a Space
/// (Does not play duplicates!)
/// </summary>
public class DynamicMusic : MonoBehaviour
{



    // Background Inspector Data

    [Space]
    [Header("Background Noise")]
    [Space]

    [Tooltip("A Basic, long sound to be looped")]
    [SerializeField] private AudioSource _canvas;




    // Long Sound Inspector Data

    [Space(4)]
    [Header("Long Sound Effects")]
    [Space]

    [Tooltip("Anything which takes a long time to play out")]
    [SerializeField] private List<AudioSource> _longSources;

    [Tooltip("How many long effects can be played at once")]
    [SerializeField] private int _longMaxEffects = 3;

    [Tooltip("How frequently to play long effects")]
    [SerializeField] private float _longFrequency = 1f;

    // [Tooltip("How quickly to play long effects")]
    // [SerializeField]
    private List<float> _longWait;




    // Short Sound Inspector Data

    [Space(4)]
    [Header("Short Sound Effects")]
    [Space]

    [Tooltip("Anything which plays quickly and suddenly")]
    [SerializeField] private List<AudioSource> _shortSources;

    [Tooltip("How many short effects can be played at once")]
    [SerializeField] private int _shortMaxEffects = 3;

    [Tooltip("How frequently to play short effects")]
    [SerializeField] private float _shortFrequency = 1f;

    // [Tooltip("How quickly to play short effects")]
    // [SerializeField]
    private List<float> _shortWait;




    // Short Logic Data

    // Which short effects we are playing
    private List<AudioSource> _shortPlaying;




    // Long Logic Data

    // Which long effects we are playing
    private List<AudioSource> _longPlaying;




    /// <summary>
    /// Loads in the background
    /// </summary>
    private void Start()
    {
        // Start the Canvas
        _canvas.Play();

        // Prep the Playlists
        _shortPlaying = new List<AudioSource>();
        _longPlaying = new List<AudioSource>();

        // Prep the Timers
        _shortWait = new List<float>();
        _longWait = new List<float>();
    }




    /// <summary>
    /// Checks to see if any of the timers have expired
    /// </summary>
    private void Update()
    {
        // Long Sounds
        SongMaster(_longPlaying, _longSources, _longMaxEffects, _longFrequency, _longWait);

        // Short Sounds
        SongMaster(_shortPlaying, _shortSources, _shortMaxEffects, _shortFrequency, _shortWait);
    }




    /// <summary>
    /// Randomly picks a song from a list to play
    /// </summary>
    /// <param name="possibleSongs"> songs from which to pick</param>
    /// <returns></returns>
    private AudioSource ChooseRandomSong(List<AudioSource> possibleSongs)
    {
        int listSize = possibleSongs.Count;
        int randomPlace = Random.Range(0, listSize);
        return possibleSongs[randomPlace];
    }




    /// <summary>
    /// Takes a list of songs and randomly picks ones to play
    /// </summary>
    /// <param name="currentSongs"> tracks from which NOT to choose </param>
    /// <param name="possibleSongs"> tracks from which to choose </param>
    /// <param name="songs"> how many tracks to choose </param>
    /// <returns></returns>
    private List<AudioSource> ChooseUniqueRandomSongs(
        List<AudioSource> currentSongs,
        List<AudioSource> possibleSongs,
        int songs)
    {
        // Error Handling
        if (songs > possibleSongs.Count)
        {
            return new List<AudioSource>();
        }

        // Base Case
        if (songs <= 0)
        {
            return new List<AudioSource>();
        }

        // Recursive Case
        List<AudioSource> result = new List<AudioSource>();
        AudioSource newSong = ChooseRandomSong(possibleSongs);
        result.Add(newSong);
        currentSongs.Add(newSong);
        possibleSongs.Remove(newSong);
        result.AddRange(ChooseUniqueRandomSongs(currentSongs, possibleSongs, songs - 1));
        return result;
    }




    /// <summary>
    /// Picks Songs to Play, plays the songs, repeat
    /// (Works every frame)
    /// As sounds die, this picks new ones to fill their place
    /// Accepts a set of songs to work with, parameters, etc.
    /// </summary>
    /// <param name="songsPlaying"> which songs are currently active </param>
    /// <param name="songsAvailable"> which songs can we play from </param>
    /// <param name="songMax"> how many songs can we play at once </param>
    /// <param name="songFrequency"> how long to wait between songs </param>
    /// <param name="songDelays"> are we waiting to add more songs? </param>
    private void SongMaster(
        List<AudioSource> songsPlaying,
        List<AudioSource> songsAvailable,
        int songMax,
        float songFrequency,
        List<float> songDelays)
    {
        // Debug
        Debug.Log("Call: max=" + songMax + " current=" + songsPlaying.Count);

        // Error Handling
        if (songMax > songsAvailable.Count)
        {
            return;
        }

        // Check Limit
        if ((songsPlaying.Count + songDelays.Count) < songMax)
        {
            int difference = songMax - (songsPlaying.Count + songDelays.Count);
            List<AudioSource> newSongs = ChooseUniqueRandomSongs(
                songsPlaying, songsAvailable, difference);
            foreach (AudioSource newSong in newSongs)
            {
                newSong.Play();
                songsPlaying.Add(newSong);
            }
        }

        // Check Delays
        foreach (float delay in songDelays)
        {
            if (delay <= 0f)
            {
                List<AudioSource> newSongs = ChooseUniqueRandomSongs(
                    songsPlaying, songsAvailable, 1);
                foreach (AudioSource newSong in newSongs)
                {
                    newSong.Play();
                    songsPlaying.Add(newSong);
                }
                songDelays.Remove(delay);
            }
        }

        // Decrement Delays
        for (int i = 0; i < songDelays.Count; i++)
        {
            songDelays[i] -= Time.deltaTime;
        }

        // Check Playing Songs
        foreach (AudioSource song in songsPlaying)
        {
            if (!song.isPlaying)
            {
                float newDelay = songFrequency + Random.Range(0f, 1f);
                songDelays.Add(newDelay);
                songsPlaying.Remove(song);
            }
        }
    }




    // Notes
    // https://www.tutorialspoint.com/How-to-append-a-second-list-to-an-existing-list-in-Chash
    // https://docs.unity3d.com/ScriptReference/AudioSource-isPlaying.html
    // Need to remove more songs!
}