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
            // ListLab.LabInsteadBPushPopTest(con, db);

            // lab 6 Hash
            // HashLab.LabSetGet(db);

            // lab 7 Set
            // SetLab.SetMembers(db);

            // lab 8 HyperLogLogs
            // HyperLogLab.LabCount(db, 10000);

            // lab 9 SortedSet
            // SortedSetLab.LabSetGet(db, 1000);

            // lab 10 PubSub
            // PubSubLab.LabChannel(con);

            // lab 11 Tran/Batch
            // TranscationLab.LabTanscation(db);

            // lab 12 Scan
            // ScanLab.LabScan(db, con);

            // lab 13 Lua
            LuaLab.LabLua(db, con);

            Console.ReadKey();
        }
    }
}
