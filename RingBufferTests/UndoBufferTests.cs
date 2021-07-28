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
			buffer.Add(1);
			Assert.That(buffer.UndoCount, Is.EqualTo(1));
			Assert.That(buffer.RedoCount, Is.EqualTo(0));

			buffer.Add(2);
			Assert.That(buffer.UndoCount, Is.EqualTo(2));
			Assert.That(buffer.RedoCount, Is.EqualTo(0));

			buffer.Add(3);
			Assert.That(buffer.UndoCount, Is.EqualTo(2));
			Assert.That(buffer.RedoCount, Is.EqualTo(0));

			Assert.That(buffer.TryGetUndo(out var undo), Is.True);
			Assert.That(undo, Is.EqualTo(3));
			Assert.That(buffer.UndoCount, Is.EqualTo(1));
			Assert.That(buffer.RedoCount, Is.EqualTo(1));

			Assert.That(buffer.TryGetRedo(out var redo), Is.True);
			Assert.That(redo, Is.EqualTo(3));
			Assert.That(buffer.UndoCount, Is.EqualTo(2));
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
			buffer.Add(4);
			Assert.That(buffer.UndoCount, Is.EqualTo(2));
			Assert.That(buffer.RedoCount, Is.EqualTo(0));

			Assert.That(buffer.TryGetUndo(out undo), Is.True);
			Assert.That(undo, Is.EqualTo(4));

			Assert.That(buffer.TryGetUndo(out undo), Is.True);
			Assert.That(undo, Is.EqualTo(2));
			Assert.That(buffer.UndoCount, Is.EqualTo(0));
			Assert.That(buffer.RedoCount, Is.EqualTo(2));

			buffer.Add(5);
			Assert.That(buffer.UndoCount, Is.EqualTo(1));
			Assert.That(buffer.RedoCount, Is.EqualTo(0));
		}

		[Test]
		public void BoundTest()
		{
			var buffer = new UndoBuffer<int>(2);
			buffer.Add(1);
			buffer.Add(2);
			buffer.Add(3);
			Assert.That(buffer.UndoCount, Is.EqualTo(2));
			Assert.That(buffer.RedoCount, Is.EqualTo(0));

			Assert.That(buffer.TryGetUndo(out var undo), Is.True);
			Assert.That(buffer.UndoCount, Is.EqualTo(1));
			Assert.That(buffer.RedoCount, Is.EqualTo(1));

			Assert.That(buffer.TryGetUndo(out undo), Is.True);
			Assert.That(buffer.UndoCount, Is.EqualTo(0));
			Assert.That(buffer.RedoCount, Is.EqualTo(2));

			Assert.That(buffer.TryGetUndo(out undo), Is.False);
			Assert.That(buffer.UndoCount, Is.EqualTo(0));
			Assert.That(buffer.RedoCount, Is.EqualTo(2));

			buffer.Add(4);

			Assert.That(buffer.TryGetRedo(out var redo), Is.False);
		}

	}
}
