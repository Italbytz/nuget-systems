using System;
using System.Collections.Generic;
using Italbytz.Networking.Abstractions;

namespace Italbytz.Networking
{
    public class CRCSolver : ICRCSolver
    {
        public CRCSolver()
        {
        }

        public ICRCSolution Solve(ICRCParameters parameters)
        {
            var crc = CRC5(parameters.Term, 0x0);
            var crcTest = CRC5(parameters.Term, crc.Item2);
            var steps = new List<string>
            {
                $"Input term: {parameters.Term} ({Convert.ToString(parameters.Term, 2).PadLeft(8, '0')}).",
                "Generator polynomial (CRC-5): 0b100101.",
                "Division trace for calculation remainder:",
                crc.Item1,
                $"Calculated remainder: {Convert.ToString(crc.Item2, 2).PadLeft(5, '0')}.",
                "Division trace for check value:",
                crcTest.Item1,
                $"Check remainder: {Convert.ToString(crcTest.Item2, 2).PadLeft(5, '0')}."
            };
            var solution = new CRCSolution
            {
                Calculation = crc.Item1,
                Check = crcTest.Item1,
                Steps = steps
            };
            return solution;
        }

        private (string, ushort) CRC5(ushort value, ushort crc)
        {
            ushort padded = (ushort)((value << 5) + crc);
            ushort poly = 0x25 << 5;
            var result = "";

            for (int i = 10; i > 4; i--)
            {
                var msb = (padded >> i) & 0x1;
                if (msb == 0x1)
                {
                    result += $"{Convert.ToString(padded, 2).PadLeft(11, '0')}\n\n{Convert.ToString(poly, 2).PadLeft(11, '0')}\n\n";
                    padded ^= poly;
                }
                poly >>= 1;
            }
            result += Convert.ToString(padded, 2).PadLeft(11, '0');
            return (result, (ushort)(padded));

        }
    }
}
