# Breath Control for Space Engineers
# Space Engineers Client Plugin Template

See base template and requirements to build here:
[Server/Client version of the template](https://github.com/sepluginloader/PluginTemplate)

## Description

The heavy breathing noise when using realistic sound in Space Engineers is very loud. It also plays VERY often. For a long duration.
I hate it. I am sure you do too, so I slapped this together to make it stop.

## Installation

At some point this should be on plugin hub. Until then, you can install this plugin by dragging the associated DLL to:
YourSteamLibraryLocation\common\SpaceEngineers\Bin64\Plugins\Local

![File location](where_to_put.png)

### Info

This plugin can be adjusted in-game through the plugin menu when you press escape.
![Config screen](config_screen.png)
![Plugin Menu](menu_preview.png)

### Release

- Always make your final release from a RELEASE build. (More optimized, removes debug code.)
- Always test your RELEASE build before publishing. Sometimes is behaves differently.
- In case of client plugins the Plugin Loader compiles your code, watch out for differences.

### Communication

- In your documentation always include how players or server admins should report bugs.
- Try to be reachable and respond on a timely manner over your communication channels.
- Be open for constructive critics.

### Abandoning your project

- Always consider finding a new maintainer, ask around at least once.
- If you ever abandon the project, then make it clear on its GitHub page.
- Abandoned projects should be made hidden on PluginHub and Torch's plugin list.
- Keep the code available on GitHub, so it can be forked and continued by others.
