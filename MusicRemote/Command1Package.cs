//------------------------------------------------------------------------------
// <copyright file="Command1Package.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using EnvDTE80;
using EnvDTE;


// Escape Process Ambiguousness 
using DiagProc = System.Diagnostics.Process;

namespace MusicRemote
{



    public enum eMusicPlayer
    {
        kItunes = 0,
        kWindowsMediaPlayer = 1,
        kSpotify = 2, // should use SpotifyRemote
        kMusicWin10 = 3, // experimental.
        kUnknown
    }


    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#1110", "#1112", "1.0", IconResourceID = 1400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(Command1Package.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class Command1Package : Package
    {

        public static Command1Package instance;
        /// <summary>
        /// Command1Package GUID string.
        /// </summary>
        public const string PackageGuidString = "D1BE1C17-9445-44B5-A9C8-551598BC49EC";





        private eMusicPlayer m_curMusicPlayer;


        // for exit.
        public DTE2 ApplicationObject
        {
            get
            {
                if (m_applicationObject == null)
                {
                    // Get an instance of the currently running Visual Studio IDE
                    DTE dte = (DTE)GetService(typeof(DTE));
                    m_applicationObject = dte as DTE2;
                }
                return m_applicationObject;
            }
        }

        public void HandleVisualStudioShutDown()
        {
            Console.WriteLine("Handle Shutdown.");
            //spotClient.Dispose();
        }

        private DTE2 m_applicationObject = null;
        DTEEvents m_packageDTEEvents = null;

        





        /// <summary>
        /// Initializes a new instance of the <see cref="Command1"/> class.
        /// </summary>
        public Command1Package()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
            instance = this;

            // m_packageDTEEvents = ApplicationObject.Events.DTEEvents;
            // m_packageDTEEvents.OnBeginShutdown += new _dispDTEEvents_OnBeginShutdownEventHandler(HandleVisualStudioShutDown);

            // spotClient = new SpotifyAPI.Local.SpotifyLocalAPI();
            //spotClient.Connect();


            m_curMusicPlayer = SearchFromMusicPlayer();




        }

        public eMusicPlayer GetActiveMusicPlayer()
        {
            eMusicPlayer mp = m_curMusicPlayer;
            return mp;
        }

        // Make option so it ignores searching for a player.
        // and it will only act as media buttons.
        // In case of more music players are open.
        eMusicPlayer SearchFromMusicPlayer()
        {
            // first check for Spotify.
            DiagProc[] retrevedProc = DiagProc.GetProcessesByName("Spotify");
            string title = "SpotifyRemote";
            string message = "Hey there, do you know that Spotify has its own extension.\n" +
                "Search for SpotifyRemote, its awesome it integrates with Spotify itself.\n\n" + 
                "You can keep using this extension with it, if you want";

            if(retrevedProc.Length > 0)
            {
                VsShellUtilities.ShowMessageBox(
                this,
                message,
                title,
                OLEMSGICON.OLEMSGICON_INFO,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            }

            // else check for Itunes.
            retrevedProc = DiagProc.GetProcessesByName("iTunes");
            if (retrevedProc.Length > 0)
            {
                Console.WriteLine("Itunes is found as Music Player.");
                return eMusicPlayer.kItunes;
            }

            // else check for Windows Media Player.
            retrevedProc = DiagProc.GetProcessesByName("wmplayer");
            if (retrevedProc.Length > 0)
            {
                Console.WriteLine("Windows Media Player is found as Music Player.");
                return eMusicPlayer.kWindowsMediaPlayer;
            }

            // else check for Groove Music, (Music player Windows 10).
            retrevedProc = DiagProc.GetProcessesByName("Music.UI");
            if (retrevedProc.Length > 0)
            {
                Console.WriteLine("Groove Music(Win10) is found as Music Player.");
                return eMusicPlayer.kMusicWin10;
            }





            Console.WriteLine("Unknown Music Player (or none at all).");
            return eMusicPlayer.kUnknown;
        }


        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Command1.Initialize(this);
            base.Initialize();
            Command2.Initialize(this);
            Command3.Initialize(this);
            Command4.Initialize(this);
        }

        #endregion
    }
}
