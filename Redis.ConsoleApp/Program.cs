using System;
using Redis.Extension;
using StackExchange.Redis;

namespace Redis.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //RedisConnectionPool.InitializeConnectionPool($"{ConfigHelper.host},password={ConfigHelper.password}");
            //ConnectionMultiplexer con = RedisConnectionPool.GetConnection();
            //RedisConnectionPool.PushConnection(con);
            ConnectionMultiplexer con = ConnectionMultiplexer.Connect($"{ConfigHelper.host},password={ConfigHelper.password},allowAdmin=true");
            con.GetServer(ConfigHelper.host).FlushDatabase(1);
            var db = con.GetDatabase(1);
            // lab 1 限流
            // StringLab.LabIncrement(db);

            // lab 2 占位
            // StringLab.LabIsExists(db);

            // lab 3 黑名单
            // StringLab.LabBit(db);

            // lab 4 队列
            // ListLab.LabPushPop(db);

            // lab 5 等待队列
            //ListLab.LabPushPopWait(db);

            // test 1 多队列测试
            //ListLab.LabPushPopTest(db);

            // test 2 代替阻塞队列实现
            // ListLab.LabInsteadPushPopTest(con, db);

            // lab 6 Hash
            // HashLab.LabSetGet(db);

            // lab 7 set
            SetLab.SetMembers(db);

            Console.ReadKey();
        }
    }
}
