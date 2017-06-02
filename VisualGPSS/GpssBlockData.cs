namespace VisualGPSS
{
    class GpssBlockData
    {
        public string[] _data { get; set; }

        public string Tag
        {
            get
            {
                return _data[0];
            }
            set
            {
                _data[0] = value;
            }
        }

        public string Type
        {
            get
            {
                return _data[1];
            }
            set
            {
                _data[1] = value;
            }
        }

        public int CurrentCount
        {
            get
            {
                return System.Convert.ToInt32(_data[2]);
            }
            set
            {
                _data[2] = value.ToString();
            }
        }

        public int EntryCount
        {
            get
            {
                return System.Convert.ToInt32(_data[3]);
            }
            set
            {
                _data[3] = value.ToString();
            }
        }

        public int SourceCodeLineNumber
        {
            get
            {
                return System.Convert.ToInt32(_data[4]);
            }
            set
            {
                _data[4] = value.ToString();
            }
        }

        public string[] Parameters { get; set; }

        public GpssBlockData()
        {
            _data = new string[5];
        }

        public GpssBlockData(string tag, string type, int current, int entry, int sloc) : this()
        {
            Tag = tag;
            Type = type;
            CurrentCount = current;
            EntryCount = entry;
            SourceCodeLineNumber = sloc;
        }
    }
}
