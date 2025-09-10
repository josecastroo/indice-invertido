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
            Node<T> nuevo = new Node<T>(data, null, null);

            if (Head == null)
            {
                Head = Tail = nuevo;
                Head.SetNext(Head);
                Head.SetPrev(Head);
            }
            else
            {
                nuevo.SetPrev(Tail);
                nuevo.SetNext(Head);
                Tail.SetNext(nuevo);
                Tail = nuevo;
                Head.SetPrev(nuevo);
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
                next = current.GetNext();

                current.SetNext(null);
                current.SetPrev(null);

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
                if (current.GetData().Equals(data)) return true;
                current = current.GetNext();
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
                array[i++] = current.GetData();
                current = current.GetNext();
            } while (current != Head);
        }

        public bool Remove(T data)
        {
            if (Head == null) return false;

            Node<T> current = Head;

            do
            {
                if (current.GetData().Equals(data))
                {
                    if (current == Head && current == Tail)
                    {
                        Head = Tail = null;
                    }
                    else if (current == Head)
                    {
                        Head = Head.GetNext();
                        Head.SetPrev(Tail);
                        Tail.SetNext(Head);
                    }
                    else if (current == Tail)
                    {
                        Tail = Tail.GetPrev();
                        Tail.SetNext(Head);
                        Head.SetPrev(Tail);
                    }
                    else
                    {
                        current.GetPrev().SetNext(current.GetNext());
                        current.GetNext().SetPrev(current.GetPrev());
                    }

                    current.SetNext(null);
                    current.SetPrev(null);
                    count--;
                    return true;
                }

                current = current.GetNext();
            } while (current != Head);

            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (Head == null) yield break;

            Node<T> current = Head;

            do
            {
                yield return current.GetData();
                current = current.GetNext();
            } while (current != Head);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class Node<T>
    {
        public T data;
        public Node<T> next;
        public Node<T> prev;

        public Node(T data, Node<T> next, Node<T> prev)
        {
            this.data = data;
            this.next = next;
            this.prev = prev;
        }
        public T GetData()
        {
            return data;
        }

        public void SetData(T value)
        {
            data = value;
        }

        public Node<T> GetNext()
        {
            return next;
        }

        public void SetNext(Node<T> value)
        {
            next = value;
        }

        public Node<T> GetPrev()
        {
            return prev;
        }

        public void SetPrev(Node<T> value)
        {
            prev = value;
        }
        
        public void Disconnect()
        {
            if (prev != null)
            {
                prev.next = next;
            }
        
            if (next != null)
            {
                next.prev = prev;
            }
        
            prev = null;
            next = null;
        }
    }
}