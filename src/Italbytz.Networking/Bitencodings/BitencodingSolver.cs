using System;
using System.Collections.Generic;
using Italbytz.Networking.Abstractions;

namespace Italbytz.Networking
{
    public class BitencodingSolver : IBitencodingSolver
    {
        private int[] Bits = Array.Empty<int>();

        private string[] NRZ()
        {
            var result = new List<string>();
            foreach (var bit in Bits)
            {
                if (bit == 0)
                {
                    result.Add("-");
                }
                else
                {
                    result.Add("+");
                }
            }
            return result.ToArray();
        }

        private string[] NRZI()
        {
            var result = new List<string>();
            var current = "-";
            foreach (var bit in Bits)
            {
                if (bit == 0)
                {
                    result.Add(current);
                }
                else
                {
                    current = (current == "-") ? "+" : "-";
                    result.Add(current);
                }
            }
            return result.ToArray();
        }

        private string[] MLT3()
        {
            var result = new List<string>();
            var current = "0";
            var direction = true;
            foreach (var bit in Bits)
            {
                if (bit == 0)
                {
                    result.Add(current);
                }
                else
                {
                    switch (current)
                    {
                        case "0":
                            if (direction)
                            {
                                current = "+";
                            }
                            else
                            {
                                current = "-";
                            }
                            break;
                        case "-":
                            current = "0";
                            direction = !direction;
                            break;
                        case "+":
                            current = "0";
                            direction = !direction;
                            break;
                        default:
                            break;
                    }
                    result.Add(current);
                }
            }
            return result.ToArray();
        }

        public IBitencodingSolution Solve(IBitencodingParameters parameters)
        {
            Bits = parameters.Bits;
            return new BitencodingSolution()
            {
                NRZ = NRZ(),
                NRZI = NRZI(),
                MLT3 = MLT3()
            };
        }
    }
}
