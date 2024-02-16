# Background

This service accepts the URL to an image and returns structured information about that image

# Caveats

This currently requires Windows as the phash library requires Bitmap from System.Common.Drawing, which I don't believe is cross-platform.
The SkiaSharp package provides a 2D drawing solution with cross-platform bindings. It may be possible to refactor the phash library to
work cross-platform with SkiaSharp (or I could be wrong and Bitmap has been ported to other platforms).

# Running

Run the project and paste a URL such as this one into your browser

https://localhost:7036/imageinfo?url=https%3A%2F%2Fcdn.mos.cms.futurecdn.net%2Fx8gPS2xw5WndVmciJvDPpR-650-80.jpg.webp


