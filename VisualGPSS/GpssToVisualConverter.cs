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
                InitBlock(gpssBlocks, res, i);
            }
            for (int i = 0; i < gpssBlocks.Length; i++)
            {
                SetBlockParams(res, i);
            }

            return res;
        }

        private static void InitBlock(GpssBlockData[] gpssBlocks, VisualBlock[] res, int i)
        {
            GpssBlockData block = gpssBlocks[i];
            switch (block.Type)
            {
                case "GENERATE":
                    {
                        res[i] = new GeneratorBlock();
                        DefaultBlockParamsInit(i, res[i], block);
                    }
                    break;
                case "TERMINATE":
                    {
                        res[i] = new TerminateBlock();
                        DefaultBlockParamsInit(i, res[i], block);
                    }
                    break;
                case "QUEUE":
                    {
                        res[i] = new QueueBlock();
                        res[i].Id = i;
                        res[i].Label = block.Parameters[0];
                        res[i].TaskCount = block.CurrentCount;
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
                            res[i].Id = i;
                            res[i].Label = gpssBlocks[idxSeize].Parameters[0];
                            res[i].TaskCount = block.CurrentCount;
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
                            res[i].Id = i;
                            res[i].Label = gpssBlocks[idxPreempt].Parameters[0];
                            res[i].TaskCount = block.CurrentCount;
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
                            res[i].Id = i;
                            res[i].Label = gpssBlocks[idxEnter].Parameters[0];
                            res[i].TaskCount = block.CurrentCount;
                        }
                    }
                    break;
                default:
                    {
                        res[i] = new VisualBlock();
                        DefaultBlockParamsInit(i, res[i], block);
                    }
                    break;
            }
        }

        private static void DefaultBlockParamsInit(int i, VisualBlock vis_block, GpssBlockData gpss_block)
        {
            vis_block.Id = i;
            vis_block.Label = gpss_block.Tag;
            vis_block.TaskCount = gpss_block.CurrentCount;
        }

        private static void SetBlockParams(VisualBlock[] res, int i)
        {
            res[i].Location = new Point(200, (i + 1) * 50);
            res[i].AutoSize = true;
            //res[i].Width = 40;//?
            //res[i].Height = 40;//?
            if (i < res.Length - 1)
            {
                res[i].Links.Add(res[i + 1]);
            }
        }

        public static void UpdateStats(VisualBlock[] blocks, GpssBlockData[] newInfo)
        {
            if (blocks != null)
                for (int i = 0; i < blocks.Length; i++)
                {
                    blocks[i].TaskCount = newInfo[i].CurrentCount;
                }
        }
    }
}
