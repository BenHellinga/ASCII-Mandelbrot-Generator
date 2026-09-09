# ASCII Mandelbrot Generator

**This project is no longer under active development.**

## About

This was just something I wanted to try one weekend in 2022, generating an ASCII mandelbrot set.

## Goal

Render the whole Mandelbrot set as text, at whatever resolution you want, written out to a file so it isn't limited to console dimensions.

## Implementation

For each point on the output grid, the program maps its coordinates to a point on the complex plane (fixed to the region that contains the whole Mandelbrot set), then repeatedly applies the Mandelbrot iteration (z = z² + c) to that point, counting whether it stays bounded or escapes to infinity within a fixed number of iterations. That gives a black-or-white value per point.

To get more than a flat grid of blocky pixels out of plain text, it renders two rows of values per line of output using the Unicode half-block characters (` ▀▄█`), effectively doubling the vertical resolution compared to just printing one character per pixel. The width is calculated automatically from the height so the output isn't stretched, since the set spans a wider range on the real axis than the imaginary axis.

## Usage

Run it with a single argument for the height in characters, for example:

```
dotnet run 100
```

Width is calculated automatically to match the Mandelbrot set's aspect ratio. If no argument is given, it defaults to a height of 60.

The result is written to `out.txt` in the project folder, meant to be viewed in an editor with word-wrap off, using a monospace font.
