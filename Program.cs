using System;
using System.Collections.Generic;
using System.IO;



class Mandelbrot
{
    // CONSTANTS



    // bounds of the region of the complex plane that contains the whole mandelbrot set
    public const double X_MIN = -2.5;
    public const double X_MAX = 1.0;
    public const double Y_MIN = -1.25;
    public const double Y_MAX = 1.25;

    // fewer iterations are needed than a deep zoom would require, this is just the whole set
    public const int ITERATIONS = 200;

    public const int DEFAULT_HEIGHT = 60;
    public const string OUTPUT_FILE = "out.txt";



    // VARIABLES



    public static int width;
    public static int height;



    // MAIN



    public static void Main(string[] args)
    {
        height = ParseHeight(args);

        SetDimensions();

        List<List<int>> pixels = RenderMandelbrot();
        Save(pixels, OUTPUT_FILE);

        Console.WriteLine("Finished");
    }



    // PUBLIC METHODS



    // reads the desired output height in characters from the command line, dotnet run <height>
    public static int ParseHeight(string[] args)
    {
        if (args.Length > 0 && int.TryParse(args[0], out int parsedHeight) && parsedHeight > 0)
            return parsedHeight;

        Console.WriteLine($"No valid height given, defaulting to {DEFAULT_HEIGHT} (usage, dotnet run <height>)");
        return DEFAULT_HEIGHT;
    }



    // width is derived from height so the image matches the mandelbrot set's aspect ratio, not stretched
    public static void SetDimensions()
    {
        width = CalculateWidth();
        Console.WriteLine($"Rendering at {width}x{height} characters");
    }



    // samples the mandelbrot set over the full pixel grid, two pixel rows per output character
    public static List<List<int>> RenderMandelbrot()
    {
        int heightPixels = height * 2; // doubled since 2 pixel rows get packed into each output character

        List<List<int>> pixels = [];

        for (int y = 0; y < heightPixels; y++)
        {
            pixels.Add([]);

            // the following is the formula to print the mandlebrot set
            // they are complex numbers where t_A = t_A-1 + z^2

            double zi = Y_MAX - y / (double)(heightPixels - 1) * (Y_MAX - Y_MIN); // z imaginary

            for (int x = 0; x < width; x++)
            {
                double zr = X_MIN + x / (double)(width - 1) * (X_MAX - X_MIN); // z real

                double tr = 0; // total real
                double ti = 0; // total imaginary

                for (int j = 0; j < ITERATIONS; j++)
                {
                    double ttr = tr * tr - ti * ti + zr; // temp total real
                    double tti = 2 * tr * ti + zi; // temp total imaginary

                    tr = ttr;
                    ti = tti;

                    if (tr * tr + ti * ti >= 4) break; // if more than 2 radius from origin, it will diverge to infinity, no need to continue
                }

                pixels[y].Add(tr * tr + ti * ti < 4 ? 1 : 0);
            }
        }

        return pixels;
    }



    // writes the pixel grid to a file, packing 2 vertical pixels per character using unicode half blocks
    public static void Save(List<List<int>> pixels, string outputPath)
    {
        if (pixels.Count == 0) return;

        char[] chars = " ▀▄█".ToCharArray();
        List<string> lines = [];

        for (int y = 0; y < pixels.Count - 1; y += 2)
        {
            char[] row = new char[pixels[y].Count];

            for (int x = 0; x < pixels[y].Count; x++)
                row[x] = chars[pixels[y][x] + pixels[y + 1][x] * 2];

            lines.Add(new string(row));
        }

        File.WriteAllLines(outputPath, lines);
    }



    // PRIVATE METHODS



    private static int CalculateWidth()
    {
        int heightPixels = height * 2;
        return (int)Math.Round(heightPixels * (X_MAX - X_MIN) / (Y_MAX - Y_MIN));
    }
}