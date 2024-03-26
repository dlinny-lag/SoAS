using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SceneModel;
using SceneServices;

namespace ScenesEditor.Data
{
    public class BatchDeserializer
    {
        struct JobItem
        {
            public JobItem(string json, int index, string name)
            {
                Json = json;
                Index = index;
                Name = name;
            }
            public string Json;
            public int Index;
            public string Name;
        }
        private readonly ManualResetEvent addingFinished;
        private bool canEnqueue = true;
        private readonly object queueLock = new object();
        private readonly Queue<JobItem> jsons = new Queue<JobItem>();
        private readonly ConcurrentDictionary<Scene, int> deserialized;
        public BatchDeserializer(int capacity)
        {
            deserialized = new ConcurrentDictionary<Scene, int>(Environment.ProcessorCount, capacity);
            addingFinished = new ManualResetEvent(false);
        }

        private int jsonIndex;
        public void AddJson(string json, string name)
        {
            if (!canEnqueue)
                throw new InvalidOperationException("All jsons added already");
            lock (queueLock)
            {
                ++jsonIndex;
                jsons.Enqueue(new JobItem(json, jsonIndex, name));
            }
        }

        public void NotifyAllAdded()
        {
            canEnqueue = false;
            addingFinished.Set();
        }

        private void ProcessJson(ref JobItem item)
        {
            Scene result = item.Json.ToScene();
            if (result == null)
            {
                Console.WriteLine($@"Unable to deserialize {item.Name}");
                return;
            }

            if (!deserialized.TryAdd(result, item.Index))
                throw new InvalidOperationException($"{item.Name} deserialized already");
        }
        private bool TryDeque(out JobItem item)
        {
            lock (queueLock)
            {
                if (jsons.Count == 0)
                {
                    item = default;
                    return false;
                }
                item = jsons.Dequeue();
                return true;
            }
        }
        private void QueueProcess()
        {
            bool terminate = false;
            do
            {
                if (TryDeque(out var item))
                    ProcessJson(ref item);
                else
                    terminate = addingFinished.WaitOne(1);

            } while (!terminate);
        }

        private List<Task> runningTasks;

        public void Start()
        {
            if (!canEnqueue)
                throw new InvalidOperationException("Can't start second time");
            jsonIndex = 0;
            deserialized.Clear();
            runningTasks = new List<Task>(Environment.ProcessorCount);
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                runningTasks.Add(Task.Factory.StartNew(QueueProcess, TaskCreationOptions.LongRunning));
            }
        }

        public IList<Scene> GetResult()
        {
            Task.WhenAll(runningTasks).Wait(); // block execution
            var retVal = new List<Scene>(deserialized.Keys);
            retVal.Sort((a, b) => deserialized[a].CompareTo(deserialized[b]));
            return retVal;
        }
    }
}