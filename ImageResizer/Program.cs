using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string sourcePath = Path.Combine(Environment.CurrentDirectory, "images");
            string destinationPath = Path.Combine(Environment.CurrentDirectory, "output"); ;

            //Init 
            ImageProcess imageProcess = new ImageProcess();
            imageProcess.Clean(destinationPath);

            // Watch
            Stopwatch swOriginal = new Stopwatch();
            Stopwatch swAdvance = new Stopwatch();

            // Async
            swAdvance.Start();
            await imageProcess.ResizeImagesAsync(sourcePath, destinationPath, 2.0);
            swAdvance.Stop();

            //Without Async
            swOriginal.Start();
            imageProcess.ResizeImages(sourcePath, destinationPath, 2.0);
            swOriginal.Stop();

            Console.WriteLine($"Advance 花費時間: { swAdvance.ElapsedMilliseconds } ms");
            Console.WriteLine($"Original 花費時間: { swOriginal.ElapsedMilliseconds } ms");
            Console.WriteLine($"Improved(%): { 100 * (swOriginal.ElapsedMilliseconds - swAdvance.ElapsedMilliseconds) / swOriginal.ElapsedMilliseconds } %");
            Console.ReadKey();
        }
    }
}
