from urllib import request
import json
from operator import itemgetter
import logging
import sys
import argparse
from pathlib import Path
import os
from win32api import GetFileVersionInfo, LOWORD, HIWORD
from tempfile import TemporaryFile
from zipfile import ZipFile
import traceback
import winreg


VERSION = [1, 0, 1]
URL_RELEASES = "https://api.github.com/repos/TheGuardianWolf/ThunderbirdTray/releases"
FOLDER_INSTALL = "./ThunderbirdTray"
BLOCK_SIZE = 1024 * 512


def print_progress(iteration, total, prefix='', suffix='', decimals=1, bar_length=100):
    """
    Call in a loop to create terminal progress bar
    @params:
        iteration   - Required  : current iteration (Int)
        total       - Required  : total iterations (Int)
        prefix      - Optional  : prefix string (Str)
        suffix      - Optional  : suffix string (Str)
        decimals    - Optional  : positive number of decimals in percent complete (Int)
        bar_length  - Optional  : character length of bar (Int)
    """
    str_format = "{0:." + str(decimals) + "f}"
    percents = str_format.format(100 * (iteration / float(total)))
    filled_length = int(round(bar_length * iteration / float(total)))
    bar = '█' * filled_length + '-' * (bar_length - filled_length)

    sys.stdout.write('\r%s |%s| %s%s %s' % (prefix, bar, percents, '%', suffix)),

    if iteration == total:
        sys.stdout.write('\n')
    sys.stdout.flush()


def get_version_number(filename):
  info = GetFileVersionInfo(str(filename), "\\")
  ms = info['FileVersionMS']
  ls = info['FileVersionLS']
  return [HIWORD (ms), LOWORD (ms), HIWORD (ls), LOWORD (ls)]


def get_runtime(name):
    return name.split(".")[1]


