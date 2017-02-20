using Performance;
using System;
using System.Threading;

namespace BackgroundTest.TestConstructor
{
    // from official readme file...

    interface IDatabase
    {
        int Get(string tableKey, string entry);
    }

    class DatabaseInterceptor : IDatabase
    {
        Func<IDatabase> interceptionCallback;
        public DatabaseInterceptor(Func<IDatabase> interceptionCallback)
        {
            this.interceptionCallback = interceptionCallback;
        }

        public int Get(string tableKey, string entry)
        {
            var instance = interceptionCallback();
            return instance.Get(tableKey, entry);
        }
    }

    class Database : IDatabase
    {
        public const int DatabaseOnlyID = 1234;
        public static TimeSpan DatabaseContructionTime { get { return TimeSpan.FromMilliseconds(200); } }

        public Database()
        {
            Thread.Sleep(DatabaseContructionTime);
        }


        public int Get(string tableKey, string entry)
        {
            return DatabaseOnlyID;
        }
    }

    class MyModel
    {
        IDatabase Database { get; set; }

        public MyModel()
        {
            Database = new Database();
        }

        public int GetID(string personName)
        {
            return Database.Get("ID", personName);
        }
    }

    class MyModelFixed
    {
        IDatabase Database { get; set; }

        public MyModelFixed()
        {
            Database = Background<IDatabase>.StartCtor(() => new Database(), () => Database, 
                cb => new DatabaseInterceptor(cb));
        }

        public int GetID(string personName)
        {
            return Database.Get("ID", personName);
        }
    }
}
