using alivegeto.TaskPlanner.DataAccess.Abstractions;
using alivegeto.TaskPlanner.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace alivegeto.TaskPlanner.DataAccess
{
    public class FileWorkItemsRepository : IWorkItemsRepository
    {
        private const string FileName = "work-items.json"; 
        private readonly Dictionary<Guid, WorkItem> _workItems;

        public FileWorkItemsRepository()
        {
            _workItems = new Dictionary<Guid, WorkItem>();

            if (File.Exists(FileName))
            {
                var json = File.ReadAllText(FileName);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    var items = JsonConvert.DeserializeObject<WorkItem[]>(json);
                    if (items != null)
                    {
                        foreach (var item in items)
                        {
                            _workItems[item.Id] = item;
                        }
                    }
                }
            }
        }

        public Guid Add(WorkItem workItem)
        {
            var copy = workItem.Clone();
            copy.Id = Guid.NewGuid();
            _workItems[copy.Id] = copy;
            return copy.Id;
        }

        public WorkItem Get(Guid id)
        {
            return _workItems.TryGetValue(id, out var item) ? item : null;
        }

        public WorkItem[] GetAll()
        {
            var array = new WorkItem[_workItems.Count];
            _workItems.Values.CopyTo(array, 0);
            return array;
        }

        public bool Update(WorkItem workItem)
        {
            if (!_workItems.ContainsKey(workItem.Id))
                return false;

            _workItems[workItem.Id] = workItem.Clone();
            return true;
        }

        public bool Remove(Guid id)
        {
            return _workItems.Remove(id);
        }

        public void SaveChanges()
        {
            var itemsArray = GetAll();
           var json = JsonConvert.SerializeObject(itemsArray, Newtonsoft.Json.Formatting.Indented);

            File.WriteAllText(FileName, json);
        }
    }
}
