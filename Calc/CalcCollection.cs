using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Dapper;

namespace Calc
{
    class JournalItem
    {
        public JournalItem()
        {
            Id = string.Empty;
            Expression = string.Empty;
        }

        public JournalItem(string expression)
        {
            Expression = expression;
        }
        public string Id
        {
            get;
            set;
        }
        public string Expression
        {
            get;
            set;
        }

    }
    class MemoryItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChange([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public MemoryItem()
        {
            Id = string.Empty;
            Result = string.Empty;
        }
        public MemoryItem(string result)
        {
            Result = result;
        }
        public string Id
        {
            get;
            set;
        }

        private string _result;
        public string Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
                OnPropertyChange(nameof(Result));
            }
        }
    }

    class JournalCollectionTxt
    {
        private string FileName
        {
            get;
        }
        public ObservableCollection<JournalItem> Collection
        {
            get;
            set;
        }


        public JournalCollectionTxt(string fileName)
        {
            FileName = fileName + ".txt";
            Collection = new ObservableCollection<JournalItem>();

            if (!File.Exists(FileName))
            {
                File.Create(FileName);
            }
        }
        public bool TryLoad()
        {
            try
            {
                string[] text = File.ReadAllText(FileName).Split('\n');
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ObservableCollection<JournalItem> Load()
        {
            string[] text = File.ReadAllText(FileName).Split('\n');
            ObservableCollection<JournalItem> collection = new ObservableCollection<JournalItem>();

            foreach (string journalItemTxt in text)
            {
                if (journalItemTxt != string.Empty)
                {
                    JournalItem journalItem = new JournalItem();
                    try
                    {
                        string[] textSplit = journalItemTxt.Split(":");

                        journalItem.Id = textSplit[1];
                        journalItem.Expression = textSplit[2];
                        collection.Add(journalItem);
                    }
                    catch
                    {
                        continue;
                    }
                
                }
            }
            for (int i = 0; i < collection.Count; i++)
            {
                collection[i].Id = (collection.Count - i).ToString();
            }

            return collection;
        }
        private void Update()
        {
            string text = string.Empty;
            foreach (JournalItem journalItem in Collection)
            {
                text += $"{journalItem.Id}:{journalItem.Expression}\n";
            }

            File.WriteAllText(FileName, text);
        }
        public void Add(JournalItem item)
        {
            Collection.Insert(0, item);
            item.Id = Collection.Count.ToString();

            Update();
        }
        public void Remove(JournalItem item)
        {
            Collection.Remove(item);
            for (int i = 0; i < Collection.Count; i++)
            {
                Collection[i].Id = (Collection.Count - i).ToString();
            }

            Update();
        }
        public void Clear()
        {
            Collection.Clear();

            Update();
        }
    }
    class MemoryCollectionTxt
    {
        private string FileName
        {
            get;
        }
        public ObservableCollection<MemoryItem> Collection
        {
            get;
            set;
        }


        public MemoryCollectionTxt(string fileName)
        {
            FileName = fileName + ".txt";
            Collection = new ObservableCollection<MemoryItem>();

            if (!File.Exists(FileName))
            {
                File.Create(FileName);
            }
        }
        public bool TryLoad()
        {
            try
            {
                string[] text = File.ReadAllText(FileName).Split('\n');
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ObservableCollection<MemoryItem> Load()
        {
            string[] text = File.ReadAllText(FileName).Split('\n');
            ObservableCollection<MemoryItem> collection = new ObservableCollection<MemoryItem>();

            foreach (string journalItemTxt in text)
            {
                if (journalItemTxt != string.Empty)
                {
                    MemoryItem memoryItem = new MemoryItem();
                    try
                    {
                        string[] textSplit = journalItemTxt.Split(":");

                        memoryItem.Id = textSplit[1];
                        memoryItem.Result = textSplit[2];
                        collection.Add(memoryItem);
                    }
                    catch
                    {
                        continue;
                    }
                
                }
            }
            for (int i = 0; i < collection.Count; i++)
            {
                collection[i].Id = (collection.Count - i).ToString();
            }

            return collection;
        }
        private void Update()
        {
            string text = string.Empty;
            foreach (MemoryItem memoryItem in Collection)
            {
                text += $"{memoryItem.Id}:{memoryItem.Result}\n";
            }

            File.WriteAllText(FileName, text);
        }
        public void Add(MemoryItem item)
        {
            Collection.Insert(0, item);
            item.Id = Collection.Count.ToString();

            Update();
        }
        public void Remove(MemoryItem item)
        {
            Collection.Remove(item);
            for (int i = 0; i < Collection.Count; i++)
            {
                Collection[i].Id = (Collection.Count - i).ToString();
            }

            Update();
        }
        public void Clear()
        {
            Collection.Clear();

            Update();
        }
        public void ChangeValue(string newValue)
        {
            Collection[0].Result = newValue;
            Update();
        }
    }

    class JournalCollectionJson
    {
        private string FileName
        {
            get;
        }
        public ObservableCollection<JournalItem> Collection
        {
            get;
            set;
        }


        public JournalCollectionJson(string fileName)
        {
            FileName = fileName + ".json";
            Collection = new ObservableCollection<JournalItem>();

            if (!File.Exists(FileName))
            {
                File.Create(FileName);
            }
        }

        public ObservableCollection<JournalItem> Load()
        {
            string json = File.ReadAllText(FileName);
            ObservableCollection<JournalItem> collection = JsonSerializer.Deserialize<ObservableCollection<JournalItem>>(json);
            for (int i = 0; i < collection.Count; i++)
            {
                collection[i].Id = (collection.Count - i).ToString();
            }

            return collection;
        }
        private void Update()
        {
            JsonSerializerOptions jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            string json = JsonSerializer.Serialize(Collection, jsonOptions);

            File.WriteAllText(FileName, json);
        }
        public void Add(JournalItem item)
        {
            Collection.Insert(0, item);
            item.Id = Collection.Count.ToString();

            Update();
        }
        public void Remove(JournalItem item)
        {
            Collection.Remove(item);
            for (int i = 0; i < Collection.Count; i++)
            {
                Collection[i].Id = (Collection.Count - i).ToString();
            }

            Update();
        }
        public void Clear()
        {
            Collection.Clear();

            Update();
        }
    }
    class MemoryCollectionJson
    {
        private string FileName
        {
            get;
        }
        public ObservableCollection<MemoryItem> Collection
        {
            get;
            set;
        }

        public MemoryCollectionJson(string fileName)
        {
            FileName = fileName + ".json";
            Collection = new ObservableCollection<MemoryItem>();

            if (!File.Exists(FileName))
            {
                File.Create(FileName);
            }
        }

        public ObservableCollection<MemoryItem> Load()
        {
            string json = File.ReadAllText(FileName);
            ObservableCollection<MemoryItem> collection = JsonSerializer.Deserialize<ObservableCollection<MemoryItem>>(json);
            for (int i = 0; i < collection.Count; i++)
            {
                collection[i].Id = (collection.Count - i).ToString();
            }

            return collection;
        }
        private void Update()
        {
            JsonSerializerOptions jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            string json = JsonSerializer.Serialize(Collection, jsonOptions);

            File.WriteAllText(FileName, json);
        }
        public void Add(MemoryItem item)
        {
            Collection.Insert(0, item);
            item.Id = Collection.Count.ToString();

            Update();
        }
        public void Remove(MemoryItem item)
        {
            Collection.Remove(item);
            for (int i = 0; i < Collection.Count; i++)
            {
                Collection[i].Id = (Collection.Count - i).ToString();
            }

            Update();
        }
        public void Clear()
        {
            Collection.Clear();

            Update();
        }

        public void ChangeValue(string newValue)
        {
            Collection[0].Result = newValue;
            Update();
        }
    }

    class JournalCollectionDB
    {
        public ObservableCollection<JournalItem> Collection
        {
            get;
            set;
        }

        public JournalCollectionDB()
        {
            // database name = "Calc.db"
            Collection = new ObservableCollection<JournalItem>();

            if (!File.Exists("Calc.db"))
            {
                SQLiteConnection.CreateFile("Calc.db");
            }
            using SQLiteConnection dataBase = new SQLiteConnection("Data Source=Calc.db; Version=3");

            string command = @$"Create table if not exists Journal(
                                Id STRING PRIMARY KEY, 
                                Expression STRING);";
            dataBase.Query(command);
        }

        public ObservableCollection<JournalItem> Load()
        { 
            ObservableCollection<JournalItem> collection = new ObservableCollection<JournalItem>();
            using SQLiteConnection dataBase = new SQLiteConnection($"Data Source=Calc.db; Version=3");

            IEnumerable<JournalItem> calcItems = dataBase.Query<JournalItem>($"Select * from Journal");
            foreach (JournalItem calcItem in calcItems)
            {
                collection.Insert(0, calcItem);
            }

            for (int i = 0; i < collection.Count; i++)
            {
                collection[i].Id = (collection.Count - i).ToString();
            }

            return collection;
        }
        public void Add(JournalItem item)
        {
            Collection.Insert(0, item);
            item.Id = Collection.Count.ToString();

            using SQLiteConnection dataBase = new SQLiteConnection($"Data Source=Calc.db; Version=3");
            string command = @$"Insert into Journal(Id, Expression)
                                Values(@id, @expression)";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("id", item.Id);
            parameters.Add("expression", item.Expression);
            dataBase.Query<JournalItem>(command, parameters);
        }
        public void Remove(JournalItem item)
        {
            Collection.Remove(item);
            for (int i = 0; i < Collection.Count; i++)
            {
                Collection[i].Id = (Collection.Count - i).ToString();
            }

            using SQLiteConnection dataBase = new SQLiteConnection($"Data Source=Calc.db; Version=3");
            dataBase.Query(@$"Delete from Journal
                                  Where Id = {item.Id}");
            for (int i = int.Parse(item.Id) + 1; i <= Collection.Count + 1; i++)
            {
                dataBase.Query(@$"Update Journal
                                      Set Id = {i - 1} where Id = {i}");
            }
        }
        public void Clear()
        {
            Collection.Clear();

            using SQLiteConnection dataBase = new SQLiteConnection($"Data Source=Calc.db; Version=3");
            dataBase.Query($"Delete from Journal");
        }
    }
    class MemoryCollectionDB
    {
        public ObservableCollection<MemoryItem> Collection
        {
            get;
            set;
        }

        public MemoryCollectionDB()
        {
            // database name = "Calc.db"
            Collection = new ObservableCollection<MemoryItem>();

            if (!File.Exists("Calc.db"))
            {
                SQLiteConnection.CreateFile("Calc.db");
            }
            using SQLiteConnection dataBase = new SQLiteConnection("Data Source=Calc.db; Version=3");

            string command = @$"Create table if not exists Memory(
                                Id STRING PRIMARY KEY, 
                                Result STRING);";
            dataBase.Query(command);
        }

        public ObservableCollection<MemoryItem> Load()
        {
            ObservableCollection<MemoryItem> collection = new ObservableCollection<MemoryItem>();
            using SQLiteConnection dataBase = new SQLiteConnection($"Data Source=Calc.db; Version=3");

            IEnumerable<MemoryItem> calcItems = dataBase.Query<MemoryItem>($"Select * from Memory");
            foreach (MemoryItem calcItem in calcItems)
            {
                collection.Insert(0, calcItem);
            }

            for (int i = 0; i < collection.Count; i++)
            {
                collection[i].Id = (collection.Count - i).ToString();
            }

            return collection;
        }
        public void Add(MemoryItem item)
        {
            Collection.Insert(0, item);
            item.Id = Collection.Count.ToString();

            using SQLiteConnection dataBase = new SQLiteConnection($"Data Source=Calc.db; Version=3");
            string command = @$"Insert into Memory(Id, Result)
                                Values(@id, @result)";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("id", item.Id);
            parameters.Add("result", item.Result);
            dataBase.Query<MemoryItem>(command, parameters);
        }
        public void Remove(MemoryItem item)
        {
            Collection.Remove(item);
            for (int i = 0; i < Collection.Count; i++)
            {
                Collection[i].Id = (Collection.Count - i).ToString();
            }

            using SQLiteConnection dataBase = new SQLiteConnection($"Data Source=Calc.db; Version=3");
            dataBase.Query(@$"Delete from Memory
                                  Where Id = {item.Id}");
            for (int i = int.Parse(item.Id) + 1; i <= Collection.Count + 1; i++)
            {
                dataBase.Query(@$"Update Memory
                                      Set Id = {i - 1} where Id = {i}");
            }
        }
        public void Clear()
        {
            Collection.Clear();

            using SQLiteConnection dataBase = new SQLiteConnection($"Data Source=Calc.db; Version=3");
            dataBase.Query($"Delete from Memory");
        }
        public void ChangeValue(string newValue)
        {
            Collection[0].Result = newValue;
            using SQLiteConnection dataBase = new SQLiteConnection($"Data Source=Calc.db; Version=3");
            dataBase.Query(@$"Update Memory
                                  Set Result = {newValue} where Id = {Collection.Count()}");
        }
    }
    


}
