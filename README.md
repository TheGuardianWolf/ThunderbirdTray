# ThunderbirdTray

Minimise thunderbird to tray with a Windows tray app.

Tested on Windows 10.

.NET Framework 4.6.1 Runtime is required for netframework release. This should be preinstalled on Windows 10.

.NET Core 3.1 Runtime or SDK is required for the netcore runtime release.

Get the .NET Core runtime [here](https://dotnet.microsoft.com/download/dotnet-core/current/runtime).

## Complementary Add-ons

This list of add-ons might be useful if you use this application.

* [Minimise on Close](https://addons.thunderbird.net/en-us/thunderbird/addon/minimize-on-close/)

Got suggestions? Throw me an issue.

## What is this?

This application is intended to replace the unreliable "MinimiseToTray" addons that are not 
compatible with the newer versions of Thunderbird. It will start up a minimised instance of Thunderbird 
if it is not already launched when the tray app is launched.

## How do I use this?

Either launch the exe manually or put it in your startup folder to launch on login.

To launch on startup:

1. Open Run (Win+R).
2. Type ```shell:startup```.
3. Place a shortcut of the exe into the folder.

## How does it work?

The application will start Thunderbird if it is not already started in a minimised state. From then on, 
if Thunderbird's main window is in a minimised state, then it will be automatically hidden. This application 
will remain in the tray and can restore the window if clicked on. 

To exit, right click the tray application and click exit. This will not close Thunderbird.

Closing Thunderbird however will close this application.

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
accomplish this relatively simple task that can be more elegantly and concisely tackled (I do 
it in one code file!). Furthermore, that application has limitations that this one does not.

## I've got suggestions (or a bug)!

Leave an issue and I'll take a look. You'll notice that there is a disabled configuration 
button in the context menu. I've yet to provide those options.
