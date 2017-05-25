using System.Linq;
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
                            res[i] = new GeneratorBlock();
                        }
                        break;
                    case "TERMINATE":
                        {
                            res[i] = new TerminateBlock();
                        }
                        break;
                    case "QUEUE":
                        {
                            res[i] = new QueueBlock();
                        }
                        break;
                    case "ADVANCE":
                        {
                            int idxSeize = gpssBlocks
                                           .Select((s, idx) => new { idx, s })
                                           .Where(t => t.idx < i)
                                           .Where(t => t.s.Type == "SEIZE")
                                           .Select(t => t.idx)
                                           .DefaultIfEmpty(0)
                                           .Max();
                            int idxRelease = gpssBlocks
                                           .Select((s, idx) => new { idx, s })
                                           .Where(t => t.idx > i)
                                           .Where(t => t.s.Type == "RELEASE")
                                           .Select(t => t.idx)
                                           .DefaultIfEmpty(0)
                                           .Min();
                            if (idxSeize < i && i < idxRelease)
                            {
                                res[i] = new FacilityBlock();
                                break;
                            }
                            int idxPreempt = gpssBlocks
                                           .Select((s, idx) => new { idx, s })
                                           .Where(t => t.idx < i)
                                           .Where(t => t.s.Type == "PREEMPT")
                                           .Select(t => t.idx)
                                           .DefaultIfEmpty(0)
                                           .Max();
                            int idxReturn = gpssBlocks
                                           .Select((s, idx) => new { idx, s })
                                           .Where(t => t.idx > i)
                                           .Where(t => t.s.Type == "RETURN")
                                           .Select(t => t.idx)
                                           .DefaultIfEmpty(0)
                                           .Min();
                            if (idxPreempt < i && i < idxReturn)
                            {
                                res[i] = new FacilityBlock();
                                break;
                            }
                            int idxEnter = gpssBlocks
                                           .Select((s, idx) => new { idx, s })
                                           .Where(t => t.idx < i)
                                           .Where(t => t.s.Type == "ENTER")
                                           .Select(t => t.idx)
                                           .DefaultIfEmpty(0)
                                           .Max();
                            int idxLeave = gpssBlocks
                                           .Select((s, idx) => new { idx, s })
                                           .Where(t => t.idx > i)
                                           .Where(t => t.s.Type == "LEAVE")
                                           .Select(t => t.idx)
                                           .DefaultIfEmpty(0)
                                           .Min();
                            if (idxEnter < i && i < idxLeave)
                            {
                                res[i] = new StorageBlock();
                            }
                        }
                        break;
                    default:
                        {
                            res[i] = new VisualBlock();
                        }
                        break;
                }
                res[i].Id = i;
                res[i].Label = block.Tag;
                res[i].Location = new Point(200, (i + 1) * 30);
                res[i].TaskCount = block.CurrentCount;
                if (i > 0)
                {
                    res[i - 1].Links.Add(res[i]);
                }
            }

            return res;
        }
    }
}
