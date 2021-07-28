using System;

namespace RingBuffer
{
    class Program
    {
        static void Main(string[] args)
        {
            UndoBuffer<int> buffer = new UndoBuffer<int>(3, 3);


            int tmp = 0;
            //buffer.GetRedo(out tmp);
            //buffer.GetUndo(out tmp);

            buffer.Add(1);
            buffer.Add(2);
            Console.WriteLine($"Buffer.UndoCount: {buffer.UndoCount()}");
            buffer.Add(3);
            Console.WriteLine($"Buffer.UndoCount: {buffer.UndoCount()}");
            buffer.Add(4);
            buffer.Add(5);
            buffer.Add(6);
            buffer.Add(7);
            Console.WriteLine($"Buffer.UndoCount: {buffer.UndoCount()}");
            buffer.Add(8);
            buffer.Add(9);
            buffer.Add(10);
            buffer.Add(11);
            buffer.Add(12);

            Console.WriteLine($"Buffer.UndoCount: {buffer.UndoCount()}");


            Console.WriteLine();
            buffer.TryGetUndo(out tmp);
            buffer.TryGetUndo(out tmp);
            buffer.TryGetUndo(out tmp);
            buffer.TryGetUndo(out tmp);
            buffer.TryGetUndo(out tmp);
            buffer.TryGetUndo(out tmp);

            Console.WriteLine($"Buffer.RedoCount: {buffer.RedoCount()}");

            Console.WriteLine();
            buffer.TryGetRedo(out tmp);
            buffer.TryGetRedo(out tmp);
            Console.WriteLine($"Buffer.RedoCount: {buffer.RedoCount()}");
            buffer.TryGetRedo(out tmp);
            Console.WriteLine($"Buffer.RedoCount: {buffer.RedoCount()}");
            buffer.TryGetRedo(out tmp);
            buffer.TryGetRedo(out tmp);
            buffer.TryGetRedo(out tmp);
            buffer.TryGetRedo(out tmp);
            Console.WriteLine($"Buffer.RedoCount: {buffer.RedoCount()}");
            buffer.TryGetRedo(out tmp);
            buffer.TryGetRedo(out tmp);

            Console.ReadLine();
        }
    }
}
