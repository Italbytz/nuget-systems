using System.Collections.Generic;
using System.Text;
using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class AsciiMemorySolver : IAsciiMemorySolver
{
    public IAsciiMemorySolution Solve(IAsciiMemoryParameters parameters)
    {
        var word = parameters.Word ?? string.Empty;
        var littleEndian = parameters.LittleEndian;
        var bytes = Encoding.ASCII.GetBytes(word);
        var totalWords = (bytes.Length + 3) / 4;
        if (totalWords == 0) totalWords = 1;

        var memoryLines = new List<string>();
        var wordsList = new List<AsciiMemoryWord>();
        var steps = new List<string>();

        steps.Add($"Zeichenkette '{word}' enthält {bytes.Length} Byte(s) im US-ASCII-Format.");

        for (var i = 0; i < totalWords; i++)
        {
            var chunk = new byte[4];
            var wordObj = new AsciiMemoryWord
            {
                Address = i * 4
            };

            for (var b = 0; b < 4; b++)
            {
                var idx = i * 4 + b;
                var val = idx < bytes.Length ? bytes[idx] : (byte)0;
                chunk[b] = val;
                var ch = idx < bytes.Length ? (char)val : '\0';

                // Register position:
                // Little Endian: chunk[0] is at lowest byte (bits 0-7, position 0 from right)
                // Big Endian: chunk[0] is at highest byte (bits 24-31, position 3 from right / position 0 from left)
                var regPos = littleEndian ? b : (3 - b);

                wordObj.Bytes[b] = new AsciiMemoryByte
                {
                    Address = idx,
                    Value = val,
                    Character = ch,
                    Hex = $"{val:X2}",
                    ByteOffsetInWord = b,
                    RegisterBytePosition = regPos
                };
            }

            string hex;
            if (littleEndian)
            {
                hex = $"{chunk[3]:X2}{chunk[2]:X2}{chunk[1]:X2}{chunk[0]:X2}";
                steps.Add($"Wort an Adresse 0x{wordObj.Address:X2}: Little Endian → Byte 0 ('{(char)chunk[0]}'=0x{chunk[0]:X2}) steht ganz rechts. Hex-Darstellung: 0x{hex}.");
            }
            else
            {
                hex = $"{chunk[0]:X2}{chunk[1]:X2}{chunk[2]:X2}{chunk[3]:X2}";
                steps.Add($"Wort an Adresse 0x{wordObj.Address:X2}: Big Endian → Byte 0 ('{(char)chunk[0]}'=0x{chunk[0]:X2}) steht ganz links. Hex-Darstellung: 0x{hex}.");
            }

            wordObj.HexWord = hex;
            memoryLines.Add(hex);
            wordsList.Add(wordObj);
        }

        return new AsciiMemorySolution
        {
            Word = word,
            LittleEndian = littleEndian,
            MemoryLines = memoryLines,
            Words = wordsList,
            Steps = steps
        };
    }
}
