using System;
using System.Collections.Generic;
using System.Text;

namespace RingBuffer
{
	public class UndoBuffer<T>
	{
		public int UndoCount()
		{
			return _undo.prevItemsCount + count;
		}
		public int RedoCount()
		{
			return _redo.forwardItemsCount;
		}

		int undo;
		int redo;

		Undo<T> _undo;
		Redo<T> _redo;

		int capacity;
		int count;

		public T[] actualBuffer;
		int spaceForNewObj;

		// enter capacity for mainArray, and capacity for undo and redo arrays
		// capacity of Undo: it stores at least as many Undo values, as there are cells in base array, capacity parameter adds additional in a form of array
		// capacity if Redo: it stores values only in additional array
		public UndoBuffer(int capacity, int capacityUndoRedo = 100)
		{
			_undo = new Undo<T>(capacity);
			//_undo = new Undo<T>(capacityUndoRedo);
			//_redo = new Redo<T>(capacityUndoRedo);
			_redo = new Redo<T>(capacity);

			this.capacity = capacity;
			actualBuffer = new T[capacity];
			count = 0;
			spaceForNewObj = 0;
		}

		public void Add(T item)
		{
			if (count < capacity)
			{
				actualBuffer[spaceForNewObj] = item;
				spaceForNewObj++;
				count++;
			}
			else if (count >= capacity)
			{
				for (int i = 0; i < actualBuffer.Length - 1; i++)
				{
					if (i == 0)
					{
						_undo.AddToUndo(actualBuffer[i]);
					}

					actualBuffer[i] = actualBuffer[i + 1];
				}
				actualBuffer[actualBuffer.Length - 1] = item;
			}

			_redo.ForgetForwardItems();
			WriteBufferState();

		}
		public bool TryGetRedo(out T item)
		{
			item = default;
			if (_redo.forwardItemsCount > 0)
			{
				if (count < capacity)
				{
					item = _redo.GetRedoItem();
					actualBuffer[spaceForNewObj] = item;

					spaceForNewObj++;
					count++;
				}
				else if (count >= capacity)
				{
					for (int i = 0; i <= actualBuffer.Length - 1; i++)
					{
						if (i == 0)
						{
							_undo.AddToUndo(actualBuffer[i]);
						}

						if (i == actualBuffer.Length - 1)
						{
							item = _redo.GetRedoItem();

							actualBuffer[i] = item;
						}
						else
						{
							actualBuffer[i] = actualBuffer[i + 1];
						}
					}
				}
				WriteBufferState();
			}
			else
			{
				item = default;
				Console.WriteLine("Can't do Redo when there's nowhere to redo");
			}
			if (!item.Equals(default)) return false;
			else return false;
		}

		public bool TryGetUndo(out T item)
		{
			if (count > 0)
			{
				item = default;
				if (_undo.prevItemsCount > 0)
				{

					for (int i = actualBuffer.Length - 1; i >= 0; i--)
					{
						if (i == 0)
						{
							actualBuffer[i] = _undo.GetUndoItem();

							break;
						}
						else if (i == actualBuffer.Length - 1)
						{
							item = actualBuffer[i];
							_redo.AddToRedo(item);
							actualBuffer[i] = actualBuffer[i - 1];
						}
						else
						{
							actualBuffer[i] = actualBuffer[i - 1];
						}
					}

				}
				else
				{
					Console.WriteLine("Can't do Undo when there's nowhere to undo");

					item = default;
				}

				WriteBufferState();
			}
			else
			{
				item = default;
				Console.WriteLine("Can't do Undo when there's nowhere to undo");
			}
			if (!item.Equals(default)) return false;
			else return false;
		}

		public void WriteBufferState()
		{
			for (int i = 0; i < actualBuffer.Length; i++)
			{
				Console.Write($"[{actualBuffer[i].ToString()}]");
			}
			Console.WriteLine();
		}


	}
	public class Undo<T>
	{
		public T[] previousItems;

		public int prevItemsCount;


		int itemIndex;

		int defaultSize;
		public Undo(int defaultSize = 100)
		{
			this.defaultSize = defaultSize;
			previousItems = new T[defaultSize];

			prevItemsCount = 0;
			itemIndex = 0;
		}

		public void AddToUndo(T item)
		{
			if (itemIndex < previousItems.Length)
			{
				previousItems[itemIndex] = item;
				itemIndex++;

				prevItemsCount++;
			}
			else  // if there's not enough space in the undo array, we simply forget oldest items
			{
				Console.WriteLine("Not enough memory in Undo");
				for (int i = 0; i < defaultSize; i++)
				{
					if (i == defaultSize - 1)
					{
						previousItems[i] = item;
					}
					else
					{
						previousItems[i] = previousItems[i + 1];
					}
				}
			}
		}

		public T GetUndoItem()
		{
			if (prevItemsCount > 0)
			{
				T tmp = previousItems[itemIndex - 1];
				previousItems[itemIndex - 1] = default;
				itemIndex--;
				prevItemsCount -= 1;

				return tmp;
			}
			else
			{
				Console.WriteLine("No items in Undo, there's no way to get back in actions");
				return default;
			}
		}
	}

	public class Redo<T>
	{
		public T[] forwardItems;

		public int forwardItemsCount;


		int itemIndex;
		int defaultSize;
		public Redo(int defaultSize = 100)
		{
			this.defaultSize = defaultSize;
			forwardItems = new T[defaultSize];

			forwardItemsCount = 0;
			itemIndex = 0;
		}

		public void AddToRedo(T item)
		{
			if (itemIndex < forwardItems.Length)
			{
				forwardItems[itemIndex] = item;
				itemIndex++;

				forwardItemsCount++;
			}
			else  // if there's not enough space in the redo array, we simply forget oldest items
			{
				for (int i = 0; i < defaultSize; i++)
				{
					if (i == defaultSize - 1)
					{
						forwardItems[i] = item;
					}
					else
					{
						forwardItems[i] = forwardItems[i + 1];
					}
				}
			}
		}

		public T GetRedoItem()
		{
			if (forwardItemsCount > 0)
			{
				T tmp = forwardItems[itemIndex - 1];
				forwardItems[itemIndex - 1] = default;
				itemIndex--;
				forwardItemsCount -= 1;

				return tmp;
			}
			else
			{
				Console.WriteLine("No items in Redo, there's no way to get forward in actions");
				return default;
			}
		}

		public void ForgetForwardItems()
		{
			if (forwardItemsCount > 0)
			{
				forwardItems = new T[defaultSize];

				forwardItemsCount = 0;
				itemIndex = 0;

				Console.WriteLine("Wiped out forward items because added new item to base array");
			}
		}
	}
}
