using System;

namespace Conditions
{
    internal class Condition
    {
        public string Author;
        public string MenuName;
        public DateTime Time;
        public string Title;

        public Condition(string title, DateTime time, string author, string menuName)
        {
            Title = title;
            Time = time;
            Author = author;
            MenuName = menuName;
        }

        public Condition()
        {
        }
    }

    internal enum ConditionOperationType
    {
        And,
        Or
    }
}