using System;
using StackExchange.Redis;
using System.Collections.Generic;

namespace Redis.ConsoleApp
{
    public class RedisConnectionPool
    {
        private static Queue<ConnectionMultiplexer> _connectionPoolQueue;

        private static int _minConnections;
        private static int _maxConnections;

        private static string _configuration;

        private static bool _initialized = false;

        public static void InitializeConnectionPool(string configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _connectionPoolQueue = new Queue<ConnectionMultiplexer>();

            _minConnections = 10;
            _maxConnections = 100;

            for (int i = 0; i < _minConnections; i++)
            {
                _connectionPoolQueue.Enqueue(OpenConnection());
            }

            _initialized = true;
        }

        public static ConnectionMultiplexer GetConnection()
        {
            if (!_initialized)
                throw new InvalidOperationException(nameof(RedisConnectionPool) + "未初始化。");
            lock (_connectionPoolQueue)
            {
                while (_connectionPoolQueue.Count > 0)
                {
                    var client = _connectionPoolQueue.Dequeue();

                    if (!client.IsConnected)
                    {
                        client.Close();
                        continue;
                    }
                    return client;
                }
            }
            return OpenConnection();
        }

        private static ConnectionMultiplexer OpenConnection()
        {
            return ConnectionMultiplexer.Connect(_configuration);
        }

        public static void PushConnection(ConnectionMultiplexer connection)
        {
            if (!_initialized)
                throw new InvalidOperationException(nameof(RedisConnectionPool) + "未初始化。");
            if (!connection.IsConnected)
            {
                connection.Close();
                return;
            }
            if (_connectionPoolQueue.Count >= _maxConnections)
            {
                connection.Close();
            }
            else
            {
                _connectionPoolQueue.Enqueue(connection);
            }
        }
    }
}
