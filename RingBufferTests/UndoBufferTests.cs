using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RingBuffer;

namespace RingBufferTests
{
	public class UndoBufferTests
	{

		[Test]
		public void UndoRedoTest()
		{
			var buffer = new UndoBuffer<int>(2);
			// нельзя т.к. ещё не было действий
			Assert.That(buffer.TryGetUndo(out var undo), Is.False);  
			Assert.That(buffer.TryGetRedo(out var redo), Is.False);

			// у буфера undo вместомость (X - кол-во ячеек в массиве (базовом) + размер массива под undo)) то есть 2 на UndoBuffer
			// и создасться еще массив под Undo с 2мя ячейками, в итоге будет 4

			buffer.Add(1);
			Assert.That(buffer.UndoCount, Is.EqualTo(1));
			Assert.That(buffer.RedoCount, Is.EqualTo(0));

			buffer.Add(2);
			Assert.That(buffer.UndoCount, Is.EqualTo(2));
			Assert.That(buffer.RedoCount, Is.EqualTo(0));

			buffer.Add(3);
			Assert.That(buffer.UndoCount, Is.EqualTo(3));
			Assert.That(buffer.RedoCount, Is.EqualTo(0));

			buffer.Add(4);
			Assert.That(buffer.UndoCount, Is.EqualTo(4));
			Assert.That(buffer.RedoCount, Is.EqualTo(0));

			buffer.Add(5);
			Assert.That(buffer.UndoCount, Is.EqualTo(4));
			Assert.That(buffer.RedoCount, Is.EqualTo(0));

			// узнать как работает замещение можно в Program.cs, поменяв несколько раз входные данные

			Assert.That(buffer.TryGetUndo(out undo), Is.True);
			Assert.That(undo, Is.EqualTo(5));
			Assert.That(buffer.UndoCount, Is.EqualTo(3));
			Assert.That(buffer.RedoCount, Is.EqualTo(1));

			Assert.That(buffer.TryGetRedo(out redo), Is.True);
			Assert.That(redo, Is.EqualTo(5));
			Assert.That(buffer.UndoCount, Is.EqualTo(4));
			Assert.That(buffer.RedoCount, Is.EqualTo(0));
		}

		[Test]
		public void UndoAddTest()
		{
			var buffer = new UndoBuffer<int>(2);
			buffer.Add(1);
			buffer.Add(2);
			buffer.Add(3);

			Assert.That(buffer.TryGetUndo(out var undo), Is.True);
			buffer.Add(4);  // Если мы перематываем действия назад, и затем добавляем элемент, все элементы redo стираются
			Assert.That(buffer.UndoCount, Is.EqualTo(3));
			Assert.That(buffer.RedoCount, Is.EqualTo(0));

			Assert.That(buffer.TryGetUndo(out undo), Is.True);
			Assert.That(undo, Is.EqualTo(4));

			Assert.That(buffer.TryGetUndo(out undo), Is.True);
			Assert.That(undo, Is.EqualTo(2));
			Assert.That(buffer.UndoCount, Is.EqualTo(1));
			Assert.That(buffer.RedoCount, Is.EqualTo(2));

			buffer.Add(5);
			Assert.That(buffer.UndoCount, Is.EqualTo(2));
			Assert.That(buffer.RedoCount, Is.EqualTo(0));
		}

		[Test]
		public void BoundTest()
		{
			var buffer = new UndoBuffer<int>(2);
			buffer.Add(1);
			buffer.Add(2);
			buffer.Add(3);
			Assert.That(buffer.UndoCount, Is.EqualTo(3));
			Assert.That(buffer.RedoCount, Is.EqualTo(0));

			Assert.That(buffer.TryGetUndo(out var undo), Is.True);
			Assert.That(buffer.UndoCount, Is.EqualTo(2));
			Assert.That(buffer.RedoCount, Is.EqualTo(1));

			Assert.That(buffer.TryGetUndo(out undo), Is.True);
			Assert.That(buffer.UndoCount, Is.EqualTo(1));
			Assert.That(buffer.RedoCount, Is.EqualTo(2));

			Assert.That(buffer.TryGetUndo(out undo), Is.True);
			Assert.That(buffer.UndoCount, Is.EqualTo(0));
			Assert.That(buffer.RedoCount, Is.EqualTo(2));

			buffer.Add(4);

			Assert.That(buffer.TryGetRedo(out var redo), Is.False);
		}

	}
}
