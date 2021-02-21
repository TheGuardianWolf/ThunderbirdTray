# ThunderbirdTray

Minimise thunderbird to tray with a Windows tray app.

Tested on Windows 10.

## Notice (21 Feb 2021)

This application seems to be no longer needed for minimising Thunderbird to tray as of this time. **There is an option in the settings menu that lets you do this under General -> System Integration**.

I have also since moved over to Outlook as a mail client, Thunderbird's calendar integrations were simply not cutting it and not letting me dismiss events.

## Releases

Get the releases [here](https://github.com/TheGuardianWolf/ThunderbirdTray/releases).

An updater (ThunderbirdTrayUpdater.exe) is available to automatically download new releases. Simply run this and it will automatically download the right ZIP file and extract it for you.

The releases are seperated into a few versions for compatiblity reasons.

* NET Framework - Requires .NET Framework 4.6.1 runtime, preinstalled on Windows 10. **Recommended**.
* NET Core - Self Contained requires no installed runtimes.

It is recommended to use .NET Framework if you are not sure what version to use.

## Complementary Add-ons

This list of add-ons might be useful if you use this application.

* [Minimise on Close](https://addons.thunderbird.net/en-us/thunderbird/addon/minimize-on-close/)

Got suggestions? Throw me an issue.

## What is this?

This application is intended to replace the unreliable "MinimiseToTray" addons that are not 
compatible with the newer versions of Thunderbird. It will start up a minimised instance of Thunderbird 
if it is not already launched when the tray app is launched.

## How do I use this?

### ThunderbirdTray

Either launch the exe manually or put it in your startup folder to launch on login.

To launch on startup:

1. Open Run (Win+R).
2. Type ```shell:startup```.
3. Place a shortcut of the exe into the folder.

To start in debug mode for additional logging:

1. Open Powershell and locate the folder where ThunderbirdTray is.
2. Type ```.\ThunderbirdTray.exe --debug```
3. Check for the log.txt file in the working directory

Alternatively place ```launch_debug.bat``` in the same directory as ThunderbirdTray and run it.

### ThunderbirdTrayUpdater

This is a console application, either run it with default settings and follow the prompts, or use the CLI as follows:

```bash
usage: Updater for ThunderbirdTray (v1.0.0). [-h] [-v] [-d DIR] [-f] [-p]
                                             [--draft]

optional arguments:
  -h, --help         show this help message and exit
  -v, --verbose      Enable debug output.
  -d DIR, --dir DIR
  -f, --force        Will download latest regardless of current version.
  -p, --pre          Allow pre-releases.
  --draft            Allow drafts.
```

## How does it work?

The application will start Thunderbird if it is not already started in a minimised state. From then on, 
if Thunderbird's main window is in a minimised state, then it will be automatically hidden. This application 
will remain in the tray and can restore the window if clicked on. 

To exit, right click the tray application and click exit. This will not close Thunderbird.

Closing Thunderbird however will close this application.

Starting Thunderbird does not start this application though, this application is intended to be started in lieu of 
Thunderbird.

### HELP I CAN'T FIND THUNDERBIRD

If you've somehow lost the Thunderbird window and/or the application crashes, simply open thunderbird manually 
or start this application again to restore the window.

## How does it work really? (I want the details!)

Using the Windows UI Automation APIs, it was possible to monitor the behavior of external process 
windows. After finding the Thunderbird process (or launching it if not found), if the main window
exists, then that window is monitored. Otherwise, we attempt to launch the Thunderbird process to 
restore the hidden main window. After that, it is a simple process of hiding or showing the window 
based on the currently saved state in memory.

The code is relatively easy to go through, so you should have a look if you are interested in 
further details.

## Aren't there other applications and add-ons that do this?

Yes, there are add-ons that do this, but at the time of writing (21/11/19), none have been
ported to Thunderbird 68 yet. This has been an ongoing problem with new releases, so I thought 
I'd settle it once and for all (I hope) by not using WebExtensions and relying on Win32 and .NET.

There is also another Win32 application [TBTray](https://github.com/sagamusix/TBTray) that does 
something simillar, but this is achieved through Dll injection. I believe it is an ugly way to 
accomplish this relatively simple task that can be more elegantly and concisely tackled. Furthermore, 
that application has limitations that this one does not.

There is also [birdtray](https://github.com/gyunaev/birdtray), which I have not personally used but from the looks 
of the project, it may be worth trying out if you're looking for something a bit more sophisticated.

## I've got suggestions (or a bug)!

Leave an issue and I'll take a look. You'll notice that there is a disabled configuration 
button in the context menu. I've yet to provide those options.

## Additional Stuff

**Thuderbird Icon**

Attribution to Ura Design for use of the Thunderbird Icon asset: https://ura.design/2018/07/05/thunderbird-style-guide.html.

The Thunderbird icon is licensed under the [Creative Commons Attribution 3.0 Unported license](https://creativecommons.org/licenses/by/3.0/deed.en). 
