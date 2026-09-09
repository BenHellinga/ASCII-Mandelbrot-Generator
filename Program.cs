// renders the whole mandelbrot set and writes it to out.txt, resolution set via command line arg

// bounds of the region of the complex plane that contains the whole mandelbrot set
const double X_MIN = -2.5;
const double X_MAX = 1.0;
const double Y_MIN = -1.25;
const double Y_MAX = 1.25;

// fewer iterations are needed than a deep zoom would require, this is just the whole set
const int ITERATIONS = 200;

const int DEFAULT_HEIGHT = 60;



int height = DEFAULT_HEIGHT;

if (args.Length > 0 && int.TryParse(args[0], out int parsedHeight) && parsedHeight > 0)
    height = parsedHeight;
else
    Console.WriteLine($"No valid height given, defaulting to {DEFAULT_HEIGHT} (usage, dotnet run <height>)");

int heightPixels = height * 2; // doubled since 2 pixel rows get packed into each output character

// width is derived from height so the image matches the mandelbrot set's aspect ratio, not stretched
int width = (int)Math.Round(heightPixels * (X_MAX - X_MIN) / (Y_MAX - Y_MIN));

Console.WriteLine($"Rendering at {width}x{height}");

List<List<int>> array = renderMandelbrot(width, heightPixels);
writeToFile(array, "out.txt");

Console.WriteLine("Finished");



// samples the mandelbrot set over the full pixel grid
List<List<int>> renderMandelbrot(int width, int height)
{
    List<List<int>> array = [];

    for (int y = 0; y < height; y++)
    {
        array.Add([]);

        // the following is the formula to print the mandlebrot set
        // they are complex numbers where t_A = t_A-1 + z^2

        double zi = Y_MAX - y / (double)(height - 1) * (Y_MAX - Y_MIN); // z imaginary

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

            array[y].Add(tr * tr + ti * ti < 4 ? 1 : 0);
        }
    }

    return array;
}



// writes the pixel grid to a file, packing 2 vertical pixels per character using unicode half blocks
void writeToFile(List<List<int>> array, string filename)
{
    if (array.Count == 0) return;

    char[] chars = " ▀▄█".ToCharArray();
    List<string> lines = [];

    for (int y = 0; y < array.Count - 1; y += 2)
    {
        char[] row = new char[array[y].Count];

        for (int x = 0; x < array[y].Count; x++)
            row[x] = chars[array[y][x] + array[y + 1][x] * 2];

        lines.Add(new string(row));
    }

    File.WriteAllLines(filename, lines);
}
