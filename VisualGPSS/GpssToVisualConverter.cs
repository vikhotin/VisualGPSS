using System.Drawing;

namespace VisualGPSS
{
    class GpssToVisualConverter
    {
        public static VisualBlock[] Convert(GpssBlockData[] gpssBlocks)
        {
            VisualBlock[] res = new VisualBlock[gpssBlocks.Length];
            for (int i = 0; i < gpssBlocks.Length; i++)
            {
                GpssBlockData block = gpssBlocks[i];
                switch (block.Type)
                {
                    case "GENERATE":
                        {
                            res[i] = new GeneratorBlock()
                            {
                                Id = i,
                                Label = block.Tag,
                                Location = new Point(50, (i + 1) * 30),
                                TaskCount = block.CurrentCount,
                            };
                            if (i > 0)
                            {
                                res[i - 1].Links.Add(res[i]);
                            }
                        }
                        break;
                    case "TERMINATE":
                        {
                            res[i] = new TerminateBlock()
                            {
                                Id = i,
                                Label = block.Tag,
                                Location = new Point(50, (i + 1) * 30),
                                TaskCount = block.CurrentCount,
                            };
                            if (i > 0)
                            {
                                res[i - 1].Links.Add(res[i]);
                            }
                        }
                        break;
                    default:
                        {
                            res[i] = new VisualBlock()
                            {
                                Id = i,
                                Label = block.Tag,
                                Location = new Point(50, (i + 1) * 30),
                                TaskCount = block.CurrentCount,
                            };
                            if (i > 0)
                            {
                                res[i - 1].Links.Add(res[i]);
                            }
                        }
                        break;
                }
            }

            return res;
        }
    }
}
