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

        var steps = new List<string>
        {
            $"Bitmuster: {signBit} | {expBits} | {mantBits}",
            $"1. Vorzeichen: s = {sign} -> Faktor (-1)^{sign} = {(sign == 1 ? "-1" : "+1")}",
            $"2. Exponent: e = {expBits}_2 = {exponentRaw}_10, unbiast: E = {exponentRaw} - 127 = {exponentUnbiased} -> Faktor 2^{exponentUnbiased} = {exponentFactor.ToString(CultureInfo.InvariantCulture)}",
            $"3. Mantisse: 1 + m/2^23 = 1 + {mantissaRaw}/8388608 = {mantissaWithImplicitOne.ToString(CultureInfo.InvariantCulture)}",
            $"4. Gesamtergebnis: f = ({signFactor}) * {mantissaWithImplicitOne.ToString(CultureInfo.InvariantCulture)} * {exponentFactor.ToString(CultureInfo.InvariantCulture)} = {value.ToString(CultureInfo.InvariantCulture)}"
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
