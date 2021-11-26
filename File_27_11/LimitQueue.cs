using System.Collections;

namespace File_27_11
{
    internal class LimitQueue<T> : Queue
    {
        public override void Enqueue(object obj)
        {
            base.Enqueue(obj); // делаем что делает родитель
            if (Count == 4)
            {
                Dequeue(); //удаляет первый элемент
            }
        }
    }
}