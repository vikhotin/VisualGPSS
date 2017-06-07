using System.Linq;
using System.Drawing;
using System;
using System.Collections.Generic;

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
                SetBlockParams(gpssBlocks, res, i);
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

        private static void SetBlockParams(GpssBlockData[] gpssBlocks, VisualBlock[] res, int i)
        {
            if (i == 0)
                for (int l = 0; l < res.Length; l++)
                    res[l].Top = (l + 1) * 50;
            res[i].Left += 200;
            res[i].AutoSize = true;
            GpssBlockData block = gpssBlocks[i];
            switch (block.Type)
            {
                case "GATE":
                case "LOOP":
                    {
                        int blockto = gpssBlocks
                                      .Select((s, idx) => new { idx, s })
                                      .Where(t => block.Parameters[1].ToUpper() == t.s.Tag.ToUpper())
                                      .Select(t => t.idx)
                                      .First();
                        res[i].Links.Add(res[blockto]);

                        if (i < res.Length - 1)
                        {
                            res[i].Links.Add(res[i + 1]);
                        }

                        if (gpssBlocks[blockto].Type == "GATE" ||
                            gpssBlocks[blockto].Type == "LOOP")
                        {
                            res[blockto].Location = new Point(res[blockto].Location.X + 150,
                                                              res[i].Location.Y);
                            for (int l = blockto+1; l < res.Length; l++)
                            {
                                res[l].Location = res[blockto].Location;
                                res[l].Top += (l - blockto) * 50;
                            }
                        }
                    }
                    break;
                case "TEST":
                    {
                        int blockto = gpssBlocks
                                      .Select((s, idx) => new { idx, s })
                                      .Where(t => block.Parameters[2].ToUpper() == t.s.Tag.ToUpper())
                                      .Select(t => t.idx)
                                      .First();
                        res[i].Links.Add(res[blockto]);

                        if (i < res.Length - 1)
                        {
                            res[i].Links.Add(res[i + 1]);
                        }
                    }
                    break;
                case "TRANSFER":
                    {
                        List<string> pars = new List<string>();
                        if (!string.IsNullOrEmpty(block.Parameters[1]))
                            pars.Add(block.Parameters[1].ToUpper());
                        if (block.Parameters.Length > 2 && !string.IsNullOrEmpty(block.Parameters[2]))
                            pars.Add(block.Parameters[2].ToUpper());
                        int[] blockto = gpssBlocks
                                        .Select((s, idx) => new { idx, s })
                                        .Where(t => pars.Contains(t.s.Tag.ToUpper()))
                                        .Select(t => t.idx)
                                        .ToArray();
                        foreach (int to in blockto)
                            res[i].Links.Add(res[to]);
                        /*
                        if (i < res.Length - 1)
                        {
                            res[i].Links.Add(res[i + 1]);
                        }
                        */
                    }
                    break;
                case "TERMINATE":
                    break;
                default:
                    if (i < res.Length - 1)
                    {
                        res[i].Links.Add(res[i + 1]);
                    }
                    break;
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
