using System.Collections;

namespace BuscadorIndiceInvertido.Utilidades
{
    public class DoubleList<T> : ICollection<T>
    {
        public Node<T> Head;
        public Node<T> Tail;
        public int count;

        public DoubleList()
        {
            Head = null;
            Tail = null;
            count = 0;
        }

        public DoubleList(T[] arr)
        {
            if (arr == null) throw new ArgumentNullException(nameof(arr));

            foreach (T item in arr)
            {
                Add(item);
            }
        }

        public int Count
        {
            get
            {
                return count;
            }
        }

        public bool IsReadOnly => false;

        public void Add(T data)
        {
            Node<T> newNode = new Node<T>(data, null, null);

            if (Head == null)
            {
                Head = Tail = newNode;
                Head.Next = Head;
                Head.Prev = Head;
            }
            else
            {
                newNode.Prev = Tail;
                newNode.Next = Head;
                Tail.Next = newNode;
                Tail = newNode;
                Head.Prev = newNode;
            }

            count++;
        }

        public void Clear()
        {
            if (Head == null) return;

            Node<T> current = Head;
            Node<T> next;

            do
            {
                next = current.Next;

                current.Next = null;
                current.Prev = null;

                current = next;
            } while (current != Head);

            Head = null;
            Tail = null;
            count = 0;
        }

        public bool Contains(T data)
        {
            if (Head == null) return false;

            Node<T> current = Head;

            do
            {
                if (current.Data.Equals(data)) return true;
                current = current.Next;
            } while (current != Head);

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (array.Length - arrayIndex < count) throw new ArgumentException("El tamaño del array no es suficiente.");

            if (Head == null) return;

            Node<T> current = Head;
            int i = arrayIndex;

            do
            {
                array[i++] = current.Data;
                current = current.Next;
            } while (current != Head);
        }

        public bool Remove(T data)
        {
            if (Head == null) return false;

            Node<T> current = Head;

            do
            {
                if (current.Data.Equals(data))
                {
                    if (current == Head && current == Tail)
                    {
                        Head = Tail = null;
                    }
                    else if (current == Head)
                    {
                        Head = Head.Next;
                        Head.Prev = Tail;
                        Tail.Next = Head;
                    }
                    else if (current == Tail)
                    {
                        Tail = Tail.Prev;
                        Tail.Next = Head;
                        Head.Prev = Tail;
                    }
                    else
                    {
                        current.Prev.Next = current.Next;
                        current.Next.Prev = current.Prev;
                    }

                    current.Next = null;
                    current.Prev = null;
                    count--;
                    return true;
                }

                current = current.Next;
            } while (current != Head);

            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (Head == null) yield break;

            Node<T> current = Head;

            do
            {
                yield return current.Data;
                current = current.Next;
            } while (current != Head);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class Node<T>
    {
        public T Data { get; set; }
        public Node<T> Next { get; set; }
        public Node<T> Prev { get; set; }

        public Node(T data, Node<T> next, Node<T> prev)
        {
            Data = data;
            Next = next;
            Prev = prev;
        }
    }
}