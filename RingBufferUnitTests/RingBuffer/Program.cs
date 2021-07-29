using System;

namespace RingBuffer
{
    class Program
    {
        static void Main(string[] args)
        {
            UndoBuffer<int> buffer = new UndoBuffer<int>(2, 2);


            int tmp = 0;
            //buffer.GetRedo(out tmp);
            //buffer.GetUndo(out tmp);

            buffer.Add(1);
            buffer.Add(2);
            buffer.Add(3);

            Console.WriteLine();
            bool result = buffer.TryGetUndo(out tmp);
            Console.WriteLine("Undo 1: " + result + " tmp: " + tmp);

            buffer.Add(4);


            Console.WriteLine();
            result = buffer.TryGetUndo(out tmp);
            Console.WriteLine("Undo 2: "+result+" tmp: "+tmp);

            Console.WriteLine();
            result = buffer.TryGetUndo(out tmp);
            Console.WriteLine("Result 1: " + result + " tmp: " + tmp);
            buffer.Add(5);
            //Console.WriteLine();
            //buffer.TryGetRedo(out tmp);
            //buffer.TryGetRedo(out tmp);
            //Console.WriteLine($"Buffer.RedoCount: {buffer.RedoCount()}");
            //buffer.TryGetRedo(out tmp);
            //Console.WriteLine($"Buffer.RedoCount: {buffer.RedoCount()}");
            //buffer.TryGetRedo(out tmp);
            //buffer.TryGetRedo(out tmp);
            //buffer.TryGetRedo(out tmp);
            //buffer.TryGetRedo(out tmp);
            //Console.WriteLine($"Buffer.RedoCount: {buffer.RedoCount()}");
            //buffer.TryGetRedo(out tmp);
            //buffer.TryGetRedo(out tmp);
            //
            //Console.ReadLine();
        }
    }
}
