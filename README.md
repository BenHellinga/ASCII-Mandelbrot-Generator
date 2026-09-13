Fractals

This project is no longer under active development.

About

This was just something I wanted to try one weekend in 2022, printing the mandelbrot set.

Goal

Render the whole Mandelbrot set as text, at whatever resolution you want, written out to a file.

Implementation

For each point on the output grid, it maps the coordinates to a point on the complex plane, then applies the Mandelbrot iteration (z = z² + c) and checks whether it stays bounded or escapes within a fixed number of iterations.

It renders two rows of values per line using the Unicode half-block characters ( ▀▄█), doubling the vertical resolution. Width is calculated automatically from the height to match the Mandelbrot set's aspect ratio.

Usage

Run it with a single argument for the height in characters, for example:

dotnet run 100

If no argument is given, it defaults to a height of 60.

The result is written to out.txt in the project folder, meant to be viewed in an editor with word-wrap off, using a monospace font.

## Example Output

`dotnet run 300`

<img width="1188" height="1052" alt="image" src="https://github.com/user-attachments/assets/a009dad0-92bb-4b78-aa8a-3fbca09c491c" />
