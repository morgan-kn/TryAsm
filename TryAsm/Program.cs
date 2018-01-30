using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace TryAsm
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Benchmark(); // execute the benchmark do method gets jitted

            Console.WriteLine($"{Process.GetCurrentProcess().Id} " + // process Id
                              $"{typeof(Program).FullName} " + // full type name
                              $"{nameof(Benchmark)} " + // benchmarked method name
                              $"{bool.TrueString} " + // printAsm
                              $"{bool.FalseString} " + // printIL
                              $"{bool.FalseString} " + // print Source
                              $"{bool.FalseString} " + // print prolog and epilog
                              "2 " + // recursive depth
                              $"{Path.GetTempFileName()}.xml"); // result xml file path

            while(true)
            {
                Console.WriteLine("Press Ctrl+C to kill the process");
                Console.ReadLine(); // block the exe, attach with Disassembler now
            }

            GC.KeepAlive(result);
        }

        private static IntPtr Benchmark()
        {
            return new IntPtr(42).Multiply(4);
        }
    }

    public static class IntPtrHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe IntPtr Multiply(this IntPtr a, int factor)
        {
            return (sizeof(IntPtr) == sizeof(int))
                ? new IntPtr((int)a * factor)
                : new IntPtr((long)a * factor);
        }
    }
}