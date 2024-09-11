/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

using ServicesDomain;

namespace OzonServiceNamespace.Tree
{
    /// <summary>
    /// Contains base settings for queue
    /// </summary>
    public struct ServiceQueueSettings
    {
        public ServiceQueueSettings() { }
        public bool StartQueue { get; set; } = true;
        public int RequestsMinInterval { get; set; } = 200;
        public int MaxRequestsPerMinute { get; set; } = 100;
        public int MinuteInterval { get; set; } = 60000;
    }

    /// <summary>
    /// Base class which would do actions with delay by queue
    /// </summary>
    public abstract class ServiceBaseQueue : ServiceBase
    {
        /// <summary>
        /// Minimum time between 2 requests in ms
        /// </summary>
        protected int RequestsMinInterval { get => _requestsMinInterval; }
        private int _requestsMinInterval = 200;

        /// <summary>
        /// The maximum number of requests that can be processed per minute
        /// </summary>
        protected int MaxRequestsPerMinute { get => _maxRequestsPerMinute; }
        private int _maxRequestsPerMinute = 100;

        /// <summary>
        /// Time for clear limits, default equal 1 minute (60000 ms)
        /// </summary>
        protected int MinuteInterval { get => _minuteInterval; }
        private int _minuteInterval = 60000;

        /// <summary>
        /// Indicates that queue is work and processes requests
        /// </summary>
        protected bool IsQueueRunning { get => _isQueueRunning; }
        private bool _isQueueRunning = false;

        // Tasks for controlling async queue
        private Task? _mainQueueTask;
        private Task? _minuteQueueTask;

        // Main queue of requests
        private Queue<TaskCompletionSource> _requestsQueue = new();

        // Used for limit count of requests per minute
        private int _currentRequestsCount = 0;

        public ServiceBaseQueue(ServiceQueueSettings queueSettings) : base()
        {
            _requestsMinInterval = queueSettings.RequestsMinInterval;
            _maxRequestsPerMinute = queueSettings.MaxRequestsPerMinute;
            _minuteInterval = queueSettings.MinuteInterval;

            if (queueSettings.StartQueue)
            {
                // Run async queue
                // Main - for call requests by min interval
                // Minute - for periodic clear limit
                _mainQueueTask = RunMainQueue();
                _minuteQueueTask = RunMinuteQueue();
            }
        }

        // Call requests by min interval
        private async Task RunMainQueue()
        {
            _isQueueRunning = true;
            while (_isQueueRunning)
            {
                if (_requestsQueue.Count > 0 && _currentRequestsCount < _maxRequestsPerMinute)
                {
                    _requestsQueue.Dequeue().SetResult();
                    _currentRequestsCount++;
                }
                await Task.Delay(_requestsMinInterval);
            }
        }

        // Periodic clear limit by minute
        private async Task RunMinuteQueue()
        {
            while (_isQueueRunning)
            {
                _currentRequestsCount = 0;
                await Task.Delay(_minuteInterval);
            }
        }

        // Add new request in queue for call
        protected void AddNewRequest(TaskCompletionSource request)
        {
            _requestsQueue.Enqueue(request);
        }

        /// <summary>
        /// Stop queue cycles
        /// </summary>
        internal void StopQueue()
        {
            _isQueueRunning = false;
        }

        /// <summary>
        /// Start queue if not running and clear queue and start new if running
        /// </summary>
        internal void RestartQueue()
        {
            if (_isQueueRunning)
            {
                _isQueueRunning = false;
                try
                {
                    if (_mainQueueTask != null && _minuteQueueTask != null)
                        Task.WaitAll(_mainQueueTask, _minuteQueueTask);
                }
                catch { }
            }
            _requestsQueue.Clear();
            _mainQueueTask = RunMainQueue();
            _minuteQueueTask = RunMinuteQueue();
        }
    }
}
