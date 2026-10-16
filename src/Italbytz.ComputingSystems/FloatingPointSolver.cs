using System;
using System.Collections.Generic;
using System.Globalization;
using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class FloatingPointSolver : IFloatingPointSolver
{
    public IFloatingPointSolution Solve(IFloatingPointParameters parameters)
    {
        var raw = parameters.BitPattern;
        var sign = (int)((raw >> 31) & 1);
        var exponentRaw = (byte)((raw >> 23) & 0xFF);
        var exponentUnbiased = (int)exponentRaw - 127;
        var mantissaRaw = raw & 0x7FFFFF;

        var mantissaFraction = (double)mantissaRaw / (1 << 23);
        var mantissaWithImplicitOne = 1.0 + mantissaFraction;
        var signFactor = sign == 1 ? -1.0 : 1.0;
        var exponentFactor = Math.Pow(2.0, exponentUnbiased);
        var value = signFactor * mantissaWithImplicitOne * exponentFactor;

        var bitStr = Convert.ToString(raw, 2).PadLeft(32, '0');
        var signBit = bitStr.Substring(0, 1);
        var expBits = bitStr.Substring(1, 8);
        var mantBits = bitStr.Substring(9, 23);

        var trimmedMant = mantBits.TrimEnd('0');
        var normBin = string.IsNullOrEmpty(trimmedMant) ? "1" : $"1,{trimmedMant}";
        var shiftDirection = exponentUnbiased >= 0 ? $"{exponentUnbiased} Stellen nach rechts" : $"{-exponentUnbiased} Stellen nach links";

        var steps = new List<string>
        {
            $"Bitmuster: {signBit} | {expBits} | {mantBits}",
            $"1. Vorzeichen: s = {sign} -> Faktor (-1)^{sign} = {(sign == 1 ? "-1" : "+1")}",
            $"2. Exponent: e = {expBits}_2 = {exponentRaw}_10 -> E = {exponentRaw} - 127 = {exponentUnbiased}",
            $"3. Mantisse mit impliziter Eins: ({normBin})_2",
            $"4. Binäre Kommaverschiebung ({shiftDirection}): ({normBin})_2 * 2^{exponentUnbiased}",
            $"5. Dezimalwert: {value.ToString(CultureInfo.InvariantCulture)}"
        };

        return new FloatingPointSolution
        {
            Sign = sign,
            ExponentRaw = exponentRaw,
            ExponentUnbiased = exponentUnbiased,
            MantissaRaw = mantissaRaw,
            MantissaFraction = mantissaFraction,
            MantissaWithImplicitOne = mantissaWithImplicitOne,
            Value = value,
            Steps = steps
        };
    }
}
