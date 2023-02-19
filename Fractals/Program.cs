using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;

/*
 * Just a quick program to print fractals cuz I was bored
 */


int MAXWIDTH = 206;
int MAXHEIGHT = 61;
int MAXHEIGHT2 = MAXHEIGHT * 2;

// ==================================================

Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);

//printPascalsTriangle(101);
//rintMandlebrot(-0.5, 0, 1, 1000);

mandlebrotZoom(-0.550015, 0.627015, 1000, 1.5);

// ==================================================



// this just zooms into the mandel brought set more every time you press enter
void mandlebrotZoom(double x, double y, int iterations, double zoom)
{
    int i = 1;
    while (true)
    {
        printMandlebrot(x, y, Math.Pow(zoom, i), iterations);
        Console.ReadLine();
        i++;
    }
}

// this improves the accuracy every time you press enter
void mandlebrotIterateIterations()
{
    int i = 1;
    while (true)
    {
        printMandlebrot(-0.5, 0, 0.5, i);
        Console.ReadLine();
        i++;
    }

    return;
}

// this print the mandlebrot set
void printMandlebrot(double X, double Y, double zoom, int interations)
{
    List<List<int>> array = new List<List<int>>();

    double tr;
    double ti;
    double zr;
    double zi;

    double ttr;
    double tti;

    for (int y = 0; y < MAXHEIGHT2; y++)
    {
        array.Add(new List<int>());

        for (int x = 0; x < MAXWIDTH; x++)
        {
            // the following is the formula to print the mandlebrot set
            // they are complex numbers where t_A = t_A-1 + z^2

            tr = 0; // total real
            ti = 0; // total imaginary

            zr = X + (x / (double)MAXWIDTH / zoom * 2 - 1 / zoom);   // z real
            zi = Y - (y / (double)MAXHEIGHT2 / zoom * 2 - 1 / zoom); // z imaginary

            for (int j = 0; j < interations; j++)
            {
                ttr = tr * tr - ti * ti + zr; // temp total real
                tti = 2 * tr * ti + zi; // temp total imaginary

                tr = ttr;
                ti = tti;

                if (tr * tr + ti * ti >= 4) break; // if more than 2 radius from origin, it will diverge to infinity. no need to continue
            }

            if (tr * tr + ti * ti < 4) array[y].Add(0);
            else array[y].Add(1);
        }
    }

    printArray(array);
    return;
}

// prints pascalls triangle
void printPascalsTriangle(int height)
{
    List<int> currentRow = new List<int>();
    List<int> lastRow = new List<int>();

    for (int j = 0; j < height; j++) Console.Write(" ");

    currentRow.Add(1);
    Console.WriteLine("██");

    for (int i = 1; i < height; i++)
    {
        lastRow = currentRow;
        currentRow = new List<int>();

        for (int j = 0; j < height - i; j++) Console.Write(" ");
        
        currentRow.Add(1);
        Console.Write("██");

        for (int j = 1; j < i; j++)
        {
            currentRow.Add((lastRow[j - 1] + lastRow[j]) % 2);
            Console.Write(currentRow[j] == 0 ? "  " : "██");
        }

        currentRow.Add(1);
        Console.WriteLine("██");
    }


    return;
}


// this uses special unicode characters to quadruple the output resoluation
// normal characters are 2 units high and 1 unit wide, so a square grid
// means each 'tile' is 2 characters, for a 2x2 unit tile
//
// using special unicode characters ' ▀▄█', you can effectively split each character into 2 tiles,
// so 2 tiles into 4 tiles instead of 1
void printArray(List<List<int>> array)
{
    if (array.Count == 0) return;

    char[] chars = " ▀▄█".ToCharArray();

    for (int y = 0; y < array.Count - 1; y += 2)
    {
        for (int x = 0; x < array[y].Count; x++)
        {
            Console.Write(chars[array[y][x] + array[y + 1][x] * 2]);
        }
        Console.WriteLine();
    }

    return;
}