using System;
using System.Collections.Generic;
using System.IO;

namespace Italbytz.OperatingSystems.Resources.Buddy
{
    /// <summary>
    /// Buddy System Algo implementation
    /// Author: Tahreem Iqbal
    /// </summary>
    public class BuddySystem
    {
        //contains list of processes
        List<Process> lstProcess = new List<Process>();

        //contains list of memory items AKA nodes
        List<Node> lstNode = new List<Node>();

        const int totalSize = 1024; //1024K = 1M
        const int chunkSize = 32;

        public List<int[]> History { get; set; } = new List<int[]>();

        /// <summary>
        /// Initialzies lstNode and adds 1024 items in lstNode. 
        /// Each item represents 1K.
        /// </summary>
        public BuddySystem()
        {
            for (int i = 0; i < totalSize; i++)
            {
                lstNode.Add(new Node()
                {
                    ProcessId = 0,
                    IsAssigned = false,
                    IsEnd = false,
                    IsStart = false
                });
            }
        }

        /// <summary>
        /// Entry point of the algorithm
        /// </summary>
        public void Start(List<Process> processes)
        {
            lstProcess.AddRange(processes);
            FeedProcesses();
        }

        /// <summary>
        /// Feed processes to algo according to their operation type. 
        /// Request = AllocateMemoery, Reelase = DeallocateMemory
        /// </summary>
        void FeedProcesses()
        {
            foreach (var processs in lstProcess)
            {
                if (processs.OpType == "Request")
                {
                    AllocateMemory(processs);
                }
                else if (processs.OpType == "Release")
                {
                    DeallocateMemory(processs);
                }
            }
        }

        /// <summary>
        /// Memory Allocation of a process
        /// </summary>
        /// <param name="p"></param>
        void AllocateMemory(Process p)
        {
            //Find the power of process size e.g 65K = 2 ^ 7
            int b = (int)Math.Ceiling(Math.Log(p.Size, 2));

            //Find length of the block to allocate. 2 ^ 7 = 128K
            int blockLength = (int)Math.Pow(2, b);

            //Define start and ending index of where the block wil be places in lstNode
            int startIndex = 0, endIndex = 0;

            //Divide memory until find the right block
            for (int i = totalSize; i >= 0; i = i / 2)
            {
                if (i == blockLength)
                {
                    //i - 1 because index start from 0 e.g. 128 will be 127
                    endIndex = i - 1;

                    //start index will start from 0
                    startIndex = i - blockLength;

                    //if any of the item between start and end index are assigned OR start index is not a multiple of block length 
                    //then keep incrementing start index
                    while ((lstNode[startIndex].IsAssigned || lstNode[endIndex].IsAssigned) || (startIndex > 1 && startIndex % blockLength != 0))
                    {
                        startIndex++;
                        endIndex = startIndex + blockLength - 1;

                        if (endIndex >= totalSize) break;
                    }

                    //else quit the loop
                    break;
                }
            }

            //this condition means algo could not find a space big enough to allocate the block
            if (lstNode[startIndex].IsAssigned || lstNode[endIndex].IsAssigned)
            {
                Console.WriteLine("No space available to allocate");
                return;
            }

            //assigning all items between start and end index to the process
            for (int i = 0; i < totalSize; i++)
            {
                if (i >= startIndex && i <= endIndex && lstNode[i].IsAssigned == false)
                {
                    lstNode[i].ProcessId = p.Id;
                    lstNode[i].IsAssigned = true;

                    if (i == startIndex)
                    {
                        lstNode[i].IsStart = true;
                    }

                    if (i == endIndex)
                    {
                        lstNode[i].IsEnd = true;
                        break;
                    }
                }
            }

            // display message about where block has been placed in the memory
            string msg = "";
            for (int k = 0; k < totalSize; k++)
            {
                if (lstNode[k].ProcessId == p.Id && lstNode[k].IsStart == true)
                {
                    msg = "Process " + p.Name + " allocated memory of " + blockLength + "K" + " from " + k + " to ";
                }

                if (lstNode[k].ProcessId == p.Id && lstNode[k].IsEnd == true)
                {
                    msg += k + " with actual size of " + p.Size + "K";
                    break;
                }
            }

            AddToHistory();
            Console.WriteLine(msg);
        }

        private void AddToHistory()
        {
            var chunks = new int[totalSize / chunkSize];
            for (int i = 0; i < totalSize / chunkSize; i++)
            {
                var node = lstNode[i * chunkSize];
                if (node.IsAssigned)
                {
                    chunks[i] = node.ProcessId;
                }
                else
                {
                    chunks[i] = -1;
                }
            }
            History.Add(chunks);
        }

        /// <summary>
        /// Remove process from the memory amd make that space available for other processes
        /// </summary>
        /// <param name="p"></param>
        void DeallocateMemory(Process p)
        {
            string msg = "";
            bool isEnd = false;

            for (int i = 0; i < totalSize; i++)
            {
                if (lstNode[i].ProcessId == p.Id && lstNode[i].IsStart == true)
                {
                    msg = "Process " + p.Name + " has been deallocated from " + i + " to ";
                }

                if (lstNode[i].ProcessId == p.Id && lstNode[i].IsEnd == true)
                {
                    msg += i;

                    isEnd = true;
                }

                if (lstNode[i].ProcessId == p.Id)
                {
                    lstNode[i].IsAssigned = false;
                    lstNode[i].IsEnd = false;
                    lstNode[i].IsStart = false;
                    lstNode[i].ProcessId = 0;
                }

                if (isEnd) break;
            }

            AddToHistory();
            Console.WriteLine(msg);
        }

        /// <summary>
        /// NOT IN USE
        /// </summary>
        /// <param name="p"></param>
        void AllocateMemoryWithoutInternalFragmentation(Process p)
        {
            int b = (int)Math.Ceiling(Math.Log(p.Size, 2));
            int blockLength = (int)Math.Pow(2, b);

            int count = 0, startIndex = 0, endIndex = 0;

            for (int j = 0; j < totalSize; j++)
            {
                if (count > 1 && lstNode[j - 1].IsAssigned == true)
                {
                    count = 0;
                }

                if (lstNode[j].IsAssigned == false)
                {
                    count++;
                    endIndex = j;
                }

                if (count == blockLength)
                    break;
            }

            if (count != blockLength)
            {
                Console.WriteLine("No space available to allocate");
                return;
            }

            startIndex = endIndex - (blockLength - 1);

            for (int i = 0; i < totalSize; i++)
            {
                if (i >= startIndex && i <= endIndex && lstNode[i].IsAssigned == false)
                {
                    lstNode[i].ProcessId = p.Id;
                    lstNode[i].IsAssigned = true;

                    if (i == startIndex)
                    {
                        lstNode[i].IsStart = true;
                    }

                    if (i == endIndex)
                    {
                        lstNode[i].IsEnd = true;
                        break;
                    }
                }
            }

            string msg = "";
            for (int k = 0; k < totalSize; k++)
            {
                if (lstNode[k].ProcessId == p.Id && lstNode[k].IsStart == true)
                {
                    msg = "Process " + p.Name + " allocated memory of " + blockLength + "K" + " from " + k + " to ";
                }

                if (lstNode[k].ProcessId == p.Id && lstNode[k].IsEnd == true)
                {
                    msg += k + " with actual size of " + p.Size + "K";
                    break;
                }
            }

            Console.WriteLine(msg);
        }
    }
}