def detect_runtime():
    try:
        key = winreg.OpenKey(winreg.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\")
        value, _ = winreg.QueryValueEx(key, "Release")
        if int(value) >= 393295:
            return "NETFramework"
    except Exception:
        return "NETCore"


def compare_versions(a, b):
    for curr, new in zip(a, b):
        if int(curr) < int(new):
            return False
    return True


def main():
    parser = argparse.ArgumentParser("Updater for ThunderbirdTray ({}).".format("v" + ".".join(map(str, VERSION))))
    parser.add_argument("-v", "--verbose", action="store_true", default=False, help="Enable debug output.")
    parser.add_argument("-d", "--dir", default=FOLDER_INSTALL)
    parser.add_argument("-f", "--force", action="store_true", default=False, help="Will download latest regardless of current version.")
    parser.add_argument("-p", "--pre", action="store_true", default=False, help="Allow pre-releases.")
    parser.add_argument("--draft", action="store_true", default=False, help="Allow drafts." )
    args = parser.parse_args()
    logging.basicConfig(level=logging.DEBUG if args.verbose else logging.INFO, format=logging.BASIC_FORMAT if args.verbose else "%(message)s")
    log = logging.getLogger(__file__)

    install_dir = Path(args.dir).absolute()

    log.info("Starting ThunderbirdTray updater ({}), CTRL-C to abort.".format("v" + ".".join(map(str, VERSION))))

    log.info("Detecting current release...")
    install_dir_exists = Path(install_dir).is_dir()
    if not install_dir_exists:
        log.warning(f"Installation directory {install_dir} does not exist and will be created.")
    else:
        log.warning(f"Installation directory {install_dir} will be cleaned on update.")

    current_version = None
    app_file = install_dir.joinpath("ThunderbirdTray.exe")
    try:
        if app_file.is_file():
            current_version = get_version_number(app_file)[0:3]
            log.info("Detected current version as v{}.".format(".".join([str(part) for part in current_version])))
        else:
            log.warning(f"ThunderbirdTray.exe not found at {app_file}.")
    except Exception:
        log.error(f"Cound not detect current version: {traceback.format_exc()}")

    log.info("Checking for latest release...")
    try:
        releases = []
        with request.urlopen(URL_RELEASES) as response:
            releases = json.load(response)
    except Exception:
        log.error(f"Could not check for the latest release, please try again later: {traceback.format_exc()}")
        input("Press enter to exit...")
        sys.exit(1)

    valid_releases = []
    for release in releases:
        if len(release["assets"]) > 0:
            if (not release["prerelease"] or args.pre) and (not release["draft"] or args.draft):
                try:
                    modified_tag = release["tag_name"][1:].replace("-pre", "") if args.pre else release["tag_name"][1:]
                    tag_version = int(modified_tag.replace(".", ""))
                    release["tag_name"] = modified_tag
                    valid_releases.append((tag_version, release))
                except Exception:
                    pass
    valid_releases.sort(key=itemgetter(0), reverse=True)

    if len(valid_releases) == 0:
        log.error("No valid release could be found.")
        input("Press enter to exit...")
        sys.exit(1)
    else:
        latest = valid_releases[0][1]
    
    version = latest["tag_name"].split(".")
    assets = latest["assets"]

    log.info("Newest release is v{}.".format(".".join(map(str, version))))

    up_to_date = compare_versions(current_version, version) if current_version is not None else False
    
    if up_to_date:
        log.info("Up to date.")
        if not args.force:
            input("Press enter to exit...")
            sys.exit(0)
        else:
            log.info("--force set, continuing.")

    runtimes = []
    file_urls = []

    for asset in assets:
        if "ThunderbirdTray.".lower() in asset["name"].lower():
            runtimes.append(get_runtime(asset["name"]))
            file_urls.append(asset["browser_download_url"])
    
    if len(runtimes) == 0:
        log.info("No runtimes exist.")
        input("Press enter to exit...")
        sys.exit(1)

    log.info("The following runtimes are available:")
    log.info(f"    1. Auto select")
    for i, runtime in enumerate(runtimes):
        log.info(f"    {i + 2}. {runtime}")

    runtime_selection = None
    while runtime_selection is None:
        try:
            runtime_selection = int(input("Please select a runtime by entering the number: "))
            if runtime_selection < 1 or runtime_selection > len(runtimes) + 1:
                runtime_selection = None
                raise ValueError("Selection is not within the valid range.")
        except Exception:
            log.error(f"Unable to recognise input, please try again: {traceback.format_exc()}")
            input("Press enter to exit...")
            sys.exit(1)

    if runtime_selection == 1:
        auto_runtime = detect_runtime()
        if auto_runtime == "NETFramework":
            log.info("Detected NetFramework >= 4.6.")
            runtime_selection = runtimes.index("NETFramework")
        else:
            log.info("No NetFramework >= 4.6 detected, using NetCore.")
            runtime_selection = runtimes.index("NETCore-selfcontained")
    else:
        runtime_selection = runtime_selection - 2

    log.info(f"Runtime selected as {runtimes[runtime_selection]}.")

    download_url = file_urls[runtime_selection]

    log.info(f"Retrieving download from {download_url}.")
    
    with TemporaryFile() as temp_file:
        try:
            with request.urlopen(download_url) as response:
                progress = 0
                file_size = int(response.getheader("Content-Length"))
                print_progress(progress, file_size)
                while True:
                    buffer = response.read(BLOCK_SIZE)
                    progress += len(buffer)
                    if len(buffer) == 0:
                        break
                    print_progress(progress, file_size)
                    temp_file.write(buffer)
        except Exception:
            log.error(f"Could not download the latest release, please try again later: {traceback.format_exc()}")
            input("Press enter to exit...")
            sys.exit(1)
        try:
            if install_dir_exists:
                install_folder_files = os.listdir(install_dir)
                for file in install_folder_files:
                    os.remove(install_dir.joinpath(file))
            else:        
                install_dir.mkdir(parents=True, exist_ok=True)
                log.info(f"Directory {install_dir} created.")
            with ZipFile(temp_file, "r") as zip_file:
                zip_file.extractall(FOLDER_INSTALL)
        except Exception:
            log.error(f"Could not unpack downloaded file: {traceback.format_exc()}")
            input("Press enter to exit...")
            sys.exit(1)

    input("Press enter to exit...")


if __name__ == '__main__':
    main()
