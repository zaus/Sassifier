Sassifier
=========

Leverages the [SassAndCoffee library]("https://github.com/xpaulbettsx/SassAndCoffee") for a standalone executable, which can be used as a 3rd-party compiler.

See also [blog post]("http://drzaus.com/snippet/standalone-scss-compiler-for-sass-in-sublime-text-2").

Purpose
=======

To use in [Sublime Text editor]("http://www.sublimetext.com/") with SASS Build System.  Here because I found someone else who asked for it - see [original issue from source]("https://github.com/xpaulbettsx/SassAndCoffee/issues/44").


Usage
=====

By Itself
---------

    sassifier.exe "path\to\css.css" true|false dependencies;separated;by;semicolons

Where `true|false` is whether or not to compress the output, and whatever dependencies it requires _(honestly, not really sure what that is)_ separated by semicolons.

With Sublime Text 2
-------------------

Put the whole shebang (i.e. compiled files) in your Packages folder under `SASS/Sassifier`.

Create a new Build System:

    {
        "cmd": ["Sassifier.exe", "$file"],
        "working_dir": "$packages/SASS/Sassifier",
        "selector": "source.sass"
    }

Save as `\Your Sublime Folder\Data\Packages\Sass\SASS (Windows).sublime-build`.

Best when used in conjunction with a [SASS syntax highlighter]("https://github.com/nathos/sass-textmate-bundle").