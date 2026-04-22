using System;
using System.Collections.Generic;
using System.Linq;
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
            var nrz = NRZ();
            var nrzi = NRZI();
            var mlt3 = MLT3();
            var steps = new List<string>
            {
                $"Input bits: {string.Join("", Bits)}.",
                $"NRZ output: {string.Join(" ", nrz)}.",
                $"NRZI output: {string.Join(" ", nrzi)}.",
                $"MLT-3 output: {string.Join(" ", mlt3)}."
            };

            steps.AddRange(Bits.Select((bit, index) =>
                $"Bit {index + 1}={bit} -> NRZ {nrz[index]}, NRZI {nrzi[index]}, MLT-3 {mlt3[index]}."));

            return new BitencodingSolution()
            {
                NRZ = nrz,
                NRZI = nrzi,
                MLT3 = mlt3,
                Steps = steps
            };
        }
    }
}
