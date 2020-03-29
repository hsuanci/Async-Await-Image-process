using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageResizer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string sourcePath = Path.Combine(Environment.CurrentDirectory, "images");
            string destinationPath = Path.Combine(Environment.CurrentDirectory, "output"); ;
            CancellationTokenSource cts = new CancellationTokenSource();

            #region 等候使用者輸入 取消 c 按鍵
            ThreadPool.QueueUserWorkItem(x =>
            {
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.C)
                {
                    cts.Cancel();
                }
            });
            #endregion

            //Init 
            ImageProcess imageProcess = new ImageProcess();
            imageProcess.Clean(destinationPath);

            // Watch
            Stopwatch swOriginal = new Stopwatch();
            Stopwatch swAdvance = new Stopwatch();

            // Async
            try
            {
                swAdvance.Start();
                await imageProcess.ResizeImagesAsync(sourcePath, destinationPath, 2.0, cts.Token);
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
            catch
            {
                Console.WriteLine($"{Environment.NewLine} 處理已經取消");
                Console.ReadKey();
            }
        }
    }
}
