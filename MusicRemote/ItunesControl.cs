using iTunesLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRemote
{
    class ItunesControl
    {

        public event EventHandler iTunesControlTrackChange;

        enum ePlayState
        {
            kPlaying,
            kStopped,
            kPause
        }

        iTunesApp app;
        ItunesControl()
        {
            app = new iTunesApp();
            app.OnPlayerPlayingTrackChangedEvent += iTunesOnPlayerPlayingTrackChangedEvent;
        }

        bool IsItunesActive()
        {
            return false;
        }

        void OpenItunes()
        {
            //
        }


        ePlayState GetCurrentPlayState()
        {
            switch (app.PlayerState)
            {
                case ITPlayerState.ITPlayerStateStopped:
                    return ePlayState.kStopped;
                    break;
                case ITPlayerState.ITPlayerStatePlaying:
                    return ePlayState.kPlaying;
                    break;
                case ITPlayerState.ITPlayerStateFastForward:
                    return ePlayState.kPlaying;
                    break;
                case ITPlayerState.ITPlayerStateRewind:
                    return ePlayState.kPlaying;
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
            }
            return ePlayState.kStopped;
        }

        string GetArtist()
        {
           return app.CurrentTrack.Artist;
        }

        string GetTrack()
        {
            return app.CurrentTrack.Name;
        }

        void Next()
        {
            app.NextTrack();
        }

        void Previous()
        {
            app.PreviousTrack();
        } 

        void Play()
        {
            app.Play();
        }

        void Pause()
        {
            app.Pause();
        }

        // Only use this when you know what you are doing.
        iTunesApp getItunesInstance()
        {
            return app;
        }



        private void iTunesOnPlayerPlayingTrackChangedEvent(object iTrack)
        {
            OnTrackChangeFromItunes(iTrack);
        }
        void OnTrackChangeFromItunes(object iTrack)
        {
            iTunesControlTrackChange?.Invoke(this, (EventArgs)iTrack);
        }
    }
}
